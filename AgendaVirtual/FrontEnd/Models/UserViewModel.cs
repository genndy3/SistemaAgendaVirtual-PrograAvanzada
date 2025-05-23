﻿using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Models
{
    public class UserViewModel
    {
        [Required]
        public string UserName { get; set; }

        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
