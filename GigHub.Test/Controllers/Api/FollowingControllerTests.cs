using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Dtos;
using GigHub.Persistence;
using GigHub.Test.Controllers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Test.Controllers.Api
{
    [TestClass]
    public class FollowingControllerTests
    {
        private FollowingsController _controller;

        public FollowingControllerTests()
        {

            var mockUoW = new Mock<IUnitOfWork>();
            _controller = new FollowingsController(mockUoW.Object);
            _controller.MockCurrentUser("fd0a36e6-563f-4729-a01e-c679112fab97", "zionLloyd05@gmail.com");

        }
            
        [TestMethod]
        public void Follow_NoArtistWithGivenIdExist_ShouldReturnNotFound()
        {
            var dto = new FollowingDto
            {
                FolloweeId = "werwer-werwer-werwer"
            };

            var result = _controller.Follow(dto);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
