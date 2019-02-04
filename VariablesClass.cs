using System;
using System.Collections.Generic;
using CodePasswordDLL;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestMailSender
{
    public static class VariablesClass
    {
        public static Dictionary<string,string> Senders
        {
            get { return dicSenders; }
        }

        private static Dictionary<string, string> dicSenders = new Dictionary<string, string>()
        {


            {"varankinva@yandex.ru", PasswordClass.getPassword("Qpdiub'2") },
            {"fotki43@yandex.ru", "111111"}//PasswordClass.getPassword("Pochta&1") }
        };
    }
}
