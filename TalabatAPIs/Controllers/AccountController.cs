using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;
using TalabatAPIs.Extensions;

namespace TalabatAPIs.Controllers
{ 
	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAuthServices _authServices;
		private readonly IMapper _mapper;

		public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
			IAuthServices authServices,
			IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_authServices = authServices;
			_mapper = mapper;
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email );
			if (user is null) return Unauthorized(new ApiResponse(401, "Invalid Login"));
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authServices.CreateTokenAsync(user ,_userManager)
			});

        }

		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.Phone

			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });
			return Ok(new UserDto
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				Token = await _authServices.CreateTokenAsync(user, _userManager)

			});

		}
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
			var user = await _userManager.FindByNameAsync(email);
			return Ok(new UserDto()
			{
				DisplayName = user?.DisplayName ?? string.Empty,
				Email = user?.Email ?? string.Empty,
				Token = await _authServices.CreateTokenAsync(user, _userManager)
			});

		}
		[Authorize]
		[HttpGet("address")]
		public async Task<ActionResult<Address>> GetUserAddress()
		{

			var user = await _userManager.FindUserAddressAysnc(User);
			return Ok(_mapper.Map<AddressDto>(user.Address));
		}
		[Authorize]
		[HttpPut("address")]
		public async Task<ActionResult<Address>> UpdateUserAdderss(AddressDto address)
		{
			var updatedAddress = _mapper.Map<Address>(address);
			var user = await _userManager.FindUserAddressAysnc(User);
			updatedAddress.Id = user.Address.Id;
			user.Address = updatedAddress;
			
			var result = await _userManager.UpdateAsync(user);
			if(!result.Succeeded) 
			{
				return BadRequest(new ApiValidationErrorResponse()
				{
					Errors = result.Errors.Select(E => E.Description)
				});
			}
			return Ok(address);


		}
	}
}
