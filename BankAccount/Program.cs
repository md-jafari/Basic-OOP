using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Inlämningsuppgift_3
{
    public class Account
    {
        public string Name;
        public decimal Balance;

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }
        public void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
            }
            else
            {
                throw new ArgumentException("Balance too low to withdraw");
            }

        }
        public void Transfer(Account toAccount, decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                toAccount.Balance += amount;
            }
            else
            {
                throw new ArgumentException("Balance too low to transfer");
            }
        }   }


    public class Share
    {
        public string Company;
        public int Amount;
        public decimal Price;

        public void ShowUserInfoShare()
        {
            Console.WriteLine($"Company: {Company} \nAmount: {Amount}kr \nPrice: {Price} kr");
        }

        public void BuyShare(int amount, Account account)
        {
            decimal totalPrice = Price * amount;
            if (account.Balance >= totalPrice)
            {
                account.Balance -= amount;
                Amount += amount;
            }
            else
            {
                throw new ArgumentException("Balance too low to buy shares");
            }

        }

        public void SellShare(int amount, Account account)
        {
            if (amount <= Amount)
            {
                decimal totalPrice = Price * amount;
                account.Balance += totalPrice;
                Amount -= amount;

            }
            else
            {
                throw new ArgumentException("Number of shares too low to sell");
            }
        }

       
    }


    public class Bank
    {
        // These two variables contain the user's accounts and shares.
        // They are static and so will be available automatically to all methods in this class.
        public static Account[] accounts =
        {
            new Account { Name = "Spar", Balance = 90000 },
            new Account { Name = "Kort", Balance = 5000 },
            new Account { Name = "Resor", Balance = 22000 }
        };

        public static Share[] shares =
        {
            new Share { Company = "Ericsson", Price = 72, Amount = 20 },
            new Share { Company = "H&M", Price = 129, Amount = 50 },
            new Share { Company = "AstraZeneca", Price = 713, Amount = 5 }
        };

        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            bool done = false;
            while (!done)
            {
                ShowUserInfo();
                Console.WriteLine();

                int option = ShowMenu("What do you want to do?", new[]
                {
                    "Deposit",
                    "Withdraw",
                    "Transfer",
                    "Buy shares",
                    "Sell shares",
                    "Exit"
                });
                Console.Clear();

                // Call one of the "Page" methods based on which option the user picks.
                // If one of the methods throws an exception, just show the error message and keep going.
                try
                {
                    if (option == 0)
                    {
                        Console.Write("How much deposit?");
                        decimal depositAmount = decimal.Parse(Console.ReadLine());
                        accounts[0].Deposit(depositAmount);
                    }
                    else if (option == 1)
                    {
                        WithdrawPage();
                    }
                    else if (option == 2)
                    {
                        TransferPage();
                    }
                    else if (option == 3)
                    {
                        BuySharePage();
                    }
                    else if (option == 4)
                    {
                        SellSharePage();
                    }
                    else if (option == 5)
                    {
                        done = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an error: " + e.Message);
                }

                Console.WriteLine();
            }

            public static void ShowUserInfo()
            {
                Console.WriteLine("Your accounts:");
                foreach (Account account in accounts)
                {
                    Console.WriteLine("- " + account.Name + " (" + account.Balance + " kr)");
                }
                Console.WriteLine();

                Console.WriteLine("Your shares:");
                foreach (Share share in shares)
                {
                    Console.WriteLine("- " + share.Company + " (" + share.Amount + " at " + share.Price + " kr)");
                }
            }

            public static void DepositPage()
            {
                int accountIndex = ShowAccountMenu("Select account to deposit into:");
                Account account = accounts[accountIndex];
                Console.WriteLine();

                Console.Write("Select amount to deposit: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                Console.Clear();
                Deposit(account, amount);
                Console.WriteLine(amount + " kr deposited into " + account.Name);
            }

            public static void WithdrawPage()
            {
                int accountIndex = ShowAccountMenu("Select account to withdraw from:");
                Account account = accounts[accountIndex];

                Console.Write("Select amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                Console.Clear();
                Withdraw(account, amount);
                Console.WriteLine(amount + " kr withdrawn from " + account.Name);
            }

            public static void TransferPage()
            {
                int fromIndex = ShowAccountMenu("Select account to transfer from:");
                Account fromAccount = accounts[fromIndex];

                int toIndex = ShowAccountMenu("Select account to transfer to:");
                Account toAccount = accounts[toIndex];

                Console.Write("Select amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                Console.Clear();
                Transfer(fromAccount, toAccount, amount);
                Console.WriteLine(amount + " kr transfered from " + fromAccount.Name + " to " + toAccount.Name);
            }

            public static void BuySharePage()
            {
                int shareIndex = ShowShareMenu("Select share to buy:");
                Share share = shares[shareIndex];

                Console.Write("Select amount to buy: ");
                int shareAmount = int.Parse(Console.ReadLine());

                int accountIndex = ShowAccountMenu("Select account to buy with:");
                Account account = accounts[accountIndex];

                Console.Clear();
                BuyShare(share, shareAmount, account);
                Console.WriteLine("Bought " + shareAmount + " shares of " + share.Company + " with account " + account.Name);
            }
        }

        [TestClass]
        public class ProgramTests
        {
            [TestMethod]
            public void ExampleTest()
            {
                using FakeConsole console = new FakeConsole("First input", "Second input");
                Program.Main();
                Assert.AreEqual("Hello!", console.Output);
            }
        }
    
}