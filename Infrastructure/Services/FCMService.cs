using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using FirebaseAdmin.Messaging;

namespace Infrastructure.Services
{
    public class FCMService: IFCMService
    {
        public FCMService()
        {
        }


        public async Task SendToTopic(string topic, string NotifyTitle, string NotifyInfo)
        {
            // This registration token comes from the client FCM SDKs.
            // var registrationToken = "YOUR_REGISTRATION_TOKEN";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                // Data = new Dictionary<string, string>()
                // {
                //     { MsgTitle, MsgInfo }
                // },
                // Token = registrationToken,
                Topic = topic,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = NotifyTitle,
                    Body = NotifyInfo,
                },
                Android = new AndroidConfig()
                {
                    TimeToLive = TimeSpan.FromHours(1),
                    Notification = new AndroidNotification()
                    {
                        Icon = "stock_ticker_update",
                        Color = "#f45342",
                    },
                },
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
        }

        public  void SendToToken(string token, string NotifyTitle, string NotifyInfo)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            // This registration token comes from the client FCM SDKs.
            // var registrationToken = "YOUR_REGISTRATION_TOKEN";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                // Data = new Dictionary<string, string>()
                // {
                //     { MsgTitle, MsgInfo }
                // },
                Token = token,
                // Topic = topic,
                Notification = new Notification()
                {
                    Title = NotifyTitle,
                    Body = NotifyInfo,
                },
                // Android = new AndroidConfig()
                // {
                //     TimeToLive = TimeSpan.FromHours(1),
                //     Notification = new AndroidNotification()
                //     {
                //         Icon = "stock_ticker_update",
                //         Color = "#f45342",
                //     },
                // },
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
        }

        public async Task SendToAllTokens(List<string> tokens
            , string NotifyTitle, string NotifyInfo)
        {
            if (tokens.Count == 0)
            {
                return;
            }
            // This registration token comes from the client FCM SDKs.
            // var registrationToken = "YOUR_REGISTRATION_TOKEN";

            // See documentation on defining a message payload.
            var message = new MulticastMessage()
            {
                // Data = new Dictionary<string, string>()
                // {
                //     { MsgTitle, MsgInfo }
                // },
                Tokens = tokens,
                // Topic = topic,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = NotifyTitle,
                    Body = NotifyInfo,
                },
                // Android = new AndroidConfig()
                // {
                //     TimeToLive = TimeSpan.FromHours(1),
                //     Notification = new AndroidNotification()
                //     {
                //         Icon = "stock_ticker_update",
                //         Color = "#f45342",
                //     },
                // },
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            var test =  await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            // Response is a message ID string.
            // if (response.FailureCount > 0)
            // {
            //     var failedTokens = new List<string>();
            //     for (var i = 0; i < response.Responses.Count; i++)
            //     {
            //         if (!response.Responses[i].IsSuccess)
            //         {
            //             // The order of responses corresponds to the order of the registration tokens.
            //             failedTokens.Add(registrationTokens[i]);
            //         }
            //     }

            //     Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
            // }
        }
    }
}