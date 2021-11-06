using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Personal_EF_API.Data.Mappings.DTOs;
using Personal_EF_API.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Personal_EF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {

        private readonly SignInManager<IdentityUser> _sign_in_Manager;
        private readonly UserManager<IdentityUser> _user_Manager;

      
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        //Microsoft configuratinons not the Automapper
        private readonly IConfiguration _config;

        public PersonalController(SignInManager<IdentityUser> sign_in_Manager , UserManager<IdentityUser> user_Manager, ILoggerService logger, IMapper mapper, IConfiguration config)
        {
            _sign_in_Manager = sign_in_Manager;
            _user_Manager = user_Manager;
            _logger = logger;
            _mapper = mapper;
            _config = config;
        }
        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="perosnalDTO"></param>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] PersonalDTO perosnalDTO )
        {
            try
            {
                var username = perosnalDTO.Username;
                var pass = perosnalDTO.Password;
                _logger.LogInfo($"Try to Sign in User: {username}");
                var result = await _sign_in_Manager.PasswordSignInAsync(username, pass, false, false);
                if (result.Succeeded)
                {
                    _logger.LogInfo("Sucessfull");
                    var user = await _user_Manager.FindByNameAsync(username);
                    var token_String = await GenerateJSONToken(user);
                    return Ok(new { token = token_String });
                }
                _logger.LogInfo($"Unsucessfull  Login {username}");
                return Unauthorized(perosnalDTO);
            }
            catch(Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Loggin.Contact Denis");

            }
          

        }

        private async Task<string> GenerateJSONToken(IdentityUser user)
        {
            var sec_Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(sec_Key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _user_Manager.GetRolesAsync(user);
            // here we get the role names which is list of strings
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));
            // here is the token
            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                ,_config["Jwt:Issuer"]
                , claims
                , null
                , expires: DateTime.Now.AddMinutes(30)
                , signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="perosnalDTO"></param>
        /// <returns></returns>
        [Route("register")] //cuz both 
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] PersonalDTO perosnalDTO)
        {
            try
            {
                var username = perosnalDTO.Username;
                var pass = perosnalDTO.Password;
                _logger.LogInfo($"Try to Register  User: {username}");
                // same as in databse table UserName
                IdentityUser crateuser = new IdentityUser { UserName = username, Email = username };
                var result = await _user_Manager.CreateAsync(crateuser, pass);
                if (result.Succeeded)
                {
                    _logger.LogInfo("Sucessfull Registration");
                    var user = await _user_Manager.FindByNameAsync(username);
             
                    return Ok(new { result.Succeeded});
                }
                _logger.LogInfo($"Unsucessfull  Registration {username}");
                return StatusCode(500);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Loggin.Contact Denis");

            }

        }



    }
}
