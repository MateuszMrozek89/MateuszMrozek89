using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleEmailService;
using Amazon.SimpleEmailService.Model;
using System.IO;

const string accessKeyId = "ACCESS_KEY_ID";
const string secretAccessKey = "SECRET_ACCESS_KEY";
const string regionEndpoint = "REGION_ENDPOINT";

const string bucketName = "NAZWA_KOSZYKA";

const string senderEmail = "nadawca@example.com";
const string recipientEmail = "odbiorca@example.com";

var client = new AmazonS3Client(accessKeyId, secretAccessKey, regionEndpoint);
var sesClient = new AmazonSimpleEmailServiceClient(accessKeyId, secretAccessKey, regionEndpoint);

var listObjectsRequest = new ListObjectsRequest
{
    BucketName = bucketName
};

var listObjectsResponse = await client.ListObjectsAsync(listObjectsRequest);

foreach (var objectSummary in listObjectsResponse.S3Objects)
{
    // Pobierz plik certyfikatu z S3
    var getObjectRequest = new GetObjectRequest
    {
        BucketName = bucketName,
        Key = objectSummary.Key
    };

    var getObjectResponse = await client.GetObjectAsync(getObjectRequest);

    // Zapisz pobrany plik do pamięci
    var certificateBytes = getObjectResponse.ResponseStream.ReadAsByteArrayAsync().Result;

    // Utwórz obiekt X509Certificate2 z pobranych bajtów
    var certificate = new X509Certificate2(certificateBytes);

    // Sprawdź datę ważności certyfikatu
    var expiryDate = certificate.NotAfter;

    var daysUntilExpiration = (expiryDate - DateTime.Now).Days;

    if (daysUntilExpiration <= 7)
    {
        // Utwórz wiadomość e-mail
        var message = new Message
        {
            Source = senderEmail,
            Destination = new Destination
            {
                ToAddresses = new List<string> { recipientEmail }
            },
            Subject = new Content("Alert o wygasającym certyfikacie"),
            Body = new Body
            {
                Text = new Content($"Certyfikat {objectSummary.Key} wygasa za {daysUntilExpiration} dni.")
            }
        };

        // Wyślij wiadomość e-mail
        await sesClient.SendEmailAsync(message);
    }
}