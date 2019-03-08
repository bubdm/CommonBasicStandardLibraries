using System;
//using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.HtmlParser
{
    public class HtmlParser
    {
        private static readonly string DEFAULT_WORD_SEPARATORS = " " + Constants.vbLf + Constants.vbCr + Constants.vbTab + "<>;,!";
        private string m_strBody = "";
        private string m_error = "";
        public string ErrorPath = "";

        public string StartTag { get; set; } = "";
        public string EndTag { get; set; } = "";




        public CustomBasicList<string> GetList(string strFirst, bool ShowErrors = true)
        {
            string TempStr;
            TempStr = m_strBody;
            if (TempStr == "")
                throw new Exception("Blank list");
            var tGetList = new CustomBasicList<string>();
            // Dim x As Integer
            do
            {
                if (DoesExist(strFirst) == false)
                {
                    if (tGetList.Count == 0)
                    {
                        if (ShowErrors == true)
                            throw new ParserException("There are no items on the list", EnumMethod.GetList) { OriginalBody = TempStr, RemainingHtml = m_strBody, FirstTag = strFirst };
                    }
                    // try to add the final item (i think)
                    tGetList.Add(m_strBody);
                    m_strBody = TempStr;
                    return tGetList;
                }

                tGetList.Add(GetTopInfo(strFirst));
                m_strBody = GetBottomInfo(strFirst, true);
            }
            while (true);// try this
        }

        public CustomBasicList<string> GetList(string strFirst, string strSecond, bool ShowErrors = true)
        {
            string TempStr;
            TempStr = m_strBody;
            if (TempStr == "")
                throw new Exception("Blank list");
            var tGetList = new CustomBasicList<string>();
            string ThisItem;

            do
            {
                if (DoesExist(strFirst, strSecond) == false)
                {
                    if (tGetList.Count == 0)
                    {
                        if (ShowErrors == true)
                            throw new ParserException("There are no items on the list", EnumMethod.GetList) { OriginalBody = TempStr, RemainingHtml = m_strBody, FirstTag = strFirst, SecondTag = strSecond };
                    }

                    m_strBody = TempStr;
                    return tGetList;
                }

                ThisItem = GetSomeInfo(strFirst, strSecond, true);
                tGetList.Add(ThisItem);
            }
            while (true);
        }

        public string Body
        {
            get
            {
                return m_strBody;
            }
            set
            {
                m_strBody = value;
            }
        }

        public bool DoesExist()
        {
            CheckStartTag(); // ending is not required
            return DoesExist(StartTag, EndTag); // i think if nothing is put in there; will use the search terms provided.  to make it a little easier to maintain
        }

        public bool DoesExist(string strFirst, string StrSecond = "")
        {
            if (m_strBody.Length == 0)
                // m_error = "HtmlParser::DoesExist - nothing in body"
                return false;

            // Try
            string strTempBody = m_strBody.ToLower();
            string strTmpFirst = strFirst.ToLower();
            string strTmpSecond = StrSecond.ToLower();

            int first_pos = strTempBody.IndexOf(strTmpFirst);
            if (first_pos < 0)
                // m_error = "HtmlParser::DoesExist - could not find [" & strFirst & "] string"
                return false;
            // used wrong variable
            if (strTmpSecond.Length > 0)
            {
                int second_pos = strTempBody.IndexOf(strTmpSecond, first_pos + strTmpFirst.Length);
                if (second_pos < 0)
                    // m_error = "HtmlParser::DoesExist - could not find [" & StrSecond & "] string"
                    return false;
            }

            bool to_ret = true;
            // Catch ex As Exception
            // m_error = "HtmlParser::DoesExist - got exception (" & ex.Source & " - " & ex.Message & ")"
            // to_ret = False
            // End Try
            return to_ret;
        }

        public async Task EliminateSeveralTopItemsAsync(CustomBasicList<string> ThisList)
        {
            await Task.Run(() =>
            {
                ThisList.ForEach(Items =>
                {
                    if (DoesExist(Items) == true)
                    {
                        Body = GetTopInfo(Items);
                    }
                });
            });
        }

        public string GetTopInfo(bool bIncludeThis = false)
        {
            if (EndTag == "")
                throw new MissingTags(EnumLocation.Ending);
            return GetTopInfo(EndTag, bIncludeThis);
        }


        public string GetTopInfo(string strTagEnd, bool bIncludeThis = false)
        {
            if (m_strBody.Length == 0)
            {
                // m_error = "HtmlParser::GetTopInfo - nothing in body"

                SaveError(m_error);
                throw new Exception("GetTopInfo is blank");
            }

            // Try
            string strTmpEnd = strTagEnd.ToLower();
            string strTmpBody = m_strBody.ToLower();

            int n_find = strTmpBody.IndexOf(strTmpEnd);
            if (n_find < 0)
            {
                m_error = "HtmlParser::GetTopInfo - string " + strTagEnd + " was not found.";
                SaveError(m_error);
                throw new ParserException(m_error, EnumMethod.GetTopInfo) { FirstTag = strTagEnd, OriginalBody = m_strBody };
            }

            strTmpBody = m_strBody;
            string to_ret;
            if (bIncludeThis)
                to_ret = strTmpBody.Substring(0, n_find + strTagEnd.Length);
            else
                to_ret = strTmpBody.Substring(0, n_find);
            // Catch ex As Exception
            // m_error = "HtmlParser::GetTopInfo - got exception (" & ex.Source & " - " & ex.Message & ")"
            // Throw ex
            // End Try

            return to_ret;
        }

        private void CheckStartTag()
        {
            if (StartTag == "")
                throw new MissingTags(EnumLocation.Beginning);
        }

        public string GetBottomInfo(bool bTakeOutBody = false, bool bIncludeThis = false)
        {
            CheckStartTag();
            return GetBottomInfo(StartTag, bTakeOutBody, bIncludeThis);
        }

        public string GetBottomInfo(string strStartTag, bool bTakeOutBody = false, bool bIncludeThis = false)
        {
            string to_ret = "";
            if (m_strBody.Length == 0)
            {
                m_error = "HtmlParser::GetBottomInfo - nothing in body";
                return to_ret;
            }

            // Try
            string strTmpStartTag = strStartTag.ToLower();
            string strTmpBody = m_strBody.ToLower();

            int n_find = strTmpBody.IndexOf(strTmpStartTag);
            if (n_find < 0)
            {
                m_error = "HtmlParser::GetBottomInfo - string " + strStartTag + " was not found.";
                SaveError(m_error);
                throw new ParserException(m_error, EnumMethod.GetBottomInfo) { FirstTag = strStartTag, OriginalBody = m_strBody };
            }

            strTmpBody = m_strBody;

            if ((bIncludeThis))
                to_ret = strTmpBody.Substring(n_find, strTmpBody.Length - n_find);
            else
                to_ret = strTmpBody.Substring(n_find + strStartTag.Length, strTmpBody.Length - n_find - strStartTag.Length);

            if (bTakeOutBody)
                m_strBody = to_ret;
            // Catch ex As Exception
            // m_error = "HtmlParser::GetBottomInfo - got exception (" & ex.Source & " - " & ex.Message & ")"
            // Throw ex
            // End Try

            return to_ret;
        }
        // Public Function GetSomeInfo(ByVal bstrStartTag As String, ByVal bstrEndTag As String) As String
        // Dim bIncludeLast As Boolean = False
        // Dim bIncludeFirst As Boolean = False
        // Dim bTakeOutBody As Boolean = False
        // Return Me.GetSomeInfo(bstrStartTag, bstrEndTag, bTakeOutBody, bIncludeFirst, bIncludeLast)
        // End Function

        public string GetSomeInfo(bool bTakeOutBody = false, bool bIncludeFirst = false, bool bIncludeLast = false)
        {
            CheckStartTag();
            if (EndTag == "")
                throw new MissingTags(EnumLocation.Ending);
            return GetSomeInfo(StartTag, EndTag, bTakeOutBody, bIncludeFirst, bIncludeLast);
        }

        public string GetSomeInfo(string bstrStartTag, string bstrEndTag, bool bTakeOutBody = false, bool bIncludeFirst = false, bool bIncludeLast = false)
        {
            if (m_strBody.Length == 0)
            {
                m_error = "HtmlParser::GetSomeInfo - nothing in body";
                SaveError(m_error);
                throw new Exception("GetSomeInfo body is blank");
            }

            // Try
            string strStartTag = bstrStartTag.ToLower();
            string strTempBody = m_strBody.ToLower();

            int n_find = strTempBody.IndexOf(strStartTag);
            if (n_find < 0)
            {
                m_error = "HtmlParser::GetSomeInfo - string number 1" + bstrStartTag + " was not found.";
                SaveError(m_error);
                throw new ParserException(m_error, EnumMethod.GetSomeInfo) { FirstTag = bstrStartTag, SecondTag = bstrEndTag, OriginalBody = m_strBody };
            }

            strTempBody = m_strBody;
            int nDeletePos;
            if ((bIncludeFirst))
                nDeletePos = n_find;
            else
                nDeletePos = (n_find + bstrStartTag.Length);

            strTempBody = strTempBody.Substring(nDeletePos, strTempBody.Length - nDeletePos);

            string strEndTag = bstrEndTag.ToLower();
            string strTempBody2 = strTempBody.ToLower();
            n_find = strTempBody2.IndexOf(strEndTag);
            if (n_find < 0)
            {
                m_error = "HtmlParser::GetSomeInfo - string number 2" + bstrEndTag + " was not found.";
                SaveError(m_error);
                throw new ParserException(m_error, EnumMethod.GetSomeInfo) { FirstTag = bstrStartTag, SecondTag = bstrEndTag, OriginalBody = m_strBody, RemainingHtml = strTempBody2 };
            }

            if (bTakeOutBody)
            {
                if ((bIncludeLast))
                    m_strBody = strTempBody.Substring(n_find + strEndTag.Length, strTempBody.Length - n_find - strEndTag.Length);
                else
                    m_strBody = strTempBody.Substring(n_find, strTempBody.Length - n_find);
            }

            string to_ret;
            if (bIncludeLast)
                to_ret = strTempBody.Substring(0, n_find + strEndTag.Length);
            else
                to_ret = strTempBody.Substring(0, n_find);
            // Catch ex As Exception
            // m_error = "HtmlParser::GetSomeInfo - got exception (" & ex.Source & " - " & ex.Message & ")"
            // Throw ex
            // End Try

            return to_ret;
        }
        public string TextWithLinks()
        {
            string to_ret = "";

            if (m_strBody.Length == 0)
                // m_error = "HtmlParser::TextWithLinks - nothing in body"
                // Throw New 
                return to_ret;

            try
            {
                string str_tmp_body = "";
                string str_lower_body = m_strBody.ToLower();
                string str_body = m_strBody;

                int body_start = str_lower_body.IndexOf("<body");
                if (body_start >= 0)
                {
                    int body_end = str_lower_body.IndexOf("</body", body_start);
                    if ((body_end < 0))
                        str_body = m_strBody.Substring(body_start, (m_strBody.Length) - body_start);
                    else
                        str_body = m_strBody.Substring(body_start, (body_end) - body_start);
                    str_lower_body = str_body.ToLower();
                }

                HtmlParser.RemoveAllTags(ref str_body, ref str_lower_body, "script");
                HtmlParser.RemoveAllTags(ref str_body, ref str_lower_body, "style");

                str_tmp_body = HtmlParser.StripTags(ref str_body);
                str_tmp_body = str_tmp_body.Replace(Constants.vbCrLf, "<br>");
                str_tmp_body = str_tmp_body.Replace(Constants.vbLf, "<br>");
                str_tmp_body = str_tmp_body.Replace(Constants.vbCr, "<br>");
                str_tmp_body = str_tmp_body.Replace("  ", "&nbsp;&nbsp;");
                str_tmp_body = str_tmp_body.Replace(" &nbsp;", "&nbsp;&nbsp;");
                str_tmp_body = str_tmp_body.Replace("&nbsp; ", "&nbsp;&nbsp;");

                int curPos = 0;
                int start_pos = -1;

                do
                {
                    while ((curPos < str_tmp_body.Length) && (str_tmp_body[curPos] == ' ' || str_tmp_body[curPos].ToString() == Constants.vbLf || str_tmp_body[curPos].ToString() == Constants.vbCr || str_tmp_body[curPos].ToString() == Constants.vbTab))
                        curPos += 1;
                    // Do While (curPos < str_tmp_body.Length) AndAlso (str_tmp_body.Chars(curPos) = " "c OrElse str_tmp_body.Chars(curPos) = ControlChars.Lf OrElse str_tmp_body.Chars(curPos) = ControlChars.Cr OrElse str_tmp_body.Chars(curPos) = ControlChars.Tab)
                    // curPos += 1
                    // Loop

                    start_pos = curPos;
                    string strToken = HtmlParser.PickWord(ref curPos, ref str_tmp_body, HtmlParser.DEFAULT_WORD_SEPARATORS);
                    if (strToken.Length == 0)
                    {
                        curPos += 1;
                        continue;
                    }

                    string to_replace_with = "";
                    string strLowerToken = strToken.ToLower();
                    if (Substr(ref strLowerToken, 0, 7) == "http://" || Substr(ref strLowerToken, 0, 8) == "https://" || Substr(ref strLowerToken, 0, 6) == "ftp://")
                        to_replace_with = string.Format("<a href=\"{0}\">{1}</a>", strToken, strToken);
                    else if (Substr(ref strLowerToken, 0, 4) == "www.")
                        to_replace_with = string.Format("<a href=\"http://{0}\">{1}</a>", strToken, strToken);
                    else if (this.ValidEmailAddressFormat(strLowerToken))
                        to_replace_with = string.Format("<a href=\"mailto:{0}\">{1}</a>", strToken, strToken);
                    if (to_replace_with.Length > 0)
                    {
                        int flag_pos;
                        if ((curPos > 0))
                            flag_pos = curPos - strToken.Length;
                        else
                            flag_pos = str_tmp_body.Length - strToken.Length;
                        str_tmp_body = str_tmp_body.Remove(flag_pos, strToken.Length);
                        str_tmp_body = str_tmp_body.Insert(flag_pos, to_replace_with);
                        curPos = flag_pos + to_replace_with.Length;
                    }

                    curPos += 1;
                }
                while ((curPos > start_pos) && (curPos < str_tmp_body.Length - 1));

                return str_tmp_body;
            }
            catch (Exception ex)
            {
                m_error = "HtmlParser::TextWithLinks - got exception (" + ex.Source + " - " + ex.Message + ")";
                throw ex;
            }

            //return to_ret;
        }
        public string HtmlToString()
        {
            string buff = this.m_strBody;

            buff = buff.Replace("<br>", Constants.vbLf);
            buff = buff.Replace("&nbsp;", " ");

            return HtmlParser.StripTags(ref buff);
        }
        public bool ValidEmailAddressFormat(string strEmailAddress)
        {
            bool to_ret = false;

            if (strEmailAddress.Length == 0)
                return to_ret;

            try
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                Regex re = new Regex(strRegex);

                if (re.IsMatch(strEmailAddress))
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return to_ret;
        }
        public static bool SaveToFile(string path, string content)
        {
            bool to_ret = true;

            FileStream fs = null;
            BinaryWriter w = null;

            try
            {
                Utils.DeleteFile(path);

                fs = new FileStream(path, FileMode.CreateNew);
                w = new BinaryWriter(fs);
                int i = 0;
                while (i < content.Length)
                {
                    w.Write(System.Convert.ToByte(VBCompat.AscW(content[i].ToString()))); //took a risk here too.
                    i += 1;
                }
            }
            catch (Exception ex)
            {
                // Throw New Exception("")
                throw ex;
            }
            // Console.WriteLine("HtmlParser::WriteContentToFile(" & path & ") - got exception: [" & ex.Source & " - " & ex.Message & "]")
            finally
            {
                if (!(w == null))
                {
                    w.Close();
                    w = null;
                }

                if (!(fs == null))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            return to_ret;
        }

        private void SaveError(string ErrorMessage)
        {
            if (ErrorPath == "")
                return;
            System.IO.File.Delete(ErrorPath);
            string NewMessage;
            NewMessage = ErrorMessage + Constants.vbCrLf + Constants.vbCrLf + this.Body;
            // My.Computer.FileSystem.WriteAllText(ErrorPath, NewMessage, False)
            System.IO.File.WriteAllText(ErrorPath, NewMessage);
        }

        public bool Save(string path)
        {
            System.IO.File.Delete(path);
            System.IO.File.WriteAllText(path, Body);
            return true;
        }
        private static int RemoveAllTags(ref string body, ref string lower_body, string tag)
        {
            int start = 0;
            int end = -1;
            int crt_pos = 0;

            while (HtmlParser.ExtractScript(ref lower_body, tag, ref start, ref end, ref crt_pos))
            {
                body = body.Remove(start, end - start);
                lower_body = lower_body.Remove(start, end - start);
                end -= (end - start);

                crt_pos = end;
            }
            return 1;
        }
        private static bool ExtractScript(ref string strSrc, string tag, ref int start, ref int end, ref int crt_pos)
        {
            string start_tag = string.Format("<{0}", tag);
            string end_tag = string.Format("</{0}>", tag);

            start = strSrc.IndexOf(start_tag, crt_pos);
            if (start < 0)
                return false;

            end = strSrc.IndexOf(end_tag, start);
            if (end < 0)
                return false;

            end += end_tag.Length;
            return true;
        }
        private static string StripTags(ref string strToStrip)
        {
            string to_ret = "";
            int sz_dest_len = strToStrip.Length;
            bool in_tag = false;

            int k = 0;
            while (k < sz_dest_len)
            {
                if (strToStrip[k] == '<')
                    in_tag = true;

                if ((!in_tag))
                    to_ret += strToStrip[k];

                if (strToStrip[k] == '>' && in_tag)
                    in_tag = false;
                k += 1;
            }

            return to_ret;
        }
        private static string PickWord(ref int start, ref string strSrc, string strDelim)
        {
            string to_ret = "";
            if (start >= strSrc.Length)
                return to_ret;

            int next_pos = strSrc.IndexOfAny(strDelim.ToCharArray(), start);

            if ((next_pos < 0))
                to_ret = strSrc.Substring(start, strSrc.Length - start);
            else
                to_ret = strSrc.Substring(start, next_pos - start);

            start = next_pos;

            return to_ret;
        }
        private static string Substr(ref string str, int index, int count)
        {
            if ((index + count) > str.Length)
                return str;

            return str.Substring(index, count);
        }
    }
}
