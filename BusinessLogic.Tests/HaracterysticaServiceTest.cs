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
    public class HaracterysticaServiceTest
    {
        private readonly HaracterysticaService service;
        private readonly Mock<IHaracterysticaRepisitory> repositoryMoq;
        public HaracterysticaServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            repositoryMoq = new Mock<IHaracterysticaRepisitory>();

            repositoryWrapperMoq.Setup(x => x.Haracterystica).
                Returns(repositoryMoq.Object);

            service = new HaracterysticaService(repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_Null_ShouldThrowNullArgumentException()
        {
            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));

            // assert
            Assert.IsType<ArgumentNullException>(ex);
            repositoryMoq.Verify(x => x.Create(It.IsAny<HaracterysticaTovarov>()), Times.Never);
        }

        public static IEnumerable<object[]> GetIncorrectUsers()
        {
            return new List<object[]>
            {
                new object[] { new HaracterysticaTovarov() { NameKategorii = "", Proizvoditel = "", StranaProizvoditelya = "", Brend = "", Material = "", Rasmer = ""} },
                new object[] { new HaracterysticaTovarov() { NameKategorii = "Test", Proizvoditel = "", StranaProizvoditelya = "", Brend = "", Material = "", Rasmer= "" } }
            };
        }
        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncNewUserShouldNotCreateNewUser(HaracterysticaTovarov m)
        {
            // arrange
            var newUser = m;

            // act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(newUser));

            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<HaracterysticaTovarov>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newm = new HaracterysticaTovarov()
            {
                NameKategorii = "Test",
                Proizvoditel = "Test",
                StranaProizvoditelya = "Test",
                Brend = "Test",
                Material = "Test",
                Rasmer = "Test"
            };

            // act
            await service.Create(newm);
            // assert
            repositoryMoq.Verify(x => x.Create(It.IsAny<HaracterysticaTovarov>()), Times.Once);
        }
    }
}
