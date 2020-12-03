using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSN.Commons.Tests
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(7)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Document Document { get; set; }
        public string Password { get; set; }
    }
}