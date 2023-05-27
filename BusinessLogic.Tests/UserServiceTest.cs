using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Tests
{
    public class UserServiceTest
    {
        private readonly UserService service;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        public UserServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            userRepositoryMoq = new Mock<IUserRepository>();

            repositoryWrapperMoq.Setup(x => x.Pokupatel).
                Returns(userRepositoryMoq.Object);

            service = new UserService(repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullUser_ShouldThrowNullArgumentException()
        {
            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<Pokupatel>()), Times.Never);
        }

        public static IEnumerable<object[]> GetIncorrectUsers()
        {
            return new List<object[]>
            {
                new object[] { new Pokupatel() { Fio = "", Adres = "", Email = "", Password = "", Phone = ""} },
                new object[] { new Pokupatel() { Fio = "Test", Adres = "", Email = "", Password = "", Phone = ""} }
            };
        }
        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncNewUserShouldNotCreateNewUser(Pokupatel pokupatel)
        {
            // arrange
            var newUser = pokupatel;

            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(newUser));

            // assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<Pokupatel>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newUser = new Pokupatel()
            {
                Fio = "Test",
                Adres = "Test",
                Email = "test@test.com",
                Phone = "+7999999999",
                Password = "A8$%#//9fs"
            };

            // act
            await service.Create(newUser);
            // assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<Pokupatel>()), Times.Once);
        }
    }
}
