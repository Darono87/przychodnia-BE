// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using API.DTO;
using API.Utils;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

namespace API.Services
{
    public class MailService : IMailService
    {
        private String smtp;
        private String recipient;
        private int port;
        private String password;
        private String login;

        public MailService(EmailConfig emailConfig)
        {
            smtp = emailConfig.SmtpAddress;
            recipient = emailConfig.RecipientAddress;
            port = emailConfig.Port;
            password = emailConfig.Password;
            login = emailConfig.Login;
        }
        public async Task<IActionResult> SendAsync(MailDto mailDto)
        {

            if (areParametersWrong())
            {
                return new JsonResult(new ExceptionDto {Message = "Could not send the email"})
                {
                    StatusCode = 422
                };
            }
            
            var message = new MimeMessage();

            var from = new MailboxAddress(mailDto.Mail, 
                mailDto.Mail);
            message.From.Add(from);

            var to = new MailboxAddress("User", 
                recipient);
            message.To.Add(to);

            message.Subject = "New message from TAB clinic from: " + mailDto.Mail;
            
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = mailDto.Message;
            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(smtp, port, true);
                await client.AuthenticateAsync(login, password);
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                return new JsonResult(new ExceptionDto {Message = "Could not send the email"})
                {
                    StatusCode = 422
                };
            }
            return new JsonResult(mailDto) {StatusCode = 201};
        }

        private Boolean areParametersWrong()
        {
            return String.IsNullOrEmpty(smtp) || String.IsNullOrEmpty(recipient) || String.IsNullOrEmpty(password) ||
                   String.IsNullOrEmpty(login) || port == 0;
        }
    }
}
