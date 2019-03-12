﻿using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public class ConditionActionPair<T>
    {
        public Action<T> Action { get; private set; }
        public Predicate<T> Predicate { get; private set; }

        public ConditionActionPair(Predicate<T> predicate, Action<T> action) //i like predicate first.
        {
            Action = action;
            Predicate = predicate;
        }
    }
}