using System.ComponentModel.DataAnnotations;

namespace NWBA.Models
{
    public class Login
    {
        [StringLength(8)]
        public string LoginID { get; set; }
        [Required, StringLength(4), RegularExpression(@"^[0-9] {4}$")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }
    }
}
