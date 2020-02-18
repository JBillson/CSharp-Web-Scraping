using System;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GetVideos("mp4");
//            GetVideos("flv");
//            GetVideos("mkv");
        }

        private static void GetVideos(string videoType)
        {
            var url = "https://sample-videos.com/";
            var web = new HtmlWeb();
            var htmlDocument = web.Load(url);

            var videosTable = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                    .Equals($"sample-{videoType}-video")).ToList()[0];

            var videos = videosTable.Descendants("a")
                .Where(node => node.GetAttributeValue("class", "")
                    .Equals("download_video")).ToList();
            
            foreach (var node in videos)
            {
                var href = node.GetAttributeValue("href", "");
                var videoUrl = url + href;
                DownloadVideo(videoUrl);
            }
        }

        private static void DownloadVideo(string url)
        {
            var httpRequest = (HttpWebRequest) WebRequest.Create(url);
            httpRequest.Method = WebRequestMethods.Http.Get;

            var httpResponse = (HttpWebResponse) httpRequest.GetResponse();
            var httpResponseStream = httpResponse.GetResponseStream();

            const int bufferSize = 1024;
            var buffer = new byte[bufferSize];
            var bytesRead = 0;

            var fileStream = File.Create($"C:/Users/Justin/Videos/{url.Split('/').Last()}");
            while (httpResponseStream != null && (bytesRead = httpResponseStream.Read(buffer, 0, bufferSize)) != 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}