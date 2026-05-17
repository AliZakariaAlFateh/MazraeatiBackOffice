using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class FirebaseNotificationService
    {
        private readonly FirebaseApp _app;

        public FirebaseNotificationService(IConfiguration configuration)
        {
            var serviceAccountPath = configuration["Firebase:ServiceAccountKeyPath"];

            if (!File.Exists(serviceAccountPath))
                throw new FileNotFoundException("Firebase service account file not found", serviceAccountPath);

            if (FirebaseApp.DefaultInstance == null)
            {
                _app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(serviceAccountPath)
                });
            }
            else
            {
                _app = FirebaseApp.DefaultInstance;
            }
        }

        public async Task<List<BatchResponse>> SendNotificationAsync(
            IEnumerable<string> tokens,
            string title,
            string body,
            Dictionary<string, string>? data = null)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            var tokenList = tokens
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            var responses = new List<BatchResponse>();

            // تقسيم التوكنز كل 500
            var chunks = tokenList
                .Select((token, index) => new { token, index })
                .GroupBy(x => x.index / 500)
                .Select(g => g.Select(x => x.token).ToList())
                .ToList();

            foreach (var chunk in chunks)
            {
                var messages = chunk.Select(token => new Message
                {
                    Token = token,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data ?? new Dictionary<string, string>()
                }).ToList();

                var response = await messaging.SendEachAsync(messages);

                responses.Add(response);
            }

            return responses;
        }
    }
}


