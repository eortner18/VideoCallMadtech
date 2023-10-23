using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmailSendingHelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("madtechpdfemail@gmail.com");
                    mail.To.Add("elias.ortner13@gmail.com");
                    mail.Subject = "PDF von Madtech";
                    mail.Body = "<h1> Dear User ur PDF is here";
                    mail.Attachments.Add(new Attachment("C:\\Users\\elias\\Downloads\\165_Ü_Angular_Routes_Northwind.pdf"));

                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("madtechpdfemail@gmail.com", "baxmhgcasdxbcgbd");
                        smtp.EnableSsl = true;
                        
                        smtp.Send(mail);
                        LabelToSend.Content = "Mail was sent";

                    }
                }

            }
            catch (Exception ex)
            {
                LabelToSend.Content += ex.ToString();
            }

        }
    }
}
