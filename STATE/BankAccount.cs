using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STATE
{
    internal abstract class BankAccountState
    {
        public decimal Balance { get; set;}
        public BankAccount Account { get; set;}
        public abstract void Deposit(decimal amount);
        public abstract void Withdrow(decimal amount);
    }

    internal class PositiveBankAccountState : BankAccountState
    {
        public PositiveBankAccountState(decimal balance,
                                        BankAccount account)
        {
            Balance = balance;
            Account = account;

        }

        public override void Deposit(decimal amount)
        {
            Console.WriteLine($"Dépot de {amount} ");
            Balance += amount;
            if(Balance > 100)
            {
                Account._state = new VIPAccountBankState(Account)
                {
                    Balance = Balance
                };
            }
        }

        public override void Withdrow(decimal amount)
        {

            Console.WriteLine($"Retrait de {amount}");
            Balance -= amount;
            if(Balance < 0)
            {
                Console.WriteLine($"Le compte passe en negatif : {Balance}");
                Account._state = new NegativeBankAccountState(Account)
                {
                    Balance = Balance
                };
            }
        }
    }


    internal class NegativeBankAccountState : BankAccountState
    {

        public NegativeBankAccountState(BankAccount account)
        {
            Account = account;

        }
        public override void Deposit(decimal amount)
        {
            Console.WriteLine($"Dépot sur compte négatif : {amount}");
            Balance += amount;
            if(Balance > 0)
            {
                Console.WriteLine($"Le compte revient positif");
                Account._state = new PositiveBankAccountState(Balance, Account)
                {
                    Balance = Balance
                };

            }
        }

        public override void Withdrow(decimal amount)
        {
            throw new CannotWithdrawOnNegativeBankAccountBalance();
        }
    }



    internal class VIPAccountBankState : BankAccountState
    {

        public VIPAccountBankState(BankAccount account)
        {
            Account = account;
        }


        public override void Deposit(decimal amount)
        {
            Console.WriteLine($" Dépot sur compte VIP : {amount} . Prime de 10%");
            Balance += amount * 1.1m;
            Console.WriteLine($"Nouveau sold : {Balance}");
        }

        public override void Withdrow(decimal amount)
        {
            Balance -= amount;
            if (Balance < 1000)
            {
                Console.WriteLine("Passage sous les 1000€, retour à un compte normal");
                Account._state = new PositiveBankAccountState(Balance, Account);
            }
        }
    }




    public class CannotWithdrawOnNegativeBankAccountBalance : Exception { }



    public class BankAccount
    {
        internal BankAccountState _state; 
        public decimal Balance => _state.Balance;

        public BankAccount(decimal amount)
        {
            _state = new PositiveBankAccountState(amount, this);
        }

        public void Withdrow(decimal amount)
        {
            _state.Withdrow(amount);
        }

        public void Deposit(decimal amount) 
        {
           _state.Deposit(amount);
        }
        
    }
}
