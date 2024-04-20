using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatStan
{
    class Bankomat : IStan
    {
        //public Bankomat Kontekst { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IStan Stan; //{ get; set; }
        public Bankomat()
        {

            Stan = new LogOut(this);

        }

        public void GetStan(IStan NewStan)
        {
            Stan = NewStan;

        }
        public void Log_Out()
        {
            Stan.Log_Out();
        }
        public void Log_In()
        {
            Stan.Log_In();
        }
        public int EnterAmount(int x)
        {
            Stan.EnterAmount(x);
            return x;
        }
        public void CollectCash()
        {
            Stan.CollectCash();
        }

    }
}
