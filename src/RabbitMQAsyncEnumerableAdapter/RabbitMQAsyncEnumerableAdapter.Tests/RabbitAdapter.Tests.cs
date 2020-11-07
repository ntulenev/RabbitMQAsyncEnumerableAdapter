using System;

using Xunit;

using FluentAssertions;


namespace RabbitMQAsyncEnumerableAdapter.Tests
{
    public class RabbitAdapterTests
    {
        [Fact(DisplayName = "The RabbitAdapter can be constructed with correct buffer size.")]
        [Trait("Category", "Unit")]
        public void CanBeConstructedWithBuffer()
        {
            var buffer = 1;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            exception.Should().BeNull();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with zero buffer size.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithZeroBuffer()
        {
            var buffer = 0;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with negative buffer size.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNegativeBuffer()
        {
            var buffer = -1;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can be constructed with correct buffer size and ack action.")]
        [Trait("Category", "Unit")]
        public void CanBeConstructedWithBufferAndAck()
        {
            var buffer = 1;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, _ => { }));

            exception.Should().BeNull();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with zero buffer size and ack action.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithZeroBufferAndAck()
        {
            var buffer = 0;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, _ => { }));

            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with negative buffer size and ack action.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNegativeBufferAndAck()
        {
            var buffer = -1;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, _ => { }));

            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with correct buffer size but null ack action.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNullAckAction()
        {
            var buffer = 1;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, null!));

            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

    }
}
