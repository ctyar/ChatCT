using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ChatCT.Wpf
{
    internal class Producer
    {
        private readonly BlockingCollection<string> _blockingCollection;
        private readonly CancellationToken _cancellationToken;

        public Producer(BlockingCollection<string> blockingCollection, CancellationToken cancellationToken)
        {
            _blockingCollection = blockingCollection;
            _cancellationToken = cancellationToken;
        }

        internal async Task GetMessagesAsync()
        {
            while (true)
            {
                try
                {
                    var message = Guid.NewGuid().ToString();

                    _blockingCollection.TryAdd(message, 2, _cancellationToken);

                    await Task.Delay(100, _cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _blockingCollection.CompleteAdding();
                    break;
                }
            }
        }
    }
}