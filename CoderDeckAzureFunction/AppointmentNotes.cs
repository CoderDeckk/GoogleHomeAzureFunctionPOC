using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CoderDeckServerlessPOC.Models;
using System.Linq;
using CoderDeckAzureFunction.Models;

namespace CoderDeckAzureFunction
{
    public static class Notes
    {
        [FunctionName("AppointmentNotes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string email = req.Query["email"];
            string notes = req.Query["notes"];
            log.LogInformation("Email Is" + req.Query["email"]);
            log.LogInformation("Notes Is" + req.Query["notes"]);

            Appointment appointment = null;
            using (CoderDeckPocContext dbContext = new CoderDeckPocContext())
            {
                 appointment = dbContext.Appointment.Where(w => w.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                log.LogInformation("Appointment data is"+JsonConvert.SerializeObject(appointment));
                if (appointment != null)
                {
                    AppointmentNotes appointmentNotes = new AppointmentNotes()
                    {
                        AppointmentId = appointment.Id,
                        Notes = notes,
                        CreatedBy = email,
                        CreatedOn=DateTime.Now
                    };

                   
                    dbContext.Add<AppointmentNotes>(appointmentNotes);
                    dbContext.SaveChanges();
                }
            }
            string webHookResponse = "You dont have any upcoming appointment";
            if (appointment != null)
            {
                webHookResponse = "Appointment notes added \n" + "You have added following notes \n" + notes;
            }
            return new ContentResult { Content = webHookResponse, ContentType = "application/json" };

        }
    }
}
