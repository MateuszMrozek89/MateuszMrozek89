using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda4
{
    public class Function
    {
        IAmazonS3 S3Client { get; set; }
        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(S3Event @event, ILambdaContext context)
        {
            var s3Event = @event.Records?[0].S3;

            if (s3Event == null)
            {
                return null;
            }

            try
            {
                var bucketName = s3Event.Bucket.Name;
                var fileName = s3Event.Object.Key;
                //var fileExtension = s3Event.Object.Extension;

                var response = await this.S3Client.GetObjectMetadataAsync(s3Event.Bucket.Name, s3Event.Object.Key/*, s3Event.Object.Extension*/);

                var message = $"Dodano plik do: {bucketName} -> {fileName} "; /*-> {fileExtension}*/
                context.Logger.LogLine(message);

                return response.Headers.ContentType;

            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function."); /*and Extension {s3Event.Object.Extension}*/
                context.Logger.LogLine(ex.Message);
                context.Logger.LogLine(ex.StackTrace);
                throw;
            }
        }
    }
}
