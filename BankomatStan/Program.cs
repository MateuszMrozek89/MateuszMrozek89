using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankomatStan
{
    class Program
    {
        static void Main(string[] args)
        {
            Bankomat myBankomat = new Bankomat();
            myBankomat.Log_In();
            Console.ReadKey();
        }
    }
}
