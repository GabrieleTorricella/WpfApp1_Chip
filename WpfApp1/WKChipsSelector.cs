using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Classe di appoggio per la treeview
    /// </summary>
    public class Group
    {
        public string GroupName { get; set; }
        public IEnumerable Items { get; set; }
    }

    /// <summary>
    /// Chip selector
    /// </summary>
    public class WKChipsSelector : Control
    {
        private ItemsControl _chipsItem;
        private ComboBox _chipsCombobox = new ComboBox();
        private TreeView treeView = new TreeView();
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ElementiDaVisualizzareProperty;
        private string _viewElement = default(string);
        private IEnumerable<Group> _itemSource = default(IEnumerable<Group>);
        private DataTemplate data = new DataTemplate();
        public List<WKChip> chips;

        /// <summary>
        /// Proprietà della dependency property ItemsSource
        /// </summary>
        [Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Content")]
        public IEnumerable<Group> ItemsSource
        {
            get => (IEnumerable<Group>)base.GetValue(WKChipsSelector.ItemsSourceProperty);
            set
            {
                if (value == null)
                {
                    base.ClearValue(WKChipsSelector.ItemsSourceProperty);
                    return;
                }
                base.SetValue(WKChipsSelector.ItemsSourceProperty, value);
            }
        }

        public string ElementiDaVisualizzare
        {
            get => (string)base.GetValue(WKChipsSelector.ElementiDaVisualizzareProperty);
            set { if (value == null) { base.ClearValue(WKChipsSelector.ElementiDaVisualizzareProperty); return; } base.SetValue(WKChipsSelector.ElementiDaVisualizzareProperty, value); }
        }

        static WKChipsSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WKChipsSelector), new FrameworkPropertyMetadata(typeof(WKChipsSelector)));

            WKChipsSelector.ItemsSourceProperty =
                DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable<Group>),
                typeof(WKChipsSelector),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsSelector.OnItemsSourceChanged)));

            WKChipsSelector.ElementiDaVisualizzareProperty =
                DependencyProperty.Register(
                    "ElementiDaVisualizzare",
                    typeof(string),
                    typeof(WKChipsSelector),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsSelector.ElementiDaVisualizzareCallback)));
        }

        /// <summary>
        /// Call back di ElementiDaVisualizzareProperty. Gestisce la dependency property
        /// </summary>
        /// <param name="d">Fa riferimento alla classe "Padre"</param>
        /// <param name="e">Fa riferimento ai cambiamenti</param>
        private static void ElementiDaVisualizzareCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChipsSelector;
            if (item != null)
            {
                var viewElements = e.NewValue as string;
                if (item.treeView == null)
                    item._viewElement = viewElements;
                else
                    //item.SetViewElemtents(viewElements);
                    item._viewElement = viewElements;
                //var el = item._itemSource != null ? item._itemSource.ToList() as List<Group> : item.ItemsSource.ToList() as List<Group>;
                //item._viewElement = el.Where(i => i.Items = viewElements);
            }
        }

        private void SetViewElemtents(string viewElements)
        {
            this.treeView.DisplayMemberPath = viewElements;
        }

        /// <summary>
        /// Call back di ItemsSourceProperty. Gestisce la dependency property
        /// </summary>
        /// <param name="d">Fa riferimento alla classe "Padre"</param>
        /// <param name="e">Fa riferimento ai cambiamenti</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChipsSelector;
            if (item != null)
            {
                var users = e.NewValue as IEnumerable<Group>;
                if (item.treeView == null)
                    item._itemSource = users;
                else
                    item.SetItemSource(users);
            }
        }

        /// <summary>
        /// Metodo che serve per inserire i valori nella proprietà ItemsSource del TreeView
        /// </summary>
        /// <param name="e">Lista da inserire</param>
        private void SetItemSource(IEnumerable<Group> e)
        {
            this.treeView.ItemsSource = e;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _chipsItem = (ItemsControl)GetTemplateChild("ChipsItems");
            treeView = (TreeView)GetTemplateChild("TrvCombobox");
            treeView.ItemsSource = _itemSource != null ? _itemSource : ItemsSource;
            treeView.DisplayMemberPath = _viewElement;
            chips = new List<WKChip>();

        }

        private void Chip_Tap(object sender, RoutedEventArgs e)
        {
            chips.Remove(sender as WKChip);
        }
    }
}