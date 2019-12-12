using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.Messenging
{
    public interface IHandleAsync<TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}