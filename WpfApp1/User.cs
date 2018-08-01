using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class User
    {
        private string _name, _surname, _mail;

        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public string Mail { get => _mail; set => _mail = value; }

        public User()
        {

        }
        public User(string name, string surname):this()
        {
            Name = name;
            Surname = surname;
        }
        public User(string name, string surname, string mail) : this(name, surname)
        {
            Mail = mail;
        }


    }
}
