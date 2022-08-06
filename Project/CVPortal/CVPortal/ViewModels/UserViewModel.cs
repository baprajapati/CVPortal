using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string[] RoleNames { get; set; } = { };

        public string RoleName { get; set; }

        [Required]
        public string HANAME { get; set; }

        [Required]
        [Display(Name = "Employee Code")]
        public string HAUSER { get; set; }

        [Required]
        public string Dept_Code { get; set; }

        [Required]
        public string InitiatorAccess { get; set; } = "Both";

        public string HANEXT { get; set; }

        public string Status { get; set; }
    }
}