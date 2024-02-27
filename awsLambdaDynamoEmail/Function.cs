using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace awsLambdaDynamoEmail
{
    public class Function
    {
        /// <summary>
        /// Prosta funkcja reaguj¹ca na dodanie nowych danych do DynamoDB
        /// Wykonujemy prost¹ analizê + wys³anie wiadomoœci email
        /// </summary>
        /// <param name="dynamoEvent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
        {
            context.Logger.LogLine($"Beginning to process {dynamoEvent.Records.Count} records...");
            StringBuilder sb = new StringBuilder();

            foreach (var record in dynamoEvent.Records)
            {
                context.Logger.LogLine($"Event ID: {record.EventID}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                // TODO: Dodaæ logikê biznesow¹ przetwarzania obiektu (w ramach Waszych testów)
                // W naszym przypadku pobieramy identyfikator klucza oraz jego wartoœæ
                foreach (var data in record.Dynamodb.NewImage)
                {
                    context.Logger.LogLine($"Key: {data.Key}");

                    // Dynamiczne mapowanie u¿ywane w DynamoDB
                    // N jest symbolem pola numerycznego
                    // S jest symbolem pola tekstowego
                    if (data.Value.N != null)
                    {
                        context.Logger.LogLine($"Value: {data.Value.N}");
                        sb.AppendLine($"Klucz: {data.Key}, Wartosc: {data.Value.N}");
                    }
                    else
                    {
                        context.Logger.LogLine($"Value: {data.Value.S}");
                        sb.AppendLine($"Klucz: {data.Key}, Wartosc: {data.Value.S}");
                    }
                }
            }

            // Ustawcie adres email zweryfikowany w obrêbie konkretnego regionu
            string senderAddress = "masterchariot007@gmail.com";
            // Pamiêtajcie o ustawieniu regionu w którym dokonaliœcie weryfikacji adresu email
            // W przeciwnym wypadku zobaczycie poni¿szy b³¹d:
            // Email address is not verified.
            // The following identities failed the check in region EU-WEST-2: adres_email@gmail.com
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUNorth1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { "masterchariot007@gmail.com" }
                    },
                    Message = new Message
                    {
                        Subject = new Content("Wpisz temat wysy³anej wiadomoœci"),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = sb.ToString()
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = "body"
                            }
                        }
                    }
                };
                var response = await client.SendEmailAsync(sendRequest);
            }

            context.Logger.LogLine("Stream processing complete.");
        }
    }
}
