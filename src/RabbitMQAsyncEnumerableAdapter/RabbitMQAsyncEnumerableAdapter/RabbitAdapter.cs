using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

using RabbitMQ.Client.Events;

namespace RabbitMQAsyncEnumerableAdapter
{

    /// <summary>
    /// This class provides <see cref="IAsyncEnumerable"/>  behaviour for EAP RabbitMQ logic.
    /// </summary>
    /// <example>
    /// <code>
    /// var adapterBuffer = 100;
    /// RabbitAdapter ra = new RabbitAdapter(adapterBuffer, messageId => channel.BasicAck(messageId, false));
    /// consumer.Received += ra.ConsumeData;
    /// await foreach (var data in ra.WithCancellation(CancellationToken.None))
    /// {
    ///     Console.WriteLine(data.Body);
    /// }   
    /// </code>
    /// </example>
    public class RabbitAdapter : IAsyncEnumerable<BasicDeliverEventArgs>
    {

        /// <summary>
        /// Creates <see cref="RabbitAdapter"/> with internal buffer of messages with size - <paramref name="bufferSize"/>. 
        /// </summary>
        /// <param name="bufferSize">Size of immemory messages buffer.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="bufferSize"/> is less then one</exception>
        public RabbitAdapter(int bufferSize)
        {
            if (bufferSize <= 0)
                throw new ArgumentException("buffer should be more that zero", nameof(bufferSize));
            _channel = Channel.CreateBounded<BasicDeliverEventArgs>(bufferSize);
        }

        /// <summary>
        /// Creates <see cref="RabbitAdapter"/> with internal buffer of messages with size - <paramref name="bufferSize"/> and acknowledge action.
        /// </summary>
        /// <param name="bufferSize">Size of immemory messages buffer.</param>
        /// <param name="acknowledge">Acknowledge logic.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="acknowledge"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="bufferSize"/> is less then one</exception>
        public RabbitAdapter(int bufferSize, Action<ulong> acknowledge)
            :
            this(bufferSize)
        {
            _acknowledge = acknowledge ?? throw new ArgumentNullException(nameof(acknowledge));
        }

        /// <summary>
        /// Method that should be called from RabbitMQ Received event handler.
        /// </summary>
        /// <param name="_">Empty parameter to maintain the contract.</param>
        /// <param name="e">Message from RabbitMQ.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="e"/> is null</exception>
        public void ConsumeData(object? _, BasicDeliverEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var spinWait = new SpinWait();

            while (!_channel.Writer.TryWrite(e))
            {
                spinWait.SpinOnce();
            }

            if (_acknowledge is not null)
            {
                _acknowledge(e.DeliveryTag);
            }
        }

        /// <summary>
        /// Creates async iterator for queue's messages.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Messages async iterator.</returns>
        public async IAsyncEnumerator<BasicDeliverEventArgs> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            while (await _channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            {
                while (_channel.Reader.TryRead(out BasicDeliverEventArgs item))
                {
                    yield return item;
                }
            }
        }

        private readonly Action<ulong>? _acknowledge;

        private readonly Channel<BasicDeliverEventArgs> _channel;
    }
}
