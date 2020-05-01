using System;
using System.Collections.Generic;

namespace CoderDeckServerlessPOC.Models
{
    public partial class Appointment
    {
        public long Id { get; set; }
        public string Appointment1 { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
    }
}
