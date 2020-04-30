using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using System;
using System.IO;

namespace WebHookProject
{
    public class Class1
    {
        public string getResponse(StreamReader reader)
        {
            JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
            //    log.LogInformation(req.Query["name"]);
            WebhookRequest request;
            //    using (var reader = new StreamReader(req.Body))
            //    {
            request = jsonParser.Parse<WebhookRequest>(reader);

            var pas = jsonParser.Parse<WebhookRequest>(reader).QueryResult.Parameters;
            var na1 = pas.Fields["name"].ToString();
            //        log.LogInformation("na1{0}", na1);
            //    }
            return na1;
        }
    }
}
