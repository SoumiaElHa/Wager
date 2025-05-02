using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Wager;
using Wager.Data;
using Wager.Services;

namespace Test
{
    public class WagerServiceTest
    {
        private readonly Mock<IExecutionContext> _executionContext;
        private Mock<ILogger<WagerService>> _logger;
        private Mock<IMemoryCache> _cache;
        private ICacheEntry _cachEntry = Mock.Of<ICacheEntry>();
        // static long Account = 10000;
        public WagerServiceTest()
        {
            _executionContext = new Mock<IExecutionContext>(MockBehavior.Strict);
            _cache = new Mock<IMemoryCache>();
            _logger = new Mock<ILogger<WagerService>>();
            _cache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(_cachEntry);
        }

        [Fact]
        public void Wager()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            long account = 10000;
            _executionContext.Setup(x => x.PlayerId).Returns(playerId);
            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 1 };
            SetAccountInCache(playerId, account);
            var service = new WagerService(_executionContext.Object, _cache.Object, _logger.Object);
            Parallel.For(0, 30, parallelOptions, i =>
            {
               
                var dto = new BetDto { Number = 3, Points = 100 };
                // Act
                var result = service.Bet(dto);

                // Assert
                Assert.IsType<BetResultDto>(result);
                if (result.Status == ResultStatus.Won)
                {
                    Assert.Equal(account + 9 * dto.Points, result.Account);
                }
                else
                {
                    Assert.Equal(account - dto.Points, result.Account);
                }

                account = result.Account;
                SetAccountInCache(playerId, account);
            });


        }

        private void SetAccountInCache(Guid playerId, long account)
        {
            _cache.Setup(x => x.TryGetValue(playerId, out It.Ref<object>.IsAny))
                .Callback((object key, out object value) =>
                {
                    value = account;
                })
            .Returns(true);
        }
    }
}