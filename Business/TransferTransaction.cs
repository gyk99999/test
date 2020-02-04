using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NWBA.Data;
using NWBA.Models;

namespace NWBA
{
    public class TransferTransaction : AbstractTransaction
    {
        private const decimal fee = (decimal)0.2;
        public TransferTransaction(int accountNumber, int destinationAccountNumber, decimal amount, string comment) : base(accountNumber, destinationAccountNumber, amount, comment)
        {

        }
        public override async Task<string> ExecuteAsync(NwbaContext _context)
        {
            var fromAccount = await _context.Accounts.FindAsync(AccountNumber);
            var toAccount = await _context.Accounts.FindAsync(DestinationAccountNumber);
            decimal transferFee = 0;
            decimal miniBalance = 0;
            if(fromAccount.Transactions.Count>=4)
            {

                transferFee = fee;
            }
            if(fromAccount.AccountType=='C')
            {
                miniBalance = 200;
            }
            if (fromAccount.Balance < Amount + transferFee+ miniBalance)
            {
                return "Blance is not enough";
            }
            fromAccount.Balance = fromAccount.Balance - Amount - fee;

            toAccount.Balance = toAccount.Balance + Amount;
            Transaction transaction = new Transaction
            {
                TransactionType = (char)Models.TransactionType.Transfer,
                Amount = Amount,
                Comment = Comment,
                AccountNumber = AccountNumber,
                DestinationAccountNumber=DestinationAccountNumber,
                TransactionTimeUtc = DateTime.UtcNow
            };
            fromAccount.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return "true";
        }
    }
}
