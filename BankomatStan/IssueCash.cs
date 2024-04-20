using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatStan
{
    class IssueCash : IStan
    {
        Bankomat Context;

        public IssueCash(Bankomat b)
        {
            Context = b;
        }

        //Bankomat IStan.Kontekst { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //
        public int EnterAmount(int x)
        {
            Console.WriteLine("Już podano kwote {0} !!!",x);
            return x;
        }
        public void Log_Out()
        {
            Console.WriteLine("Najpierw wejź kase !!!");
        }

        public void CollectCash()
        {
            Console.WriteLine("Oto Twoje pieniądze !!!");
            Context.GetStan(new LogOut(Context));
        }

        public void Log_In()
        {
            Console.WriteLine("Jesteś już zalogowany");
        }
    }
}
