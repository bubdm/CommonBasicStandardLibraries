using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public partial class RandomGenerator
    {
        public ITestPerson GetTestSinglePerson<T>(EnumAgeRanges defaultAge = EnumAgeRanges.Adult) where T : ITestPerson, new()
        {
            ITestPerson output = new T();
            output.FirstName = NextAnyName();
            output.LastName = NextLastName();
            output.LastDate = NextDate(Simple: true);
            output.PostalCode = NextZipCode();
            output.State = NextState();
            output.Address = NextAddress();
            output.IsActive = NextBool(70); //wants to lean towards active
            output.CreditCardNumber = NextCreditCardNumber();
            output.Age = NextAge(defaultAge);
            output.SSN = NextSSN();
            output.City = NextCity(); //i guess its okay that city is nonsense.
            output.EmailAddress = NextEmail();
            return output;
        }
        public CustomBasicList<ITestPerson> GetTestPeopleList<T>(int HowMany, EnumAgeRanges DefaultAge = EnumAgeRanges.Adult) where T : ITestPerson, new()
        {
            CustomBasicList<ITestPerson> ThisList = new CustomBasicList<ITestPerson>();
            for (int i = 0; i < HowMany; i++)
            {
                ThisList.Add(GetTestSinglePerson<T>(DefaultAge));
            }
            return ThisList;
        }
    }
}
