namespace CodingMonkey.CodeExecutor
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class PreExecutionSecurity
    {
        /// <summary>
        /// A list of namespaces which are allowed to be included in using statements by
        /// user submitted code.
        /// </summary>
        private static readonly IList<string> AllowedNamespaces = new ReadOnlyCollection<string>
            (new List<string>()
                 {
                     "System",
                     "System.Collections.Generic"
                 });

        public string SanitiseCode(string codeToSanitise)
        {
            string sanitisedCode;

            sanitisedCode = SanitiseUsings(codeToSanitise);

            return sanitisedCode;
        }

        private string SanitiseUsings(string codeToSanitise)
        {
            string sanitisedCode = codeToSanitise;

            const string UsingPattern = "using.+;";
            sanitisedCode = Regex.Replace(sanitisedCode, UsingPattern, "");

            IList<string> usingStatements = this.GetAllowedUsingStatements();

            string usingStatementsFlaterned = string.Join(Environment.NewLine, usingStatements);

            sanitisedCode = usingStatementsFlaterned + Environment.NewLine + sanitisedCode;

            return sanitisedCode;
        }

        private IList<string> GetAllowedUsingStatements()
        {
            return AllowedNamespaces.Select(x => x = "using " + x + ";").ToList();
        }
    }
}
