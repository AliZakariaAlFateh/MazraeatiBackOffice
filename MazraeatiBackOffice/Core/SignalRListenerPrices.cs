using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    // Mazraeati.MVC/Listeners/SignalRListenerPrices.cs
    public class SignalRListenerPrices
    {
        private readonly HubConnection _connection;
        private static readonly ConcurrentQueue<string> _priceNotifications = new ConcurrentQueue<string>();

        public SignalRListenerPrices()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://5.189.180.190/MazareatiAPI/priceHub")  // API Hub URL For Pro
                //.WithUrl("http://localhost:61366/priceHub") // API Hub URL For Dev
                .WithAutomaticReconnect()
                .Build();

            // استماع لتحديث سعر
            _connection.On<object>("PricesBatchUpdated", (price) =>
            {
                string message = $"📢 Price Updated : {price}";
                _priceNotifications.Enqueue(message);
                Console.WriteLine(message);
            });
        }

        public async Task StartAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("✅ Connected to SignalR Price Hub successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SignalR Price connection failed: {ex.Message}");
            }
        }

        public static IEnumerable<string> GetPriceNotifications()
        {
            return _priceNotifications.ToArray();
        }
    }
}
