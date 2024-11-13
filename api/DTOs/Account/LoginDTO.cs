using System;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account;

public class LoginDTO
{
  [Required]
  public string UserName { get; set; }
  public string Password { get; set; }
}
