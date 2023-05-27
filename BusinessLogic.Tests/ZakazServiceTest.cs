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
    public class ZakazServiceTest
    {
        private readonly ZakazService service;
        private readonly Mock<IZakazRepisitory> repositoryMoq;
        public ZakazServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            repositoryMoq = new Mock<IZakazRepisitory>();

            repositoryWrapperMoq.Setup(x => x.Zakaz).
                Returns(repositoryMoq.Object);

            service = new ZakazService(repositoryWrapperMoq.Object);
        }
        [Fact]
        public async Task CreateAsync_Null_ShouldThrowNullArgumentException()
        {
            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            repositoryMoq.Verify(x => x.Create(It.IsAny<Zakaz>()), Times.Never);
        }

        public static IEnumerable<object[]> GetIncorrectUsers()
        {
            return new List<object[]>
            {
                new object[] { new Zakaz() { DateZakaz = DateTime.MaxValue, SrokSborki = 0, SposobDostavci = "", StatusDostavci = ""} },
                new object[] { new Zakaz() { DateZakaz = DateTime.MaxValue, SrokSborki = 0, SposobDostavci = "", StatusDostavci = "Test"} },
                new object[] { new Zakaz() { DateZakaz = DateTime.MaxValue, SrokSborki = 0, SposobDostavci = "Test", StatusDostavci = "Test"} },
            };
        }
        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncNewUserShouldNotCreateNewUser(Zakaz m)
        {
            // arrange
            var newUser = m;

            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(newUser));

            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<Zakaz>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newm = new Zakaz()
            {
                DateZakaz = DateTime.MaxValue,
                SrokSborki = 30,
                SposobDostavci = "Самовывоз",
                StatusDostavci = "выполнен"
            };

            // act
            await service.Create(newm);
            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<Zakaz>()), Times.Once);
        }
    }
}
