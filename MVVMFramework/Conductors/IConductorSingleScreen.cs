namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    /// <summary>
    /// this focuses on where you
    /// </summary>
    /// <typeparam name="T">this is usually a view model implementation but does not have to be.</typeparam>
    public interface IConductorSingleScreen<T> : IConductorCollectionSingleActive<T>
    {
        bool CanOpenChild { get; } //i have toi implement.  otherwise, the reflection won't pick it up.
    }
}