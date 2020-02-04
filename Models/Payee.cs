
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA.Models
{
    public class Payee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayeeID { get; set; }

        [Required, StringLength(50)]
        public string PayeeName { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(20)]
        public string State { get; set; }


        [StringLength(4)]
        public string PostCode { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

    }
}
