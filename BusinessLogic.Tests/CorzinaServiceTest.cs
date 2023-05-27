using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tests
{
    public class CorzinaServiceTest
    {
        private readonly CorzinaService service;
        private readonly Mock<ICorzinaRepisitory> repositoryMoq;
        public CorzinaServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            repositoryMoq = new Mock<ICorzinaRepisitory>();

            repositoryWrapperMoq.Setup(x => x.Corzina).
                Returns(repositoryMoq.Object);

            service = new CorzinaService(repositoryWrapperMoq.Object);
        }
        [Fact]
        public async Task CreateAsync_Null_ShouldThrowNullArgumentException()
        {
            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            repositoryMoq.Verify(x => x.Create(It.IsAny<Corzina>()), Times.Never);
        }

        public static IEnumerable<object[]> GetIncorrectUsers()
        {
            return new List<object[]>
            {
                new object[] { new Corzina() { Price = 0, Kolichestvo = 0, Discount = 0, StatusTovara  = ""} },
                new object[] { new Corzina() { Price = 234.43, Kolichestvo = 0, Discount = 0, StatusTovara = "Test"} }
            };
        }
        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncNewUserShouldNotCreateNewUser(Corzina m)
        {
            // arrange
            var newUser = m;

            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(newUser));

            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<Corzina>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newm = new Corzina()
            {
                Price = 234.43,
                Kolichestvo = 1,
                Discount = 40,
                StatusTovara = "Tst"
            };

            // act
            await service.Create(newm);
            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<Corzina>()), Times.Once);
        }
    }
}
