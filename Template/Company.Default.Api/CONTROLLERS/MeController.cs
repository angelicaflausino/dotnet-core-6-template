using $ext_safeprojectname$.Cloud.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace $safeprojectname$.Controllers
{
    /// <summary>
    /// Sample of controller calls Microsoft Graph Api
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IGraphMeService _meService;

        public MeController(IGraphMeService graphMeService)
        {
            _meService = graphMeService;
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var me = await _meService.GetAsync();

            return Ok(me);
        }

        /// <summary>
        /// Gets user with only selected properties
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        [HttpGet("properties")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetProperties([FromQuery] string[] properties)
        {
            var me = await _meService.GetAsync(properties);

            return Ok(me);
        }

        /// <summary>
        /// Get encoded base64 user profile picture
        /// </summary>
        /// <returns></returns>
        [HttpGet("photo")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetPhoto()
        {
            var photo = await _meService.GetProfilePhotoAsBase64();

            return Ok(photo);
        }
    }
}
