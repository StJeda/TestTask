using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTask.Domain.Models
{
    public class DocumentStatus()
    {
        public int DocumentStatusId { get; set; }
        public int DocumentId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateTime { get; set; }
        [JsonIgnore]
        public virtual Document Document { get; set; } = new Document();
    }
}
