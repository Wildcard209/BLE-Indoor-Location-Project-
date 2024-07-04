using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebClient.Controllers;

namespace WebClient.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string Username { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

    }
}
