using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    public class SignalRListenerFarms
    {
        private readonly HubConnection _connection;
        private static readonly ConcurrentQueue<string> _notifications = new ConcurrentQueue<string>();

        public SignalRListenerFarms()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:61366/farmHub")//.WithUrl("http://5.189.180.190/MazareatiAPI/farmHub") //.WithUrl("http://localhost:61366/farmHub") // ✅ API Hub URL
                .WithAutomaticReconnect()
                .Build();

            _connection.On<object>("FarmAdded", (farm) =>
            {
                string message = $"📢 New farm added: {farm}";
                _notifications.Enqueue(message);
                //Console.WriteLine(message);
            });
        }

        public async Task StartAsync()
        {
            try
            {
                await _connection.StartAsync();
                Console.WriteLine("✅ Connected to SignalR Hub successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SignalR connection failed: {ex.Message}");
            }
        }

        // Allow controllers/views to read notifications
        public static IEnumerable<string> GetNotifications()
        {
            return _notifications.ToArray();
        }
    }
}
