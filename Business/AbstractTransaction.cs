using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NWBA.Data;

namespace NWBA
{
    public abstract class AbstractTransaction
    {
        public int TransactionID { get; }
        public char TransactionType { get; }
        public int AccountNumber { get; }
        public int DestinationAccountNumber { get; }
        public decimal Amount { get; }
        public string Comment { get; }
        public string TransactionTimeUtc { get; }

        protected AbstractTransaction(int accountNumber, int destinationAccountNumber, decimal amount, string comment)
        {
            AccountNumber = accountNumber;
            DestinationAccountNumber = destinationAccountNumber;
            Amount = amount;
            Comment = comment;
        }

        public abstract Task<string> ExecuteAsync(NwbaContext _context);
    }
}
