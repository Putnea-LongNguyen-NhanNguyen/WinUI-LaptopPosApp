using LaptopPosApp.Dao;
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

        // might delete this
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

                string voucherTableRows = "";

                var i = 1;
                foreach (var voucher in vouchers)
                {
                    if (voucher.Type == VoucherType.Percentage)
                    {
                        voucherTableRows += $@"
                            <tr>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Code}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Value}%</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.StartDate}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.EndDate}</td>
                            </tr>
                        ";
                    }
                    else
                    {
                        voucherTableRows += $@"
                            <tr>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Code}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Value:#,### đ}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.StartDate}</td>
                                <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.EndDate}</td>
                            </tr>
                        ";
                    }
                    i++;
                }

                string voucherTableString = @$"
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
                            {voucherTableRows}
                        </tbody>
                    </table>
                ";
                string emailBody = emailBodyTemplate(customer.Name, voucherEmailDescription, voucherTableString);
                SendEmail(customerEmail, voucherEmailSubject, emailBody);
            }
        }

        public static void SendVoucherEmail(Customer customer, Voucher voucher)
        {
            // use your mail to demonstrate
            string customerEmail = "tinnhan1806@gmail.com";

            string voucherTableRows = "";

            var i = 1;

            if (voucher.Type == VoucherType.Percentage)
            {
                voucherTableRows += $@"
                    <tr>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Code}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Value}%</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.StartDate}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.EndDate}</td>
                    </tr>
                ";
            }
            else
            {
                voucherTableRows += $@"
                    <tr>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Code}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.Value:#,### đ}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.StartDate}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{voucher.EndDate}</td>
                    </tr>
                ";
            }

            string voucherTableString = @$"
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
                        {voucherTableRows}
                    </tbody>
                </table>
            ";
            string emailBody = emailBodyTemplate(customer.Name, voucherEmailDescription, voucherTableString);
            SendEmail(customerEmail, voucherEmailSubject, emailBody);
        }

        public static void SendOrderEmail(Order order)
        {
            // get customer mail here
            string customerEmail = order.Customer.Email;
            // change to this to demonstrate
            customerEmail = "tinnhan1806@gmail.com";
            string orderProductTableRows = "";

            int i = 1;
            order.Products.ForEach(op =>
            {
                var tempPrice = op.Product.TemporaryPrices
                    .Where(p => op.Order.Timestamp >= p.StartDate && op.Order.Timestamp <= p.EndDate)
                    .FirstOrDefault()?.Price ?? op.Product.Price;
                orderProductTableRows += $@"
                    <tr style=""background-color: #f2f2f2;"">
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{op.Product.Name}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{op.Quantity}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{tempPrice:#,###đ}</td>
                    </tr>
                ";
                i++;
            });

            i = 1;
            string? voucherTableRows = null;
            string? voucherRows = null;
            order.Vouchers.ForEach(v =>
            {
                voucherRows += $@"
                    <tr style=""background-color: #f2f2f2;"">
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{i}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"" colspan=""2"">{v.Code}</td>
                        <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">{v.ValueString}</td>
                    </tr>
                ";
                i++;
            });

            if (voucherRows != null)
            {
                voucherTableRows = $@"
                    <thead>
                        <tr style=""background-color: #f2f2f2;"">
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">STT mã giảm</th>
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"" colspan=""2"">Mã giảm</th>
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Giá trị giảm</th>
                        </tr>
                    </thead>
                    <tbody style=""border-bottom: 1rem; border-top: 0.5rem;"">
                        {voucherRows}
                    </tbody>
                ";
            }

            string orderTableString = $@"
                <table class=""voucher-table"" style=""width: 100%; border-collapse: collapse;"">
                    <thead>
                        <tr style=""background-color: #f2f2f2;"">
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">STT hàng</th>
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Tên hàng</th>
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Số lượng</th>
                            <th style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Giá</th>
                        </tr>
                    </thead>
                    <tbody style=""border-bottom: 1rem; border-top: 0.5rem;"">
                        {orderProductTableRows}
                    </tbody>
                    
                    {voucherTableRows ?? ""}

                    <tbody style=""border-bottom: 1rem; border-top: 0.5rem;"">
                        <tr style=""background-color: #f2f2f2;"">
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"">Tổng giá trị hóa đơn</td>
                            <td style=""text-align: center; padding: 8px; border: 1px solid #ddd;"" colspan=""3"">{order.TotalPrice:###,# đồng}</td>
                        </tr>
                    </tbody>
                </table>
            ";

            string emailBody = emailBodyTemplate(order.Customer.Name, orderEmailDescription, orderTableString);
            SendEmail(customerEmail, orderEmailSubject, emailBody);
        }

        private static string emailBodyTemplate(string customerName, string description, string content)
        {
            return @$"
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
                            <h4 style=""margin-bottom: 0.5rem;"">Thân gửi {customerName}</h4>

                            {description}

                            {content}
                        </div>          
                    </div>
                </body>
                </html>
            ";
        }

        private static readonly string voucherEmailSubject = "Kính gửi voucher";
        private static readonly string orderEmailSubject = "Kính gửi order";

        private static readonly string voucherEmailDescription = "<p>Làm miếng voucher?</p>";
        private static readonly string orderEmailDescription = "<p>Làm miếng hóa đơn?</p>";
    }
}