using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class FileInUseCommonChecker : IFileInUseChecker
    {
        private const int TIMEOUT = 10;
        private const int WAIT_MILLISECONDS = 500;


        public bool IsFileInUse(string filePath)
        {
            int cnt = 0;
            while (cnt < TIMEOUT && CheckIsFileInUse(new FileInfo(filePath)))
            {
                Thread.Sleep(WAIT_MILLISECONDS);
                cnt++;
            }
            if (cnt >= TIMEOUT)
            {
                return true;
            }
            return false;
        }

        // Proverava da li je datoteka sa prosledjenom lokacijom trenutno zauzeta
        private bool CheckIsFileInUse(FileInfo file)
        {
            FileStream fs = null;
            try
            {
                fs = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // fajl nije dostupan jer se u njega i dalje upisuje ili se obradjuje od strane neke druge niti ili ne postoji
                return true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
            // fajl nije zakljucan
            return false;
        }
    }
}
