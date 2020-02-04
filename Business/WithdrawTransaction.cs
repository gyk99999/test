using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NWBA.Data;
using NWBA.Models;

namespace NWBA
{
    public class WithdrawTransaction : AbstractTransaction
    {
        private const decimal fee = (decimal)0.1;
        public WithdrawTransaction(int accountNumber, int destinationAccountNumber, decimal amount, string comment):base(accountNumber,destinationAccountNumber, amount, comment)
        {

        }
        public override async Task<string> ExecuteAsync(NwbaContext _context)
        {
            decimal transferFee = 0;
            decimal miniBalance = 0;
            var account = await _context.Accounts.FindAsync(AccountNumber);
            if (account.Transactions.Count >= 4)
            {

                transferFee = fee;
            }
            if (account.AccountType == 'C')
            {
                miniBalance = 200;
            }
            if (account.Balance < Amount + transferFee + miniBalance)
            {
                return "Blance is not enough";
            }
            account.Balance = account.Balance - Amount - fee;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = (char)Models.TransactionType.Withdraw,
                    Amount = Amount,
                    Comment=Comment,
                    TransactionTimeUtc = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();
            return "true";
        }

    }
}
