using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public partial class RandomGenerator
    {
        public ITestPerson GetTestSinglePerson<T>(EnumAgeRanges DefaultAge = EnumAgeRanges.Adult) where T : ITestPerson, new()
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
            output.Age = NextAge(DefaultAge);
            output.SSN = NextSSN();
            output.City = NextCity(); //i guess its okay that city is nonsense.
            output.EmailAddress = NextEmail();
            
            return output;
        }
        public CustomBasicList<ITestPerson> GetTestPeopleList<T>(int HowMany, EnumAgeRanges DefaultAge = EnumAgeRanges.Adult) where T: ITestPerson, new()
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
