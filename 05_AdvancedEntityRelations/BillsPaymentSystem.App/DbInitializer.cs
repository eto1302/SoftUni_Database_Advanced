using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using BillsPaymentSystem.Models.Enums;

namespace BillsPaymentSystem.App
{
    public class DbInitializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            SeedUsers(context);
            SeedCreditCards(context);
            SeedBankAccounts(context);
            SeedPaymentMethods(context);
        }

        private static void SeedUsers(BillsPaymentSystemContext context)
        {
            string[] firstNames = { "Gosho", "Pesho", "Ivan", "Kiro", null, "" };
            string[] lastNames = { "Goshev", "Peshev", "Ivanov", "Kirov", null, "" };
            string[] emails = { "gosho@abv.bg", "pesho@abv.bg", "ivan@abv.bg", "kiro@abv.bg", null, "ERROR" };
            string[] passwords = { "Gosho1", "Pesho1", "Ivan1", "Kiro1", null, "ERROR" };

            List<User> Users = new List<User>();

            for (int i = 0; i < firstNames.Length; i++)
            {
                var user = new User
                {
                    FirstName = firstNames[i],
                    Email = emails[i],
                    LastName = emails[i],
                    Password = emails[i],

                };

                if (!IsValid(user))
                {
                    continue;
                }
                Users.Add(user);
            }

            context.Users.AddRange(Users);
            context.SaveChanges();
        }

        private static void SeedCreditCards(BillsPaymentSystemContext context)
        {
            int[] limits = { 22, 23, 556, 66, 444, 54, 33 };
            int[] moneyOwed = { 11, 20, 222, 53, 333, 53, 20 };
            DateTime CommonExpirationDate = DateTime.Now;

            List<CreditCard> CreditCards = new List<CreditCard>();

            for (int i = 0; i < limits.Length; i++)
            {
                var creditCard = new CreditCard
                {
                    ExpirationDate = CommonExpirationDate,
                    Limit = limits[i],
                    MoneyOwed = moneyOwed[i]
                };
                if (!IsValid(creditCard)) continue;
                CreditCards.Add(creditCard);
            }


            context.CreditCards.AddRange(CreditCards);
            context.SaveChanges();
        }

        private static void SeedBankAccounts(BillsPaymentSystemContext context)
        {
            decimal[] balances = { 110.5m, 11.4m, 4123, 63745.65m, 3524.54m };
            string[] bankNames = { "BankName", "TestBankName", "SeedBankName", "NotFakedBankName", "RandomBankName" };
            string[] swiftCodes = { "code", "code1", "code2", "code3", "code4" };

            List<BankAccount> BankAccounts = new List<BankAccount>();

            for (int i = 0; i < balances.Length; i++)
            {
                var bankAccount = new BankAccount
                {
                    Balance = balances[i],
                    BankName = bankNames[i],
                    SWIFTCode = swiftCodes[i]
                };

                if (!IsValid(bankAccount))
                {
                    continue;
                }
                BankAccounts.Add(bankAccount);
            }



            context.BankAccounts.AddRange(BankAccounts);
            context.SaveChanges();
        }

        private static void SeedPaymentMethods(BillsPaymentSystemContext context)
        {
            var paymentMethods = new List<PaymentMethod>();

            for (int i = 0; i < 8; i++)
            {
                var paymentMethod = new PaymentMethod
                {
                    UserId = new Random().Next(1, 5),
                    Type = (PaymentType)new Random().Next(0, 2)
                };

                if (i % 3 == 0)
                {
                    paymentMethod.CreditCardId = 1;
                    paymentMethod.BankAccountId = 1;
                }

                else if (i % 2 == 0)
                {
                    paymentMethod.CreditCardId = 1;
                }

                else
                {
                    paymentMethod.BankAccountId = 1;
                }

                if (!IsValid(paymentMethod))
                {
                    continue;
                }

                paymentMethods.Add(paymentMethod);
                
            }
            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            return isValid;
        }
    }
}
