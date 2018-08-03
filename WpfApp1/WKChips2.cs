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
    public class WKChips2 : Control, INotifyPropertyChanged
    {
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WKChips2));

        // Provide CLR accessors for the event
        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        // This method raises the Tap event
        void RaiseTextChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(WKChips2.TextChangedEvent);
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
        private List<WKChip> lstChip;

        public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; NotifyPropertyChanged("FilterString"); collectionView.Refresh(); }
        }
        public string ElementiDaVisualizzare
        {
            get => (string)base.GetValue(WKChips2.ElementiDaVisualizzareProperty);
            set { if (value == null) { base.ClearValue(WKChips2.ElementiDaVisualizzareProperty); return; } base.SetValue(WKChips2.ElementiDaVisualizzareProperty, value); }
        }
        public IEnumerable<string> SearchProperties
        {
            get => (IEnumerable<string>)base.GetValue(WKChips2.SearchPropertiesProperty);
            set { if (value == null) { base.ClearValue(WKChips2.SearchPropertiesProperty); return; } base.SetValue(WKChips2.SearchPropertiesProperty, value); }
        }
        public string GroupNameProp
        {
            get => (string)base.GetValue(WKChips2.GroupNameProperty);
            set { if (value == null) { base.ClearValue(WKChips2.GroupNameProperty); return; } base.SetValue(WKChips2.GroupNameProperty, value); }
        }

        static WKChips2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WKChips2), new FrameworkPropertyMetadata(typeof(WKChips2)));

            WKChips2.ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource",
                typeof(IEnumerable),
                typeof(WKChips2),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChips2.OnItemsSourceChanged)));

            WKChips2.ElementiDaVisualizzareProperty =
                DependencyProperty.Register(
                    "ElementiDaVisualizzare",
                    typeof(string),
                    typeof(WKChips2),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChips2.ElementiDaVisualizzareCallback)));

            WKChips2.SearchPropertiesProperty =
               DependencyProperty.Register(
                   "SearchProperties",
                   typeof(IEnumerable<string>),
                   typeof(WKChips2),
                   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChips2.SearchPropertiesCallback)));

            WKChips2.GroupNameProperty =
               DependencyProperty.Register(
                   "GroupName",
                   typeof(string),
                   typeof(WKChips2),
                   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChips2.GroupNameCallBack)));
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
            get => (IEnumerable)base.GetValue(WKChips2.ItemsSourceProperty);
            set
            {
                if (value == null)
                {
                    base.ClearValue(WKChips2.ItemsSourceProperty);
                    return;
                }
                base.SetValue(WKChips2.ItemsSourceProperty, value);
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
            var item = d as WKChips2;
            if (item != null)
            {
                var viewElements = e.NewValue as string;
                if (item.listBox == null)
                    item._viewElement = viewElements;
                else
                    //item.SetViewElemtents(viewElements);
                    item._viewElement = viewElements;
                //var el = item._itemSource != null ? item._itemSource.ToList() as List<Group> : item.ItemsSource.ToList() as List<Group>;
                //item._viewElement = el.Where(i => i.Items = viewElements);
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
            var _wkChips2 = d as WKChips2;
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
            _chipsItem = (ItemsControl)GetTemplateChild("ChipsItems");
            listBox = (ListBox)GetTemplateChild("Anagrafica");
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
            lstChip = new List<WKChip>();

        }

        private void ListBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chipItem = new WKChip();
            //TODO CHANGE FOR MANAGE GENERIC TYPES
            var user = listBox.SelectedItem as User;
            chipItem.InfoUser = user.Name;
            _listTo.Items.Add(chipItem);
            lstChip.Add(chipItem);

            chipItem.DeleteChip += ChipItem_DeleteChip;
            //_listTo.Items.Add();
            _searchBox.Text = default(string);
        }

        private void ChipItem_DeleteChip(object sender, RoutedEventArgs e)
        {
            lstChip.Remove(sender as WKChip);
            _listTo.Items.Remove(sender as WKChip);
        }



        private void _searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //List<IGroup> result = new List<IGroup>();
            //result.AddRange(ItemsSource.Where(x => x.GroupName.Contains(_searchBox.Text)));
            ////var res = ItemsSource.Except(result).Where(x=>x.Items.GetType();
            ////var res2 = res.ToList().ForEach(x=> x.)
            ////result.AddRange(ItemsSource.Where(x => !x.GroupName.Contains(_searchBox.Text)));
            //treeView.ItemsSource = result;
            //treeView.UpdateLayout();
            FilterString = _searchBox.Text;
            //TextValue = _searchBox.Text;
            //RaiseTextChangedEvent(); 
        }
    }
}