using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

using RabbitMQ.Client.Events;

namespace RabbitMQAsyncEnumerableAdapter
{

    public class RabbitAdapter : IAsyncEnumerable<BasicDeliverEventArgs>
    {

        public RabbitAdapter(int bufferSize)
        {
            if (bufferSize <= 0)
                throw new ArgumentException("buffer should be more that zero", nameof(bufferSize));
            _channel = Channel.CreateBounded<BasicDeliverEventArgs>(bufferSize);
        }

        public RabbitAdapter(int bufferSize, Action<ulong> acknowledge)
            :
            this(bufferSize)
        {
            _acknowledge = acknowledge ?? throw new ArgumentNullException(nameof(acknowledge));
        }

        public void ConsumeData(object _, BasicDeliverEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));

            var spinWait = new SpinWait();

            while (!_channel.Writer.TryWrite(e))
            {
                spinWait.SpinOnce();
            }

            if (_acknowledge is not null)
                _acknowledge(e.DeliveryTag);
        }

        public async IAsyncEnumerator<BasicDeliverEventArgs> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            while (await _channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
                while (_channel.Reader.TryRead(out BasicDeliverEventArgs item))
                    yield return item;
        }

        private readonly Action<ulong>? _acknowledge;

        private readonly Channel<BasicDeliverEventArgs> _channel;
    }
}
