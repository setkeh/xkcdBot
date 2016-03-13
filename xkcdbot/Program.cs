using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace xkcdBot
{
    class Program
    {
        // Set your Commic download Directory.
        string ComicDir = @"C:\Users\setkeh\Desktop\Comics";

        // Specify the metadata URL for the commic you wish to download *Set to the Latest Comic by Default*
        static string metadata = "http://xkcd.com/info.0.json";

        static void Main(string[] args)
        {
            // Start The Application.
            MakeRequest(CreateRequest());
        }

        public static string CreateRequest()
        {
            // This provides the Direct URL to the metadata.
            string UrlRequest = metadata;
            return (UrlRequest);
        }

        public static void MakeRequest(string requestUrl)
        {
            /* 
            This is where most of the magic Happens.
            Here is where Network issues could throw exceptions hence why they are handled with try/catch.
            This section makes the HTTP Request to xkcd for the JSON Metadata and Parses it into a JSON string.
            */
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));

                    // This is the Parser that takes the metadata and produces a readable string.
                    string rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    JObject Pjson = JObject.Parse(rawJson);

                    // This simply begins the next non-static function.
                    Program p = new Program();
                    p.DownloadComic(Pjson);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }
        }

        // This does what it says on the tin it grabs the image url of the comic and downloads it the the Directory you specified in string ComicDir;
        public void DownloadComic(JObject Json)
        {
            string remoteFileUrl = Json.SelectToken("img").ToString();
            string ComicTitle = Json.SelectToken("title").ToString();
            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFileUrl, ComicDir + "\\" + ComicTitle + ".png");
        }
    }
}