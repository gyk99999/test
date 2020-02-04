using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA.Models
{
    public class BillPay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillPayID { get; set; }

        [ForeignKey("FK_BillPay_Account_AccountNumber")]
        public int AccountNumber { get; set; }

        [ForeignKey("FK_BillPay_Payee_PayeeID")]
        public int PayeeID { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public DateTime ScheduleDate { get; set; }

        public char Period { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
