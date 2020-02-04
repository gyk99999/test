using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA.Models
{
    public class Customer
    {
        // d a dsd sa d
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, StringLength(50), RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(4), RegularExpression(@"^[0-9] {4}$")]
        public string PostCode { get; set; }

        [StringLength(15), RegularExpression(@"^[0-9]*$")]
        public string Phone { get; set; }

        [StringLength(11), RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string TFN { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }
}
// fdar ewqr eqywqt weqtwetqwe t tew   gfsahteqw tqw