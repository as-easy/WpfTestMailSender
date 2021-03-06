﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;

namespace WpfTestMailSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class WpfMailSender: Window
    {


        public WpfMailSender()
        {
            InitializeComponent();
            cbSenderSelect.ItemsSource = VariablesClass.Senders;
            cbSenderSelect.DisplayMemberPath = "Key";
            cbSenderSelect.SelectedValuePath = "Value";
            cbSmptSelect.ItemsSource = VariablesClass.Smtps;
            cbSmptSelect.DisplayMemberPath = "Key";
            cbSmptSelect.SelectedValuePath = "Value";

            DBclass db = new DBclass();
            dgEmails.ItemsSource = db.Emails;
            tscTabSwitcher.btnNextClick += TscTabSwitcher_btnNextClick;
           
        }
        private void TscTabSwitcher_btnNextClick(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //List<string> listStrMails = new List<string> { "varankinva@yandex.ru"}; // Список  email'ов //кому мы отправляем письмо
            //string strPassword = PasswordBox1.Password; // для WinForms - string strPassword = passwordBox.Text;
            //foreach (string mail in listStrMails)
            //{
            //    // Используем using, чтобы гарантированно удалить объект MailMessage после использования
            //    using (MailMessage mm = new MailMessage("avniknarav@gmail.com", mail))
            //    {
            //        // Формируем письмо
            //        mm.Subject = "Привет из C#"; // Заголовок письма
            //        mm.Body = "Hello, world!"; // Тело письма
            //        mm.IsBodyHtml = false; // Не используем html в теле письма
            //                               // Авторизуемся на smtp-сервере и отправляем письмо
            //                               // Оператор using гарантирует вызов метода Dispose, даже если при вызове
            //                               // методов в объекте происходит исключение.
            //        using (SmtpClient sc = new SmtpClient("smtp.gmail.com", 587))
            //        {
            //            sc.EnableSsl = true;
            //            sc.Credentials = new NetworkCredential("avniknarav@gmail.com", strPassword);
            //            try
            //            {
            //                sc.Send(mm);
            //            }

            //            catch (Exception ex)
            //            {
            //                MessageBox.Show("Невозможно отправить письмо " + ex.ToString());
            //            }
            //        }
            //    } //using (MailMessage mm = new MailMessage("sender@yandex.ru", mail))
            //}

            SendEndWindow sew = new SendEndWindow();
            sew.ShowDialog();

           // MessageBox.Show("Работа завершена.");
        }

        private void MiClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnClock_Click(object sender, RoutedEventArgs e)
        {

            tabControl.SelectedItem = tabPlanner;
        }

        private void BtnSendAtOnce_Click(object sender, RoutedEventArgs e)
        {
            string strLogin = cbSenderSelect.Text;
            string strPassword = cbSenderSelect.SelectedValue.ToString();
            string strSmtp = cbSmptSelect.Text;
            int iSmtpPort = Convert.ToInt32(cbSmptSelect.SelectedValue);

            if(string.IsNullOrEmpty(emailBody2.Text))
            {
                MessageBox.Show("Письмо не заполнено");
                tabControl.SelectedIndex = 2;
                return;
            }

            if (string.IsNullOrEmpty(strLogin))
            {
                MessageBox.Show("Выберите отправителя");
                return;
            }
            if (string.IsNullOrEmpty(strPassword))
            {
                MessageBox.Show("Укажите пароль отправителя");
                return;
            }
            if (string.IsNullOrEmpty(strSmtp))
            {
                MessageBox.Show("Выберите smtp порт");
                return;
            }
            EmailSendServiceClass emailSender = new EmailSendServiceClass(strLogin, strPassword, strSmtp, iSmtpPort);
            emailSender.SendMails((IQueryable<Email>)dgEmails.ItemsSource);



        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SchedulerClass sc = new SchedulerClass();
            TimeSpan tsSendTime = sc.GetSendTime(tbTimePicker.Text);
            if (tsSendTime == new TimeSpan())
            {
                MessageBox.Show("Некорректный формат даты");
                return;
            }
            DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
            if (dtSendDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата и время отправки писем не могут быть раньше, чем настоящее время");
            return;
            }

            string strLogin = cbSenderSelect.Text;
            string strPassword = cbSenderSelect.SelectedValue.ToString();
            string strSmtp = cbSmptSelect.Text;
            int iSmtpPort = Convert.ToInt32(cbSmptSelect.SelectedValue);
            EmailSendServiceClass emailSender = new EmailSendServiceClass(strLogin, strPassword, strSmtp, iSmtpPort);
            sc.SendEmails(dtSendDateTime, emailSender, (IQueryable<Email>)dgEmails.ItemsSource);

        }
        //public event RoutedEventHandler btnNextClick;
        //public event RoutedEventHandler btnPreviosClick;

        //private void btnPrevios_Click(object sender, RoutedEventArgs e)
        //{
        //    btnPreviosClick?.Invoke(sender, e);

        //}

        //private void btnNext_Click(object sender, RoutedEventArgs e)
        //{
        //    btnNextClick?.Invoke(sender, e);
        //}

        
    }
}
