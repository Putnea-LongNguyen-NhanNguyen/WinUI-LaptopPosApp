using LaptopPosApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LaptopPosApp.Services
{
    public sealed class SendMailService
    {
        private static void SendEmail(string to, string subject, string body, bool isBodyHtml = true)
        {
            if (!Regex.IsMatch(to, @"^\w+([-+.']\w+)*@(\[*\w+)([-.]\w+)*\.\w+([-.]\w+\])*$"))
            {
                throw new ArgumentException("Invalid email address");
            }



            string fromMail = "ntnhan221@clc.fitus.edu.vn";
            string? fromPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
            if (fromPassword == null)
            {
                return;
            }

            MailMessage message = new()
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
            };

            message.To.Add(new MailAddress(to));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.SendAsync(message, null);
        }

        public static void SendVoucherEmail(Dictionary<Customer, List<Voucher>> dictionary)
        {
            foreach (var entry in dictionary)
            {
                Customer customer = entry.Key;

                // use your mail to demonstrate
                string customerEmail = "tinnhan1806@gmail.com";

                List<Voucher> vouchers = entry.Value;
                if (vouchers == null || vouchers.Count == 0)
                {
                    continue;
                }

                vouchers = [.. vouchers.Where(voucher => voucher.EndDate > DateTime.Now)];
                if (vouchers.Count == 0)
                {
                    continue;
                }

                string voucherTableString = @"
                    <table class=""voucher-table"" style=""width: 100%; border-collapse: collapse;"">
                        <thead>
                            <tr style=""background-color: #f2f2f2;"">
                                <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">STT</th>
                                <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Mã</th>
                                <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Giảm</th>
                                <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Ngày bắt đầu</th>
                                <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Ngày kết thúc</th>
                            </tr>
                        </thead>
                        <tbody>
                            <Replace-Table-Rows-Here>
                        </tbody>
                    </table>
                ";
                string voucherTableRows = "";

                var i = 1;
                foreach (var voucher in vouchers)
                {
                    voucherTableRows += $@"
                        <tr>
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Code}</td>
                    ";
                    if (voucher.Type == VoucherType.Percentage)
                    {
                        voucherTableRows += $@"
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Value}%</td>
                    ";
                    }
                    else
                    {
                        voucherTableRows += $@"
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Value:#,### đ}</td>
                    ";
                    }
                    voucherTableRows += $@"
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.StartDate}</td>
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.EndDate}</td>
                        </tr>
                    ";
                    i++;
                }

                voucherTableString = voucherTableString.Replace("<Replace-Table-Rows-Here>", voucherTableRows);
                string emailBody = emailBodyTemplate.Replace("<Replace-Customer-Name-Here>", customer.Name)
                    .Replace("<Replace-Description-Here>", voucherEmailDescription)
                    .Replace("<Replace-Content-Here>", voucherTableString);
                SendEmail(customerEmail, "LmaoPosApp", emailBody);
            }
        }

        public static void SendOrderEmail(Customer customer, Order order)
        {
            string emailBody = emailBodyTemplate.Replace("<Replace-Customer-Name-Here>", customer.Name)
                .Replace("<Replace-Description-Here", orderEmailDescription);
        }

        private static readonly string emailBodyTemplate = @"
            <!DOCTYPE html>
            <html lang=""en"">

            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Voucher Email</title>
            </head>

            <body>
                <div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0;"">
                    <div style=""border-radius: 1rem; padding: 1rem;"">
                        <div style=""background-color: red; padding: 1rem; text-align: center;"">
                            <h1 style=""margin: 0; color: white;"">Lmao Pos App</h1>
                        </div>
                        <h4 style=""margin-bottom: 0.5rem;"">Thân gửi <Replace-Customer-Name-Here></h4>

                        <Replace-Description-Here>

                        <Replace-Content-Here>
                    </div>          
                </div>
            </body>
            </html>
        ";

        private static readonly string voucherEmailDescription = "<p>Làm miếng voucher?</p>";
        private static readonly string orderEmailDescription = "<p>Cảm ơn bạn đã mua sắm tạo LmaoPos, đây là hóa đơn của bạn</p>";
    }
}