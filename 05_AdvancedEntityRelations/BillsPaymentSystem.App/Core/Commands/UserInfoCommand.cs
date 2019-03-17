using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BillsPaymentSystem.App.Core.Commands.Contracts;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class UserInfoCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;
        public UserInfoCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            int userId = int.Parse(args[0]);

            var user = this.context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)    
            {
                throw new ArgumentNullException($"User with id {userId} not found!");
            }

            return GetResult(user);
        }

        private string GetResult(User user)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"User: {user.FirstName} {user.LastName}");
            result.AppendLine("Bank Accounts:");
            foreach (var paymentMethod in this.context.PaymentMethods.Where(p => p.UserId == user.UserId && p.BankAccountId != null))
            {
                BankAccount bankAccount =
                    this.context.BankAccounts.FirstOrDefault(b => b.BankAccountId == paymentMethod.BankAccountId);
                result.AppendLine($"-- ID: {bankAccount.BankAccountId}");
                result.AppendLine($"--- Balance: {bankAccount.Balance}");
                result.AppendLine($"--- Bank: {bankAccount.BankName}");
                result.AppendLine($"--- SWIFT: {bankAccount.SWIFTCode}");
            }
            result.AppendLine("Credit Cards:");
            foreach (var paymentMethod in this.context.PaymentMethods.Where(p => p.UserId == user.UserId && p.CreditCardId != null))
            {
                CreditCard creditCard =
                    this.context.CreditCards.FirstOrDefault(c => c.CreditCardId == paymentMethod.CreditCardId);
                result.AppendLine($"-- ID: {creditCard.CreditCardId}");
                result.AppendLine($"--- Limit: {creditCard.Limit}");
                result.AppendLine($"--- Money Owed: {creditCard.MoneyOwed}");
                result.AppendLine($"--- Limit Left: {creditCard.LimitLeft}");
                result.AppendLine($"--- Expiration Date: {creditCard.ExpirationDate}");
            }

            return result.ToString().TrimEnd();
        }
    }
}
