using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi.Models
{
    public class Student
    {
        public string FullName { get => ToString(); }
        public uint Number => _number;
        public bool Present {
            get => _present;
            set => _present = value;
        }

        uint _number;
        string name;
        string surname;
        bool _present = true;

        public Student(string line)
        {
            string[] split = line.Split(' ');
            if (split.Length != 3)
                throw new ArgumentException("Provided line is not a valid student representation!");
            _number = uint.Parse(split[0]);
            name = split[1];
            surname = split[2];
        }

        public ulong GetDeterministicHashCode()
        {
            ulong res = _number;
            res = (res * 397) ^ Utils.GetStringHash(name);
            res = (res * 397) ^ Utils.GetStringHash(surname);
            return res;
        }

        public override string ToString() => $"{_number} {name} {surname}";
    }
}
