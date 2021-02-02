using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int AuthorID { get; set; }
        [ForeignKey("AuthorID")]
        public virtual Author A { get; set; }
    }
}