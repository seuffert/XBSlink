using System;
using System.Collections.Generic;
using System.Text;

namespace XBSlink.Translator.Web
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;

    public abstract class WebResourceProvider
    {
        private HttpStatusCode m_httpStatusCode;
        private int m_nPause;
        private int m_nTimeout;
        private string m_strAgent;
        private string m_strContent;
        private string m_strError;
        private string m_strReferer;
        private DateTime m_tmFetchTime;

        public WebResourceProvider()
        {
            this.reset();
        }

        protected virtual bool continueFetching()
        {
            return false;
        }

        public void fetchResource()
        {
            if (this.init())
            {
                bool flag = false;
                do
                {
                    string url = this.getFetchUrl();
                    this.getContent(url);
                    if (this.m_httpStatusCode == HttpStatusCode.OK)
                    {
                        this.parseContent();
                    }
                }
                while (flag && this.continueFetching());
            }
        }

        private void getContent(string url)
        {
            if (this.m_nPause > 0)
            {
                int totalMilliseconds = 0;
                do
                {
                    if ((totalMilliseconds == 0) && (this.m_tmFetchTime != DateTime.MinValue))
                    {
                        TimeSpan span = (TimeSpan)(this.m_tmFetchTime - DateTime.Now);
                        totalMilliseconds = (int)span.TotalMilliseconds;
                    }
                    int millisecondsTimeout = 100;
                    if (totalMilliseconds < this.m_nPause)
                    {
                        Thread.Sleep(millisecondsTimeout);
                        totalMilliseconds += millisecondsTimeout;
                    }
                }
                while (totalMilliseconds < this.m_nPause);
            }
            string requestUriString = url;
            if (!requestUriString.StartsWith("http://"))
            {
                requestUriString = "http://" + requestUriString;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUriString);
            request.AllowAutoRedirect = true;
            request.UserAgent = this.m_strAgent;
            request.Referer = this.m_strReferer;
            if (this.m_nTimeout != 0)
            {
                request.Timeout = this.m_nTimeout;
            }
            string s = this.getPostData();
            if (s != null)
            {
                byte[] bytes = new ASCIIEncoding().GetBytes(s);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            this.m_strError = "";
            this.m_strContent = "";
            HttpWebResponse response = null;
            try
            {
                this.m_tmFetchTime = DateTime.Now;
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception exception)
            {
                if (exception is WebException)
                {
                    WebException exception2 = exception as WebException;
                    this.m_strError = exception2.Message;
                }
                return;
            }
            finally
            {
                if (response != null)
                {
                    this.m_httpStatusCode = response.StatusCode;
                }
            }
            try
            {
                this.m_strContent = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception)
            {
            }
        }

        protected abstract string getFetchUrl();
        protected virtual string getPostData()
        {
            return null;
        }

        protected virtual bool init()
        {
            return true;
        }

        protected virtual void parseContent()
        {
        }

        public void reset()
        {
            this.m_strAgent = "Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0)";
            this.m_strReferer = "";
            this.m_strError = "";
            this.m_strContent = "";
            this.m_httpStatusCode = HttpStatusCode.OK;
            this.m_nPause = 0;
            this.m_nTimeout = 0;
            this.m_tmFetchTime = DateTime.MinValue;
        }

        public string Agent
        {
            get
            {
                return this.m_strAgent;
            }
            set
            {
                this.m_strAgent = (value == null) ? "" : value;
            }
        }

        public string Content
        {
            get
            {
                return this.m_strContent;
            }
        }

        public string ErrorMsg
        {
            get
            {
                return this.m_strError;
            }
        }

        public DateTime FetchTime
        {
            get
            {
                return this.m_tmFetchTime;
            }
        }

        public int Pause
        {
            get
            {
                return this.m_nPause;
            }
            set
            {
                this.m_nPause = value;
            }
        }

        public string Referer
        {
            get
            {
                return this.m_strReferer;
            }
            set
            {
                this.m_strReferer = (value == null) ? "" : value;
            }
        }

        public int Timeout
        {
            get
            {
                return this.m_nTimeout;
            }
            set
            {
                this.m_nTimeout = value;
            }
        }
    }
}

