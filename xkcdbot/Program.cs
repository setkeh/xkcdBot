using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace xkcdBot
{
    class Program
    {
        string ComicDir = @"C:\Users\setkeh\Desktop\Comics";
        static void Main(string[] args)
        {
            MakeRequest(CreateRequest());
        }

        public static string CreateRequest()
        {
            string UrlRequest = "http://xkcd.com/info.0.json";
            return (UrlRequest);
        }

        public static void MakeRequest(string requestUrl)
        {
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

                    string rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    JObject Pjson = JObject.Parse(rawJson);

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

        public void DownloadComic(JObject Json)
        {
            string remoteFileUrl = Json.SelectToken("img").ToString();
            string ComicTitle = Json.SelectToken("title").ToString();
            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFileUrl, ComicDir + "\\" + ComicTitle + ".png");
        }
    }
}