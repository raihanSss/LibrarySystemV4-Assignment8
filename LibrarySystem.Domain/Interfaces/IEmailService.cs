using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendMail(MailData mailData, RegisterModel registerModel);
    }
}
