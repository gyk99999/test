using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NWBA.Data;
using NWBA.Models;

namespace NWBA
{
    public class DepositTransaction : AbstractTransaction
    {
        public DepositTransaction(int accountNumber, int destinationAccountNumber, decimal amount, string comment) : base(accountNumber, destinationAccountNumber, amount, comment)
        {
        }

        public override async Task<string> ExecuteAsync(NwbaContext _context)
        {
            var account =await _context.Accounts.FindAsync(AccountNumber);
            
            account.Balance = account.Balance + Amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = (char)Models.TransactionType.Deposit,
                    Amount = Amount,
                    Comment=Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                }) ;

             _context.SaveChanges();
            return "true";
        }
    }
}
