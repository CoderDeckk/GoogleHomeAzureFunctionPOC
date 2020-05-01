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
//using Google.Cloud.Dialogflow.V2;
//using Google.Protobuf;

namespace CoderDeckAzureFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
            string name = string.Empty;
            
            var response = req.Content.ReadAsStringAsync().Result;
            log.LogInformation("Got it" + response);
            var myParameters= response.ToString().Substring(response.ToString().IndexOf("parameters"));

            log.LogInformation("My Parameters {0}", myParameters);
              myParameters = myParameters.Substring(0, myParameters.IndexOf("}")+1);
            log.LogInformation("My Exact Parameters {0}", myParameters);
            myParameters = myParameters.Substring(myParameters.IndexOf("{"));
            var parametersObject = JsonConvert.DeserializeObject<MyRequestBody>(myParameters);
            log.LogInformation("Final name"+ parametersObject.name);
            
           
            Appointment appointment = null;
            using (CoderDeckPocContext dbContext = new CoderDeckPocContext())
            {
                appointment = dbContext.Appointment.Where(w => w.UserId.ToString().Equals(parametersObject.name, StringComparison.InvariantCultureIgnoreCase) && w.AppointmentDate > DateTime.Now).OrderBy(o => o.AppointmentDate).FirstOrDefault();
            }
            var dialogflowResponse = new WebhookResponse
            {
                FulfillmentText = $"{ appointment?.Appointment1 } at { appointment?.AppointmentDate }",
                FulfillmentMessages =
                { new Intent.Types.Message
                    { SimpleResponses = new Intent.Types.Message.Types.SimpleResponses
                        { SimpleResponses_ =
                            { new Intent.Types.Message.Types.SimpleResponse
                                {
                                   DisplayText = $"{ appointment?.Appointment1} at {appointment?.AppointmentDate}",
                                   TextToSpeech = $"{ appointment?.Appointment1} at {appointment?.AppointmentDate}",
                                   Ssml = $"<speak>{appointment?.Appointment1}</speak>"
                                }
                            }
                        }
                    }
            }
            };

            //Response responseObject = new Response()
            //{
            //    textToSpeech = $"{ appointment.Appointment1} at {appointment.AppointmentDate}",
            //    displayText = $"{ appointment.Appointment1} at {appointment.AppointmentDate}",
            //    ssml = string.Empty
            //};


            //    return req.CreateResponse(HttpStatusCode.OK,responseObject);
            var jsonResponse = dialogflowResponse.ToString();
            return new ContentResult { Content = jsonResponse, ContentType = "application/json" };

        }
    }
    public class MyRequestBody
    {
        public string name { get; set; }
      
    }
    public class Response
    {
        public string textToSpeech { get; set; }
        public string displayText { get; set; }
        public string ssml { get; set; }
    }
}

