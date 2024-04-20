using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankomatStan
{
    class LoginVerification
    {
     
        static string[] LoginTab;
        static string[] PasswordTab;
        static float[] BalanceTab;
        public int ID;
     
        public void CheckData()
        {

            List<string> tabLogin = new List<string>();
            List<string> tabPassword = new List<string>();
            List<string> tabBalance = new List<string>();
            List<float> tabCash = new List<float>();
            StreamReader sr = new StreamReader("LoginData.txt");
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                string[] words = s.Split('\t');
                tabLogin.Add(words[0]);
                tabPassword.Add(words[1]);
                tabBalance.Add(words[2]);
            }
            sr.Close();
            
            LoginTab = tabLogin.ToArray();
            PasswordTab = tabPassword.ToArray();
            foreach (var x in tabBalance)
            {
                float y;
                y = float.Parse(x);
                tabCash.Add(y);
            }
            BalanceTab = tabCash.ToArray();
        }
        public void SaveData()
        {
            File.WriteAllText("LoginData.txt", string.Empty);

            StreamWriter sw = new StreamWriter("LoginData.txt", true);
            for (int i = 0; i < BalanceTab.Length; i++)
            {
                sw.WriteLine(LoginTab[i] + "\t" + PasswordTab[i] + "\t" + BalanceTab[i]);
            }
            sw.Close();
        }
        public bool CheckLoginData(string login, string password)
        {
            for (int i = 0; i < LoginTab.Length; i++)
            {
                for (int j = 0; j < PasswordTab.Length; j++)
                {
                    if (LoginTab[i] == login && PasswordTab[j] == password)
                    {
                        ID += i;
                        Console.WriteLine("\tWitaj {0}. \n\tPoprawnie zalogowano.", LoginTab[i]);
                        CheckBalance();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Błędne dane logowania");
                    }
                }
            }
            return false;
        }
        
       public void CheckBalance()
        {
            Console.WriteLine("\n\tWprowadź kwotę do wypłaty.");
            float valueIssue = float.Parse(Console.ReadLine());
            float balance = BalanceTab[ID];
            if (valueIssue <= balance)
            {
                Console.WriteLine("\tWypłacam z Twojego konta {0} zł.",valueIssue);
                BalanceTab[ID] -= valueIssue;
                SaveData();
            }
            if (valueIssue >= balance)
            {
                Console.WriteLine("\tPodana kwota jest błędna.");
            }
       }       
    }
}
