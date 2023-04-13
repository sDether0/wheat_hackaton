﻿
using Wheat.Models.Requests;
using Wheat.Models.Responses;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wheat.Api.Service;
using Wheat.DataLayer.DataBase;
using Wheat.Models.Entities;

namespace Wheat.Api.Controllers
{
    public class AuthManagementController : Controller
    {
        private readonly UserManager<WIdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly WheatDbContext _wheatDbContext;

        public AuthManagementController(UserManager<WIdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            WheatDbContext wheatDbContext)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _wheatDbContext = wheatDbContext;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest user)
        {
            // Check if the incoming request is valid
            if (ModelState.IsValid)
            {
                // check i the user with the same email exist
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                                            "Email already exist"
                                        }
                    });
                }

                var newUser = new WIdentityUser() { Email = user.Email, UserName = user.Email, Code = user.Code};
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    return Ok(await GenerateJwtToken(newUser));
                }

                return new JsonResult(new RegistrationResponse()
                {
                    Success = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                }
                        )
                { StatusCode = 500 };
            }

            return BadRequest(new RegistrationResponse()
            {
                Success = false,
                Errors = new List<string>(){
                                            "Invalid payload"
                                        }
            });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var res = await VerifyToken(tokenRequest);

                if (res == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Invalid tokens"
                        },
                        Success = false
                    });
                }

                return Ok(res);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }

        private async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var valPar = _tokenValidationParameters;
                valPar.ValidateLifetime = false;
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, valPar, out var validatedToken);
                valPar.ValidateLifetime = true;

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }


                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);


                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Success = false
                    };
                }


                var storedRefreshToken = await _wheatDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Success = false
                    };
                }


                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Success = false
                    };
                }


                if (storedRefreshToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been used" },
                        Success = false
                    };
                }


                if (storedRefreshToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been revoked" },
                        Success = false
                    };
                }


                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;


                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Success = false
                    };
                }

                storedRefreshToken.IsUsed = true;
                _wheatDbContext.RefreshTokens.Update(storedRefreshToken);
                await _wheatDbContext.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {

            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {

                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {

                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Invalid authentication request"
                        }
                    });
                }


                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (isCorrect)
                {
                    return Ok(await GenerateJwtToken(existingUser));
                }
                else
                {

                    return BadRequest(new RegistrationResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Invalid authentication request"
                        }
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Success = false,
                Errors = new List<string>(){
                    "Invalid payload"
                }
            });
        }

        private async Task<AuthResult> GenerateJwtToken(WIdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()
            };

            await _wheatDbContext.RefreshTokens.AddAsync(refreshToken);
            await _wheatDbContext.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
