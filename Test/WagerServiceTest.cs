using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Wager;
using Wager.Data;
using Wager.Services;
using Xunit.Repeat;

namespace Test
{
    public class WagerServiceTest
    {
        private readonly Mock<IExecutionContext> _executionContext;
        private Mock<ILogger<WagerService>> _logger;
        private Mock<IMemoryCache> _cache;
        private ICacheEntry _cachEntry = Mock.Of<ICacheEntry>();
        public WagerServiceTest()
        {
            _executionContext = new Mock<IExecutionContext>(MockBehavior.Strict);
            _cache = new Mock<IMemoryCache>();
            _logger = new Mock<ILogger<WagerService>>(MockBehavior.Loose);
            _cache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(_cachEntry);
        }

        [Theory()]
        [Repeat(30)]
        public void Wager(int iterationNumber)
        {
            // Arrange
            long account = 10000;
            var playerId = Guid.NewGuid();
            _executionContext.Setup(x => x.PlayerId).Returns(playerId);
            _cache.Setup(x => x.TryGetValue(playerId, out It.Ref<object>.IsAny))
                .Callback((object key, out object value) =>
                {
                    value = account;
                })
           .Returns(true);
            var dto = new BetDto { Number = 3, Points = 100 };
            // Act
            var service = new WagerService(_executionContext.Object, _cache.Object, _logger.Object);
            var result = service.Bet(dto);

            // Assert
            Assert.IsType<BetResultDto>(result);
            if (result.Status == ResultStatus.Won)
            {
                Assert.Equal(10000 + 9 * dto.Points, result.Account);
            }
            else
            {
                Assert.Equal(10000 - dto.Points, result.Account);
            }
        }
    }
}