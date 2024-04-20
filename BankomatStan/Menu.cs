using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatStan
{
    class Menu
    {
        public string PrintLoginMenu()
        {
            Console.WriteLine("Witaj w Bankomacie zaloguj się aby uzyskać dostęp do konta.");
            Console.WriteLine("\t Podaj login");
            string login = Console.ReadLine();
            return login;
        }
        public string PrintPasswordMenu()
        {
            Console.WriteLine("\t Podaj hasło");
            string password = Console.ReadLine();
            return password;
        }
        //public void PrintUserMenu(bool loginStat)
        //{
        //    if (loginStat == true)
        //    {
        //        Console.WriteLine("Witaj");
        //        Console.WriteLine("\t 1. Stan konta. \n\t 2.Dokonaj wyplaty. \n\t 3.Wyloguj. ");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Błędne dane logowania");
        //    }
        //}
    }
}
