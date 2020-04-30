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
//using Google.Cloud.Dialogflow.V2;
//using Google.Protobuf;

namespace CoderDeckAzureFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
            string name = string.Empty;
            WebhookRequest request;
            //using (var reader = new StreamReader(req.Body))
            //{
            //StreamReader sr =  new StreamReader(req.Content.ReadAsStreamAsync().Result);
            //log.LogInformation("My Body{0}", sr);
            string s = "";
            MyRequestBody body;
            var response = req.Content.ReadAsStringAsync().Result;
            log.LogInformation("Got it" + response);
            var myParameters= response.ToString().Substring(response.ToString().IndexOf("parameters"));

            log.LogInformation("My Parameters {0}", myParameters);
              myParameters = myParameters.Substring(0, myParameters.IndexOf("}")+1);
            log.LogInformation("My Exact Parameters {0}", myParameters);
            myParameters = myParameters.Substring(myParameters.IndexOf("{"));
            var myfinalResponse = JsonConvert.DeserializeObject<MyRequestBody>(myParameters);
            log.LogInformation("Final name"+myfinalResponse.name);
            
           
            Appointment appointment = null;
            using (CoderDeckPocContext dbContext = new CoderDeckPocContext())
            {
                appointment = dbContext.Appointment.Where(w => w.Email.Equals(myfinalResponse.name, StringComparison.InvariantCultureIgnoreCase) && w.AppointmentDate > DateTime.Now).OrderBy(o=>o.AppointmentDate).FirstOrDefault();
            }

            Response responseObject = new Response()
            {
                Source = "WebHook",
                Speech = $"{ appointment.Appointment1} at {appointment.AppointmentDate}",
                DisplayText = $"{ appointment.Appointment1} at {appointment.AppointmentDate}"
            };


                return req.CreateResponse(HttpStatusCode.OK,responseObject);
             
        }
    }
    public class MyRequestBody
    {
        public string name { get; set; }
      
    }
    public class Response
    {
        public string Speech { get; set; }
        public string DisplayText { get; set; }
        public string Source { get; set; }
    }
}

