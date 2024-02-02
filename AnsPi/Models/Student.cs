using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi.Models
{
    public class Student
    {
        public string FullName { get => $"{_number} {name} {surname}"; }
        public uint Number => _number;
        public bool Present {
            get => _present;
            set
            {
                if(value != _present)
                {
                    _present = value;
                    onPropertyChanged();
                }
            }
        }

        uint _number;
        string name;
        string surname;
        bool _present;
        private Action onPropertyChanged;

        public Student(string line, Action onPropertyChanged)
        {
            this.onPropertyChanged = onPropertyChanged;
            string[] split = line.Split(' ');
            if (split.Length < 3 || split.Length > 4)
                throw new ArgumentException("Provided line is not a valid student representation!");
            _number = uint.Parse(split[0]);
            name = split[1];
            surname = split[2];
            _present = split.Length == 3 || split[3] == "True";
        }

        public ulong GetDeterministicHashCode()
        {
            ulong res = _number;
            res = (res * 397) ^ Utils.GetStringHash(name);
            res = (res * 397) ^ Utils.GetStringHash(surname);
            return res;
        }

        public override string ToString() => $"{_number} {name} {surname} {_present}";
    }
}
