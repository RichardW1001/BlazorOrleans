using System.Threading.Tasks;
using Orleans;

namespace MuliplayerClicker.Grains
{
    public interface IClickerGrain : IGrainWithGuidKey
    {
        Task<int> GetValue();
        Task Increment();
    }
}
