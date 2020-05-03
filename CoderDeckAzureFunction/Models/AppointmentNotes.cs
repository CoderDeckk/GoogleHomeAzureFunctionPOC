using System;
using System.Collections.Generic;
using System.Text;

namespace CoderDeckAzureFunction.Models
{
   public  class AppointmentNotes
    {
        public long Id { get; set; }
        public long AppointmentId { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
