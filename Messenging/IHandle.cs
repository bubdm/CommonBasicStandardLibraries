namespace CommonBasicStandardLibraries.Messenging
{
    public interface IHandle<TMessage>
    {
        void Handle(TMessage message);
    }
}
