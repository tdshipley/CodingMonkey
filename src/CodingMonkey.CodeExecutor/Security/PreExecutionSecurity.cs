namespace CodingMonkey.CodeExecutor.Security
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    enum PreExecutionSecurityCodes
    {
        FailedSanitisation = 9001
    }

    public class PreExecutionSecurity
    {
        public int LinesOfCodeAdded => SecurityLists.SafeNamespaces.Count;

        public string SanitiseCode(string codeToSanitise, int santisationLoopLimit = 5)
        {
            string sanitisedCode = codeToSanitise;
            int noOfReplacementsMade = -1;
            int tryCount = 0;
            string SanitisationFailedMessage = $"Compilation / Execution of code failed. Code: {(int)PreExecutionSecurityCodes.FailedSanitisation}";

            while (noOfReplacementsMade != 0)
            {
                if (tryCount > santisationLoopLimit)
                {
                   throw new TimeoutException(SanitisationFailedMessage);
                }

                sanitisedCode = this.SanitiseTypes(sanitisedCode, out noOfReplacementsMade);
                tryCount++;
            }

            noOfReplacementsMade = -1;
            tryCount = 0;
            while (noOfReplacementsMade != 0)
            {
                if (tryCount > santisationLoopLimit)
                {
                    throw new TimeoutException(SanitisationFailedMessage);
                }

                sanitisedCode = this.SanitiseUsings(sanitisedCode, out noOfReplacementsMade);
                tryCount++;
            }

            noOfReplacementsMade = -1;
            tryCount = 0;
            while (noOfReplacementsMade != 0)
            {
                if (tryCount > santisationLoopLimit)
                {
                    throw new TimeoutException(SanitisationFailedMessage);
                }

                sanitisedCode = this.SanitiseNamespaces(sanitisedCode, out noOfReplacementsMade);
                tryCount++;
            }

            return sanitisedCode;
        }

        /// <summary>
        /// Adds any safe using statements which are not currently in
        /// the code.
        /// </summary>
        /// <param name="codeToSanitise"></param>
        /// <returns></returns>
        private string SanitiseUsings(string codeToSanitise, out int noOfAdditionsMade)
        {
            string sanitisedCode = codeToSanitise;
            noOfAdditionsMade = 0;

            IList<string> safeUsingStatements = SecurityLists.SafeUsingStatements;

            foreach (var safeUsingStatement in safeUsingStatements)
            {
                // Add any using statements which are safe that the user hasn't added
                // Makes things easier for new developers who might not yet understand
                // using statements - makes things a little more script like.
                if (!sanitisedCode.Contains(safeUsingStatement))
                {
                    sanitisedCode = safeUsingStatement + Environment.NewLine + sanitisedCode;
                    noOfAdditionsMade++;
                }
            }

            return sanitisedCode;
        }

        private string SanitiseNamespaces(string codeToSanitise, out int noOfReplacementsMade)
        {
            string sanitisedCode = codeToSanitise;
            noOfReplacementsMade = 0;

            foreach (var bannedNamespace in SecurityLists.BannedNamespaces)
            {
                string nsPatternIncludingExtraWhitespace = this.GetNamspaceRegexPatternIgnoreSpaces(bannedNamespace);
                string nsPatternIncludingExtraWhitespaceAndTrailingDot = nsPatternIncludingExtraWhitespace + @"\s*[.]";

                int namespaceMatches = Regex.Matches(sanitisedCode, nsPatternIncludingExtraWhitespace).Count;
                int namespaceWithTrailingDotMatches =
                    Regex.Matches(sanitisedCode, nsPatternIncludingExtraWhitespaceAndTrailingDot).Count;

                if (namespaceWithTrailingDotMatches > 0)
                {
                    sanitisedCode = Regex.Replace(sanitisedCode, nsPatternIncludingExtraWhitespaceAndTrailingDot, "");
                }

                if (namespaceMatches > 0)
                {
                    sanitisedCode = Regex.Replace(sanitisedCode, nsPatternIncludingExtraWhitespace, "");
                }

                noOfReplacementsMade += namespaceWithTrailingDotMatches + namespaceMatches;
            }

            return sanitisedCode;
        }

        private string SanitiseTypes(string codeToSanitise, out int noOfReplacementsMade)
        {
            string sanitisedCode = codeToSanitise;
            noOfReplacementsMade = 0;

            foreach (var bannedType in SecurityLists.BannedTypes)
            {
                // Remove the notiation that this type takes generics args
                string bannedTypeToSearchFor = Regex.Replace(bannedType, @"`\d", "");

                string bannedTypeToSearchForPatternWithDot = $"{bannedType}\\s*[.]";
                int typeWithTrailingDotMatches = Regex.Matches(sanitisedCode, bannedTypeToSearchForPatternWithDot).Count;

                if (typeWithTrailingDotMatches > 0)
                {
                    // Remove Type with any static / property usages
                    sanitisedCode = Regex.Replace(sanitisedCode, bannedTypeToSearchForPatternWithDot, "");
                }

                int typeMatches = Regex.Matches(sanitisedCode, bannedTypeToSearchFor).Count;

                if (typeMatches > 0)
                {
                    // Remove Type
                    sanitisedCode = sanitisedCode.Replace(bannedTypeToSearchFor, "");
                }

                noOfReplacementsMade += typeMatches + typeWithTrailingDotMatches;
            }

            return sanitisedCode;
        }

        private string GetNamspaceRegexPatternIgnoreSpaces(string namespaceToSearchFor)
        {
            string[] namespaceParts = namespaceToSearchFor.Split('.');

            string regexPattern = namespaceParts[0] + @"\s*";

            for (int i = 1; i < namespaceParts.Length; i++)
            {
                regexPattern = regexPattern + "[.]" + @"\s*" + namespaceParts[i];
            }

            return regexPattern;
        }
    }
}
