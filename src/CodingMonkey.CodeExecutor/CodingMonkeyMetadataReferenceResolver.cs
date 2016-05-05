namespace CodingMonkey.CodeExecutor
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using System.Linq;

    using Microsoft.CodeAnalysis.Scripting;

    public class CodingMonkeyMetadataReferenceResolver : MetadataReferenceResolver
    {
        private MetadataReferenceResolver _defaultResolver = ScriptOptions.Default.MetadataResolver;

        public override bool ResolveMissingAssemblies => false;

        public override bool Equals(object other)
        {
            return this._defaultResolver.Equals(other);
        }

        public override int GetHashCode() => _defaultResolver.GetHashCode();

        public override ImmutableArray<PortableExecutableReference> ResolveReference(
            string reference,
            string baseFilePath,
            MetadataReferenceProperties properties)
        {
           return this._defaultResolver.ResolveReference(reference, baseFilePath, properties);
        }

        public override PortableExecutableReference ResolveMissingAssembly(MetadataReference definition, AssemblyIdentity referenceIdentity)
        {
            return null;
        }
    }
}
