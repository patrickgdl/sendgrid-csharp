using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SendEmailCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var exampleData = new ExampleTemplateData
            {
                Subject = "Default e-mail test",
                Title = "Hi! Hello! Just a test",
                Description = "Birmingham",
                IsReceipt = true,
                Receipt = new Receipt
                {
                    Title = "Example Secondary Title",
                    Total = 200,
                    Items = new List<Item>
                        {
                            new Item {
                                Text = "New Line Sneakers",
                                Image = "https://marketing-image-production.s3.amazonaws.com/uploads/8dda1131320a6d978b515cc04ed479df259a458d5d45d58b6b381cae0bf9588113e80ef912f69e8c4cc1ef1a0297e8eefdb7b270064cc046b79a44e21b811802.png",
                                Price = 79
                            },
                            new Item {
                                Text = "New Line 2",
                                Image = "https://marketing-image-production.s3.amazonaws.com/uploads/8dda1131320a6d978b515cc04ed479df259a458d5d45d58b6b381cae0bf9588113e80ef912f69e8c4cc1ef1a0297e8eefdb7b270064cc046b79a44e21b811802.png",
                                Price = 89
                            }
                        }
                }
            };

            ExecuteMessage(exampleData).Wait();
        }

        static async Task ExecuteMessage(object dynamicTemplateData)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY"); // SendGrid API key set on windows environment variable
            Console.WriteLine("API KEY: " + apiKey);

            var credentials = new NetworkCredential("user", "password");
            var proxy = new WebProxy("http://proxy-here", true, null, credentials);
            var client = new SendGridClient(proxy, apiKey);

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("dx@sendgrid.com", "FromName"));
            msg.AddTo(new EmailAddress("dx@sendgrid.com", "ToName"));
            msg.SetTemplateId("d-1edc1791a1fd49dcbbcc9c8eae276338"); // template id from sendgrid | example.html

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            Console.WriteLine(response.StatusCode);
        }

        private class ExampleTemplateData
        {
            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("isReceipt")]
            public bool IsReceipt { get; set; }

            [JsonProperty("receipt")]
            public Receipt Receipt { get; set; }
        }

        private class Receipt
        {
            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("items")]
            public List<Item> Items { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }

        private class Item
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("image")]
            public string Image { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }
        }
    }
}

