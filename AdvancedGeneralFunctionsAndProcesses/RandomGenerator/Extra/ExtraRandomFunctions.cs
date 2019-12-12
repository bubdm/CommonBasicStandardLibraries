using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator.RandomGenerator.Data;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public partial class RandomGenerator
    {

        #region Basic

        public string NextString(int howMany, string stringsToPick) //i think i could go ahead and allow this as public.
        {
            CustomBasicList<char> tempList = stringsToPick.ToCustomBasicList();
            StringBuilder thisStr = new StringBuilder();
            for (int i = 0; i < howMany; i++)
            {
                thisStr.Append(tempList.GetRandomItem());
            }
            return thisStr.ToString();
        }

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="decimals">Decimals count.</param>
        /// <returns>Returns random generated double.</returns>
        internal double NextDouble(double min = double.MinValue + 1.0, double max = double.MaxValue - 1.0, uint decimals = 4)
        {
            var _fixed = Math.Pow(10, decimals);
            var num = NextLong((int)(min * _fixed), (int)(max * _fixed));
            var numFixed = (num / _fixed).ToString("N" + decimals);

            return double.Parse(numFixed);
        }

        /// <summary>
        /// Returns a random long.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns a random long.</returns>
        internal long NextLong(long min = long.MinValue + 1, long max = long.MaxValue - 1)
        {
            if (min > max)
            {
                throw new ArgumentException("Min cannot be greater than Max.", nameof(min));
            }

            return (long)Math.Floor(_r!() * (max - min + 1) + min);
        }

        internal string GetDigits(int howMany, int startAt = 0, int endAt = 9)
        {
            StringBuilder ThisStr = new StringBuilder();
            for (int i = 0; i < howMany; i++)
            {
                ThisStr.Append(GetRandomNumber(endAt, startAt));
            }
            return ThisStr.ToString();
        }

        /// <summary>
        /// Return a semi-pronounceable random (nonsense) word.
        /// </summary>
        /// <param name="capitalize">True to capitalize a word.</param>
        /// <param name="syllablesCount">Number of syllables which the word will have.</param>
        /// <param name="length">Length of a word.</param>
        /// <returns>Returns random generated word.</returns>
        internal string NextWord(bool capitalize = false, int? syllablesCount = 2, int? length = null)
        {
            if (syllablesCount != null && length != null)
            {
                throw new ArgumentException("Cannot specify both syllablesCount AND length.");
            }

            var text = "";

            if (length.HasValue)
            {
                do
                {
                    text += NextSyllable();
                } while (text.Length < length.Value);

                text = text.Substring(0, length.Value);
            }
            else if (syllablesCount.HasValue)
            {
                for (var i = 0; i < syllablesCount.Value; i++)
                {
                    text += NextSyllable();
                }
            }

            if (capitalize)
            {
                text = text.Capitalize();
            }

            return text;
        }

        /// <summary>
        /// Return a semi-speakable syllable, 2 or 3 letters.
        /// </summary>
        /// <param name="length">Length of a syllable.</param>
        /// <param name="capitalize">True to capitalize a syllable.</param>
        /// <returns>Returns random generated syllable.</returns>
        internal string NextSyllable(int length = 2, bool capitalize = false)
        {
            const string consonats = "bcdfghjklmnprstvwz";
            const string vowels = "aeiou";
            const string all = consonats + vowels;
            var text = "";
            var chr = -1;

            for (var i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    chr = NextChar(all);
                }
                else if (consonats.IndexOf((char)chr) == -1) //(consonats[chr] == -1)
                {
                    chr = NextChar(consonats);
                }
                else
                {
                    chr = NextChar(vowels);
                }

                text += (char)chr;
            }

            if (capitalize)
            {
                text = text.Capitalize();
            }

            return text;
        }

        /// <summary>
        /// Returns a random character.
        /// </summary>
        /// <param name="pool">Characters pool</param>
        /// <param name="alpha">Set to True to use only an alphanumeric character.</param>
        /// <param name="symbols">Set to true to return only symbols.</param>
        /// <param name="casing">Default casing.</param>
        /// <returns>Returns a random character.</returns>
        internal char NextChar(string? pool = null, bool alpha = false, bool symbols = false, EnumCasingRules casing = EnumCasingRules.MixedCase)
        {
            const string s = "!@#$%^&*()[]";
            string letters, p;

            if (alpha && symbols)
            {
                throw new ArgumentException("Cannot specify both alpha and symbols.");
            }

            if (casing == EnumCasingRules.LowerCase)
            {
                letters = CharsLower;
            }
            else if (casing == EnumCasingRules.UpperCase)
            {
                letters = CharsUpper;
            }
            else
            {
                letters = CharsLower + CharsUpper;
            }

            if (!string.IsNullOrEmpty(pool))
            {
                p = pool;
            }
            else if (alpha)
            {
                p = letters;
            }
            else if (symbols)
            {
                p = s;
            }
            else
            {
                p = letters + Numbers + s;
            }
            CustomBasicList<char> ThisList = p.ToCustomBasicList();
            return ThisList.GetRandomItem();
            //return p[NextNatural(max: p.Length - 1)];
        }


        #endregion


        #region Person

        /// <summary>
        /// Generates a random age
        /// </summary>
        /// <param name="types">Age range.</param>
        /// <returns>Returns random generated age.</returns>
        public int NextAge(EnumAgeRanges types = EnumAgeRanges.Adult)
        {
            var range = types switch
            {
                EnumAgeRanges.Child => new[] { 1, 12 },
                EnumAgeRanges.Teen => new[] { 13, 19 },
                EnumAgeRanges.Senior => new[] { 65, 100 },
                EnumAgeRanges.All => new[] { 1, 100 },
                _ => new[] { 18, 65 },
            };
            return GetRandomNumber(range[1], range[0]);
        }
        public CustomBasicList<string> GetSeveralUniquePeople(int howMany)
        {
            HashSet<string> firstList = new HashSet<string>();
            do
            {
                firstList.Add(NextAnyName());
                if (howMany == firstList.Count)
                    return firstList.ToCustomBasicList();
            } while (true);
        }


        /// <summary>
        /// Generates a random first name.
        /// </summary>
        /// <param name="isFemale">True to generate female names.</param>
        /// <returns>Returns random generated first name.</returns>
        public string NextFirstName(bool isFemale = false)

        {
            CustomBasicList<string> listToUse;
            if (isFemale == false)
                listToUse = FirstNamesFemale;
            else
                listToUse = FirstNamesMale;
            listToUse.rs = this;
            return listToUse.GetRandomItem();
        }

        public string NextAnyName()
        {
            CustomBasicList<string> allList = FirstNamesFemale.ToCustomBasicList();
            allList.AddRange(FirstNamesMale);
            allList.rs = this;
            return allList.GetRandomItem();
        }

        /// <summary>
        /// Generates a random last name.
        /// </summary>
        /// <returns>Returns random generated last name.</returns>
        public string NextLastName()
        {
            var thisList = LastNames;
            thisList.rs = this;
            return thisList.GetRandomItem();
        }
        //public string NextLastName() => LastNames.GetRandomItem();

        /// <summary>
        /// Generates a random gender.
        /// </summary>
        /// <returns>Returns either <c>Male</c> or <c>Female</c>.</returns>
        public string NextSex()
        {
            CustomBasicList<string> thisList = new CustomBasicList<string> { "Male", "Female" };
            thisList.rs = this;
            return thisList.GetRandomItem();
        }

        /// <summary>
        /// Generates a random name.
        /// </summary>
        /// <param name="isMale">True to generate male names.</param>
        /// <param name="middle">True to use middle names.</param>
        /// <param name="isFull">True to use full name prefixes.</param>
        /// <param name="gender">Male of Female.</param>
        /// <returns>Returns random generated name.</returns>
        public string NextName(bool isMale = false, bool middle = false)
        {
            string first = NextFirstName(isMale),
                last = NextLastName(),
                name;

            if (middle)
            {
                name = $"{first} {NextFirstName(isMale)} {last}";
            }
            else
            {
                name = $"{first} {last}";
            }
            return name;
        }

        /// <summary>
        /// Generates a random social security number.
        /// </summary>
        /// <param name="ssnFour">True to generate last 4 digits.</param>
        /// <param name="dashes">False to remove dashes.</param>
        /// <returns>Returns random generated social security number.</returns>
        public string NextSSN(bool ssnFour = false, bool dashes = true)
        {
            const string ssnPool = "1234567890";
            string ssn, dash = dashes ? "-" : "";
            if (!ssnFour)
            {
                ssn = NextString(3, ssnPool) + dash + NextString(2, ssnPool) +
                      dash + NextString(4, ssnPool);
            }
            else
            {
                ssn = NextString(4, ssnPool);
            }

            return ssn;
        }



        #endregion


        #region Web


        /// <summary>
        /// Returns a random domain with a random Top Level Domain.
        /// </summary>
        /// <param name="tld">Custom Top Level Domain.</param>
        /// <returns>Returns random generated domain name.</returns>
        public string NextDomainName(string? tld = null) => NextWord() + "." + (tld ?? NextTopLevelDomain());

        /// <summary>
        /// Returns a random Top Level Domain.
        /// </summary>
        /// <returns>Returns a random Top Level Domain.</returns>
        public string NextTopLevelDomain() => Tlds.GetRandomItem();



        /// <summary>
        /// Returns a random email with a random domain.
        /// </summary>
        /// <param name="domain">Custom Top Level Domain</param>
        /// <param name="length">Length of an email address.</param>
        /// <returns>Returns a random email with a random domain.</returns>
        public string NextEmail(string? domain = null, int length = 7)
            => NextWord(length: length, syllablesCount: null) + "@" + (domain ?? NextDomainName());


        /// <summary>
        /// Returns a random hashtag. This is a string of the form ‘#thisisahashtag’.
        /// </summary>
        /// <returns>Returns a random hashtag.</returns>
        public string NextHashtag() => $"#{NextWord()}";

        /// <summary>
        /// Returns a random IP Address.
        /// </summary>
        /// <returns>Returns a random IP Address.</returns>
        public string NextIP()
            => $"{GetRandomNumber(254, 1)}.{GetRandomNumber(255, 0)}.{GetRandomNumber(255, 0)}.{GetRandomNumber(254, 0)}";


        /// <summary>
        /// Returns a random twitter handle.
        /// </summary>
        /// <returns>Returns a random twitter handle.</returns>
        public string NextTwitterName() => $"@{NextWord()}";

        /// <summary>
        /// Returns a random url.
        /// </summary>
        /// <param name="protocol">Custom protocol.</param>
        /// <param name="domain">Custom domain.</param>
        /// <param name="domainPrefix">Custom domain prefix.</param>
        /// <param name="path">Url path.</param>
        /// <param name="extensions">A list of Url extensions to pick one from.</param>
        /// <returns>Returns a random url.</returns>
        public string NextUrl(string protocol = "http", string? domain = null, string? domainPrefix = null, string? path = null, CustomBasicList<string>? extensions = null)
        {
            domain ??= NextDomainName();
            var ext = extensions != null && extensions.Any() ? "." + extensions.GetRandomItem() : "";
            var dom = !string.IsNullOrEmpty(domainPrefix) ? domainPrefix + "." + domain : domain;

            return $"{protocol}://{dom}/{path}{ext}";
        }

        /// <summary>
        /// Returns a random color.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="grayscale">True then only grayscale colors be generated.</param>
        /// <param name="casing"></param>
        /// <returns>Returns a random color.</returns>
        public string NextColor()
        {
            return ColorNames.GetRandomItem();
        }

        #endregion

        #region Location

        /// <summary>
        /// Generates a random (U.S.) zip code.
        /// </summary>
        /// <param name="plusfour">True to return a Zip+4.</param>
        /// <returns>Returns random generated U.S. zip code.</returns>
        public string NextZipCode(bool plusfour = true)
        {
            var zip = GetDigits(5);
            if (!plusfour)
            {
                return zip;
            }
            zip += "-";
            string others = GetDigits(4);
            zip += others;
            return zip;
        }

        private (string Name, string Abb) StreetSuffix() => StreetSuffixes.GetRandomItem();

        /// <summary>
        /// Generates a random street.
        /// </summary>
        /// <param name="syllables">Number of syllables.</param>
        /// <param name="shortSuffix">True to use short suffix.</param>
        /// <returns>Returns random generated street name.</returns>
        public string NextStreet(int syllables = 2, bool shortSuffix = true)
            => NextWord(syllablesCount: syllables).Capitalize() + " " + (shortSuffix
                ? StreetSuffix().Name
                : StreetSuffix().Abb);

        /// <summary>
        /// Returns a random U.S. state.
        /// </summary>
        /// <param name="fullName">True to return full name.</param>
        /// <returns>Returns a random U.S. state.</returns>
        public string NextState(bool fullName = false)
        {
            var (Name, Abb) = USStates.GetRandomItem();
            if (fullName == true)
                return Name;
            return Abb;
        }
        //=> fullName ? PickRandomItem(UsStates).Key : PickRandomItem(UsStates).Value;


        /// <summary>
        /// Generates a random longitude.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns>Returns random generated longitude.</returns>
        public double NextLongitude(double min = -180.0, double max = 180.0, uint decimals = 5)
            => NextDouble(min, max, decimals);

        /// <summary>
        /// Generates a random latitude.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <param name="decimals">Number of decimals.</param>
        /// <returns>Returns random generated latitude.</returns>
        public double NextLatitude(double min = -90, double max = 90, uint decimals = 5)
            => NextDouble(min, max, decimals);

        /// <summary>
        /// Generates a random geohash.
        /// </summary>
        /// <param name="length">Length of geohash.</param>
        /// <returns>Returns random generated geohash.</returns>
        public string NextGeohash(int length = 7)
            => NextString(length, "0123456789bcdefghjkmnpqrstuvwxyz");


        /// <summary>
        /// Generates a random city name.
        /// </summary>
        /// <returns>Returns random generated city name.</returns>
        public string NextCity() => NextWord(syllablesCount: 3).Capitalize();

        /// <summary>
        /// Generates a random street address.
        /// </summary>
        /// <param name="syllables">Number of syllables.</param>
        /// <param name="shortSuffix">True to use short suffix.</param>
        /// <returns>Returns random generated address.</returns>
        public string NextAddress(int syllables = 2, bool shortSuffix = true)
            => $"{GetRandomNumber(6000, 100)} {NextStreet(syllables, shortSuffix)}";

        #endregion

        #region Date\Time

        /// <summary>
        /// Generates a random year.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns>Returns random generated year.</returns>
        public int NextYear(int min = 2000, int max = -1)
        {
            if (max == -1)
                max = DateTime.Now.Year;
            return GetRandomNumber(max, min);
        }

        /// <summary>
        /// Generates a random month.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 12.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated month.</returns>
        public string NextMonth(int min = 1, int max = 12)
        {
            if (min < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 1.");
            }

            if (max > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 12.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            var Firsts = Months.Skip(min - 1).Take(max - 1).ToCustomBasicList();
            return Firsts.GetRandomItem().Month;

            //return PickRandomItem(Months.Skip(min - 1).Take(max - min - 1).ToList()).Item1;
        }

        /// <summary>
        /// Generates a random second.
        /// </summary>
        /// <returns>Returns random generated second.</returns>
        public int NextSecond() => GetRandomNumber(59, 0);

        /// <summary>
        /// Generates a random minute.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 59.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated minute.</returns>
        public int NextMinute(int min = 0, int max = 59)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 0.");
            }

            if (max > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 59.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            return GetRandomNumber(max, min);
        }

        /// <summary>
        /// Generates a random hour.
        /// </summary>
        /// <param name="twentyfourHours">True to use 24-hours format.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 23.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>max</c> is greater than 12 in 12-hours format.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>min</c> is greater than <c>max</c>.</exception>
        /// <returns>Returns random generated hour.</returns>
        public int NextHour(bool twentyfourHours = true, int? min = null, int? max = null)
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be less than 0.");
            }

            if (twentyfourHours && max > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 23 for twentyfourHours option.");
            }

            if (!twentyfourHours && max > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "max cannot be greater than 12.");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "min cannot be greater than max.");
            }

            min ??= (twentyfourHours ? 0 : 1);
            max ??= (twentyfourHours ? 23 : 12);

            return GetRandomNumber(max.Value, min.Value);
        }

        /// <summary>
        /// Generates a random millisecond.
        /// </summary>
        /// <returns>Returns random generated millisecond.</returns>
        public int NextMillisecond() => GetRandomNumber(999);

        /// <summary>
        /// Generates a random date.
        /// </summary>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns>Returns random generated date.</returns>
        public DateTime NextDate(DateTime? min = null, DateTime? max = null, bool Simple = false)
        {
            if (min.HasValue && max.HasValue)
            {
                DateTime TempDate = DateUtils.UnixTimestampToDateTime(NextLong((long)DateUtils.DateTimeToUnixTimestamp(min.Value),
                    (long)DateUtils.DateTimeToUnixTimestamp(max.Value)));
                if (Simple == false)
                    return TempDate;
                return new DateTime(TempDate.Year, TempDate.Month, TempDate.Day);
            }
            var m = GetRandomNumber(12, 1);
            var d = GetRandomNumber(Months[m - 1].Day, 1);
            var y = NextYear();
            if (Simple == true)
                return new DateTime(y, m, d);
            return new DateTime(y, m, d, NextHour(), NextMinute(),
                NextSecond(), NextMillisecond());
        }

        #endregion

        #region Finance

        private (string Company, string Abb, string Code, int Digits) CcType(string? name = null)
        {
            (string Company, string Abb, string Code, int Digits) cc;

            if (!string.IsNullOrEmpty(name))
            {
                cc = CcTypes.FirstOrDefault(tcc => tcc.Company == name || tcc.Abb == name);

                if (cc == default)
                {
                    throw new ArgumentOutOfRangeException(nameof(name),
                        "Credit card type '" + name + "'' is not supported");
                }
            }
            else
            {
                cc = CcTypes.GetRandomItem();
            }

            return cc;
        }


        /// <summary>
        /// Generates a random credit card number. This card number will pass the Luhn algorithm so it looks like a legit card.
        /// </summary>
        /// <param name="cardType">The available types are:
        /// American Express - amex
        /// Discover Card - discover
        /// Mastercard - mc
        /// Visa - visa
        /// </param>
        /// <returns>Returns random generated credit card number.</returns>
        public long NextCreditCardNumber(string? cardType = null)
        {
            var (_, _, Code, Digits) = CcType(cardType);
            var toGenerate = Digits - Code.Length - 1;
            var number = Code;
            //number += string.Join("", NextList<int>(new Func<int, int, int>(NextInt), toGenerate, 0, 9));

            string ThisGroup = GetDigits(toGenerate);
            number += ThisGroup;
            number += CreditCardUtils.LuhnCalcualte(long.Parse(number));

            return long.Parse(number);
        }

        #endregion

        #region Miscellaneous

        /// <summary>
        /// Returns a random GUID.
        /// </summary>
        /// <param name="version">Custom version.</param>
        /// <returns>Returns a random GUID.</returns>
        public string NextGUID(int version = 5)
        {
            const string guidPool = "abcdef1234567890";
            const string variantPool = "ab89";
            string strFn(string pool, int len) => NextString(len, pool);

            return strFn(guidPool, 8) + "-" +
                   strFn(guidPool, 4) + "-" +
                   version +
                   strFn(guidPool, 3) + "-" +
                   strFn(variantPool, 1) +
                   strFn(guidPool, 3) + "-" +
                   strFn(guidPool, 12);
        }

        #endregion

    }
}
