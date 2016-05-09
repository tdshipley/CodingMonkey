namespace CodingMonkey.CodeExecutor.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class PreExecutionSecurity
    {
        public int LinesOfCodeAdded => SecurityLists.SafeNamespaces.Count;

        public string SanitiseCode(string codeToSanitise)
        {
            string sanitisedCode = codeToSanitise;

            sanitisedCode = this.SanitiseTypes(sanitisedCode);
            sanitisedCode = this.SanitiseUsings(sanitisedCode);
            sanitisedCode = this.SanitiseNamespaces(sanitisedCode);

            return sanitisedCode;
        }

        /// <summary>
        /// Adds any safe using statements which are not currently in
        /// the code.
        /// </summary>
        /// <param name="codeToSanitise"></param>
        /// <returns></returns>
        private string SanitiseUsings(string codeToSanitise)
        {
            string sanitisedCode = codeToSanitise;

            IList<string> safeUsingStatements = SecurityLists.SafeUsingStatements;

            foreach (var safeUsingStatement in safeUsingStatements)
            {
                // Add any using statements which are safe that the user hasn't added
                // Makes things easier for new developers who might not yet understand
                // using statements - makes things a little more script like.
                if (!sanitisedCode.Contains(safeUsingStatement))
                {
                    sanitisedCode = safeUsingStatement + Environment.NewLine + sanitisedCode;
                }
            }

            return sanitisedCode;
        }

        private string SanitiseNamespaces(string codeToSanitise)
        {
            string sanitisedCode = codeToSanitise;
            foreach (var bannedNamespace in SecurityLists.BannedNamespaces)
            {
                string nsPatternIncludingExtraWhitespace = this.GetNamspaceRegexPatternIgnoreSpaces(bannedNamespace);
                string nsPatternIncludingExtraWhitespaceAndTrailingDot = nsPatternIncludingExtraWhitespace + @"\s*[.]";

                sanitisedCode = Regex.Replace(sanitisedCode, nsPatternIncludingExtraWhitespaceAndTrailingDot, "");
                sanitisedCode = Regex.Replace(sanitisedCode, nsPatternIncludingExtraWhitespace, "");
            }

            return sanitisedCode;
        }

        private string SanitiseTypes(string codeToSanitise)
        {
            string sanitisedCode = codeToSanitise;

            foreach (var bannedType in SecurityLists.BannedTypes)
            {
                // Remove the notiation that this type takes generics args
                string bannedTypeToSearchFor = Regex.Replace(bannedType, @"`\d", "");

                // Remove Type with any static / property usages
                string bannedTypeToSearchForPatternWithDot = $"{bannedType}\\s*[.]";
                sanitisedCode = Regex.Replace(sanitisedCode, bannedTypeToSearchForPatternWithDot, "");

                // Remove Type
                sanitisedCode = sanitisedCode.Replace(bannedTypeToSearchFor, "");
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
