using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatStan
{
    class LogIn : IStan
    {
        Bankomat Context;

        //Bankomat IStan.Kontekst { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LogIn(Bankomat b)
        {
            Context = b;
        }
        public void Log_Out()
        {
            Console.WriteLine("Ok, wylogowano");
            Context.GetStan(new LogOut(Context));
        }
        public void Log_In()
        {
            Console.WriteLine("Jesteś juz zalogownay");
        }
        public int EnterAmount(int x)
        {
            Console.WriteLine("Ok, zaraz wydam gotówkę w kwocie {0} PLN",x);
            Context.GetStan(new IssueCash(Context));
            return x;
        }
        public void CollectCash()
        {
            Console.WriteLine("Nie podano kwoty!!!");
        }
    }
}
