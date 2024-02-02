using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using System.Threading.Channels;
using IronPdf;
using NAudio.Wave;
using Google.Cloud.Translation.V2;
public class Program
{
    private static void Main(string[] args)
    {
        var client = new SpeechClientBuilder
        {
            GoogleCredential = GoogleCredential.FromFile("C:\\Users\\Elias\\Downloads\\uebersetzermad-effde603bc6c.json"),
        }.Build();

        var translationClient = TranslationClient.CreateFromApiKey("AIzaSyAGuf100LPu72MeWCdyKslrowuGL1aLkhQ");


        var config = new RecognitionConfig
        {
            Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
            SampleRateHertz = 16000,
            LanguageCode = Google.Cloud.Translation.V2.LanguageCodes.English
        };

        using (var waveIn = new WaveInEvent())
        {
            waveIn.DataAvailable += (sender, e) =>
            {
                byte[] buffer = e.Buffer;
                var audio = RecognitionAudio.FromBytes(buffer);

                var response = client.Recognize(config, audio);
                foreach (var result in response.Results)
                {
                    string text = result.Alternatives[0].Transcript;
                    Console.WriteLine($"Transcribed: {text}");


                    var translation = translationClient.TranslateText(text, "de", "en-GB");
                    Console.WriteLine($"Translated: {translation.TranslatedText}");
                }
            };

            waveIn.StartRecording();
            Console.WriteLine("Speak now...");
            Console.ReadLine();
            waveIn.StopRecording();
        }
    }
}