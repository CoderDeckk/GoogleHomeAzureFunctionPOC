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

            string name = req.Query["name"];
            Appointment appointment = null;
            using (CoderDeckPocContext dbContext = new CoderDeckPocContext())
            {
                appointment = dbContext.Appointment.Where(w => w.Email.Equals(name, StringComparison.InvariantCultureIgnoreCase) && w.AppointmentDate > DateTime.Now).FirstOrDefault();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new JsonResult(appointment)
                : new BadRequestObjectResult(@"Please pass a name on the query string or in the request body");
        }
    }
}
