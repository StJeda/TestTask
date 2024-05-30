using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTask.Domain.Models
{
    public class Document()
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public virtual ICollection<DocumentStatus> Statuses { get; set; } = new List<DocumentStatus>(); 
    }
}
