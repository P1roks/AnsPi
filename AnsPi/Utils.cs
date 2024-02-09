using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnsPi
{
    static class Utils
    {
        static Utils()
        {
            var dirPath = Path.Combine(FileSystem.AppDataDirectory, "lucky");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                var f = File.Create(Path.Combine(dirPath, "lucky.txt"));
                f.WriteByte(48); // 48 = 0
                f.Close();
            }
        }
           

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
            uint.Parse(File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, "lucky/lucky.txt")));

        public static void SetLuckyNumber(uint newLucky) =>
            File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "lucky/lucky.txt"), newLucky.ToString());
    }
}
