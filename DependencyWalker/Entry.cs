using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DependencyWalker
{
    public static class Entry
    {
        private static IList<string> GetLibraries(string path)
        {
            Console.WriteLine($"reading from {path}");
            var di = new DirectoryInfo(path);
            if (!di.Exists) return null;

            var fis = di.GetFiles("*.dll");
            return fis.Select(fi => fi.FullName).ToList();
        }

        private static string TrimFullName(string fullName)
        {
            var redundantParts = new string[] { ", Culture=neutral", ", PublicKeyToken=null"};
            return redundantParts.Aggregate(fullName, (current, parts) => current.Replace(parts, ""));
        }

        private static void PrintAssemblyInfo(Assembly asm)
        {
            var attr1 = asm.GetCustomAttribute<AssemblyFileVersionAttribute>();
            var attr2 = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var assemblyVersion = asm.GetName().Version!.ToString();
            var assemblyFileVersion = attr1?.Version ?? assemblyVersion;
            var assemblyInformationalVersion = attr2?.InformationalVersion ?? assemblyFileVersion;

            Console.Write($"main\t\t{TrimFullName(asm.GetName().FullName)}");
            Console.Write($"\t{assemblyVersion}");
            Console.Write($"\t{assemblyFileVersion}");
            Console.WriteLine($"\t{assemblyInformationalVersion}");
        }

        private static void GetReferences(Assembly asm)
        {
            var depends = asm.GetReferencedAssemblies();
            foreach (var depend in depends)
            {
                Console.Write($"\t{TrimFullName(asm.GetName().FullName)}");
                Console.Write($"\t{TrimFullName(depend.FullName)}");
                // Console.Write($"\t{depend.VersionCompatibility.ToString()}"); // SameMachine
                // Console.Write($"\t{depend.ProcessorArchitecture}");           // None
                Console.WriteLine($"\t{depend.Version}");
            }
        }

        private static void GetDependencies(string absoluteFileName, ref Dictionary<string, string> problems)
        {
            try
            {
                var asm = Assembly.LoadFrom(absoluteFileName);
                PrintAssemblyInfo(asm);
                GetReferences(asm);
            }
            catch (Exception e)
            {
                problems.Add(absoluteFileName, e.Message);
            }
        }
        
        public static void Main(string[] args)
        {
            var path = args?.Length > 0 ? args[0] : "../latest/";
            var dlls = GetLibraries(path);
            if (dlls == null || dlls.Count == 0)
            {
                Console.WriteLine($"nothing to process from {path}");
                return;
            }
            var problems = new Dictionary<string, string>();
            foreach (var dll in dlls)
            {
                GetDependencies(dll, ref problems);
            }
            Console.WriteLine("\n\n");
            foreach (var problem in problems)
            {
                Console.WriteLine($"{problem.Key} issue: {problem.Value}");
            }
        }
    }
}