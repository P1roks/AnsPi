using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi
{
    static class Utils
    {
        //djb2
        public static ulong GetStringHash(string str)
        {
            ulong hash = 5381;

            foreach(char c in str)
            {
                hash = (hash << 5) + hash + c;
            }

            return hash;
        }

        public static uint GetLuckyNumber() =>
            uint.Parse(File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "luckynumber")));

        public static void SetLuckyNumber(uint newLucky) =>
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "luckynumber"), newLucky.ToString());
    }
}
