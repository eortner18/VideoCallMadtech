using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using System.Threading.Channels;
using IronPdf;
using NAudio.Wave;

//using Google.Cloud.Translation.V2;

//Translator translator = new Translator("78294158-9bfc-7a3a-354f-52fff7652c9d:fx");
//var response = translator.TranslateTextAsync("Elias will nachhause für LOL", "DE", "EN-GB");

//Console.WriteLine(response.Result);

var client = new SpeechClientBuilder
{
    GoogleCredential = GoogleCredential.FromFile("C:\\Users\\daure\\Downloads\\uebersetzermad-effde603bc6c.json"),
}.Build();



var config = new RecognitionConfig
{
    Encoding = RecognitionConfig.Types.AudioEncoding.EncodingUnspecified,
    SampleRateHertz = 16000,
    LanguageCode = LanguageCodes.English.UnitedStates
};

var audio = RecognitionAudio.FromFile("C:\\Users\\daure\\Downloads\\veigar.mp3");

Console.WriteLine("de audio geht");

var response = client.Recognize(config, audio);

Console.WriteLine(response.Results);
//var client = TranslationClient.CreateFromApiKey("AIzaSyAGuf100LPu72MeWCdyKslrowuGL1aLkhQ");
//var response = client.TranslateText("Elias will nachhause für LOL", LanguageCodes.English, LanguageCodes.German);
//Console.WriteLine(response.TranslatedText);


//ChromePdfRenderer renderer = new ChromePdfRenderer();
//PdfDocument pdf = renderer.RenderHtmlAsPdf("<h1>Meine Test PDF für MAD</h1>" +
//    "<p> Elias will nachhause</p>");
//pdf.SaveAs(@"C:\Users\daure\OneDrive\Desktop\4B\Mad\MyPdfFile.pdf");

//string strMP3Folder = "C:\\Users\\daure\\Downloads\\";
//string strMP3SourceFilename = "veigar.mp3";
//string file = ".mp3";
//string strMP3OutputFilename = "newveigar";

//using (Mp3FileReader reader = new Mp3FileReader(strMP3Folder + strMP3SourceFilename))
//{
//    Mp3Frame mp3Frame;
//    System.IO.FileStream _fs = new System.IO.FileStream(strMP3Folder + strMP3OutputFilename+file, System.IO.FileMode.Create, System.IO.FileAccess.Write);

//    while ((mp3Frame = reader.ReadNextFrame() ) != null)
//    {
//        if (reader.CurrentTime<=TimeSpan.FromSeconds(3.0))
//        {
//            _fs.Write(mp3Frame.RawData, 0, mp3Frame.RawData.Length);
//        }

//    } 

//    _fs.Close();
//}