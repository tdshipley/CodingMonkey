using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
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

            CompilerResults results = compiler.CompileFromSource();

            var appDomain = GetAppDomain(results.PathToAssembly,
                "sandbox",
                this.PermissionFlags);

            var newDomainInstance = GetNewDomainInstance(appDomain);

            string untrusedAssembly = "HelloWorld";
            string className = "HelloWorld.Hello";
            string entryPoint = "Main";

            newDomainInstance.ExecuteUntrustedCode(untrusedAssembly, className, entryPoint, null);
        }

        public void ExecuteUntrustedCode(string assemblyName, string typeName, string entryPoint, Object[] parameters)
        {
            //Assembly.GetEntryAssembly()
            //Load the MethodInfo for a method in the new Assembly. This might be a method you know, or 
            //you can use Assembly.EntryPoint to get to the main function in an executable.
            MethodInfo target = Assembly.Load(assemblyName).GetType(typeName).GetMethod(entryPoint);
            try
            {
                //Now invoke the method.
                bool retVal = (bool)target.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
                // When we print informations from a SecurityException extra information can be printed if we are 
                //calling it with a full-trust stack.
                (new PermissionSet(PermissionState.Unrestricted)).Assert();
                Console.WriteLine("SecurityException caught:\n{0}", ex.ToString());
                CodeAccessPermission.RevertAssert();
                Console.ReadLine();
            }
        }

        private Executor GetNewDomainInstance(AppDomain newDomain)
        {
            ObjectHandle handle = Activator.CreateInstanceFrom(
            newDomain, typeof(Executor).Assembly.ManifestModule.FullyQualifiedName,
            typeof(Executor).FullName
            );

            return (Executor)handle.Unwrap();
        }

        private AppDomain GetAppDomain(string dllPath, string name, List<SecurityPermissionFlag> permissionFlags)
        {
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(dllPath);

            var permissionSet = GetPermissionSet(permissionFlags);
            StrongName fullTrustAssembly = GetAssemblyStrongName();

            return AppDomain.CreateDomain(name, null, adSetup, permissionSet, fullTrustAssembly);
        }

        private StrongName GetAssemblyStrongName()
        {
            return typeof(Executor).Assembly.Evidence.GetHostEvidence<StrongName>();
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
