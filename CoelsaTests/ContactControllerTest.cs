using CoelsaWebApi.Controllers;
using CoelsaWebApi.Services;
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

        private void Init()
        {
            _contactServiceMock = new Mock<IContactService>();
            _controller = new ContactController(_contactServiceMock.Object);
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
