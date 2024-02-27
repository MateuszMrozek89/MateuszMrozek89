using System;
using System.Text;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]



// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaSMSNotification
{
    public class Function
    {
        public async Task FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
        {
            context.Logger.LogLine($"Beginning to process {dynamoEvent.Records.Count} records...");
            StringBuilder sb = new StringBuilder();

            foreach (var record in dynamoEvent.Records)
            {
                context.Logger.LogLine($"Event ID: {record.EventID}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                // TODO: Dodaæ logikê biznesow¹ przetwarzania obiektu
                // W naszym przypadku pobieramy identyfikator klucza oraz jego wartoœæ
                foreach (var data in record.Dynamodb.NewImage)
                {
                    context.Logger.LogLine($"Key: {data.Key}");

                    // Dynamiczne mapowanie u¿ywane w DynamoDB
                    // N jest symbolem pora numerycznego
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

            await Task.CompletedTask;

            // Wymagana paczka: Amazon.SimpleNotificationService
            var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.EUNorth1);

            // Reqest zawiera treœæ wiadomoœci oraz numer telefonu
            var request = new PublishRequest
            {
                Message = sb.ToString(),
                PhoneNumber = "+48519411924"
            };

            try
            {
                var response = await snsClient.PublishAsync(request);
            }
            catch (Exception ex)
            {
                // W razie niepowodzenia logujemy stosown¹ informacjê
                context.Logger.LogLine($"Error sending message: {ex}");
            }

            await Task.CompletedTask;
        }
    }
}
