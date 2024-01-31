using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi.Models
{
    public class Person
    {
        public string FullName { get => ToString(); }
        public uint Number => _number;
        public bool Present => _present;

        uint _number;
        string name;
        string surname;
        bool _present = true;

        public Person(string line)
        {
            string[] split = line.Split(' ');
            _number = uint.Parse(split[0]);
            name = split[1];
            surname = split[2];
        }

        public override int GetHashCode()
        {
            int res = _number.GetHashCode();
            res = (res * 397) ^ name.GetHashCode();
            res = (res * 397) ^ surname.GetHashCode();
            return res;
        }

        public override string ToString() => $"{_number} {name} {surname}";
    }
}
