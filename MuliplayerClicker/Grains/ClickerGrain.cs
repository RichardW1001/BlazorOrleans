using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

namespace MuliplayerClicker.Grains
{
    public class ClickerGrain : Grain, IClickerGrain
    {
        private readonly IPersistentState<ClickerState> _state;

        public ClickerGrain([PersistentState(nameof(ClickerState))] IPersistentState<ClickerState> state)
        {
            _state = state;
        }

        public Task<int> GetValue()
        {
            return Task.FromResult(_state.State.Count);
        }

        public Task Increment()
        {
            _state.State.Count++;
            _state.WriteStateAsync();

            GetStreamProvider("ClickerStream")
                .GetStream<ClickerNotification>(Guid.Empty, nameof(IClickerGrain))
                .OnNextAsync(new ClickerNotification {Count = _state.State.Count})
                .Ignore();

            return Task.CompletedTask;
        }
    }
}