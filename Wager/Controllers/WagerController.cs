using Microsoft.AspNetCore.Mvc;
using Wager.Data;
using Wager.Services;

namespace Wager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WagerController(IWagerService wagerService) : ControllerBase
    {
        private readonly IWagerService _wagerService = wagerService;

        /// <summary>
        /// A player bet action
        /// </summary>
        /// <param name="bet">The bet of the player</param>
        /// <returns>The result of the bet</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BetResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BetResultDto> Bet([FromBody] BetDto bet)
        {
            try
            {
                var result = _wagerService.Bet(bet);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
