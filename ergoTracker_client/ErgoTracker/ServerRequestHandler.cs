using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ErgoTracker
{
    public class ServerRequestHandler
    {
        string url = "http://52.89.152.186:3000"; // TODO: need to edit.
        string result = null;

        public event EventHandler ReceivedScoreData;
            
        public void postKinectData(string jsonData)
        {
            result = null;
            string url_to_use = url + "/post/kinect_data";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_to_use);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            try
            {
                using (var streamwriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamwriter.Write(jsonData);
                    streamwriter.Flush();
                    streamwriter.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            httpWebRequest.BeginGetResponse(new AsyncCallback(GetKinectDataResponseCallback), httpWebRequest);
        }

        private void GetKinectDataResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                using (HttpWebResponse response = (HttpWebResponse)myRequest.EndGetResponse(asynchronousResult))
                {
                    Stream responseStream = response.GetResponseStream();
                    using (var reader = new StreamReader(responseStream))
                    {
                        result = reader.ReadToEnd();
                        if (ReceivedScoreData != null)
                        {
                            ReceivedScoreData(this, new DataEventHandlerArgs(result));
                        }
                        Console.WriteLine(result);
                    }
                    responseStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void getDiagnosticData(DateTime fromDate, DateTime toDate)
        {
            result = null;
            string url_to_use = url + "/get/diagnostic_data?";

            string dateTimeFormat = "ddd MMM d yyyy";
            string fromDateWithFormat = fromDate.ToString(dateTimeFormat);
            string toDateWithFormat = toDate.ToString(dateTimeFormat);

            url_to_use += "fromDate=\'" + fromDateWithFormat + "\'&";
            url_to_use += "toDate=\'" + toDateWithFormat + "\'&";
            url_to_use += "userid=\'" + ApplicationInformation.Instance.getUsername();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url_to_use);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            httpWebRequest.BeginGetResponse(new AsyncCallback(GetDiagnosticDataCallback), httpWebRequest);
            
        }

        private void GetDiagnosticDataCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                using (HttpWebResponse response = (HttpWebResponse)myRequest.EndGetResponse(asynchronousResult))
                {
                    Stream responseStream = response.GetResponseStream();
                    using (var reader = new StreamReader(responseStream))
                    {
                        result = reader.ReadToEnd();
                        
                    }
                    responseStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void getTodaysScore(string jsonData)
        {

        }
    }
    
    public class DataEventHandlerArgs : EventArgs
    {
        private readonly string data;

        public DataEventHandlerArgs(string data)
        {
            this.data = data;
        }

        public string Data
        {
            get { return this.data; }
        }
    }
}
