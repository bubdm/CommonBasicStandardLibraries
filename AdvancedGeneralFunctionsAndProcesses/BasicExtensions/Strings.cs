using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Strings
    {
        private static string _monthReplace = "";
        public static CustomBasicList<string> CommaDelimitedList(string payLoad)
        {
            return payLoad.Split(",").ToCustomBasicList(); //comma alone.
        }
        public static bool IsNumeric(this string thisStr)
        {
            return int.TryParse(thisStr, out _); //you are forced to assign variable no matter what now.
        }
        public static int FindMonthInStringLine(this string thisStr) // will return a number  this will assume that there is never a case where 2 months appear
        {
            CustomBasicList<string> possList = new CustomBasicList<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int possNum;
            possNum = 0;
            _monthReplace = "";
            int currentNum;
            foreach (var thisPoss in possList)
            {
                if (thisStr.Contains(thisPoss) == true)
                {
                    currentNum = thisPoss.GetMonthID();
                    if (currentNum > 0)
                    {
                        _monthReplace = thisPoss;
                        if (possNum > 0)
                            throw new Exception("There should not have been 2 months in the same line.  Rethink");
                        possNum = currentNum;
                    }
                }
            }
            return possNum;
        }
        public static CustomBasicList<string> SplitStringEliminateMonth(this string thisStr)
        {
            thisStr = thisStr.Trim();
            if (_monthReplace == null == true || _monthReplace == "")
                return new CustomBasicList<string>() { thisStr };
            var thisList = thisStr.Split(_monthReplace);
            if (thisList.Count() != 2)
                throw new Exception("There should be only 2 items splited, not " + thisList.Count()); //i am guessing has to be this way now.
            CustomBasicList<string> newList = new CustomBasicList<string>();
            foreach (var thisItem in thisList)
            {
                newList.Add(thisItem.Trim());
            }
            return newList;
        }
        public static int GetMonthID(this string monthString)
        {
            // if nothing found, then return 0
            if (monthString == null)
                return 0;// if return nothing, then return 0
            if (monthString == "January")
                return 1;
            if (monthString == "February")
                return 2;
            if (monthString == "March")
                return 3;
            if (monthString == "April")
                return 4;
            if (monthString == "May")
                return 5;
            if (monthString == "June")
                return 6;
            if (monthString == "July")
                return 7;
            if (monthString == "August")
                return 8;
            if (monthString == "September")
                return 9;
            if (monthString == "October")
                return 10;
            if (monthString == "November")
                return 11;
            if (monthString == "December")
                return 12;
            return 0;
        }

        public static CustomBasicList<string> GetSentences(this string sTextToParse)
        {
            CustomBasicList<string> al = new CustomBasicList<string>();
            string sTemp = sTextToParse;
            sTemp = sTemp.Replace(Environment.NewLine, " ");
            string[] CustomSplit = new[] { ".", "?", "!", ":" };
            var Splits = sTemp.Split(CustomSplit, StringSplitOptions.RemoveEmptyEntries).ToList();
            int pos;
            foreach (var thisSplit in Splits)
            {
                pos = sTemp.IndexOf(thisSplit);
                var thisChar = sTemp.Trim().ToCharArray();
                if (pos + thisSplit.Length <= thisChar.Length - 1)
                {
                    char c = thisChar[pos + thisSplit.Length];
                    al.Add(thisSplit.Trim() + c.ToString());
                    sTemp = sTemp.Replace(thisSplit, ""); // because already accounted for.
                }
                else
                {
                    al.Add(thisSplit);
                }
            }
            if (al.First().StartsWith("\"") == true)
                throw new Exception("I don't think the first one can start with quotes");
            int x;
            var loopTo = al.Count - 1; // this is used so the quotes go into the proper places.
            for (x = 1; x <= loopTo; x++)
            {
                string firstItem;
                string secondItem;
                firstItem = al[x - 1];
                secondItem = al[x];
                if (secondItem.StartsWith("\"") == true)
                {
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + "\"";
                    al[x - 1] = al[x - 1].Trim();
                }
                else if (secondItem.StartsWith(")") == true)
                {
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + ")";
                    al[x - 1] = al[x - 1].Trim();
                }
                else if (secondItem.Length == 1)
                {
                    var ThisStr = secondItem.ToString();
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + ThisStr;
                    al[x - 1] = al[x - 1].Trim();
                }
            }
            int numbers = al.Where(Items => Items == "").Count();
            int opening = al.Where(Items => Items == "(").Count();
            int closing = al.Where(Items => Items == ")").Count();
            foreach (var thisItem in al)
            {
                if (numbers == 1 || numbers == 3)
                    throw new Exception("Quotes are not correct.  Has " + numbers + " Quotes");
                if (opening != closing)
                    throw new Exception("Opening and closing much match for ( and ) characters");
            }
            al = (from Items in al
                  where Items != ""
                  select Items).ToCustomBasicList();
            return al;
        }
        public static string StripHtml(this string htmlText) //unfortunately not perfect.
        {
            var thisText = Regex.Replace(htmlText, "<.*?>", "");
            if (thisText.Contains("<sup") == true)
            {
                var index = thisText.IndexOf("<sup");
                thisText = thisText.Substring(0, index);
            }
            if (thisText.Contains("<div class=" + "\"") == true)
                thisText = thisText.Replace("<div class=" + "\"", "");
            if (thisText.Contains("<a") == true)
            {
                var index = thisText.IndexOf("<a");
                thisText = thisText.Substring(0, index);
            }
            thisText = thisText.Replace("[a]", "");
            thisText = thisText.Replace("[b]", ""); // because even if its b, needs to go away as well.
            thisText = thisText.Replace("[c]", "");
            thisText = thisText.Replace("[d]", "");
            thisText = thisText.Replace("[e]", "");
            thisText = thisText.Replace("[f]", "");
            thisText = thisText.Replace("[g]", "");
            thisText = thisText.Replace("[h]", "");
            thisText = thisText.Replace("[i]", "");
            thisText = thisText.Replace("[j]", "");
            thisText = thisText.Replace("[k]", "");
            thisText = thisText.Replace("[l]", "");
            thisText = thisText.Replace("[m]", "");
            thisText = thisText.Replace("[n]", "");
            thisText = thisText.Replace("[o]", "");
            thisText = thisText.Replace("[p]", "");
            thisText = thisText.Replace("[q]", "");
            thisText = thisText.Replace("[r]", "");
            thisText = thisText.Replace("[s]", "");
            thisText = thisText.Replace("[t]", "");
            thisText = thisText.Replace("[u]", "");
            thisText = thisText.Replace("[v]", "");
            thisText = thisText.Replace("[w]", "");
            thisText = thisText.Replace("[x]", "");
            thisText = thisText.Replace("[y]", "");
            thisText = thisText.Replace("[z]", "");
            var nextText = System.Net.WebUtility.HtmlDecode(thisText);
            return nextText.Trim();
        }
        public static string TextWithSpaces(this string thisText)
        {
            string newText = thisText;
            int x = 0;
            string finals = "";
            foreach (var thisChar in newText)
            {
                bool rets = int.TryParse(thisChar.ToString(), out _);
                if (char.IsLower(thisChar) == false && x > 0 && rets == false)
                    finals += " " + thisChar;
                else
                    finals += thisChar;
                x++;
            }
            return finals;
        }
        public static int GetSeconds(this string timeString)
        {
            var tempList = timeString.Split(":").ToCustomBasicList();
            if (tempList.Count > 3)
                throw new Exception("Can't handle more than 3 :");
            if (tempList.Count == 3)
            {
                int firstNum;
                int secondNum;
                int thirdNum;
                firstNum = int.Parse(tempList.First().ToString());
                secondNum = int.Parse(tempList[1]);
                thirdNum = int.Parse(tempList.Last());
                int firstSecs;
                firstSecs = firstNum * 60 * 60;
                var secondSecs = secondNum * 60;
                var thirdSecs = thirdNum;
                return firstSecs + secondSecs + thirdSecs;
            }
            if (tempList.Count == 2)
            {
                int firstSecs = int.Parse(tempList.First()) * 60;
                return firstSecs + int.Parse(tempList.Last());
            }
            if (tempList.Count == 0)
                throw new Exception("I think its wrong");
            if (tempList.Count == 1)
                throw new Exception("Should just return as is");
            throw new Exception("Not sure");
        }
        public static bool IsValidDate(this string dateStr, out DateTime? newDate) // can't use nothing as a variable type.  too bad the function did not allow that.  can try one more thing
        {
            string thisText;
            thisText = dateStr;
            newDate = null;
            if (thisText.Length != 6)
                return false;
            var newText = thisText.Substring(0, 2) + "/" + thisText.Substring(2, 2) + "/" + thisText.Substring(4, 2);
            bool rets;
            rets = DateTime.TryParse(newText, out DateTime TempDate);
            if (rets == false)
                return false;
            newDate = TempDate;
            return true; // i think
        }
        public static CustomBasicList<Tuple<string, int?>> GetStringIntegerCombos(this string thisStr)
        {
            CustomBasicList<Tuple<string, int?>> thisList = new CustomBasicList<Tuple<string, int?>>();
            if (thisStr == "")
                return thisList;
            string item1;
            item1 = "";
            string item2;
            item2 = "";
            int x = 0;
            // if the first item is a number, then the first string will be blank and the second part will be a integer.  this is used for games like farmville where there is a string and number associated to it.  if 0 is entered, then will return 0 items.  its up to the process to decide what to do with a blank number
            bool hadNumber = false;
            bool hadStr = false;
            bool reject = false;
            bool lastAlpha = false;
            foreach (var thisItem in thisStr.ToList())
            {
                x += 1;
                bool isAlpha;
                isAlpha = thisItem.IsAlpha(true);
                bool isInt;
                isInt = thisItem.IsInteger();
                if (isInt == true)
                    hadNumber = true;
                else if (isAlpha == true)
                    hadStr = true;
                if (isInt == false && isAlpha == false)
                {
                    reject = true;
                    break;
                }
                if (isAlpha == true && isInt == true)
                    throw new Exception("Cannot be both alpha and integers.");
                if (x == 1)
                {
                    item1 = thisItem.ToString();
                }
                else if (lastAlpha == true && isAlpha == true)
                {
                    item1 += thisItem;
                }
                else if (lastAlpha == false && isInt == true)
                {
                    item2 += thisItem;
                }
                else if (lastAlpha == true && isInt == true)
                {
                    item2 = thisItem.ToString(); // start of a number
                }
                else
                {
                    int? ThisInt;
                    if (item2 == "")
                        ThisInt = default;
                    else
                        ThisInt = int.Parse(item2);
                    Tuple<string, int?> thisTuple = new Tuple<string, int?>(item1, ThisInt);
                    thisList.Add(thisTuple);
                    item1 = thisItem.ToString(); // try this way.
                    item2 = null!; // try this
                }
                lastAlpha = isAlpha; // hopefully this is the only bug
            }
            if (reject == true)
            {
                thisList = new CustomBasicList<Tuple<string, int?>>();
                return thisList;
            }
            if (hadNumber == false && thisList.Count == 0)
            {
                thisList.Add(new Tuple<string, int?>(thisStr, default));
            }
            else if (hadStr == false && thisList.Count == 0 && hadNumber == true)
            {
                thisList.Add(new Tuple<string, int?>("", int.Parse(thisStr)));
            }
            else if (hadStr == true && hadNumber == true)
            {
                int? ThisInt;
                if (item2 == "")
                    ThisInt = default;
                else
                    ThisInt = int.Parse(item2);
                Tuple<string, int?> ThisTuple = new Tuple<string, int?>(item1, ThisInt);
                thisList.Add(ThisTuple);
            }
            else
            {
                thisList = new CustomBasicList<Tuple<string, int?>>();
                return thisList;
            }
            return thisList;
        }
        public static bool IsCompleteAlpha(this string thisStr)
        {
            if (thisStr.Where(Items => Items.IsAlpha() == true).Any() == true)
                return false;
            return true;
        }
        public static CustomBasicList<string> GetRange(this CustomBasicList<string> thisList, string startWithString, string endWithString)
        {
            int firstIndex = thisList.IndexOf(startWithString);
            int secondIndex = thisList.IndexOf(endWithString);
            if (firstIndex == -1)
                throw new BasicBlankException(startWithString + " is not found for the start string");
            if (secondIndex == -1)
                throw new BasicBlankException(endWithString + " is not found for the end string");
            if (firstIndex > secondIndex)
                throw new BasicBlankException("The first string appears later in the last than the second string");
            return (thisList.Skip(firstIndex).Take(secondIndex - firstIndex + 1)).ToCustomBasicList();
        }
        public static CustomBasicList<string> Split(this string thisStr, string words)
        {
            int oldCount = thisStr.Count();
            CustomBasicList<string> tempList = new CustomBasicList<string>();
            do
            {
                if (thisStr.Contains(words) == false)
                {
                    if (thisStr != "")
                        tempList.Add(thisStr);
                    return tempList;
                }

                tempList.Add(thisStr.Substring(0, thisStr.IndexOf(words)));
                if (tempList.Count > oldCount)
                    throw new Exception("Can't be more than " + oldCount);
                thisStr = thisStr.Substring(thisStr.IndexOf(words) + words.Count());
            }
            while (true);
        }
        public static string Join(this CustomBasicList<string> thisList, string words)
        {
            string newWord = "";
            thisList.ForEach(Temps =>
            {
                if (newWord == "")
                    newWord = Temps;
                else
                    newWord = newWord + words + Temps;
            });
            return newWord;
        }
        public static string Join(this CustomBasicList<string> thisList, string words, int skip, int take)
        {
            var newList = (thisList.Skip(skip)).ToCustomBasicList();
            if (take > 0)
                newList = (thisList.Take(take)).ToCustomBasicList();
            return Join(newList, words);
        }
        public static bool ContainNumbers(this string thisStr)
        {

            return thisStr.Where(Items => char.IsNumber(Items) == true).Any();
        }

        public static string PartialString(this string fullString, string searchFor, bool beginning)
        {
            if (fullString.Contains(searchFor) == false)
                throw new Exception(searchFor + " is not contained in " + fullString);
            if (beginning == true)
                return fullString.Substring(0, fullString.IndexOf(searchFor)).Trim();
            return fullString.Substring(fullString.IndexOf(searchFor) + searchFor.Count()).Trim();
        }
        public static bool ContainsFromList(this string thisStr, CustomBasicList<string> thisList)
        {
            string temps;
            temps = thisStr.ToLower();
            foreach (var thisItem in thisList)
            {
                var news = thisItem.ToLower();
                if (temps.Contains(news) == true)
                    return true;
            }
            return false;
        }
        public static bool PostalCodeValidForUS(this string postalCode)
        {
            if (postalCode.Length < 5)
                return false;
            if (postalCode.Length == 5)
                return int.TryParse(postalCode, out _);
            if (postalCode.Length == 9)
                return int.TryParse(postalCode, out _);
            if (postalCode.Length == 10)
            {
                int index;
                index = postalCode.IndexOf("-");
                if (index != 5)
                    return false;
                postalCode = postalCode.Replace("-", "");
                return int.TryParse(postalCode, out _);
            }
            return false;
        }
        public static string GetWords(this string thisWord) // each upper case will represent a word.  for now; will not publish to bob's server.  if i need this function or needed for bob; then rethink that process
        {
            var tempList = thisWord.ToCustomBasicList();
            int x = 0;
            string newText = "";
            if (thisWord.Contains(" ") == true)
                throw new Exception(thisWord + " cannot contain spaces already");
            tempList.ForEach(thisItem =>
            {
                if (char.IsUpper(thisItem) == true && x > 0)
                    newText = newText + " " + thisItem;
                else
                    newText += thisItem;
                x += 1;
            });
            newText = newText.Replace("I P ", " IP ");
            return newText;
        }


        //looks like there are some serious setbacks.
        //if i need extension to convert, needs to figure out what to do.
        //for now, i should comment and see what happens.
        //try to not even convertcase.

        //public static void ConvertCase(this string info, bool doAll)
        //{
        //    string tempStr = "";
        //    bool isSpaceOrDot = false;
        //    if (doAll)
        //    {
        //        var loopTo = info.Length - 1;
        //        for (int i = 0; i <= loopTo; i++)
        //        {
        //            if (info[i].ToString() != " " & info[i].ToString() != ".")
        //            {
        //                if (i == 0 | isSpaceOrDot)
        //                {
        //                    tempStr += char.ToUpper(info[i]);
        //                    isSpaceOrDot = false;
        //                }
        //                else
        //                    tempStr += char.ToLower(info[i]);
        //            }
        //            else
        //            {
        //                isSpaceOrDot = true;
        //                tempStr += info[i];
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var loopTo1 = info.Length - 1;
        //        for (int i = 0; i <= loopTo1; i++)
        //        {
        //            if (info[i].ToString() != " " & info[i].ToString() != ".")
        //            {
        //                if (isSpaceOrDot)
        //                {
        //                    tempStr += char.ToUpper(info[i]);
        //                    isSpaceOrDot = false;
        //                }
        //                else if (i == 0)
        //                    tempStr += char.ToUpper(info[0]);
        //                else
        //                    tempStr += char.ToLower(info[i]);
        //            }
        //            else
        //            {
        //                if (info[i].ToString() != " ")
        //                    isSpaceOrDot = !isSpaceOrDot;
        //                tempStr += info[i];
        //            }
        //        }
        //    }
        //    info = tempStr;
        //}
        //public static void RemoveKeyWords(this string Text, List<string> WordList, bool DoAll)
        //{
        //    if (Text == "")
        //        return;
        //    string FirstText;
        //    FirstText = Text.ToLower();
        //    string Fins;
        //    foreach (var ThisWord in WordList)
        //    {
        //        Fins = ThisWord.ToLower();
        //        FirstText = FirstText.Replace(Fins, "");
        //    }
        //    if (FirstText == "")
        //    {
        //        Text = "";
        //        return;
        //    }
        //    do
        //    {
        //        if (FirstText.Contains("  ") == false)
        //            break;
        //        FirstText = FirstText.Replace("  ", " ");
        //    }
        //    while (true);
        //    FirstText.ConvertCase(DoAll);
        //    FirstText = FirstText.Trim();
        //    Text = FirstText;
        //}

        //public static void CleanAddresses(this string FullAddress, string Address2)
        //{
        //    if (Address2 == "")
        //        return;
        //    if (FullAddress.Contains(Address2) == true)
        //        FullAddress = FullAddress.Replace(Address2, "");
        //    FullAddress = FullAddress.Trim();
        //}

        //public static bool IsPOBox(this string Address)
        //{
        //    Address = Address.ToLower();
        //    if (Address.StartsWith("box ") == true)
        //        return true;
        //    List<string> ReplaceList = new List<string>() { " ", "." };
        //    Address.RemoveKeyWords(ReplaceList, true);
        //    Address = Address.ToLower();
        //    if (Address.Contains("(") == true)
        //        return false;// i think
        //    if (Address.Contains("pobox") == true)
        //        return true;
        //    return false;
        //}

        public static string FixBase64ForFileData(this string str_Image)
        {
            // *** Need to clean up the text in case it got corrupted travelling in an XML file
            // i think its best to have as public.  because its possible its only corrupted because of this.
            // has had the experience before with smart phones.
            // however; with mango and windows phones 7; I can use a compact edition database (which would be very helpful).
            // if doing this; then what would have to happen is I would have to have a method to check back in the music information.
            // maybe needs to be xml afterall (don't know though).  otherwise; may have to do serializing/deserializing.
            // some stuff is iffy at this point.
            StringBuilder sbText = new System.Text.StringBuilder(str_Image, str_Image.Length);
            sbText.Replace(@"\r\n", string.Empty);
            sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }

        public static void SaveFile(this string data, string path)
        {
            byte[] Bytes = System.Convert.FromBase64String(data);
            System.IO.FileStream FileStream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            FileStream.Write(Bytes, 0, Bytes.Length);
            FileStream.Flush();
            FileStream.Close();
        }

        public async static Task SaveFileAsync(this string data, string path)
        {
            byte[] Bytes = System.Convert.FromBase64String(data);
            FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            await fileStream.WriteAsync(Bytes, 0, Bytes.Length);
            await fileStream.FlushAsync();
            fileStream.Close();
            fileStream.Dispose(); // did not have dispose before though
        }

        public static string GetFileData(this string path)
        {
            FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            byte[] Bytes = new byte[fileStream.Length - 1 + 1];
            fileStream.Read(Bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            return System.Convert.ToBase64String(Bytes);
        }

        public async static Task<string> GetFileDataAsync(this string path)
        {
            FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            byte[] Bytes = new byte[fileStream.Length - 1 + 1];
            await fileStream.ReadAsync(Bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            return System.Convert.ToBase64String(Bytes);
        }
        public static CustomBasicList<string> GenerateSentenceList(this string entireText)
        {
            return entireText.Split(Constants.vbCrLf).ToCustomBasicList();
        }
        public static string BodyFromStringList(this CustomBasicList<string> thisList)
        {
            if (thisList.Count == 0)
                throw new Exception("Must have at least one item in order to get the body from the string list");
            StrCat cats = new StrCat();
            thisList.ForEach(ThisItem =>
            {
                cats.AddToString(ThisItem, Constants.vbCrLf);
            });
            return cats.GetInfo();
        }
        public static int GetColumnNumber(this string columnString) // will be 0 based
        {
            string newStr = columnString.ToLower();
            CustomBasicList<string> AlphabetList = new CustomBasicList<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            var tempList = newStr.ToList();
            if (tempList.Count > 2)
                throw new Exception("Currently; only 2 digit strings can be done for figuring out the column number");
            var index = AlphabetList.IndexOf(tempList.First().ToString());
            if (index == -1)
                throw new Exception(tempList.First() + " is not part of the alphabet for the first digit of the string");
            if (tempList.Count == 1)
                return index;
            index += 1;
            var finalIndex = AlphabetList.IndexOf(tempList.Last().ToString());
            if (finalIndex == -1)
                throw new Exception(tempList.Last() + " is not part of the alphabet for the last digit of the string");
            return (index * 26) + finalIndex;
        }
        public static (int Days, int Hours, int Minutes) GetTime(this string timeString)
        {
            BasicBlankException thisEx = new BasicBlankException("Incorrect.  Should have used validation");
            _previousTime = (0, 0, 0);
            var tempList = timeString.Split(':').ToList();
            bool rets;
            int newInt;
            if (tempList.Count == 1)
            {
                rets = int.TryParse(timeString, out newInt);
                if (rets == false)
                    throw thisEx;
                return (0, 0, newInt);
            }
            if (tempList.Count > 3)
                throw thisEx;
            CustomBasicList<int> newList = new CustomBasicList<int>();
            foreach (var thisItem in tempList)
            {
                rets = int.TryParse(thisItem, out newInt);
                if (rets == false)
                    throw thisEx;
                newList.Add(newInt);
            }
            int d;
            int h;
            int m;
            if (newList.Count == 2)
            {
                d = 0;
                h = newList.First();
                m = newList.Last();
            }
            else
            {
                d = newList.First();
                h = newList[1];
                m = newList.Last();
            }
            _previousString = timeString;
            _previousTime = (d, h, m);
            return (d, h, m);
        }
        private static string _previousString = "";
        private static (int Days, int Hours, int Minutes) _previousTime;
        public static int GetTotalSeconds(this string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString) == true)
                throw new BasicBlankException("Never got the time using the GetTime format");
            if (timeString != _previousString)
                throw new BasicBlankException("You did not use the sanme string as when using the GetTime function");
            TimeSpan thisSpan = new TimeSpan(_previousTime.Days, _previousTime.Hours, _previousTime.Minutes, 0);
            return (int)thisSpan.TotalSeconds;
        }
    }
}