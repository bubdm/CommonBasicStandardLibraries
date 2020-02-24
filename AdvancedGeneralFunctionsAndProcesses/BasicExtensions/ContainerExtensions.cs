using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.ContainerClasses;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ContainerExtensions
    {
        public static void RegisterSQLServerConnector<T>(this ContainerMain container)
        {
            container.RegisterInstance<T>("sqlserver");
        }
        public static void RegisterSQLiteConnector<T>(this ContainerMain container)
        {
            container.RegisterInstance<T>("sqlite");
        }
        public static T GetSQLServerConnector<T>(this IResolver resolver)
        {
            return resolver.Resolve<T>("sqlserver");
        }
        public static T GetSQLiteConnector<T>(this IResolver resolver)
        {
            return resolver.Resolve<T>("sqlite");
        }
    }
}
