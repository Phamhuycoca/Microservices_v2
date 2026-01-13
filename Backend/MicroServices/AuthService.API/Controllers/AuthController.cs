using AuthService.Application.DTO;
using AuthService.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenService _authenService;
        public AuthController(IAuthenService authenService)
        {
            _authenService = authenService;
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO dto)
        {
            var result =await _authenService.Login(dto);
            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        public Task<IActionResult> GetAction()
        {
            return Task.FromResult((IActionResult)Ok("Auth Service is running"));
        }
    }
}
