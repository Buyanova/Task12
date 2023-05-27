using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tests
{
    public class TovarServiceTest
    {
        private readonly TovarService service;
        private readonly Mock<ITovarRepisitory> repositoryMoq;
        public TovarServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            repositoryMoq = new Mock<ITovarRepisitory>();

            repositoryWrapperMoq.Setup(x => x.Tovar).
                Returns(repositoryMoq.Object);

            service = new TovarService(repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_Null_ShouldThrowNullArgumentException()
        {
            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            repositoryMoq.Verify(x => x.Create(It.IsAny<Tovar>()), Times.Never);
        }

        public static IEnumerable<object[]> GetIncorrectUsers()
        {
            return new List<object[]>
            {
                new object[] { new Tovar() { Name = "", Kolichestvo = 0, Price = 0, OpisanieTovara = ""} },
                new object[] { new Tovar() { Name = "Test", Kolichestvo = 1, Price = 1.0, OpisanieTovara = ""} }
            };
        }
        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncNewUserShouldNotCreateNewUser(Tovar m)
        {
            // arrange
            var newUser = m;

            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(newUser));

            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<Tovar>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newm = new Tovar()
            {
                Name = "Test",
                Kolichestvo = 2,
                Price = 233.5,
                OpisanieTovara = "Test"
            };

            // act
            await service.Create(newm);
            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<Tovar>()), Times.Once);
        }
    }
}
