
using System;
using System.Collections.Generic;
using XBSlink.Translator.Strings;
using XBSlink.Translator.Web;




namespace XBSlink.Translator
{


  
    /// <summary>
    /// Translates text using Google's online language tools.
    /// </summary>
    public class TranslatorEngine : WebResourceProvider
    {

      

        #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Translator"/> class.
            /// </summary>
        public TranslatorEngine()
            {
                this.SourceLanguage = "English";
                this.TargetLanguage = "French";
                this.Referer = "http://www.google.com";
            }

        #endregion

        #region Properties

            /// <summary>
            /// Gets or sets the source "
            /// </summary>
            /// <value>The source "</value>
            public string SourceLanguage {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the target "
            /// </summary>
            /// <value>The target "</value>
            public string TargetLanguage {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the source text.
            /// </summary>
            /// <value>The source text.</value>
            public string SourceText {
                get;
                set;
            }

            /// <summary>
            /// Gets the translation.
            /// </summary>
            /// <value>The translated text.</value>
            public string Translation {
                get;
                private set;
            }

            /// <summary>
            /// Gets the reverse translation.
            /// </summary>
            /// <value>The reverse translated text.</value>
            public string ReverseTranslation {
                get;
                private set;
            }

        #endregion        

        #region Public methods

            /// <summary>
            /// Attempts to translate the text.
            /// </summary>
            public void Translate()
            {
                // Validate source and target languages
                if (string.IsNullOrEmpty (this.SourceLanguage) ||
                    string.IsNullOrEmpty (this.TargetLanguage) ||
                    this.SourceLanguage.Trim().Equals (this.TargetLanguage.Trim())) {
                    throw new Exception ("An invalid source or target language was specified.");
                }

                // Delegate to base class
                this.fetchResource();
            }

        #endregion

        #region WebResourceProvider implementation

            /// <summary>
            /// Returns the url to be fetched.
            /// </summary>
            /// <returns>The url to be fetched.</returns>
            protected override string getFetchUrl()
            {
              return "http://translate.google.com/translate_t";
            }

            /// <summary>
            /// Retrieves the POST data (if any) to be sent to the url to be fetched.
            /// The data is returned as a string of the form "arg=val[&arg=val]...".
            /// </summary>
            /// <returns>A string containing the POST data or null if none.</returns>
            protected override string getPostData()
            {
              // Set translation mode
              string strPostData = string.Format ("hl=en&ie=UTF8&oe=UTF8submit=Translate&langpair={0}|{1}",
                                                   LanguageEnumToIdentifier (this.SourceLanguage),
                                                   LanguageEnumToIdentifier (this.TargetLanguage));

              // Set text to be translated
              strPostData += "&text=\"" + this.SourceText + "\"";
              return strPostData;
            }

            /// <summary>
            /// Parses the fetched content.
            /// </summary>
            protected override void parseContent()
            {
                // Initialize the scraper
                this.Translation = string.Empty;
                string strContent = this.Content;
                StringParser parser = new StringParser (strContent);

                // Scrape the translation
                string strTranslation = string.Empty;
                if (parser.skipToEndOf ("<span id=result_box")) {
                    if (parser.skipToEndOf ("onmouseout=\"this.style.backgroundColor='#fff'\">")) {
                        if (parser.extractTo("</span>", ref strTranslation)) {
                            strTranslation = StringParser.removeHtml (strTranslation);
                        }
                    }
                }

                #region Fix up the translation
                    int startClean = 0;
                    int endClean = 0;
                    int i=0;
                    while (i < strTranslation.Length) {
                        if (Char.IsLetterOrDigit (strTranslation[i])) {
                            startClean = i;
                            break;
                        }
                        i++;
                    }
                    i = strTranslation.Length - 1;
                    while (i > 0) {
                        char ch = strTranslation[i];
                        if (Char.IsLetterOrDigit (ch) ||
                            (Char.IsPunctuation (ch) && (ch != '\"'))) {
                            endClean = i;
                            break;
                        }
                        i--;
                    }
                    this.Translation = strTranslation.Substring (startClean, endClean - startClean + 1).Replace ("\"", "");
                #endregion
            }

        #endregion

            public class Languages
            {

                public static string _AUTODETECT = "Autodetect";
                public static string _ENGLISH = "English";
                public static string _SPANISH = "Spanish";

                public string description { get; set; }
                public string code { get; set; }
            }
        
            public static List<Languages> GetLanguages()
            {
                List<Languages> lang = new List<Languages>();
                lang.Add(new Languages() { description ="Afrikaans",  code = "af" });
                lang.Add(new Languages() { description ="Albanian",   code = "sq" });
                lang.Add(new Languages() { description ="Arabic",     code = "ar" });
                lang.Add(new Languages() { description ="Belarusian", code = "be" });
                lang.Add(new Languages() { description ="Bulgarian",  code = "bg" });
                lang.Add(new Languages() { description ="Catalan",    code = "ca" });
                lang.Add(new Languages() { description ="Chinese",    code = "zh-CN" });
                lang.Add(new Languages() { description ="Croatian",   code = "hr" });
                lang.Add(new Languages() { description ="Czech",      code = "cs" });
                lang.Add(new Languages() { description ="Danish",     code = "da" });
                lang.Add(new Languages() { description ="Dutch",      code = "nl" });
                lang.Add(new Languages() { description ="English",    code = "en" });
                lang.Add(new Languages() { description ="Estonian",   code = "et" });
                lang.Add(new Languages() { description ="Filipino",   code = "tl" });
                lang.Add(new Languages() { description ="Finnish",    code = "fi" });
                lang.Add(new Languages() { description ="French",     code = "fr" });
                lang.Add(new Languages() { description ="Galician",   code = "gl" });
                lang.Add(new Languages() { description ="German",     code = "de" });
                lang.Add(new Languages() { description ="Greek",      code = "el" });
                lang.Add(new Languages() { description ="Haitian Creole ALPHA",   code = "ht" });
                lang.Add(new Languages() { description ="Hebrew",     code = "iw" });
                lang.Add(new Languages() { description ="Hindi",      code = "hi" });
                lang.Add(new Languages() { description ="Hungarian",  code = "hu" });
                lang.Add(new Languages() { description ="Icelandic",  code = "is" });
                lang.Add(new Languages() { description ="Indonesian", code = "id" });
                lang.Add(new Languages() { description ="Irish",      code = "ga" });
                lang.Add(new Languages() { description ="Italian",    code = "it" });
                lang.Add(new Languages() { description ="Japanese",   code = "ja" });
                lang.Add(new Languages() { description ="Korean",     code = "ko" });
                lang.Add(new Languages() { description ="Latvian",    code = "lv" });
                lang.Add(new Languages() { description ="Lithuanian", code = "lt" });
                lang.Add(new Languages() { description ="Macedonian", code = "mk" });
                lang.Add(new Languages() { description ="Malay",      code = "ms" });
                lang.Add(new Languages() { description ="Maltese",    code = "mt" });
                lang.Add(new Languages() { description ="Norwegian",  code = "no" });
                lang.Add(new Languages() { description ="Persian",    code = "fa" });
                lang.Add(new Languages() { description ="Polish",     code = "pl" });
                lang.Add(new Languages() { description ="Portuguese", code = "pt" });
                lang.Add(new Languages() { description ="Romanian",   code = "ro" });
                lang.Add(new Languages() { description ="Russian",    code = "ru" });
                lang.Add(new Languages() { description ="Serbian",    code = "sr" });
                lang.Add(new Languages() { description ="Slovak",     code = "sk" });
                lang.Add(new Languages() { description ="Slovenian",  code = "sl" });
                lang.Add(new Languages() { description ="Spanish",    code = "es" });
                lang.Add(new Languages() { description ="Swahili",    code = "sw" });
                lang.Add(new Languages() { description ="Swedish",    code = "sv" });
                lang.Add(new Languages() { description ="Thai",       code = "th" });
                lang.Add(new Languages() { description ="Turkish",    code = "tr" });
                lang.Add(new Languages() { description ="Ukrainian",  code = "uk" });
                lang.Add(new Languages() { description ="Vietnamese", code = "vi" });
                lang.Add(new Languages() { description ="Welsh",      code = "cy" });
                lang.Add(new Languages() { description ="Yiddish",    code = "yi" });
                lang.Add(new Languages() { description ="Autodetect",    code = "auto" });
                return lang;
            }

            public static string TranslateText(string SourceText, string TargetLanguage)
            {
                return TranslateText(SourceText, Languages._AUTODETECT, TargetLanguage);
            }

            public static string TranslateText(string SourceText, string SourceLanguage, string TargetLanguage)
            {
                TranslatorEngine t = new TranslatorEngine();
                t.SourceLanguage = SourceLanguage;
                t.TargetLanguage = TargetLanguage;
                t.SourceText = SourceText;

                try
                {
                    t.Translate();
                    return t.Translation;
                }
                catch (Exception ex)
                {
                    xbs_messages.addDebugMessage("ERROR in Translator Engine: " + ex.Message, xbs_message_sender.TRANSLATOR, xbs_message_type.ERROR);
                }
                finally
                {

                }
                return "";
            }

        #region Private methods

            /// <summary>
            /// Converts a language to its identifier.
            /// </summary>
            /// <param name="language">The language."</param>
            /// <returns>The identifier or <see cref="string.Empty"/> if none.</returns>
            private static string LanguageEnumToIdentifier
                (string language)
            {
                if (_languageModeMap == null)
                {
                    _languageModeMap = new Dictionary<string, string>();
                    _languageModeMap.Add("Afrikaans", "af");
                    _languageModeMap.Add("Albanian", "sq");
                    _languageModeMap.Add("Arabic", "ar");
                    _languageModeMap.Add("Belarusian", "be");
                    _languageModeMap.Add("Bulgarian", "bg");
                    _languageModeMap.Add("Catalan", "ca");
                    _languageModeMap.Add("Chinese", "zh-CN");
                    _languageModeMap.Add("Croatian", "hr");
                    _languageModeMap.Add("Czech", "cs");
                    _languageModeMap.Add("Danish", "da");
                    _languageModeMap.Add("Dutch", "nl");
                    _languageModeMap.Add ("English",     "en");
                    _languageModeMap.Add ("Estonian",    "et");
                    _languageModeMap.Add ("Filipino",    "tl");
                    _languageModeMap.Add ("Finnish",     "fi");
                    _languageModeMap.Add ("French",      "fr");
                    _languageModeMap.Add ("Galician",    "gl");
                    _languageModeMap.Add ("German",      "de");
                    _languageModeMap.Add ("Greek",       "el");
                    _languageModeMap.Add ("Haitian Creole ALPHA",    "ht");
                    _languageModeMap.Add ("Hebrew",      "iw");
                    _languageModeMap.Add ("Hindi",       "hi");
                    _languageModeMap.Add ("Hungarian",   "hu");
                    _languageModeMap.Add ("Icelandic",   "is");
                    _languageModeMap.Add ("Indonesian",  "id");
                    _languageModeMap.Add ("Irish",       "ga");
                    _languageModeMap.Add ("Italian",     "it");
                    _languageModeMap.Add ("Japanese",    "ja");
                    _languageModeMap.Add ("Korean",      "ko");
                    _languageModeMap.Add ("Latvian",     "lv");
                    _languageModeMap.Add ("Lithuanian",  "lt");
                    _languageModeMap.Add ("Macedonian",  "mk");
                    _languageModeMap.Add ("Malay",       "ms");
                    _languageModeMap.Add ("Maltese",     "mt");
                    _languageModeMap.Add ("Norwegian",   "no");
                    _languageModeMap.Add ("Persian",     "fa");
                    _languageModeMap.Add ("Polish",      "pl");
                    _languageModeMap.Add ("Portuguese",  "pt");
                    _languageModeMap.Add ("Romanian",    "ro");
                    _languageModeMap.Add ("Russian",     "ru");
                    _languageModeMap.Add ("Serbian",     "sr");
                    _languageModeMap.Add ("Slovak",      "sk");
                    _languageModeMap.Add ("Slovenian",   "sl");
                    _languageModeMap.Add ("Spanish",     "es");
                    _languageModeMap.Add ("Swahili",     "sw");
                    _languageModeMap.Add ("Swedish",     "sv");
                    _languageModeMap.Add ("Thai",        "th");
                    _languageModeMap.Add ("Turkish",     "tr");
                    _languageModeMap.Add ("Ukrainian",   "uk");
                    _languageModeMap.Add ("Vietnamese",  "vi");
                    _languageModeMap.Add ("Welsh",       "cy");
                    _languageModeMap.Add ("Yiddish",     "yi");
                    _languageModeMap.Add("Autodetect",   "auto");
                }
                string mode = string.Empty;
                _languageModeMap.TryGetValue (language, out mode);
                return mode;
            }

        #endregion

        #region Fields

            /// <summary>
            /// The language to translation mode map.
            /// </summary>
            private static Dictionary<string, string> _languageModeMap;

        #endregion
    }
}
