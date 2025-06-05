using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamseven.PhyGen.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Gửi email đến địa chỉ được chỉ định với tiêu đề và nội dung cụ thể.
        /// </summary>
        /// <param name="toEmail">Địa chỉ email người nhận.</param>
        /// <param name="subject">Tiêu đề email.</param>
        /// <param name="body">Nội dung email.</param>
        void SendEmail(string toEmail, string subject, string body);
    }
}