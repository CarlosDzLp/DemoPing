using System;
using System.Net;
using System.Threading.Tasks;
using DemoPing.iOS;
using Foundation;
using Xamarin.Forms;
using Xamarin.SimplePing;

[assembly:Dependency(typeof(PingDemo))]
namespace DemoPing.iOS
{
    public class PingDemo: IPing
    {
        public const string HostName = "www.microsoft.com";

        private SimplePing pinger;
        private NSTimer sendTimer;
        public async Task<string> Ping(string host)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    pinger = new SimplePing("www.apple.com");
                    pinger.Started += OnStarted;
                    pinger.Failed += OnFailed;
                    pinger.Sent += OnSent;
                    pinger.SendFailed += OnSendFailed;
                    pinger.ResponseRecieved += OnResponseRecieved;
                    pinger.UnexpectedResponse += OnUnexpectedResponse;
                    pinger.Start();
                });
                return "";
            }
            catch(Exception ex)
            {
                return "";
            }
        }
        public void Stop()
        {
            Console.WriteLine("stop");

            pinger?.Stop();
            pinger = null;

            sendTimer?.Invalidate();
            sendTimer = null;
        }

        public void SendPing()
        {
            pinger?.SendPing(null);
        }
        private void OnResponseRecieved(object sender, SimplePingResponseRecievedEventArgs e)
        {
            Console.WriteLine(e.SequenceNumber + " received, size = " + e.Packet.Length);
            var seq = e.SequenceNumber;
            var packet = e.Packet;
            Console.WriteLine(seq);
            Console.WriteLine(packet);
        }

        private void OnStarted(object sender, SimplePingStartedEventArgs e)
        {
            var endpoint = e.EndPoint;
            Console.WriteLine(endpoint);
            SendPing();
            Console.WriteLine("OnStarted: ");
            // And start a timer to send the subsequent pings.

            sendTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(1.0), t => SendPing());
        }

        private void OnFailed(object sender, SimplePingFailedEventArgs e)
        {
            Console.WriteLine("failed: " + GetShortError(e.Error));

            Stop();
        }

        private void OnSent(object sender, SimplePingSentEventArgs e)
        {
            Console.WriteLine(e.SequenceNumber + " sent");
        }
        private void OnSendFailed(object sender, SimplePingSendFailedEventArgs e)
        {
            Console.WriteLine(e.SequenceNumber + " send failed: " + GetShortError(e.Error));
        }

        private void OnUnexpectedResponse(object sender, SimplePingUnexpectedResponseEventArgs e)
        {
            Console.WriteLine("unexpected packet received, size = " + e.Packet.Length);
        }

        private string GetDisplayAddress(IPEndPoint endpoint)
        {
            var entry = Dns.GetHostEntry(endpoint.Address);
            return entry?.HostName ?? "?";
        }

        private string GetShortError(NSError error)
        {
            var result = error.LocalizedFailureReason;
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            return error.LocalizedDescription;
        }
    }
}
