using System;
using api.DTOs.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
  private readonly UserManager<AppUser> _userManager;
  public AccountController(UserManager<AppUser> userManager)
  {
    _userManager = userManager;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
  {
    try
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var appUser = new AppUser
      {
        UserName = registerDTO.Username,
        Email = registerDTO.EmailAddress
      };

      var createduser = await _userManager.CreateAsync(appUser, registerDTO.Password);

      if (createduser.Succeeded)
      {
        var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

        if (roleResult.Succeeded)
        {
          return Ok("User created");
        }
        else
        {
          return StatusCode(500, roleResult.Errors);
        }
      }
      else
      {
        return StatusCode(500, createduser.Errors);
      }
    }
    catch (Exception e)
    {
      return StatusCode(500, e);
    }
  }
}
