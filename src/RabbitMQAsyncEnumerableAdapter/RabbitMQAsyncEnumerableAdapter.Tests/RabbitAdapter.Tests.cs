﻿using System;

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
            // Arrange
            var buffer = 1;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            // Assert
            exception.Should().BeNull();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with zero buffer size.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithZeroBuffer()
        {
            // Arrange
            var buffer = 0;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with negative buffer size.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNegativeBuffer()
        {
            // Arrange
            var buffer = -1;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can be constructed with correct buffer size and ack action.")]
        [Trait("Category", "Unit")]
        public void CanBeConstructedWithBufferAndAck()
        {
            // Arrange
            var buffer = 1;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, _ => { }));

            // Assert
            exception.Should().BeNull();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with zero buffer size and ack action.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithZeroBufferAndAck()
        {
            // Arrange
            var buffer = 0;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, _ => { }));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with negative buffer size and ack action.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNegativeBufferAndAck()
        {
            // Arrange
            var buffer = -1;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, _ => { }));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not be constructed with correct buffer size but null ack action.")]
        [Trait("Category", "Unit")]
        public void CanNotBeConstructedWithNullAckAction()
        {
            // Arrange
            var buffer = 1;

            // Act
            var exception = Record.Exception(
                () => new RabbitAdapter(buffer, null!));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can not consume null message.")]
        [Trait("Category", "Unit")]
        public void CanNotRunConsumeWithNullMessage()
        {
            // Arrange
            var ra = new RabbitAdapter(1);

            // Act
            var exception = Record.Exception(
                () => ra.ConsumeData(null, null!));

            // Assert
            exception.Should().NotBeNull().And.BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = "The RabbitAdapter can consume message.")]
        [Trait("Category", "Unit")]
        public void CaRunConsumeWithNotNullMessage()
        {
            // Arrange
            var ra = new RabbitAdapter(1);

            // Act
            var exception = Record.Exception(
                () => ra.ConsumeData(null, new RabbitMQ.Client.Events.BasicDeliverEventArgs()));

            // Assert
            exception.Should().BeNull();
        }

        [Fact(DisplayName = "The RabbitAdapter can consume message with ack.")]
        [Trait("Category", "Unit")]
        public void CaRunConsumeWithNotNullMessageWithAck()
        {
            // Arrange
            var message = new RabbitMQ.Client.Events.BasicDeliverEventArgs() { DeliveryTag = 42 };
            ulong ackId = 0;
            var ra = new RabbitAdapter(1, id =>
             {
                 ackId = id;
             });

            // Act
            ra.ConsumeData(null, message);

            // Assert
            ackId.Should().Be(message.DeliveryTag);
        }
    }
}
