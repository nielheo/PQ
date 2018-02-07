using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer
{
    [EnableCors("CorsPolicy")]
    public class ChatHub : Hub
    {
        private int minDelay = 5;
        private int maxDelay = 5;

        private int getRandomDelayTime()
        {
            var r = new Random().Next(minDelay, maxDelay);
            return r * 1000;
        }

        public async Task SendSingle(string name, string message)
        {
            var msg = message + "|" + DateTime.Now.ToString("HH:mm:ss.ms");
            await Task.Run(() => SendSingleArray(name, msg));
        }

        private async Task SendSingleArray(string name, string message)
        {
            message += "|" + DateTime.Now.ToString("HH:mm:ss.ms");
            await Task.Delay(getRandomDelayTime());
            message += "|" + DateTime.Now.ToString("HH:mm:ss.ms");
            await Clients.All.InvokeAsync("sendToAll", name, message);
        }
        
        public void SendArray(string name, string message, int numberOfRequest)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < numberOfRequest; i++)
            {
                var msg = message + " - " + i + "|" + DateTime.Now.ToString("HH:mm:ss.ms");
                tasks.Add(Task.Run(() => SendSingleArray(name, msg)));
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}