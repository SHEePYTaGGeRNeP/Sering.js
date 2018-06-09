using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        private const string Url = "https://tbd";

        static void Main(string[] args)
        {
            
            InitializeVoice();

            RunClient();


        }

        private static void InitializeVoice()
        {
            LogMessage("Initializing voice");
            ReadOnlyCollection<InstalledVoice> voices =
                _speechSynthesizer.GetInstalledVoices();

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
                var uri = new Uri(Url);
                client.Timeout = TimeSpan.FromDays(1);
                client.BaseAddress = new Uri($"{uri.Scheme}://{uri.Authority}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                while (true)
                {
                    try
                    {
                        LogMessage("Polling for messages...");
                        // client.DefaultRequestHeaders.Add("apiKey", apiKey);
                        //var response = client.GetAsync(uri.AbsoluteUri, new StringContent(message, Encoding.UTF8, "application/json")).Result;
                        var response = client.GetAsync(uri.AbsoluteUri).Result;
                        var messageContent = response.Content.ReadAsStringAsync().Result;

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            // return JsonConvert.DeserializeObject<SpeakMessage>(messageContent);
                            Speak(messageContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"Error occurred: {ex.Message}");
                    }

                    Thread.Sleep(3000);
                }
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