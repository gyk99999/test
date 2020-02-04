using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA.Models
{
    public enum AccountType
    {
        Checking = 'C',
        Saving = 'S'
    }

    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Display(Name = "Type")]
        public char AccountType { get; set; }
        [Required, StringLength(4), RegularExpression(@"^[0-9] {4}$")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
    }
}
