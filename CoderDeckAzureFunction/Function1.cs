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
using Google.Protobuf;
using Google.Cloud.Dialogflow.V2;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
namespace CoderDeckAzureFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string email = req.Query["email"];
            log.LogInformation("Email Is" + req.Query["email"]);

            Appointment appointment = null;
            using (CoderDeckPocContext dbContext = new CoderDeckPocContext())
            {
                appointment = dbContext.Appointment.Where(w => w.Email.ToString().Equals(email, StringComparison.InvariantCultureIgnoreCase) && w.AppointmentDate > DateTime.Now).OrderBy(o => o.AppointmentDate).FirstOrDefault();
            }
            string webHookResponse = "You dont have any future appointment.Please check with the advisor";
            if (appointment != null)
            {
                webHookResponse = appointment.Appointment1 + " at" + appointment.AppointmentDate+"\n"+"Do you want to add note to this appointment?";
            }
                return new ContentResult { Content = webHookResponse, ContentType = "application/json" };

        }
    }
}

