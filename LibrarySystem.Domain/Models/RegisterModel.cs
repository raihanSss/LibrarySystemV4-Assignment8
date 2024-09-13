using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Domain.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fname { get; set; }

        public string Lname { get; set; }
        public string Phone { get; set; }

        public string Role { get; set; }

        public IFormFileCollection Attachments { get; set; }
    }
}
