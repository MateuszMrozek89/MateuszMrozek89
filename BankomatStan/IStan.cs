using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatStan
{
    interface IStan
    {
        //Bankomat Kontekst
        //{
        //    get;
        //    set;
        //}
        void Log_Out();
        void Log_In();
        int EnterAmount(int x);
        void CollectCash();
    }
}
