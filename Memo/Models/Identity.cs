using System.ComponentModel.DataAnnotations;

namespace Memo.Models
{
    public abstract class Identity
    {
        [Key]
        public long Id { get; set; }
    }
}