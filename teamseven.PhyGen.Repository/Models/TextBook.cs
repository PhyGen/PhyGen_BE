using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Repository.Models
{
    public partial class Textbook
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } //  (ví dụ: Cánh Diều)

        public int GradeId { get; set; } 

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Grade Grade { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }

}
