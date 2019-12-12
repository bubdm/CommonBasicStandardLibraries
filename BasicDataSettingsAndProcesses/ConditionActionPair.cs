using System;
namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public class ConditionActionPair<T>
    {
        public Action<T, string> Action { get; private set; }
        public Predicate<T> Predicate { get; private set; }
        public string Value { get; private set; }
        public ConditionActionPair(Predicate<T> predicate, Action<T, string> action, string value = "") //i like predicate first.
        {
            Action = action;
            Value = value;
            Predicate = predicate;
        }
    }
}