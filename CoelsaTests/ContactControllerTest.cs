using CoelsaWebApi.Controllers;
using CoelsaWebApi.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoelsaTests
{
    public class ContactControllerTest
    {
        private ContactController _controller;

        private Mock<IContactService> _contactServiceMock;
        private Mock<NullLoggerFactory> _loggerFactoryMock;

        private void Init()
        {
            _contactServiceMock = new Mock<IContactService>();
            _loggerFactoryMock = new Mock<NullLoggerFactory>();
            _controller = new ContactController(_contactServiceMock.Object, _loggerFactoryMock.Object);
        }

        [Fact]
        public async Task Should_Call_GetContact_By_Id()
        {
            //Arrange
            Init();

            //Act
            var result = await _controller.GetContact(1);


            //Assert
            _contactServiceMock.Verify(m => m.GetContact(1), Times.Once);
        }
    }
}
