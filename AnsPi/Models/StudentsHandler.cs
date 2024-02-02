using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi.Models
{
    public class StudentsHandler: ObservableCollection<Student>
    {
        public string className = null;
        private ulong[] lastRolled = new ulong[3] { 0, 0, 0 };

        public uint GetRandomStudentNumber() => this[new Random().Next(Count)].Number;
        private string GetPath() => Path.Combine(FileSystem.AppDataDirectory, Path.ChangeExtension(className, ".txt"));

        public static string[] GetClasses() =>
        Directory.GetFiles(FileSystem.AppDataDirectory, "*.txt")
            .Select(Path.GetFileNameWithoutExtension)
            .ToArray();

        public bool ChangeClass(string className)
        {
            this.className = className;
            Clear();
            var (rolled, students) = GetStudents(className);
            if (students == null || rolled == null) return false;
            foreach(var student in students)
            {
                Add(student);
            }
            lastRolled = rolled;
            return true;
        }

        public void LoadAfterEdit(string lines)
        {
            Clear();
            foreach (var line in lines.Split('\r'))
            {
                try
                {
                    Add(new(line, SaveToFile));
                }
                catch
                {
                    continue;
                }
            }
            SaveToFile();
        }

        public Student RollStudent(uint luckyNumber)
        {
            var valid = this.Where
                (student => student.Number != luckyNumber &&
                student.Present && !lastRolled.Contains(student.GetDeterministicHashCode()));
            int chosen = new Random().Next(valid.Count());

            var chosenStudent = valid.ElementAt(chosen);
            UpdateLastRolled(chosenStudent.GetDeterministicHashCode());

            return chosenStudent;
        }


        private (ulong[], IEnumerable<Student>) GetStudents(string className)
        {
            string fileName = Path.ChangeExtension(className, ".txt");
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            if(File.Exists(filePath))
            {
                var lines = File.ReadLines(filePath);
                var lastRolled = lines.First().Split(' ').Select(ulong.Parse).ToArray();
                var students =  from line in lines.Skip(1) select new Student(line, SaveToFile);
                return (lastRolled, students);
            }
            else
            {
                File.Create(filePath);
                return (null,null);
            }
        }

        private void UpdateLastRolled(ulong inserted)
        {
            lastRolled[0] = lastRolled[1];
            lastRolled[1] = lastRolled[2];
            lastRolled[2] = inserted;

            var path = GetPath();
            var lines = File.ReadAllLines(path);
            lines[0] = string.Join(' ', lastRolled);
            File.WriteAllLines(path, lines);
        }

        private void SaveToFile()
        {
            if (className == null) return;
            using StreamWriter output = new(GetPath());
            output.WriteLine(string.Join(' ', lastRolled));
            foreach (var student in this)
            {
                output.WriteLine(student);
            }
        }
    }
}
