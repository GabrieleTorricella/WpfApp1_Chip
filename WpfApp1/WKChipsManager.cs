using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp1
{
    /// <summary>
    /// Chip selector
    /// </summary>
    public class WKChipsManager : Control, INotifyPropertyChanged
    {
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WKChipsManager));

        // Provide CLR accessors for the event
        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        // This method raises the Tap event
        void RaiseTextChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(WKChipsManager.TextChangedEvent);
            RaiseEvent(newEventArgs);
        }

        // For demonstration purposes we raise the event when the MyButtonSimple is clicked
        private ItemsControl _chipsItem;
        private ComboBox _chipsCombobox = new ComboBox();
        private ListBox listBox = new ListBox();
        private TextBox _searchBox = new TextBox();
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ElementiDaVisualizzareProperty;
        public static readonly DependencyProperty SearchPropertiesProperty;
        public static readonly DependencyProperty GroupNameProperty;
        private string _viewElement = default(string);
        public string TextValue { get; private set; }
        private ICollectionView collectionView;
        private DataTemplate data = new DataTemplate();
        private string _filterString = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
        private ListBox _listTo = new ListBox();

        public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; NotifyPropertyChanged("FilterString"); collectionView.Refresh(); }
        }
        public string ElementiDaVisualizzare
        {
            get => (string)base.GetValue(WKChipsManager.ElementiDaVisualizzareProperty);
            set { if (value == null) { base.ClearValue(WKChipsManager.ElementiDaVisualizzareProperty); return; } base.SetValue(WKChipsManager.ElementiDaVisualizzareProperty, value); }
        }
        public IEnumerable<string> SearchProperties
        {
            get => (IEnumerable<string>)base.GetValue(WKChipsManager.SearchPropertiesProperty);
            set { if (value == null) { base.ClearValue(WKChipsManager.SearchPropertiesProperty); return; } base.SetValue(WKChipsManager.SearchPropertiesProperty, value); }
        }
        public string GroupNameProp
        {
            get => (string)base.GetValue(WKChipsManager.GroupNameProperty);
            set { if (value == null) { base.ClearValue(WKChipsManager.GroupNameProperty); return; } base.SetValue(WKChipsManager.GroupNameProperty, value); }
        }

        static WKChipsManager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WKChipsManager), new FrameworkPropertyMetadata(typeof(WKChipsManager)));

            WKChipsManager.ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource",
                typeof(IEnumerable),
                typeof(WKChipsManager),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.OnItemsSourceChanged)));

            WKChipsManager.ElementiDaVisualizzareProperty =
                DependencyProperty.Register(
                    "ElementiDaVisualizzare",
                    typeof(string),
                    typeof(WKChipsManager),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.ElementiDaVisualizzareCallback)));

            WKChipsManager.SearchPropertiesProperty =
               DependencyProperty.Register(
                   "SearchProperties",
                   typeof(IEnumerable<string>),
                   typeof(WKChipsManager),
                   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.SearchPropertiesCallback)));

            WKChipsManager.GroupNameProperty =
               DependencyProperty.Register(
                   "GroupName",
                   typeof(string),
                   typeof(WKChipsManager),
                   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.GroupNameCallBack)));
        }

        private bool CustomerFilter(object item)
        {
            //IGroup customer = item as IGroup;
            if (!string.IsNullOrEmpty(_filterString) && SearchProperties != null && SearchProperties.Any())
            {
                return SearchProperties.Any(x => (item.GetType().GetProperty(x).GetValue(item)?.ToString().ToLower() ?? string.Empty).Contains(_filterString.ToLower()));
            }
            return true;
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Proprietà della dependency property ItemsSource
        /// </summary>
        [Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Content")]
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)base.GetValue(WKChipsManager.ItemsSourceProperty);
            set
            {
                if (value == null)
                {
                    base.ClearValue(WKChipsManager.ItemsSourceProperty);
                    return;
                }
                base.SetValue(WKChipsManager.ItemsSourceProperty, value);
            }
        }

        private static void GroupNameCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void SearchPropertiesCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var ChipsSelector = d as WKChips2;
            //if (ChipsSelector != null)
            //{
            //    var users = e.NewValue as IEnumerable<string>;
            //    if (ChipsSelector.treeView != null)
            //        ChipsSelector.SetItemSource(users);
            //}
        }

        /// <summary>
        /// Call back di ElementiDaVisualizzareProperty. Gestisce la dependency property
        /// </summary>
        /// <param name="d">Fa riferimento alla classe "Padre"</param>
        /// <param name="e">Fa riferimento ai cambiamenti</param>
        private static void ElementiDaVisualizzareCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChipsManager;
            if (item != null)
            {
                var viewElements = e.NewValue as string;
                if (item.listBox == null)
                    item._viewElement = viewElements;
                else
                    item._viewElement = viewElements;
            }
        }

        private void ItemSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void SetViewElemtents(string viewElements)
        {
            this.listBox.DisplayMemberPath = viewElements;
        }

        /// <summary>
        /// Call back di ItemsSourceProperty. Gestisce la dependency property
        /// </summary>
        /// <param name="d">Fa riferimento alla classe "Padre"</param>
        /// <param name="e">Fa riferimento ai cambiamenti</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _wkChips2 = d as WKChipsManager;
            if (_wkChips2 != null)
            {
                var users = e.NewValue as IEnumerable;
                if (_wkChips2.listBox != null)
                    _wkChips2.SetItemSource(users);
            }
        }

        /// <summary>
        /// Metodo che serve per inserire i valori nella proprietà ItemsSource del TreeView
        /// </summary>
        /// <param name="e">Lista da inserire</param>
        private void SetItemSource(IEnumerable e)
        {
            this.listBox.ItemsSource = e;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            listBox = (ListBox)GetTemplateChild("ListItems");
            _listTo = (ListBox)GetTemplateChild("ListTo");
            collectionView = CollectionViewSource.GetDefaultView(ItemsSource);
            collectionView.Filter = CustomerFilter;
            if (!string.IsNullOrEmpty(GroupNameProp))
            {
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription(GroupNameProp));
            }
            listBox.ItemsSource = collectionView;
            listBox.DisplayMemberPath = _viewElement;
            listBox.SelectionChanged += ListBoxItems_SelectionChanged;
            _searchBox = (TextBox)GetTemplateChild("SearchBox");
            _searchBox.TextChanged += _searchBox_TextChanged;
            _searchBox.Focus();
        }

        private void ListBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chipItem = new WKChip();
            //TODO CHANGE FOR MANAGE GENERIC TYPES
            var user = listBox.SelectedItem as User;
            if (user != null)
            {
                chipItem.InfoUser = user.Name;
                _listTo.Items.Add(chipItem);
                chipItem.DeleteChip += ChipItem_DeleteChip;
                _searchBox.Text = default(string);
            }
            e.Handled = true;
            listBox.SelectedIndex = -1;
            Console.WriteLine("Selected index changed");
        }

        private void ChipItem_DeleteChip(object sender, RoutedEventArgs e)
        {
            _listTo.Items.Remove(sender as WKChip);
        }

        private void _searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterString = _searchBox.Text;
        }
    }
}