using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public abstract class SimpleWebPage : IDisposable
    {
        protected int WebRequestTimeOut { get; set; } = 50000;    // try 50 seconds.  can change if needed
        protected CookieCollection cookies = new CookieCollection();

        protected string GetLinkFromSpecificPage(string Link)
        {
            // logging in is so iffy; that cannot be on the base part of it.
            string result;
            HttpWebRequest http = WebRequest.Create(Link) as HttpWebRequest;
            CookieContainer cookiesContainer = new CookieContainer();
            http.AllowWriteStreamBuffering = false;
            http.Method = "GET";
            http.KeepAlive = false;
            http.Timeout = WebRequestTimeOut;
            http.AllowAutoRedirect = true;
            http.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            http.Method = "GET";
            http.CookieContainer = cookiesContainer;
            http.CookieContainer.Add(cookies);
            HttpWebResponse httpResponse = http.GetResponse() as HttpWebResponse;
            Stream responseStream = httpResponse.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            result = sr.ReadToEnd();
            sr.Close();
            responseStream.Close();

            //Cookie cook;
            foreach (Cookie cook in httpResponse.Cookies)
                cookies.Add(cook);
            httpResponse.Close();
            return result;
        }

        protected bool FillOutSimpleForm(string url, string QueryString)
        {
            // if it fails; then returns false
            try
            {
                string result = "";
                CookieContainer cookiesContainer = new CookieContainer();
                HttpWebRequest http = WebRequest.Create(url) as HttpWebRequest;
                http.MaximumAutomaticRedirections = 100;
                http.Method = "POST";
                http.ContentType = "application/x-www-form-urlencoded";
                // Dim postData As String = "userModel.login=" & UserName & "&userModel.password=" & Password & "&submit.x=24&submit.y=13"
                // Dim HiddenpostData As String = "username=" & PasswordData.UserName & "&password=" & PasswordData.Password

                byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(QueryString); // i think
                http.ContentLength = dataBytes.Length;
                http.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                http.KeepAlive = true;
                http.AllowAutoRedirect = false;
                http.CookieContainer = cookiesContainer;
                http.Timeout = WebRequestTimeOut;
                http.ReadWriteTimeout = 60000;
                using (Stream postStream = http.GetRequestStream())
                {
                    postStream.Write(dataBytes, 0, dataBytes.Length);
                }
                HttpWebResponse httpResponse = http.GetResponse() as HttpWebResponse;
                Stream responseStream = httpResponse.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                result = sr.ReadToEnd();
                // Console.WriteLine("Logon request sent and response recieved.")
                // If (result.Contains("Invalid username/password entered")) Then
                // sr.Close()
                // responseStream.Close()
                // httpResponse.Close()
                // Throw New Exception("Login Failure: Username and/or password is incorrect.")
                // End If
                sr.Close();
                responseStream.Close();
                httpResponse.Close();
                foreach (Cookie cook in httpResponse.Cookies)
                    cookies.Add(cook);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        // i think this can be used to download any picture
        protected Tuple<byte[], bool, int> GetBinaryFromSpecificPage(string pictureURL)
        {
            // this is for pictures
            try
            {
                HttpWebRequest http = WebRequest.Create(pictureURL) as HttpWebRequest;
                string targetPath = string.Empty;
                CookieContainer cookiesContainer = new CookieContainer();

                http.AllowWriteStreamBuffering = false;
                http.Method = "GET";
                http.KeepAlive = false;
                http.AllowAutoRedirect = true;
                http.CookieContainer = cookiesContainer;
                http.CookieContainer.Add(cookies);
                HttpWebResponse httpResponse = http.GetResponse() as HttpWebResponse;
                Stream responseStream = httpResponse.GetResponseStream();

                ArrayList wrlUriList = new ArrayList();

                Int64 iSize = httpResponse.ContentLength;
                byte[] inBuf = new byte[iSize - 1 + 1];
                int bytesToRead = System.Convert.ToInt32(inBuf.Length);
                int bytesRead = 0;
                int n;
                while ((bytesToRead > 0))
                {
                    n = responseStream.Read(inBuf, bytesRead, bytesToRead);
                    bytesRead += n;
                    bytesToRead -= n;
                }


                foreach (Cookie cook in httpResponse.Cookies)
                    cookies.Add(cook);
                return new Tuple<byte[], bool, int>(inBuf, true, bytesRead);
            }
            catch (Exception ex)
            {
                if ((ex.Message.Contains(" (404) Not Found")))
                    return new Tuple<byte[], bool, int>(null, false, 0);
                throw ex;
            }
        }

        protected virtual void FinalDispose()
        {
        }

        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                    FinalDispose();
            }
            this.disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        // Protected Overrides Sub Finalize()
        // ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        // Dispose(False)
        // MyBase.Finalize()
        // End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
