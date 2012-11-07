namespace XBSlink.Translator.Strings
{
    using System;
    using System.Collections;

    public class StringParser
    {
        private int m_nIndex;
        private string m_strContent;
        private string m_strContentLC;

        public StringParser()
        {
            this.m_strContent = "";
            this.m_strContentLC = "";
            this.m_nIndex = 0;
        }

        public StringParser(string strContent)
        {
            this.m_strContent = "";
            this.m_strContentLC = "";
            this.m_nIndex = 0;
            this.Content = strContent;
        }

        public bool at(string strString)
        {
            return (this.m_strContent.IndexOf(strString, this.Position) == this.Position);
        }

        public bool atNoCase(string strString)
        {
            strString = strString.ToLower();
            return (this.m_strContentLC.IndexOf(strString, this.Position) == this.Position);
        }

        public bool extractTo(string strString, ref string strExtract)
        {
            int index = this.m_strContent.IndexOf(strString, this.Position);
            if (index != -1)
            {
                strExtract = this.m_strContent.Substring(this.m_nIndex, index - this.m_nIndex);
                this.m_nIndex = index + strString.Length;
                return true;
            }
            return false;
        }

        public void extractToEnd(ref string strExtract)
        {
            strExtract = "";
            if (this.Position < this.m_strContent.Length)
            {
                int length = this.m_strContent.Length - this.Position;
                strExtract = this.m_strContent.Substring(this.Position, length);
            }
        }

        public bool extractToNoCase(string strString, ref string strExtract)
        {
            strString = strString.ToLower();
            int index = this.m_strContentLC.IndexOf(strString, this.Position);
            if (index != -1)
            {
                strExtract = this.m_strContent.Substring(this.m_nIndex, index - this.m_nIndex);
                this.m_nIndex = index + strString.Length;
                return true;
            }
            return false;
        }

        public bool extractUntil(string strString, ref string strExtract)
        {
            int index = this.m_strContent.IndexOf(strString, this.Position);
            if (index != -1)
            {
                strExtract = this.m_strContent.Substring(this.m_nIndex, index - this.m_nIndex);
                this.m_nIndex = index;
                return true;
            }
            return false;
        }

        public bool extractUntilNoCase(string strString, ref string strExtract)
        {
            strString = strString.ToLower();
            int index = this.m_strContentLC.IndexOf(strString, this.Position);
            if (index != -1)
            {
                strExtract = this.m_strContent.Substring(this.m_nIndex, index - this.m_nIndex);
                this.m_nIndex = index;
                return true;
            }
            return false;
        }

        public static void getLinks(string strString, string strRootUrl, ref ArrayList documents, ref ArrayList images)
        {
            strString = removeComments(strString);
            strString = removeScripts(strString);
            StringParser parser = new StringParser(strString);
            parser.replaceEvery("'", "\"");
            string uri = "";
            if (strRootUrl != null)
            {
                uri = strRootUrl.Trim();
            }
            if ((uri.Length > 0) && !uri.EndsWith("/"))
            {
                uri = uri + "/";
            }
            string strExtract = "";
            parser.resetPosition();
            while (parser.skipToEndOfNoCase("href=\""))
            {
                if (parser.extractTo("\"", ref strExtract))
                {
                    strExtract = strExtract.Trim();
                    if ((strExtract.Length > 0) && (strExtract.IndexOf("mailto:") == -1))
                    {
                        if (!strExtract.StartsWith("http://") && !strExtract.StartsWith("ftp://"))
                        {
                            try
                            {
                                UriBuilder builder = new UriBuilder(uri);
                                builder.Path = strExtract;
                                strExtract = builder.Uri.ToString();
                            }
                            catch (Exception)
                            {
                                strExtract = "http://" + strExtract;
                            }
                        }
                        if (!documents.Contains(strExtract))
                        {
                            documents.Add(strExtract);
                        }
                    }
                }
            }
            parser.resetPosition();
            while (parser.skipToEndOfNoCase("src=\""))
            {
                if (parser.extractTo("\"", ref strExtract))
                {
                    strExtract = strExtract.Trim();
                    if (strExtract.Length > 0)
                    {
                        if (!strExtract.StartsWith("http://") && !strExtract.StartsWith("ftp://"))
                        {
                            try
                            {
                                UriBuilder builder2 = new UriBuilder(uri);
                                builder2.Path = strExtract;
                                strExtract = builder2.Uri.ToString();
                            }
                            catch (Exception)
                            {
                                strExtract = "http://" + strExtract;
                            }
                        }
                        if (!images.Contains(strExtract))
                        {
                            images.Add(strExtract);
                        }
                    }
                }
            }
        }

        public static string removeComments(string strString)
        {
            string str = "";
            string strExtract = "";
            StringParser parser = new StringParser(strString);
            while (parser.extractTo("<!--", ref strExtract))
            {
                str = str + strExtract;
                if (!parser.skipToEndOf("-->"))
                {
                    return strString;
                }
            }
            parser.extractToEnd(ref strExtract);
            return (str + strExtract);
        }

        public static string removeEnclosingAnchorTag(string strString)
        {
            string str = strString.ToLower();
            int index = str.IndexOf("<a");
            if (index != -1)
            {
                index++;
                index = str.IndexOf(">", index);
                if (index != -1)
                {
                    index++;
                    int num2 = str.LastIndexOf("</a>");
                    if (num2 != -1)
                    {
                        return strString.Substring(index, num2 - index);
                    }
                }
            }
            return strString;
        }

        public static string removeEnclosingQuotes(string strString)
        {
            int index = strString.IndexOf("\"");
            if (index != -1)
            {
                int num2 = strString.LastIndexOf("\"");
                if (num2 > index)
                {
                    return strString.Substring(index, (num2 - index) - 1);
                }
            }
            return strString;
        }

        public static string removeHtml(string strString)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("&nbsp;", " ");
            hashtable.Add("&amp;", "&");
            hashtable.Add("&aring;", "");
            hashtable.Add("&auml;", "");
            hashtable.Add("&eacute;", "");
            hashtable.Add("&iacute;", "");
            hashtable.Add("&igrave;", "");
            hashtable.Add("&ograve;", "");
            hashtable.Add("&ouml;", "");
            hashtable.Add("&quot;", "\"");
            hashtable.Add("&szlig;", "");
            StringParser parser = new StringParser(strString);
            foreach (string str in hashtable.Keys)
            {
                string strReplacement = hashtable[str] as string;
                if (strString.IndexOf(str) != -1)
                {
                    parser.replaceEveryExact(str, strReplacement);
                }
            }
            parser.replaceEveryExact("&#0", "&#");
            parser.replaceEveryExact("&#39;", "'");
            parser.replaceEveryExact("</", " <~/");
            parser.replaceEveryExact("<~/", "</");
            hashtable.Clear();
            hashtable.Add("<br>", " ");
            hashtable.Add("<p>", " ");
            foreach (string str3 in hashtable.Keys)
            {
                string str4 = hashtable[str3] as string;
                if (strString.IndexOf(str3) != -1)
                {
                    parser.replaceEvery(str3, str4);
                }
            }
            strString = parser.Content;
            string str5 = "";
            int startIndex = 0;
            int num2 = 0;
            while ((num2 = strString.IndexOf("<", startIndex)) != -1)
            {
                string str6 = strString.Substring(startIndex, num2 - startIndex);
                str5 = str5 + str6;
                startIndex = num2 + 1;
                int index = strString.IndexOf(">", startIndex);
                if (index == -1)
                {
                    break;
                }
                startIndex = index + 1;
            }
            if (startIndex < strString.Length)
            {
                str5 = str5 + strString.Substring(startIndex, strString.Length - startIndex);
            }
            strString = str5;
            str5 = "";
            parser.Content = strString;
            parser.replaceEveryExact("  ", " ");
            strString = parser.Content.Trim();
            return strString;
        }

        public static string removeScripts(string strString)
        {
            string str = "";
            string strExtract = "";
            StringParser parser = new StringParser(strString);
            while (parser.extractToNoCase("<script", ref strExtract))
            {
                str = str + strExtract;
                if (!parser.skipToEndOfNoCase("</script>"))
                {
                    parser.Content = str;
                    return strString;
                }
            }
            parser.extractToEnd(ref strExtract);
            return (str + strExtract);
        }

        public int replaceEvery(string strOccurrence, string strReplacement)
        {
            int num = 0;
            strOccurrence = strOccurrence.ToLower();
            for (int i = this.m_strContentLC.IndexOf(strOccurrence); i != -1; i = this.m_strContentLC.IndexOf(strOccurrence))
            {
                string str = this.m_strContent.Substring(0, i) + strReplacement;
                int startIndex = i + strOccurrence.Length;
                if (startIndex < this.m_strContent.Length)
                {
                    string str2 = this.m_strContent.Substring(startIndex, this.m_strContent.Length - startIndex);
                    str = str + str2;
                }
                this.m_strContent = str;
                this.m_strContentLC = this.m_strContent.ToLower();
                num++;
            }
            return num;
        }

        public int replaceEveryExact(string strOccurrence, string strReplacement)
        {
            int num = 0;
            while (this.m_strContent.IndexOf(strOccurrence) != -1)
            {
                this.m_strContent = this.m_strContent.Replace(strOccurrence, strReplacement);
                num++;
            }
            this.m_strContentLC = this.m_strContent.ToLower();
            return num;
        }

        public void resetPosition()
        {
            this.m_nIndex = 0;
        }

        private bool seekTo(string strString, bool bNoCase, bool bPositionAfter)
        {
            if (this.Position >= this.m_strContent.Length)
            {
                return false;
            }
            int index = 0;
            if (bNoCase)
            {
                strString = strString.ToLower();
                index = this.m_strContentLC.IndexOf(strString, this.Position);
            }
            else
            {
                index = this.m_strContent.IndexOf(strString, this.Position);
            }
            if (index == -1)
            {
                return false;
            }
            this.m_nIndex = index;
            if (bPositionAfter)
            {
                this.m_nIndex += strString.Length;
            }
            return true;
        }

        public bool skipToEndOf(string strString)
        {
            return this.seekTo(strString, false, true);
        }

        public bool skipToEndOfNoCase(string strText)
        {
            return this.seekTo(strText, true, true);
        }

        public bool skipToStartOf(string strString)
        {
            return this.seekTo(strString, false, false);
        }

        public bool skipToStartOfNoCase(string strText)
        {
            return this.seekTo(strText, true, false);
        }

        public string Content
        {
            get
            {
                return this.m_strContent;
            }
            set
            {
                this.m_strContent = value;
                this.m_strContentLC = this.m_strContent.ToLower();
                this.resetPosition();
            }
        }

        public int Position
        {
            get
            {
                return this.m_nIndex;
            }
        }
    }
}

