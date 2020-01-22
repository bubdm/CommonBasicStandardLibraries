namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public static class BindHelpers
    {
        public static string BindPath(this string firstPath, string secondPath)
        {
            return $"{firstPath}_{secondPath}";
        }
    }
}