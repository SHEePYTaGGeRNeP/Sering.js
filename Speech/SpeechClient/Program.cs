using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Speech.Synthesis;
using System.Threading;

namespace SpeechClient
{
    class Program
    {
        private static readonly SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            InitializeVoice();

            RunClient();
        }

        private static void InitializeVoice()
        {
            LogMessage("Initializing voice");
            var voices = _speechSynthesizer.GetInstalledVoices();

            if (!string.IsNullOrWhiteSpace(Settings1.Default.VoiceGender) &&
                voices.Any(v => v.VoiceInfo.Name == Settings1.Default.VoiceGender))
            {
                _speechSynthesizer.SelectVoice(Settings1.Default.VoiceGender);
            }
            else
            {
                if (Settings1.Default.VoiceGender.Equals("Male",
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    _speechSynthesizer.SelectVoiceByHints(VoiceGender.Male);
                }
                else if (Settings1.Default.VoiceGender.Equals("Female",
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    _speechSynthesizer.SelectVoiceByHints(VoiceGender.Female);
                }
                else
                {
                    _speechSynthesizer.SelectVoiceByHints(VoiceGender.Neutral);
                }
            }

            _speechSynthesizer.Rate = Settings1.Default.VoiceRate;
            _speechSynthesizer.Volume = Settings1.Default.VoiceVolume;
        }

        private static void RunClient()
        {
            LogMessage("Running client");
            using (var client = new HttpClient())
            {
                var uri = new Uri(Settings1.Default.ServiceUrl);
                client.Timeout = TimeSpan.FromSeconds(5);
                client.BaseAddress = new Uri($"{uri.Scheme}://{uri.Authority}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                while (true)
                {
                    try
                    {
                        LogMessage("Polling for new message...");
                        var response = client.GetAsync(uri.AbsoluteUri).Result;
                        var messageContent = response.Content.ReadAsStringAsync().Result.Replace(@"""", "");

                        LogMessage($"Message received: {messageContent}");

                        if (response.StatusCode != HttpStatusCode.OK)
                            continue;

                        if (messageContent == "empty")
                        {
                            LogMessage("No message available");
                            continue;
                        }

                        var substanceVolume = string.Empty;
                            
                        var messageArray = messageContent.ToLowerInvariant().Split('_');

                        substanceVolume = messageArray.Length == 1 
                            ? "full" 
                            : messageArray[1];
                                
                        switch (messageArray[0])
                        {
                            case @"beer":
                            case @"cola":
                            case @"jus":
                            {
                                Speak($"The glass contains {messageArray[0]} and is {substanceVolume}");
                                break;
                            }
                            default:
                            {
                                Speak("The glass contains an unknown substance and has an unknown volume");
                                break;
                            }

                        }

                        if (substanceVolume == "empty")
                        {
                            Thread.Sleep(2000);
                            Speak($"{Settings1.Default.HomeBot}, call William to get {messageArray[0]}");
                            CallDiscord();
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        LogMessage($"Error occurred: {errorMessage}");
                    }
                    finally
                    {
                        Thread.Sleep(3000);
                    }
                }
            }
        }

        private static void CallDiscord()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(Settings1.Default.DiscordUrl);
                    client.Timeout = TimeSpan.FromSeconds(5);
                    client.BaseAddress = new Uri($"{uri.Scheme}://{uri.Authority}");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.GetAsync(uri.AbsoluteUri);

                }
            }
            catch(Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                LogMessage($"Error occurred sending to discord: {errorMessage}");
            }
           
        }

        private static void Speak(string message)
        {
            LogMessage($"Speak {message}");
             _speechSynthesizer.SpeakAsync(message);
        }

        private static void LogMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now} - {message}");
        }
    }
}
