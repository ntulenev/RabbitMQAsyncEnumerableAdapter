using System;

using Xunit;

using FluentAssertions;


namespace RabbitMQAsyncEnumerableAdapter.Tests
{
    public class RabbitAdapterTests
    {
        [Fact(DisplayName = "The RabbitAdapter can be constructed with buffer size.")]
        [Trait("Category", "Unit")]
        public void CanBeConstructedWithBuffer()
        {
            var buffer = 1;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            exception.Should().BeNull();
        }

        [Fact(DisplayName = "The RabbitAdapter can be constructed with zero buffer size.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithZeroBuffer()
        {
            var buffer = 0;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can be constructed with negative buffer size.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNegativeBuffer()
        {
            var buffer = 0;

            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

    }
}
