using System.ComponentModel.DataAnnotations.Schema;

namespace BSN.Commons.Test.Data
{
    public class Document
    {
        [ForeignKey("User")]
        public long Id { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
    }
}