using Amazon;
using Amazon.Lambda.Core;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SchLambda
{
    public class Function
    {
        public async Task FunctionHandler( ILambdaContext context)
        {
            string sb = "test";

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
        }
    }
}
