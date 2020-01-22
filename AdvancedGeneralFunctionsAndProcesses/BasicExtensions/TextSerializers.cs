using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class TextSerializers
    {
        public static async Task SaveTextFileAsync<T>(this IModel payLoad, string path)
            where T : IModel
        {
            var properties = GetProperties<T>();
            CustomBasicList<string> list = new CustomBasicList<string>();
            properties.ForEach(p =>
            {
                list.Add(p.GetValue(payLoad).ToString());
            });
            await File.WriteAllLinesAsync(path, list);
        }

        public static async Task SaveTextAsync<T>(this CustomBasicList<T> payLoad, string path, string delimiter = ",")
        {
            //will be utf8

            var properties = GetProperties<T>();
            CustomBasicList<string> list = new CustomBasicList<string>();
            foreach (var item in payLoad)
            {
                StrCat cats = new StrCat();
                properties.ForEach(p =>
                {
                    //if item is null, then will be empty.
                    var value = p.GetValue(item);
                    if (value != null)
                    {
                        cats.AddToString(value.ToString(), delimiter);
                        //if (p.PropertyType == typeof(int))
                        //{
                        //    //try to parse to integers.
                        //    //bool rets = int.TryParse(value.ToString(), out int out)
                        //}
                    }
                    else
                    {
                        cats.AddToString("", delimiter);
                    }
                });
            }
            await File.WriteAllLinesAsync(path, list, Encoding.UTF8);
        }

        private static CustomBasicList<PropertyInfo> GetProperties<T>()
        {
            Type type = typeof(T);
            //first check to see if there is anything i can't handle.
            CustomBasicList<PropertyInfo> output = type.GetProperties().ToCustomBasicList();
            if (output.Exists(x => x.IsSimpleType()) == false)
            {
                throw new BasicBlankException("There are some properties that are not simple.  This only handles simple types for now");
            }
            return output;
        }

        public static async Task<T> LoadTextSingleAsync<T>(string path) where T : new()
        {
            var properties = GetProperties<T>();
            var lines = await File.ReadAllLinesAsync(path);
            if (lines.Count() != properties.Count)
            {
                throw new BasicBlankException("Text file corrupted because the delimiter count don't match the properties");
            }
            T output = new T();
            int x = 0;
            properties.ForEach(p =>
            {
                string item = lines[x];
                PopulateValue(item, output, p);
                x++;
            });
            return output;
        }

        /// <summary>
        /// this will load the text file and return a list of the object you want.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static async Task<CustomBasicList<T>> LoadTextListAsync<T>(string path, string delimiter = ",")
            where T : new()
        {
            var properties = GetProperties<T>();

            CustomBasicList<T> output = new CustomBasicList<T>();
            var lines = await File.ReadAllLinesAsync(path);
            foreach (var line in lines)
            {
                var items = line.Split(delimiter).ToCustomBasicList();
                if (items.Count != properties.Count)
                {
                    throw new BasicBlankException("Text file corrupted because the delimiter count don't match the properties");
                }
                //if i decide to ignore, then won't be a problem.  don't worry for now.
                int x = 0;
                T row = new T();
                properties.ForEach(p =>
                {
                    string item = items[x];
                    PopulateValue(item, row, p);
                    x++;
                });
                output.Add(row);
            }
            return output;
        }

        private static void PopulateValue<T>(string item, T row, PropertyInfo p)
        {
            if (p.PropertyType == typeof(int))
            {
                //try to parse to integers.
                bool rets = int.TryParse(item, out int y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to integer.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple.
            }
            else if (p.PropertyType == typeof(int?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = int.TryParse(item, out int y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to integer.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple.
                }

            }
            else if (p.PropertyType.IsEnum)
            {
                bool rets = int.TryParse(item, out int y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to enum.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }

            else if (p.PropertyType.IsNullableEnum())
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = int.TryParse(item, out int y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to enum.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }

            else if (p.PropertyType == typeof(bool))
            {
                bool rets = bool.TryParse(item, out bool y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to boolean.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)

            }
            else if (p.PropertyType == typeof(bool?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = bool.TryParse(item, out bool y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to boolean.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(decimal))
            {
                bool rets = decimal.TryParse(item, out decimal y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to decimal.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)

            }
            else if (p.PropertyType == typeof(decimal?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = decimal.TryParse(item, out decimal y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to decimal.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(float))
            {
                bool rets = float.TryParse(item, out float y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to float.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)

            }
            else if (p.PropertyType == typeof(float?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = float.TryParse(item, out float y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to float.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(double))
            {
                bool rets = double.TryParse(item, out double y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to double.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)

            }
            else if (p.PropertyType == typeof(double?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = double.TryParse(item, out double y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to double.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(DateTime))
            {
                bool rets = DateTime.TryParse(item, out DateTime y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to datetime.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)

            }
            else if (p.PropertyType == typeof(DateTime?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = DateTime.TryParse(item, out DateTime y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to datetime.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(DateTimeOffset))
            {
                bool rets = DateTimeOffset.TryParse(item, out DateTimeOffset y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to datetimeoffset.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)

            }
            else if (p.PropertyType == typeof(DateTimeOffset?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = DateTimeOffset.TryParse(item, out DateTimeOffset y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to datetimeoffset.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(Guid))
            {
                bool rets = Guid.TryParse(item, out Guid y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to guid.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(Guid?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = Guid.TryParse(item, out Guid y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to guid.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(char))
            {
                bool rets = char.TryParse(item, out char y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to char.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(char?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = char.TryParse(item, out char y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to char.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(short))
            {
                bool rets = short.TryParse(item, out short y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to short.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(short?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = short.TryParse(item, out short y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to short.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(ushort))
            {
                bool rets = ushort.TryParse(item, out ushort y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to ushort.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(ushort?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = ushort.TryParse(item, out ushort y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to ushort.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(uint))
            {
                bool rets = uint.TryParse(item, out uint y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to uint.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(uint?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = uint.TryParse(item, out uint y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to uint.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(long))
            {
                bool rets = long.TryParse(item, out long y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to long.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(long?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = long.TryParse(item, out long y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to long.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(ulong))
            {
                bool rets = ulong.TryParse(item, out ulong y);
                if (rets == false)
                {
                    throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to ulong.  Means corruption");
                }
                p.SetValue(row, y); //hopefully this simple (?)
            }
            else if (p.PropertyType == typeof(ulong?))
            {
                if (item == "")
                {
                    p.SetValue(row, null);
                }
                else
                {
                    bool rets = ulong.TryParse(item, out ulong y);
                    if (rets == false)
                    {
                        throw new BasicBlankException($"When trying to parse column {p.Name}, failed to parse to ulong.  Means corruption");
                    }
                    p.SetValue(row, y); //hopefully this simple (?)
                }
            }
            else if (p.PropertyType == typeof(string))
            {
                p.SetValue(row, item); //easiest part
            }
            else
            {
                throw new BasicBlankException($"Property with name of {p.Name} has unsupported property type of {p.PropertyType.Name}.  Rethink");
            }
        }
    }
}