﻿using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.FileFunctions.FileFunctions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.ConfigProcesses
{
    public class SimpleTabDelimitedConfigClass : ISimpleConfig
    {
        private readonly IConfigLocation _locator;
        public SimpleTabDelimitedConfigClass(IConfigLocation locator)
        {
            _locator = locator;
        }
        async Task<string> ISimpleConfig.GetStringAsync(string key)
        {
            string path = await _locator.GetConfigLocationAsync(); //this is intended to get from local disk;
            if (FileExists(path) == false)
                throw new BasicBlankException($"Path at {path} does not exist.");
            if (path.ToLower().EndsWith("txt") == false)
                throw new BasicBlankException(@"Only text files are supported.  Rethink");
            CustomBasicList<string> firstList = await TextFromFileListAsync(path);
            Dictionary<string, string> output = new Dictionary<string, string>();
            //CustomBasicList<ConfigPair> output = new CustomBasicList<ConfigPair>();
            firstList.ForEach(row =>
            {
                CustomBasicList<string> nextList = row.Split(Constants.vbTab).ToCustomBasicList();
                if (nextList.Count != 2)
                    throw new BasicBlankException($"Needs 2 items for value pair.  Value or row was {row}");
                bool rets = output.TryAdd(nextList.First(), nextList.Last());
                if (rets == false)
                    throw new BasicBlankException($"{key} was duplicated");
                //output.Add(nextList.First(), nextList.Last());
            });
            return output[key];
            //if (output.Exists(items => items.Key == key) == false)
            //    throw new BasicBlankException($"{key} was not found");
            //if (output.HasDuplicates(items => items.Key == key))

            //return output.Single(items =>items.Key == key).Value;
        }
    }
}