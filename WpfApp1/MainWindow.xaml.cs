using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnLoad();
        }

        private void OnLoad()
        {
            Chip.ItemsSource = new List<Group>()
            {
                new Group()
                {
                    GroupName ="parties",
                    Items =  new List<User>()
                        {
                            new User("Pippo","Pappo"),
                            new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
                            new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
                        }
                },
                new Group()
                {
                    GroupName ="other",
                    Items =  new List<User>()
                        {
                            new User("Pippo","Pappo"),
                            new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
                            new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
                        }
                }

            };
            Chip.ElementiDaVisualizzare = "Name";
            //var dic = new Dictionary<string, IEnumerable<User>>();
            //dic.Add("parties", new List<User>()
            //    {
            //        new User("Pippo","Pappo"),
            //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
            //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
            //    });
            //dic.Add("other",
            //    new List<User>()
            //    {
            //        new User("Pippo","Pappo"),
            //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
            //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
            //    })
            //;
        }
    }
}
