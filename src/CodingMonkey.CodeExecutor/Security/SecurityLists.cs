namespace CodingMonkey.CodeExecutor.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.Reflection;

    internal static class SecurityLists
    {
        /// <summary>
        /// A list of namespaces which are allowed to be included in using statements by
        /// user submitted code.
        /// </summary>
        internal static readonly IList<string> SafeNamespaces = new ReadOnlyCollection<string>
            (new List<string>()
                 {
                     "System",
                     "System.Collections",
                     "System.Collections.Generic"
                 });

        /// <summary>
        /// A list of types which are allowed to be included in user submitted code
        /// </summary>
        internal static readonly IList<string> SafeTypes = new ReadOnlyCollection<string>
            (new List<string>()
                 {
                    "Array",
                    "ArrayList",
                    "BitArray",
                    "Boolean",
                    "Buffer",
                    "Byte",
                    "CaseInsensitiveComparer",
                    "Char",
                    "CharEnumerator",
                    "CollectionBase",
                    "CompareInfo",
                    "CompareOptions",
                    "Comparer",
                    "Comparer`1",
                    "Convert",
                    "CultureTypes",
                    "Currency",
                    "DateTime",
                    "DateTimeKind",
                    "DateTimeOffset",
                    "DictionaryBase",
                    "Dictionary`2",
                    "Double",
                    "Enum",
                    "Enumerable",
                    "EqualityComparer`1",
                    "HashSet`1",
                    "Hashtable",
                    "Int16",
                    "Int32",
                    "Int64",
                    "KeyCollection",
                    "KeyedByTypeCollection`1",
                    "LinkedListNode`1",
                    "LinkedList`1",
                    "List`1",
                    "Math",
                    "Number",
                    "Object",
                    "Queue",
                    "Queue`1",
                    "Random",
                    "ReadOnlyCollectionBase",
                    "SortedDictionary`2",
                    "SortedList",
                    "SortedList`2",
                    "SortedSet`1",
                    "Stack",
                    "Stack`1",
                    "String",
                    "StringComparer",
                    "StringReader",
                    "StringSplitOptions",
                    "StringWriter",
                    "StructuralComparisons",
                    "SynchronizedCollection`1",
                    "SynchronizedKeyedCollection`2",
                    "SynchronizedReadOnlyCollection`1",
                    "TimeSpanFormat",
                    "TimeSpanParse",
                    "Type",
                    "UInt16",
                    "UInt32",
                    "UInt64",
                    "ValueCollection",
                    "ValueType",
                    "Variant",
                    "Void"
                 });

        internal static IList<string> BannedNamespaces => GetBannedNamespaces();

        internal static IList<string> BannedTypes => GetBannedTypes();

        internal static IList<string> SafeUsingStatements => SafeNamespaces.Select(x => x = "using " + x + ";").ToList();

        private static Assembly MscorlibAssembly => typeof(object).GetTypeInfo().Assembly;
        private static Assembly SystemCoreAssembly => typeof(Enumerable).GetTypeInfo().Assembly;

        private static IList<string> GetBannedNamespaces()
        {
            List<string> bannedNamespaces = new List<string>();

            var mscorlibNamespaces = MscorlibAssembly.GetTypes().Select(t => t.Namespace).Where(t => !SafeNamespaces.Contains(t)).Distinct().ToList();
            var systemCoreNamespaces = SystemCoreAssembly.GetTypes().Select(t => t.Namespace).Where(t => !SafeNamespaces.Contains(t)).Distinct().ToList();

            bannedNamespaces.AddRange(mscorlibNamespaces);
            bannedNamespaces.AddRange(systemCoreNamespaces);
            bannedNamespaces.RemoveAll(x => x == null);

            return bannedNamespaces.Distinct().ToList();

        }

        private static IList<string> GetBannedTypes()
        {
            List<string> bannedTypes = new List<string>();

            var mscorlibTypes = MscorlibAssembly.GetTypes().Select(t => t.Name).Where(t => !SafeTypes.Contains(t)).Distinct().ToList();
            var systemCoreTypes = SystemCoreAssembly.GetTypes().Select(t => t.Name).Where(t => !SafeTypes.Contains(t)).Distinct().ToList();

            bannedTypes.AddRange(mscorlibTypes);
            bannedTypes.AddRange(systemCoreTypes);
            bannedTypes.RemoveAll(x => x == null);

            return bannedTypes;
        }
    }
}
