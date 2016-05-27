using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace WinEmailExtract
{
    public class Extract
    {
        public delegate void Completion(object o, EventArgs e);
        public event Completion OnCompletion;

        string _Input = string.Empty;
        string _URL = string.Empty;
        List<string> _Emails = new List<string>();

        public const string ALLOWED_CHARS = @"[a-zA-Z0-9-_]";

        public const string REGEX_EMAILS = @"^((?<emails>" + ALLOWED_CHARS + @"+(\." + ALLOWED_CHARS + @"+)*@" + ALLOWED_CHARS + @"+(\." + ALLOWED_CHARS + @"+)*)*(.|\n|\r\n)*?)+$";

        public string Input
        {
            get { return _Input; }
            set { _Input = value; }
        }

        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        public List<string> Emails
        {
            get { return _Emails; }
            set { _Emails = value; }
        }

        public string GetPage()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.URL.Trim());
            request.ContentType = "text/html";
            request.Method = "GET";
            request.Proxy = WebProxy.GetDefaultProxy();
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            string Input = string.Empty;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        Input = sr.ReadToEnd();
                        // Close and clean up the StreamReader
                        sr.Close();
                    }
                }
            }
            catch (Exception ex) { }

            return Input;
        }

        public void ExtractEmails(string Input)
        {
            Match m = Regex.Match(Input, REGEX_EMAILS);

            if (m.Success)
            {
                foreach (Capture c in m.Groups["emails"].Captures)
                {
                    Emails.Add(c.Value.Trim());
                }
            }
        }

        public void Start()
        {
            ExtractEmails(this.GetPage());

            if (OnCompletion != null)
            {
                OnCompletion(this, new EventArgs());
            }
        }
    }
}
