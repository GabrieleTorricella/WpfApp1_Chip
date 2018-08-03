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
        List<WKChip> chip;
        public MainWindow()
        {
            InitializeComponent();
            OnLoad();
            //chip = Chip.chips = new List<WKChip>();
            //prova.Tap += Chip1_Tap;
            //chip.Add(prova);
            //WKChip c = new WKChip();
            //c.Tap += Chip1_Tap;
            //lstChips.Items.Add(c);
        }

        //private void Chip1_Tap(object sender, RoutedEventArgs e)
        //{
        //    //lstChips.Remove(sender as WKChip);
        //    lstChips.Items.Remove(sender as WKChip);
        //}

        /*private void OldOnLoad()
        //{
        //    Chip.ItemsSource = new List<Group>()
        //    {
        //        new Group()
        //        {
        //            GroupName ="parties",
        //            Items =  new List<User>()
        //                {
        //                    new User("Pippo","Pappo"),
        //                    new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
        //                    new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
        //                }
        //        },
        //        new Group()
        //        {
        //            GroupName ="other",
        //            Items =  new List<User>()
        //                {
        //                    new User("Pippo","Pappo"),
        //                    new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
        //                    new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
        //                }
        //        }

        //    };
        //    Chip.ElementiDaVisualizzare = "Name";
        //    //var dic = new Dictionary<string, IEnumerable<User>>();
        //    //dic.Add("parties", new List<User>()
        //    //    {
        //    //        new User("Pippo","Pappo"),
        //    //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
        //    //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
        //    //    });
        //    //dic.Add("other",
        //    //    new List<User>()
        //    //    {
        //    //        new User("Pippo","Pappo"),
        //    //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"),
        //    //        new User("Toto","asd","aaaaaaaaa@bbbbbbb.com")
        //    //    })
        //    //;
        }*/
        private void OnLoad()
        {
            Chip.ItemsSource = new List<User>()
            {
                new User("Pippo","Pappo"){ GroupName="aa"},
                new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"){ GroupName="aa"},
                new User("Toto","asd","aaaaasdddaaaa@bbbbbbb.com"){GroupName ="bb"},
                new User("SSS","asd","adfasdsfdsafdasaaaaaaaaa@bbbbbbb.com"){GroupName ="bb"},
                new User("TADFDto","asd","aaaaaaaadfadfasaa@bbbbbbb.com"){GroupName ="bb"},
                new User("adfsfasd","zzz","aaaaaaaadfasdfadfasaa@bbbbbbb.com"){GroupName ="cc"}
            };

            Chip.ElementiDaVisualizzare = "Name";
            Chip.SearchProperties = new List<string>() { "Name", "Surname", "Mail" };

        }
    }
}
