﻿namespace CodingMonkey.CodeExecutor
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class PreExecutionSecurity
    {
        public int LinesOfCodeAdded => SafeNamespaces.Count;

        /// <summary>
        /// A list of namespaces which are allowed to be included in using statements by
        /// user submitted code.
        /// </summary>
        private static readonly IList<string> SafeNamespaces = new ReadOnlyCollection<string>
            (new List<string>()
                 {
                     "System",
                     "System.Collections",
                     "System.Collections.Generic"
                 });


        private IList<string> BannedNamespaces => this.GetBannedNamespaces();

        private IList<string> SafeUsingStatements => SafeNamespaces.Select(x => x = "using " + x + ";").ToList();

        public string SanitiseCode(string codeToSanitise)
        {
            string sanitisedCode;

            sanitisedCode = SanitiseUsings(codeToSanitise);
            sanitisedCode = SanitiseNamespaces(sanitisedCode);

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

            IList<string> safeUsingStatements = this.SafeUsingStatements;

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
            foreach (var bannedNamespace in this.BannedNamespaces)
            {
                string nsPatternIncludingExtraWhitespace = this.GetNamspaceRegexPatternIgnoreSpaces(bannedNamespace);
                string nsPatternIncludingExtraWhitespaceAndTrailingDot = nsPatternIncludingExtraWhitespace + @"\s*[.]";

                sanitisedCode = Regex.Replace(sanitisedCode, nsPatternIncludingExtraWhitespaceAndTrailingDot, "");
                sanitisedCode = Regex.Replace(sanitisedCode, nsPatternIncludingExtraWhitespace, "");
            }

            return sanitisedCode;
        }

        private IList<string> GetBannedNamespaces()
        {
            List<string> bannedNamespacesList = new List<string>();

            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(Enumerable).GetTypeInfo().Assembly;

            var mscorlibTypes = mscorlib.GetTypes().Select(t => t.Namespace).Where(t => !SafeNamespaces.Contains(t)).Distinct().ToList();
            var systemCoreTypes = systemCore.GetTypes().Select(t => t.Namespace).Where(t => !SafeNamespaces.Contains(t)).Distinct().ToList();

            bannedNamespacesList.AddRange(mscorlibTypes);
            bannedNamespacesList.AddRange(systemCoreTypes);
            bannedNamespacesList.RemoveAll(x => x == null);

            return bannedNamespacesList.Distinct().ToList();

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
