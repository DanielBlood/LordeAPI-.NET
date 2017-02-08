using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LordeAPI
{
    public class Chat
    {
        //VB.NET Class writeen by Daniel Blood
        //Github: http://github.com/danielblood
        //Version v1.0 First public release

        public string API_Endpoint = "https://lorde.material-cloud.net/api.php";
        //The API Endpoint, Don't change this unless we moved the API
        public string API_Key { get; set; }
        //Your API Key, you need to register it
        public string Client = "C# Client";
        public object SendRequest(string Message, bool ReplaceNewLine = false, string Username = "Unknown")
        {
            //First check for Empty settings
            if (API_Key == null)
                throw new Exception("Missing property: API_Key");
            if (Message == null)
                throw new Exception("Missing argument: Message");
            if (Client == null)
                Client = "C# Client";
            if (Username == null)
                Username = "Unknown";

            //Prepare the request
            System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(API_Endpoint);
            //Add the headers
            webRequest.Headers.Add("client", Client);
            webRequest.Headers.Add("username", Username);
            webRequest.Headers.Add("key", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(API_Key)));
            webRequest.Headers.Add("msg", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Message)));

            //Read the response
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)webRequest.GetResponse();
            System.IO.Stream receiveStream = response.GetResponseStream();
            System.IO.StreamReader readStream = new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string StrResponse = readStream.ReadToEnd();

            //Check response status
            if (StrResponse.ToLower().Contains("{\"status\":\"500\""))
            {
                throw new Exception("Error 500: Server error/Being updated");
            }
            else if (StrResponse.ToLower().Contains("{\"status\":\"401\""))
            {
                throw new Exception("Error 401: Invalid/Banned/Incorrect API Key");
            }
            else if (StrResponse.ToLower().Contains("{\"status\":\"400\""))
            {
                throw new Exception("Error 400: Invalid/Empty message");
            }
            else if (StrResponse.ToLower().Contains("{\"status\":\"200\""))
            {
                //Use Regex to read the JSON Response
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(".*?\".*?\".*?\".*?\".*?\".*?\".*?(\".*?\")", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                System.Text.RegularExpressions.Match m = r.Match(StrResponse);
                if ((m.Success))
                {
                    //Remove the quotes in a lazy way.
                    string G1 = m.Groups[1].ToString();
                    string G2 = G1.Remove(0, 1);

                    //Finally return the response from Lorde
                    if (ReplaceNewLine == true)
                    {
                        return G2.Substring(0, G2.Length - 1).Replace("\n", System.Environment.NewLine);
                    }
                    else
                    {
                        return G2.Substring(0, G2.Length - 1);
                    }

                }
                else
                {
                    //If it cannot get the response from Lorde, Simply return the full JSON Data
                    return StrResponse;
                }
            }
            else
            {
                throw new Exception("Error 0: Unknown Response");
            }
        }
    }
}
