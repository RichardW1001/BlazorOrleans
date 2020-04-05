using System;
using System.Threading.Tasks;
using MuliplayerClicker.Grains;
using Orleans;
using Orleans.Streams;

namespace MuliplayerClicker.Services
{
    public class ClickerService
    {
        private readonly IClusterClient _client;

        public ClickerService(IClusterClient client)
        {
            _client = client;
        }

        public async Task IncrementClicker()
        {
            var grain = _client.GetGrain<IClickerGrain>(Guid.Empty);

            await grain.Increment();
        }

        public async Task<int> InitialValue()
        {
            var grain = _client.GetGrain<IClickerGrain>(Guid.Empty);

            return await grain.GetValue();
        }

        public async Task<StreamSubscriptionHandle<ClickerNotification>> Subscribe(Func<ClickerNotification, Task> action)
        {
            await action(new ClickerNotification {Count = await InitialValue()});

            return await _client.GetStreamProvider("ClickerStream")
                .GetStream<ClickerNotification>(Guid.Empty, nameof(IClickerGrain))
                .SubscribeAsync(new ClickerSubscriber(action));
        }

        private class ClickerSubscriber : IAsyncObserver<ClickerNotification>
        {
            private readonly Func<ClickerNotification, Task> _action;

            public ClickerSubscriber(Func<ClickerNotification, Task> action)
            {
                _action = action;
            }

            public Task OnNextAsync(ClickerNotification item, StreamSequenceToken token = null)
            {
                return _action(item);
            }

            public Task OnCompletedAsync() => Task.CompletedTask;

            public Task OnErrorAsync(Exception ex) => Task.CompletedTask;
        }
    }
}