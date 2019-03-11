using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicStandardLibraries.Exceptions;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Strings
    {



        private static string MonthReplace;

        public static bool IsNumeric(this string ThisStr)
        {
            return int.TryParse(ThisStr, out _); //you are forced to assign variable no matter what now.
        } //sometimes it did without the 2 braces but not this time.


        public static int FindMonthInStringLine(this string ThisStr) // will return a number  this will assume that there is never a case where 2 months appear
        {
            List<string> PossList = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            int PossNum;
            PossNum = 0;
            MonthReplace = "";
            int CurrentNum;
            foreach (var ThisPoss in PossList)
            {
                if (ThisStr.Contains(ThisPoss) == true)
                {
                    CurrentNum = ThisPoss.GetMonthID();
                    if (CurrentNum > 0)
                    {
                        MonthReplace = ThisPoss;
                        if (PossNum > 0)
                            throw new Exception("There should not have been 2 months in the same line.  Rethink");
                        PossNum = CurrentNum;
                    }
                }
            }
            return PossNum;
        }

        public static List<string> SplitStringEliminateMonth(this string ThisStr)
        {
            ThisStr = ThisStr.Trim();
            if (MonthReplace == null == true || MonthReplace == "")
                return new List<string>() { ThisStr };
            var ThisList = ThisStr.Split(MonthReplace);
            if (ThisList.Count != 2)
                throw new Exception("There should be only 2 items splited, not " + ThisList.Count);
            List<string> NewList = new List<string>();
            foreach (var ThisItem in ThisList)
            {
                NewList.Add(ThisItem.Trim());
            }
            return NewList;
        }


        public static int GetMonthID(this string MonthString)
        {
            // if nothing found, then return 0
            if (MonthString == null == true)
                return 0;// if return nothing, then return 0
            if (MonthString == "January")
                return 1;
            if (MonthString == "February")
                return 2;
            if (MonthString == "March")
                return 3;
            if (MonthString == "April")
                return 4;
            if (MonthString == "May")
                return 5;
            if (MonthString == "June")
                return 6;
            if (MonthString == "July")
                return 7;
            if (MonthString == "August")
                return 8;
            if (MonthString == "September")
                return 9;
            if (MonthString == "October")
                return 10;
            if (MonthString == "November")
                return 11;
            if (MonthString == "December")
                return 12;
            return 0;
        }

        public static List<string> GetSentences(this string sTextToParse)
        {
            List<string> al = new List<string>();
            string sTemp = sTextToParse;
            sTemp = sTemp.Replace(Environment.NewLine, " ");
            string[] CustomSplit = new[] { ".", "?", "!", ":" };
            var Splits = sTemp.Split(CustomSplit, StringSplitOptions.RemoveEmptyEntries).ToList();
            int pos;
            foreach (var ThisSplit in Splits)
            {
                pos = sTemp.IndexOf(ThisSplit);
                var ThisChar = sTemp.Trim().ToCharArray();
                if (pos + ThisSplit.Length <= ThisChar.Length - 1)
                {
                    char c = ThisChar[pos + ThisSplit.Length];
                    al.Add(ThisSplit.Trim() + c.ToString());
                    sTemp = sTemp.Replace(ThisSplit, ""); // because already accounted for.
                }
                else
                    al.Add(ThisSplit);
            }
            if (al.First().StartsWith("\"") == true)
                throw new Exception("I don't think the first one can start with quotes");
            int x;
            var loopTo = al.Count - 1; // this is used so the quotes go into the proper places.
            for (x = 1; x <= loopTo; x++)
            {
                string FirstItem;
                string SecondItem;
                FirstItem = al[x - 1];
                SecondItem = al[x];
                if (SecondItem.StartsWith("\"") == true)
                {
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + "\"";
                    al[x - 1] = al[x - 1].Trim();
                }
                else if (SecondItem.StartsWith(")") == true)
                {
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + ")";
                    al[x - 1] = al[x - 1].Trim();
                }
                else if (SecondItem.Length == 1)
                {
                    var ThisStr = SecondItem.ToString();
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + ThisStr;
                    al[x - 1] = al[x - 1].Trim();
                }
            }

            int Numbers = al.Where(Items => Items == "").Count();
            
            int Opening = al.Where(Items => Items == "(").Count();
            int Closing = al.Where(Items => Items == ")").Count();

            foreach (var ThisItem in al)
            {


                if (Numbers == 1 || Numbers == 3)
                    throw new Exception("Quotes are not correct.  Has " + Numbers + " Quotes");


                if (Opening != Closing)
                    throw new Exception("Opening and closing much match for ( and ) characters");
            }

            al = (from Items in al
                  where Items != ""
                  select Items).ToList();
            return al;
        }

        public static string StripHtml(this string HtmlText)
        {
            var ThisText = Regex.Replace(HtmlText, "<.*?>", "");
            if (ThisText.Contains("<sup") == true)
            {
                var Index = ThisText.IndexOf("<sup");
                ThisText = ThisText.Substring(0, Index);
            }
            if (ThisText.Contains("<div class=" + "\"") == true)
                ThisText = ThisText.Replace("<div class=" + "\"", "");
            if (ThisText.Contains("<a") == true)
            {
                var Index = ThisText.IndexOf("<a");
                ThisText = ThisText.Substring(0, Index);
            }
            ThisText = ThisText.Replace("[a]", "");
            ThisText = ThisText.Replace("[b]", ""); // because even if its b, needs to go away as well.
            ThisText = ThisText.Replace("[c]", "");
            ThisText = ThisText.Replace("[d]", "");
            ThisText = ThisText.Replace("[e]", "");
            ThisText = ThisText.Replace("[f]", "");
            ThisText = ThisText.Replace("[g]", "");
            ThisText = ThisText.Replace("[h]", "");
            ThisText = ThisText.Replace("[i]", "");
            ThisText = ThisText.Replace("[j]", "");
            ThisText = ThisText.Replace("[k]", "");
            ThisText = ThisText.Replace("[l]", "");
            ThisText = ThisText.Replace("[m]", "");
            ThisText = ThisText.Replace("[n]", "");
            ThisText = ThisText.Replace("[o]", "");
            ThisText = ThisText.Replace("[p]", "");
            ThisText = ThisText.Replace("[q]", "");
            ThisText = ThisText.Replace("[r]", "");
            ThisText = ThisText.Replace("[s]", "");
            ThisText = ThisText.Replace("[t]", "");
            ThisText = ThisText.Replace("[u]", "");
            ThisText = ThisText.Replace("[v]", "");
            ThisText = ThisText.Replace("[w]", "");
            ThisText = ThisText.Replace("[x]", "");
            ThisText = ThisText.Replace("[y]", "");
            ThisText = ThisText.Replace("[z]", "");
            if (ThisText.Contains("<") == true || ThisText.Contains(">") == true)
            {
            }

            var NextText = System.Net.WebUtility.HtmlDecode(ThisText);
            return NextText.Trim();
        }

        public static int GetSeconds(this string TimeString)
        {
            var TempList = TimeString.Split(":").ToList();
            if (TempList.Count > 3)
                throw new Exception("Can't handle more than 3 :");
            if (TempList.Count == 3)
            {
                int FirstNum;
                int SecondNum;
                int ThirdNum;
                FirstNum = int.Parse(TempList.First().ToString());
                SecondNum = int.Parse(TempList[1]);
                ThirdNum = int.Parse(TempList.Last());
                int FirstSecs;
                FirstSecs = FirstNum * 60 * 60;
                var SecondSecs = SecondNum * 60;
                var ThirdSecs = ThirdNum;
                return FirstSecs + SecondSecs + ThirdSecs;
            }

            if (TempList.Count == 2)
            {
                int FirstSecs = int.Parse(TempList.First()) * 60;
                return FirstSecs + int.Parse(TempList.Last());
            }
            if (TempList.Count == 0)
                throw new Exception("I think its wrong");
            if (TempList.Count == 1)
                throw new Exception("Should just return as is");
            throw new Exception("Not sure");
        }

        public static bool IsValidDate(this string DateStr, out DateTime? NewDate) // can't use nothing as a variable type.  too bad the function did not allow that.  can try one more thing
        {
            string ThisText;
            ThisText = DateStr;
            NewDate = null;
            if (ThisText.Length != 6)
                return false;
            var NewText = ThisText.Substring(0, 2) + "/" + ThisText.Substring(2, 2) + "/" + ThisText.Substring(4, 2);
            bool rets;
            rets = DateTime.TryParse(NewText, out DateTime TempDate);
            if (rets == false)
                return false;
            NewDate = TempDate;
            return true; // i think
        }

        public static List<Tuple<string, int?>> GetStringIntegerCombos(this string ThisStr)
        {
            List<Tuple<string, int?>> ThisList = new List<Tuple<string, int?>>();
            if (ThisStr == "")
                return ThisList;
            string Item1;
            Item1 = "";
            string Item2;
            Item2 = "";
            int x = 0;
            // if the first item is a number, then the first string will be blank and the second part will be a integer.  this is used for games like farmville where there is a string and number associated to it.  if 0 is entered, then will return 0 items.  its up to the process to decide what to do with a blank number
            bool HadNumber = false;
            bool HadStr = false;
            bool Reject = false;
            bool LastAlpha = false;
            foreach (var ThisItem in ThisStr.ToList())
            {
                x += 1;
                bool IsAlpha;
                IsAlpha = ThisItem.IsAlpha(true);
                bool IsInt;
                IsInt = ThisItem.IsInteger();
                if (IsInt == true)
                    HadNumber = true;
                else if (IsAlpha == true)
                    HadStr = true;
                if (IsInt == false && IsAlpha == false)
                {
                    Reject = true;
                    break;
                }
                if (IsAlpha == true && IsInt == true)
                    throw new Exception("Cannot be both alpha and integers.");
                if (x == 1)
                    Item1 = ThisItem.ToString();
                else if (LastAlpha == true && IsAlpha == true)
                    Item1 += ThisItem;
                else if (LastAlpha == false && IsInt == true)
                    Item2 += ThisItem;
                else if (LastAlpha == true && IsInt == true)
                    Item2 = ThisItem.ToString(); // start of a number
                else
                {
                    int? ThisInt;
                    if (Item2 == "")
                        ThisInt = default;
                    else
                        ThisInt = int.Parse(Item2);
                    Tuple<string, int?> ThisTuple = new Tuple<string, int?>(Item1, ThisInt);
                    ThisList.Add(ThisTuple);
                    Item1 = ThisItem.ToString(); // try this way.
                    Item2 = null; // try this
                }
                LastAlpha = IsAlpha; // hopefully this is the only bug
            }
            if (Reject == true)
            {
                ThisList = new List<Tuple<string, int?>>();
                return ThisList;
            }
            if (HadNumber == false && ThisList.Count == 0)
                ThisList.Add(new Tuple<string, int?>(ThisStr, default));
            else if (HadStr == false && ThisList.Count == 0 && HadNumber == true)
                ThisList.Add(new Tuple<string, int?>("", int.Parse(ThisStr)));
            else if (HadStr == true && HadNumber == true)
            {
                int? ThisInt;
                if (Item2 == "")
                    ThisInt = default;
                else
                    ThisInt = int.Parse(Item2);
                Tuple<string, int?> ThisTuple = new Tuple<string, int?>(Item1, ThisInt);
                ThisList.Add(ThisTuple);
            }
            else
            {
                ThisList = new List<Tuple<string, int?>>();
                return ThisList;
            }
            return ThisList;
        }

        public static bool IsCompleteAlpha(this string ThisStr)
        {
            if (ThisStr.Where(Items => Items.IsAlpha() == true).Any() == true)
                return false;
            return true;
        }

        public static List<string> GetRange(this List<string> ThisList, string StartWithString, string EndWithString)
        {
            int FirstIndex = ThisList.IndexOf(StartWithString);
            int SecondIndex = ThisList.IndexOf(EndWithString);
            if (FirstIndex == -1)
                throw new Exception(StartWithString + " is not found for the start string");
            if (SecondIndex == -1)
                throw new Exception(EndWithString + " is not found for the end string");
            if (FirstIndex > SecondIndex)
                throw new Exception("The first string appears later in the last than the second string");
            return (ThisList.Skip(FirstIndex).Take(SecondIndex - FirstIndex + 1)).ToList();
        }

        public static List<string> Split(this string ThisStr, string Words)
        {
            int OldCount = ThisStr.Count();
            List<string> TempList = new List<string>();
            do
            {
                if (ThisStr.Contains(Words) == false)
                {
                    if (ThisStr != "")
                        TempList.Add(ThisStr);
                    return TempList;
                }

                TempList.Add(ThisStr.Substring(0, ThisStr.IndexOf(Words)));
                if (TempList.Count > OldCount)
                    throw new Exception("Can't be more than " + OldCount);
                ThisStr = ThisStr.Substring(ThisStr.IndexOf(Words) + Words.Count());
            }
            while (true);
        }

        public static string Join(this List<string> ThisList, string Words)
        {
            string NewWord = "";
            ThisList.ForEach(Temps =>
            {
                if (NewWord == "")
                    NewWord = Temps;
                else
                    NewWord = NewWord + Words + Temps;
            });
            return NewWord;
        }

        public static string Join(this List<string> ThisList, string Words, int Skip, int Take)
        {
            var NewList = (ThisList.Skip(Skip)).ToList();
            if (Take > 0)
                NewList = (ThisList.Take(Take)).ToList();
            return Join(NewList, Words);
        }

        public static bool ContainNumbers(this string ThisStr)
        {

            return ThisStr.Where(Items => char.IsNumber(Items) == true).Any();
        }

        public static string PartialString(this string FullString, string SearchFor, bool Beginning)
        {
            if (FullString.Contains(SearchFor) == false)
                throw new Exception(SearchFor + " is not contained in " + FullString);
            if (Beginning == true)
                return FullString.Substring(0, FullString.IndexOf(SearchFor)).Trim();
            return FullString.Substring(FullString.IndexOf(SearchFor) + SearchFor.Count()).Trim();
        }

        public static bool ContainsFromList(this string ThisStr, List<string> ThisList)
        {
            string Temps;
            Temps = ThisStr.ToLower();
            foreach (var ThisItem in ThisList)
            {
                var News = ThisItem.ToLower();
                if (Temps.Contains(News) == true)
                    return true;
            }
            return false;
        }

        public static bool PostalCodeValidForUS(this string PostalCode)
        {
            if (PostalCode.Length < 5)
                return false;
            if (PostalCode.Length == 5)
                return int.TryParse(PostalCode, out _);
            if (PostalCode.Length == 9)
                return int.TryParse(PostalCode, out _);
            if (PostalCode.Length == 10)
            {
                int Index;
                Index = PostalCode.IndexOf("-");
                if (Index != 5)
                    return false;
                PostalCode = PostalCode.Replace("-", "");
                return int.TryParse(PostalCode, out _);
            }
            return false;
        }

        public static string GetWords(this string ThisWord) // each upper case will represent a word.  for now; will not publish to bob's server.  if i need this function or needed for bob; then rethink that process
        {
            var TempList = ThisWord.ToList();
            int x = 0;
            string NewText = "";
            if (ThisWord.Contains(" ") == true)
                throw new Exception(ThisWord + " cannot contain spaces already");
            TempList.ForEach(ThisItem =>
            {
                if (char.IsUpper(ThisItem) == true && x > 0)
                    NewText = NewText + " " + ThisItem;
                else
                    NewText += ThisItem;
                x += 1;
            });
            NewText = NewText.Replace("I P ", " IP ");
            return NewText;
        }

        public static void ConvertCase(this string Info, bool DoAll)
        {
            string tempStr = "";
            bool isSpaceOrDot = false;
            if (DoAll)
            {
                var loopTo = Info.Length - 1;
                for (int i = 0; i <= loopTo; i++)
                {
                    if (Info[i].ToString() != " " & Info[i].ToString() != ".")
                    {
                        if (i == 0 | isSpaceOrDot)
                        {
                            tempStr += char.ToUpper(Info[i]);
                            isSpaceOrDot = false;
                        }
                        else
                            tempStr += char.ToLower(Info[i]);
                    }
                    else
                    {
                        isSpaceOrDot = true;
                        tempStr += Info[i];
                    }
                }
            }
            else
            {
                var loopTo1 = Info.Length - 1;
                for (int i = 0; i <= loopTo1; i++)
                {
                    if (Info[i].ToString() != " " & Info[i].ToString() != ".")
                    {
                        if (isSpaceOrDot)
                        {
                            tempStr += char.ToUpper(Info[i]);
                            isSpaceOrDot = false;
                        }
                        else if (i == 0)
                            tempStr += char.ToUpper(Info[0]);
                        else
                            tempStr += char.ToLower(Info[i]);
                    }
                    else
                    {
                        if (Info[i].ToString() != " ")
                            isSpaceOrDot = !isSpaceOrDot;
                        tempStr += Info[i];
                    }
                }
            }
            Info = tempStr;
        }

        public static void RemoveKeyWords(this string Text, List<string> WordList, bool DoAll)
        {
            if (Text == "")
                return;
            string FirstText;
            FirstText = Text.ToLower();
            string Fins;
            foreach (var ThisWord in WordList)
            {
                Fins = ThisWord.ToLower();
                FirstText = FirstText.Replace(Fins, "");
            }
            if (FirstText == "")
            {
                Text = "";
                return;
            }
            do
            {
                if (FirstText.Contains("  ") == false)
                    break;
                FirstText = FirstText.Replace("  ", " ");
            }
            while (true);
            FirstText.ConvertCase(DoAll);
            FirstText = FirstText.Trim();
            Text = FirstText;
        }

        public static void CleanAddresses(this string FullAddress, string Address2)
        {
            if (Address2 == "")
                return;
            if (FullAddress.Contains(Address2) == true)
                FullAddress = FullAddress.Replace(Address2, "");
            FullAddress = FullAddress.Trim();
        }

        public static bool IsPOBox(this string Address)
        {
            Address = Address.ToLower();
            if (Address.StartsWith("box ") == true)
                return true;
            List<string> ReplaceList = new List<string>() { " ", "." };
            Address.RemoveKeyWords(ReplaceList, true);
            Address = Address.ToLower();
            if (Address.Contains("(") == true)
                return false;// i think
            if (Address.Contains("pobox") == true)
                return true;
            return false;
        }

        public static string FixBase64ForFileData(this string str_Image)
        {
            // *** Need to clean up the text in case it got corrupted travelling in an XML file
            // i think its best to have as public.  because its possible its only corrupted because of this.
            // has had the experience before with smart phones.
            // however; with mango and windows phones 7; I can use a compact edition database (which would be very helpful).
            // if doing this; then what would have to happen is I would have to have a method to check back in the music information.
            // maybe needs to be xml afterall (don't know though).  otherwise; may have to do serializing/deserializing.
            // some stuff is iffy at this point.
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(str_Image, str_Image.Length);
            sbText.Replace(@"\r\n", string.Empty);
            sbText.Replace(" ", string.Empty);
            return sbText.ToString();
        }

        public static void SaveFile(this string Data, string Path)
        {
            System.Byte[] Bytes = System.Convert.FromBase64String(Data);
            System.IO.FileStream FileStream = new System.IO.FileStream(Path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            FileStream.Write(Bytes, 0, Bytes.Length);
            FileStream.Flush();
            FileStream.Close();
        }

        public async static Task SaveFileAsync(this string Data, string Path)
        {
            System.Byte[] Bytes = System.Convert.FromBase64String(Data);
            System.IO.FileStream FileStream = new System.IO.FileStream(Path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            await FileStream.WriteAsync(Bytes, 0, Bytes.Length);
            await FileStream.FlushAsync();
            FileStream.Close();
            FileStream.Dispose(); // did not have dispose before though
        }

        public static string GetFileData(this string Path)
        {
            System.IO.FileStream FileStream = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            System.Byte[] Bytes = new System.Byte[FileStream.Length - 1 + 1];
            FileStream.Read(Bytes, 0, (int)FileStream.Length);
            FileStream.Close();
            return System.Convert.ToBase64String(Bytes);
        }

        public async static Task<string> GetFileDataAsync(this string Path)
        {
            System.IO.FileStream FileStream = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            System.Byte[] Bytes = new System.Byte[FileStream.Length - 1 + 1];
            await FileStream.ReadAsync(Bytes, 0, (int)FileStream.Length);
            FileStream.Close();
            return System.Convert.ToBase64String(Bytes);
        }

        public static List<string> GenerateSentenceList(this string EntireText)
        {
            return EntireText.Split(Constants.vbCrLf).ToList();
        }


        public static string BodyFromStringList(this List<string> ThisList)
        {
            if (ThisList.Count == 0)
                throw new Exception("Must have at least one item in order to get the body from the string list");
            StrCat cats = new StrCat();
            ThisList.ForEach(ThisItem =>
            {
                cats.AddToString(ThisItem, Constants.vbCrLf);
            });
            return cats.GetInfo();
        }

        public static int GetColumnNumber(this string ColumnString) // will be 0 based
        {
            string NewStr = ColumnString.ToLower();
            List<string> AlphabetList = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            var TempList = NewStr.ToList();
            if (TempList.Count > 2)
                throw new Exception("Currently; only 2 digit strings can be done for figuring out the column number");
            var Index = AlphabetList.IndexOf(TempList.First().ToString());
            if (Index == -1)
                throw new Exception(TempList.First() + " is not part of the alphabet for the first digit of the string");
            if (TempList.Count == 1)
                return Index;
            Index += 1;
            var FinalIndex = AlphabetList.IndexOf(TempList.Last().ToString());
            if (FinalIndex == -1)
                throw new Exception(TempList.Last() + " is not part of the alphabet for the last digit of the string");
            return (Index * 26) + FinalIndex;
        }

		public static (int Days, int Hours, int Minutes) GetTime(this string TimeString)
		{

			Exception ThisEx = new BasicBlankException("Incorrect.  Should have used validation");
			PreviousTime = (0, 0, 0);
			var TempList = TimeString.Split(':').ToList();
			bool rets;
			int NewInt;

			if (TempList.Count == 1)
			{
				rets = int.TryParse(TimeString, out NewInt);
				if (rets == false)
					throw ThisEx;

				return (0, 0, NewInt);

			}
			// Dim TempList = TimeString.Split(":").ToList
			if (TempList.Count > 3)
				throw ThisEx;


			List<int> NewList = new List<int>();
			foreach (var ThisItem in TempList)
			{
				rets = int.TryParse(ThisItem, out NewInt);
				if (rets == false)
					throw ThisEx;
				NewList.Add(NewInt);
			}
			int D;
			int H;
			int M;
			if (NewList.Count == 2)
			{
				D = 0;
				H = NewList.First();
				M = NewList.Last();
			}
			else
			{
				D = NewList.First();
				H = NewList[1];
				M = NewList.Last();
			}
			PreviousString = TimeString;
			PreviousTime = (D, H, M);
			return (D, H, M);
		}
		private static string PreviousString;

		private static (int Days, int Hours, int Minutes) PreviousTime;

		public static int GetTotalSeconds(this string TimeString)
		{
			if (string.IsNullOrWhiteSpace(TimeString) == true)
				throw new BasicBlankException("Never got the time using the GetTime format");
			if (TimeString != PreviousString)
				throw new BasicBlankException("You did not use the sanme string as when using the GetTime function");
			TimeSpan ThisSpan = new TimeSpan(PreviousTime.Days, PreviousTime.Hours, PreviousTime.Minutes, 0);
			return (int) ThisSpan.TotalSeconds;
		}


	}
}
