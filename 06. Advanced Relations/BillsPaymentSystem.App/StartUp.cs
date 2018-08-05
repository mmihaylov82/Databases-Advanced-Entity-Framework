using System;

using BillsPaymentSystem.Data;
using BillsPaymentSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BillsPaymentSystem.Data.Models.Enums;

namespace BillsPaymentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();

                context.Database.EnsureCreated();

                Seed(context);

                Console.Write($"Enter an user ID [{(context.Users.Any() ? 1 : 0)} - " +
                    $"{context.Users.Last().UserId}]: ");
                int userId = int.Parse(Console.ReadLine());

                var isUserFound = context.Users.Find(userId) != null;

                if (isUserFound)
                {
                    var user = context.Users
                      .Where(u => u.UserId == userId)
                      .Select(u => new
                      {
                          Name = $"{u.FirstName} {u.LastName}",
                          BankAccounts = u.PaymentMethods
                              .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                              .Select(pm => pm.BankAccount)
                              .ToList(),
                          CreditCards = u.PaymentMethods
                              .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                              .Select(pm => pm.CreditCard)
                              .ToList()
                      })
                      .FirstOrDefault();

                    Console.WriteLine($"User: {user.Name}");

                    if (user.BankAccounts.Count > 0)
                    {
                        Console.WriteLine($"Bank Accounts: {Environment.NewLine}{String.Join(Environment.NewLine, user.BankAccounts)}");
                    }

                    if (user.CreditCards.Count > 0)
                    {
                        Console.WriteLine($"Credit Cards: {Environment.NewLine}{String.Join(Environment.NewLine, user.CreditCards)}");
                    }
                }
                else
                {
                    Console.WriteLine($"User with id {userId} not found!");
                }
            }

            PayBills();
        }

        public static void PayBills()
        {
            using (var context = new BillsPaymentSystemContext())
            {
                try
                {
                    Console.WriteLine("Bills Payment");
                    Console.Write("Enter user ID: ");
                    int userId = int.Parse(Console.ReadLine());
                    Console.Write("Enter amount: ");
                    decimal amount = decimal.Parse(Console.ReadLine());

                    var accounts = context.PaymentMethods
                        .Include(pm => pm.BankAccount)
                        .Where(pm => pm.UserId == userId && pm.Type == PaymentMethodType.BankAccount)
                        .Select(pm => pm.BankAccount)
                        .ToList();

                    var cards = context.PaymentMethods
                        .Include(pm => pm.CreditCard)
                        .Where(pm => pm.UserId == userId && pm.Type == PaymentMethodType.CreditCard)
                        .Select(pm => pm.CreditCard)
                        .ToList();

                    var moneyAvailable = accounts.Select(ba => ba.Balance).Sum() + cards.Select(cc => cc.LimitLeft).Sum();

                    if(moneyAvailable < amount)
                    {
                        throw new Exception("Insufficient Funds");
                    }

                    foreach (var account in accounts)
                    {
                        if (amount == 0 || account.Balance == 0)
                        {
                            continue;
                        }

                        decimal moneyInAccount = account.Balance;
                        if (moneyInAccount < amount)
                        {
                            account.Withdraw(moneyInAccount);
                            amount -= moneyInAccount;
                        }
                        else
                        {
                            account.Withdraw(amount);
                            amount -= amount;
                        }
                    }


                    foreach (var creditCard in cards)
                    {
                        if (amount == 0 || creditCard.LimitLeft == 0)
                        {
                            continue;
                        }

                        decimal limitLeft = creditCard.LimitLeft;
                        if (limitLeft < amount)
                        {
                            creditCard.Withdraw(limitLeft);
                            amount -= limitLeft;
                        }
                        else
                        {
                            creditCard.Withdraw(amount);
                            amount -= amount;
                        }
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void Seed(BillsPaymentSystemContext context)
        {
            var users = new[]
            {
                new User
                {
                    FirstName = "Miroslav",
                    LastName = "Ivanov",
                    Email = "miro12@abv.bg",
                    Password = "123"
                },

                new User
                {
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Email = "gosho@abv.com",
                    Password = "123"
                },

                new User
                {
                    FirstName = "Milen",
                    LastName = "Toshev",
                    Email = "milen@yahoo.com",
                    Password = "123"
                },

                new User
                {
                    FirstName = "Ivan",
                    LastName = "Popov",
                    Email = "ivan@abv.bg",
                    Password = "123"
                }
            };

            context.Users.AddRange(users);

            var creditCards = new[]
            {
                new CreditCard(new DateTime(2018, 6, 20), 15000.00m, 1500.00m),
                new CreditCard(new DateTime(2018, 6, 25), 20000m, 1800m),
                new CreditCard(new DateTime(2019, 7, 4), 15000m, 14000m),
                new CreditCard(new DateTime(2019, 2, 5), 16000m, 4500m)
            };

            context.CreditCards.AddRange(creditCards);

            var bankAccounts = new[]
            {
                new BankAccount(2455m, "Expresbank", "TGBHJKL"),
                new BankAccount(12000m, "Unicredit", "TBGINKFL"),
                new BankAccount(14000m, "UBB", "TBGDSK"),
                new BankAccount(8500m, "Raiffensen bank", "TBGFRF")
            };

            context.BankAccounts.AddRange(bankAccounts);

            var paymentMethods = new[]
            {
                new PaymentMethod
                {
                    User = users[0],
                    Type = PaymentMethodType.BankAccount,
                    BankAccount = bankAccounts[0]
                },

                new PaymentMethod
                {
                    User = users[0],
                    Type = PaymentMethodType.BankAccount,
                    BankAccount = bankAccounts[1]
                },

                new PaymentMethod
                {
                    User = users[0],
                    Type = PaymentMethodType.CreditCard,
                    CreditCard = creditCards[0]
                },

                new PaymentMethod
                {
                    User = users[1],
                    Type = PaymentMethodType.CreditCard,
                    CreditCard = creditCards[1]
                },

                new PaymentMethod
                {
                    User = users[2],
                    Type = PaymentMethodType.BankAccount,
                    BankAccount = bankAccounts[2]
                },

                new PaymentMethod
                {
                    User = users[2],
                    Type = PaymentMethodType.CreditCard,
                    CreditCard = creditCards[2]
                },

                new PaymentMethod
                {
                    User = users[2],
                    Type = PaymentMethodType.CreditCard,
                    CreditCard = creditCards[3]
                },

                new PaymentMethod
                {
                    User = users[3],
                    Type = PaymentMethodType.BankAccount,
                    BankAccount = bankAccounts[3]
                }
            };

            context.PaymentMethods.AddRange(paymentMethods);

            context.SaveChanges();
        }
    }
}
