using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace CodingMonkey.CodeExecutor
{
    public class Executor
    {
        public string CodeToCompile { get; set; }
        private List<SecurityPermissionFlag> PermissionFlags { get; set; }
        private Zone PresetZone { get; set; }

        public Executor(List<SecurityPermissionFlag> permissionFlags, Zone presetZone, string codeToCompile)
        {
            this.PermissionFlags = permissionFlags;
            this.PresetZone = presetZone;
            this.CodeToCompile = codeToCompile;
        }

        public void Execute()
        {
            Compiler compiler = new Compiler(CodeToCompile, new List<string>()
            {
                "System.dll"
            }, false);

            CompilerResults results = compiler.Compile();
        }

        private AppDomain GetAppDomain(Zone presetZone)
        {
            var permissionSet = GetPermissionSet(presetZone);
            throw new NotImplementedException();
        }

        private PermissionSet GetPermissionSet(List<SecurityPermissionFlag> permissionFlags)
        {
            var permissionSet = new PermissionSet(PermissionState.None);
            permissionFlags.ForEach(flag => permissionSet.AddPermission(new SecurityPermission(flag)));
            return permissionSet;
        }

        private PermissionSet GetPermissionSet(Zone presetZone)
        {
            var evidence = new Evidence();
            evidence.AddHostEvidence(presetZone);
            return SecurityManager.GetStandardSandbox(evidence);
        }
    }
}
