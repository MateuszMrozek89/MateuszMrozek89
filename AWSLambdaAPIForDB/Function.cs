using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using AWSLambdaAPIForDB.AWSLambdaAPIForDB.Models;
//using AWSLambda5.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaAPIForDB
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Carshowroom> FunctionHandler(Carshowroom input, ILambdaContext context)
        {
            try
            {
                using (var client = new AmazonDynamoDBClient())
                {
                    // Generujemy jednoznaczny identyfikator naszego rekordu
                    string orderId = Guid.NewGuid().ToString();

                    // Wymagana paczka: using Amazon.DynamoDBv2;
                    // Korzystamy z dost�pnej metody PutItemAsync
                    await client.PutItemAsync(new PutItemRequest
                    {
                        // Podajemy nazw� tabeli, kt�r� utworzyli�my w konsoli AWS
                        TableName = "CarShowroom",
                        Item = new Dictionary<string, AttributeValue>
                     {
                         { "OrderId", new AttributeValue { S = orderId.ToString() }},
                         { "Brand", new AttributeValue { S =  input.Brand }},
                         { "Model", new AttributeValue { S = input.Model }},
                         { "Color", new AttributeValue { S = input.Color }},
                         { "Price", new AttributeValue { S = string.Format("{0} z�", input.Price) }},
                     }
                    });

                    // Krok troch� na wyrost...chcia�em jednak pokaza� mo�liwo�� edycji danych przychodz�cych
                    // Pole Price zdefiniowane zosta�o jako string tak, aby m�c nim manipulowa�
                    Dictionary<string, AttributeValue> carDetails = client.GetItemAsync(new GetItemRequest
                    {
                        TableName = "CarShowroom",
                        Key = new Dictionary<string, AttributeValue>
                     {
                         { "OrderId", new AttributeValue { S = orderId.ToString() } }
                     }
                    }).Result.Item;

                    // Ustawiamy w naszym obiekcie pola, kt�re mog�y zosta� zmodyfikowane (np. Price)
                    input.OrderId = carDetails["OrderId"].S;
                    input.Brand = carDetails["Brand"].S;
                    input.Model = carDetails["Model"].S;
                    input.Color = carDetails["Color"].S;
                    input.Price = carDetails["Price"].S;
                }
            }
            catch (Exception ex)
            {
                // W razie b��d�w chcemy mie� mo�liwo�� sprawdzenia co si� sta�o
                context.Logger.Log(ex.ToString());
            }

            return input;
        }
    }

    // Pami�tajcie o odpowiedniej strukturze folder�w Waszego projektu
    namespace AWSLambdaAPIForDB.Models
    {
        public class Carshowroom
        {
            public string OrderId { get; set; }

            public string Brand { get; set; }

            public string Model { get; set; }

            public string Color { get; set; }

            // Pole Price celowo ustawione na string...
            // W logice biznesowej dodamy do ceny sufix 'z�'
            public string Price { get; set; }
        }
    }
}
