using System;
using System.Security.Cryptography.X509Certificates;

namespace STATE
{
    internal class Program
    {
        static BankAccount? account = new BankAccount(50);

        static void Main(string[] args)
        {
            account.Deposit(10);
            account.Withdrow(100);

            try
            {
                account.Deposit(100);
            }
            catch
            {
                Console.WriteLine("Impossible de faire le retrait sur le compte négatif");
            }

            account.Deposit(100);
            account.Withdrow(10);

            account.Deposit(1000);
            account.Deposit(100);

            account.Withdrow(1100);




        }
    }
}
