using AnsPi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi
{
    internal static class Storage
    {
        public static string[] GetClasses() =>
        Directory.GetFiles(FileSystem.AppDataDirectory, "*.txt")
            .Select(Path.GetFileNameWithoutExtension)
            .ToArray();

        public static IEnumerable<Student> GetStudents(string className)
        {
            string fileName = Path.ChangeExtension(className, ".txt");
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            if(File.Exists(filePath))
            {
                var lines = File.ReadLines(filePath);
                return from line in lines select new Student(line);
            }
            else
            {
                return Enumerable.Empty<Student>();
            }
        }

        public static void AddClass(string className)
        {
            string fileName = Path.ChangeExtension(className, ".txt");
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            if(!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }
    }
}
