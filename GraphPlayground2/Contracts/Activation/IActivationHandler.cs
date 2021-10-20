using System.Threading.Tasks;

namespace GraphPlayground2.Contracts.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle();

        Task HandleAsync();
    }
}
