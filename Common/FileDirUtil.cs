using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileDirUtil
    {
        // Poziva metodu IsPathValid koja proverava validnost unesene (zadate) putanje, ukoliko je validna poziva se CreateDirIfNotExists metoda
        public static void CheckCreatePath(string path)
        {
            if (!IsPathValid(path))
            {
                throw new CustomException($"Invalid path: {path}");
            }
            CreateDirIfNotExists(path);
        }

        // Proverava da li je unesena putanja validna
        public static bool IsPathValid(string path)
        {
            return Path.IsPathRooted(path);
        }

        // Nakon sto je validnost putanje potvrdjena proverava se da li postoji ili treba da se kreira direktorijum
        public static bool CreateDirIfNotExists(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        // Uzima sve fajlove iz foldera koji se zavrsavaju sa .csv
        public static string[] GetAllFiles(string path)
        {
            return Directory.GetFiles(path, "*.csv", SearchOption.TopDirectoryOnly);
        }
    }
}
