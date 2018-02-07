using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer
{
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

        public void SendSingleArray(string name, string message)
        {
            message += "|" + DateTime.Now.ToString("HH:mm:ss.ms");
            Thread.Sleep(getRandomDelayTime());
            message += "|" + DateTime.Now.ToString("HH:mm:ss.ms");
            Clients.All.InvokeAsync("sendToAll", name, message);
        }
        
        public void SendArray(string name, string message, int numberOfRequest)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < numberOfRequest; i++)
            {
                var msg = message + " - " + i + "|" + DateTime.Now.ToString("HH:mm:ss.ms");
                tasks.Add(Task.Run(() => SendSingleArray(name, msg)));
            //    SendSingleArray(name, message + " - " + i);
            }
            Task.WaitAll(tasks.ToArray());
        }
        
        //public void SendToAll(string name, string message)
        //{
        //    List<Task> tasks = new List<Task>();
        //    for(int i=0; i<100;i++)
        //    {
        //        var msg = "Async Task " + i.ToString();
        //        tasks.Add(Task.Run(() => asyncFunc(name, msg)));
        //        //tasks.Add(asyncFunc(name, msg));

        //        //var msg1 = "Sync Task " + i.ToString();
        //        //tasks.Add(Task.Run(() => syncFunc(name, msg1)));
        //        //tasks.Add(syncFunc(name, msg1));
        //    }

        //    Task.WaitAll(tasks.ToArray());

        //}

        //private void syncFunc(string name, string message)
        //{
        //    message += ": " + DateTime.Now.ToString();
        //    //int milliseconds = 5000;
        //    //await Task.Delay(milliseconds);
        //    var r = new Random().Next(1, 5);
        //    //int milliseconds = r*1000;
        //    //Thread.Sleep(milliseconds);
        //    Task.Delay(5000).Wait();
        //    //await Task.Delay(milliseconds);
        //    message += " //: " + DateTime.Now.ToString() + " **************************************************";
        //    Trace.WriteLine(message);
        //    Clients.All.InvokeAsync("sendToAll", name, message);
        //}

        //private async Task asyncFunc(string name, string message)
        //{
        //    message += ": " + DateTime.Now.ToString();
        //    int milliseconds = 5000;
        //    //await Task.Delay(milliseconds);
        //    var r = new Random().Next(1, 5);
        //    //int milliseconds = r*1000;
        //    //Thread.Sleep(milliseconds);
        //    await Task.Delay(milliseconds);
        //    message += " // " + DateTime.Now.ToString();
        //    Trace.WriteLine(message);
        //    await Clients.All.InvokeAsync("sendToAll", name, message);
        //}
    }
}