using System;
using System.IO;
using System.Net;
using System.Text;

namespace ArchPM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpComposer
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static readonly HttpComposer Instance = new HttpComposer();

        /// <summary>
        /// Makes Http Web GET Request
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public String GET(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;

            }
        }

        /// <summary>
        /// Makes Http Web POST Request with Json
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="jsonData">The json data.</param>
        /// <returns></returns>
        public String POST(String url, String jsonData)
        {
            jsonData = jsonData.Replace("\"", "\\\"");
            jsonData = String.Format("\"{0}\"", jsonData);
            jsonData = jsonData.Replace("\r\n", "");

            String result = String.Empty;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonData);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";
            request.Accept = @"application/json";


            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            using (StreamReader reader = new StreamReader(responseStream))
            {
                result = reader.ReadToEnd();
            }

            result = result.Replace("\\u000d\\u000a", "\r\n");

            return result;
        }
    }
}
