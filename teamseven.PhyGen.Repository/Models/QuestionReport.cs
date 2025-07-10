using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Repository.Models
{
    public partial class QuestionReport
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int ReportedByUserId { get; set; }

        [Required, MaxLength(1000)]
        public string Reason { get; set; } 
        public DateTime ReportDate { get; set; }
        public bool IsHandled { get; set; }

        public virtual User ReportedByUser { get; set; }

        public virtual Question Question { get; set; }
    }
}
