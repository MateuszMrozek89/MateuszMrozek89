using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatStan
{
    class LogOut : IStan
    {
        Bankomat Context;

        //Bankomat IStan.Kontekst { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LogOut(Bankomat b)
        {
            Context = b;
        }
        public void Log_Out()
        {
            Console.WriteLine("Już jesteś wylogowany");
        }
        public void Log_In()
        {
            LoginVerification user = new LoginVerification();
            Menu userMenu = new Menu();
            user.CheckData();
            user.CheckLoginData(userMenu.PrintLoginMenu(), userMenu.PrintPasswordMenu());
            Context.GetStan(new LogIn(Context));
        }
        public int EnterAmount(int x)
        {
            Console.WriteLine("Musisz być zalogowany dla wykonania tej operacji !!!");
            return x;
        }
        public void CollectCash()
        {
            Console.WriteLine("Musisz być zalogowany dla wykonania tej operacji !!!");
        }

    }
}
