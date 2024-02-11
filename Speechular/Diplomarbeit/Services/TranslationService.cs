using MadTechLib;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using System.Threading.Channels;
using System.Text;
namespace Diplomarbeit.Services;

using DeepL;
using System.Text;

public class TranslationService
{

    private MadTechContext _context;



    public string TranslateText(string textToTranslate, string languageCodeA, string languageCodeB)
    {
        // Ensure valid language codes

        // if (languageCodeA == null) { languageCodeA = "de"; }
        //     if (languageCodeB == null) { languageCodeB = "en"; }

        // var config = new RecognitionConfig
        // {
        //     Encoding = RecognitionConfig.Types.AudioEncoding.EncodingUnspecified,
        //     SampleRateHertz = 16000,
        //     LanguageCode = Google.Cloud.Translation.V2.LanguageCodes.German
        // };
        // var config2 = new RecognitionConfig
        // {
        //     Encoding = RecognitionConfig.Types.AudioEncoding.EncodingUnspecified,
        //     SampleRateHertz = 16000,
        //     LanguageCode = Google.Cloud.Translation.V2.LanguageCodes.English
        // };
        // var translationClient = TranslationClient.Create();

        //var translationResponse = translationClient.TranslateText(
        //    projectId: Environment.GetEnvironmentVariable("MadTech"),
        //    text: textToTranslate,
        //    targetLanguageCode: languageCodeB,
        //    sourceLanguageCode: languageCodeA,
        //    encoding = Encoding.UTF8
        //);

        // if (translationResponse.Translations.Any(t => t.Errors.Any()))
        // {
        //     var errorMessages = translationResponse.Translations
        //         .SelectMany(t => t.Errors)
        //         .Select(error => error.Message);
        //     throw new Exception($"Translation failed: {string.Join(", ", errorMessages)}");
        // }

        // return translationResponse.Translations[0].TranslatedText;


        Translator translator = new Translator("78294158-9bfc-7a3a-354f-52fff7652c9d:fx");
        var response = translator.TranslateTextAsync(textToTranslate, languageCodeA, languageCodeB);
        return response.Result.ToString();


    }
}

