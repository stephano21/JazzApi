using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.TRA
{
    [Table("GoalActivity",Schema ="TRA")]
    public class GoalActivity
    {
        [Key]
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int Counter { get; set; }
        public long ActivityId { get; set; }
        public long GoalId { get; set; }
        [ForeignKey("GoalId")]
        public virtual Goal Goal { get; set; }
        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }
    }
}
