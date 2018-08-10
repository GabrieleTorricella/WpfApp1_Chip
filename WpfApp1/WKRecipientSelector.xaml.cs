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
    /// Interaction logic for WKRecipientSelector.xaml
    /// </summary>
    public partial class WKRecipientSelector : UserControl
    {
        public WKRecipientSelector()
        {
            InitializeComponent();
            OnLoad();
        }
        private void OnLoad()
        {
            List<UserCase> caseUsers = new List<UserCase>()
            {
                new UserCase("alfa","alfa","alfa@alfa.it","CASE PARTIES", "Clients"),
                new UserCase("beta","beta","beta@beta.it","CASE PARTIES", "Clients"),
            new UserCase("gamma", "gamma", "gamma@gamma.it", "CASE PARTIES", "Clients"),
            new UserCase("delta","delta","delta@delta.it","CASE PARTIES", "Opposite Parties"),
            new UserCase("epsilon","epsilon","epsilon@epsilon.it","CASE PARTIES", "Opposite Parties"),
            new UserCase("zeta","zeta","zeta@zeta.it","CASE PARTIES", "Opposite Parties"),
            new UserCase("eta","eta","eta@eta.it","CASE PARTIES", "Opposite Parties"),
            new UserCase("theta","theta","theta@theta.it","CASE PARTIES", "Other Contacts"),
            new UserCase("iota","iota","iota@iota.it","CASE PARTIES", "Other Contacts")
        };

            List<User> users= new List<User>()
            {
                new User("Pippo","Pappo"){ GroupName="aa"},
                new User("Toto","asd","aaaaaaaaa@bbbbbbb.com"){ GroupName="aa"},
                new User("Franco","Ciccio","aaaaasdddaaaa@bbbbbbb.com"){GroupName ="bb"},
                new User("SSS","asd","adfasdsfdsafdasaaaaaaaaa@bbbbbbb.com"){GroupName ="bb"},
                new User("TADFDto","asd","aaaaaaaadfadfasaa@bbbbbbb.com"){GroupName ="bb"},
                new User("adfsfasd","zzz","aaaaaaaadfasdfadfasaa@bbbbbbb.com"){GroupName ="cc"}
            };
            Chip.ItemsSource = caseUsers;

            Chip.ElementiDaVisualizzare = "Mail";
            Chip.ImageName = "Image";
            Chip.SearchProperties = new List<string>() { "Name", "Surname", "Mail" };
            Chip.GroupNameProp = new List<string>()
            {
                "GroupName",
                "GroupNameCategory"
            };
        
        EventManager.RegisterClassHandler(typeof(WKChip),
                                               WKChip.AddButtonClickChipEvent,
                                               new RoutedEventHandler(ChipItem_AddChip), true);
        }

        private void ChipItem_AddChip(object sender, RoutedEventArgs e)
        {
            Window w = new Window();
            w.Width = 300;
            w.Height = 100;

            w.Content = new TextBlock() { Text = "Nuovo utente" };
            w.Show();
        }
    }
}
