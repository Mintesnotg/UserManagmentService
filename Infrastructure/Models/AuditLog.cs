using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Enums.Enumerators;

namespace Infrastructure.Models
{
    public class AuditLog
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string TimeZoneInfo { get; set; } = string.Empty;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredDate { get; set; }
        public string RegisteredBy { get; set; } = string.Empty;
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public RecordStatus RecordStatus { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
