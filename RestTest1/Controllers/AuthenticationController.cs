using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestTest1.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestTest1.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class AuthenticationController : Controller
    {
		private static string USERNAME = "testuser";
		private static string PASSWORD = "password123";

		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login([FromBody] AuthenticationRequest authenticationRequest)
		{
			AuthenticationResponse toReturn = null;

			if (authenticationRequest != null && authenticationRequest.Username != null && authenticationRequest.Username.ToLower().Equals(USERNAME) &&
				authenticationRequest.Password != null && authenticationRequest.Password.Equals(PASSWORD))
			{
				toReturn = new AuthenticationResponse();
				DateTime current = DateTime.Now + TokenAuthOption.ExpiresSpan;
				
				toReturn.Token = GenerateToken(authenticationRequest.Username, current);
			}
			else
			{
				return Unauthorized();
			}

			return Ok(toReturn);
		}

		private string GenerateToken(string username, DateTime expires)
		{
			var handler = new JwtSecurityTokenHandler();

			ClaimsIdentity identity = new ClaimsIdentity(
				new GenericIdentity(username, "TokenAuth"),
				new[] {
					new Claim("SomeKey", "SomeValue")
				}
			);
			var securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = TokenAuthOption.Issuer,
				Audience = TokenAuthOption.Audience,
				SigningCredentials = TokenAuthOption.SigningCredentials,
				Subject = identity,
				Expires = expires
			});
			return handler.WriteToken(securityToken);
		}
	}
}
