using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Chip selector
    /// </summary>
    ///
    [TemplatePart(Name = "pupListBox", Type = typeof(Popup))]
    [TemplatePart(Name = "_searchBox", Type = typeof(TextBox))]
    [TemplateVisualState(Name = "PopupOpenOnTextEdit", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "PopupOpenOnFocus", GroupName = "ValueStates")]
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
        public static readonly DependencyProperty ChipEditorTemplateProperty;
        private string _viewElement = default(string);
        public string TextValue { get; private set; }
        private ICollectionView collectionView;
        private DataTemplate data = new DataTemplate();
        private string _filterString = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
        private ListBox _listTo = new ListBox();
        private List<WKChip> lstChip;
        private Popup pupListBox;
        private bool isFocusedSearchBox;
        private IList SelectedItemsEditable;




        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageNameProperty =
            DependencyProperty.Register("ImageNameChip", typeof(string), typeof(WKChipsManager), new PropertyMetadata(default(string)));



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
        public IEnumerable<string> GroupNameProp
        {
            get => (IEnumerable<string>)base.GetValue(WKChipsManager.GroupNameProperty);
            set { if (value == null) { base.ClearValue(WKChipsManager.GroupNameProperty); return; } base.SetValue(WKChipsManager.GroupNameProperty, value); }
        }



        public bool ShowPopupOnFocus
        {
            get { return (bool)GetValue(ShowPopupOnFocusProperty); }
            set { SetValue(ShowPopupOnFocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowPopupOnFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPopupOnFocusProperty =
            DependencyProperty.Register("ShowPopupOnFocus", typeof(bool), typeof(WKChipsManager), new PropertyMetadata(default(bool)));



        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(WKChipsManager), new PropertyMetadata());


        public WKChipsManager()
        {
            SelectedItems = new ObservableCollection<object>() { new AddChipTemplate()};
            SelectedItemsEditable = new ObservableCollection<object>();
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
                   "GroupNames",
                   typeof(IEnumerable<string>),
                   typeof(WKChipsManager),
                   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.GroupNameCallBack)));
            WKChipsManager.ChipEditorTemplateProperty =
              DependencyProperty.Register(
                  "ChipEditorTemplate",
                  typeof(DataTemplate),
                  typeof(WKChipsManager),
                  new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.ChipEdiotrTemplateCallBack)));

        }

        private static void ChipEdiotrTemplateCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as WKChipsManager;
            sender.SetChipEditorTemplate();
        }

        private void SetChipEditorTemplate()
        {
            if (this._chipsItem != null)
                this._chipsItem.ItemTemplate = ChipEditorTemplate;
        }

        public DataTemplate ChipEditorTemplate
        {
            get { return (DataTemplate)GetValue(WKChipsManager.ChipEditorTemplateProperty); }
            set { SetValue(ChipEditorTemplateProperty, value); }
        }

        //// Using a DependencyProperty as the backing store for ChipEditorTemplate.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ChipEditorTemplateProperty =
        //    DependencyProperty.Register("ChipEditorTemplate", typeof(DataTemplate), typeof(WKChipsManager), new PropertyMetadata(default(DataTemplate)));



        public DataTemplate DataTemplateItemsListbox
        {
            get { return (DataTemplate)GetValue(DataTemplateItemsListboxProperty); }
            set { SetValue(DataTemplateItemsListboxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataTemplateItemsListbox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataTemplateItemsListboxProperty =
            DependencyProperty.Register("DataTemplateItemsListbox", typeof(DataTemplate), typeof(WKChipsManager), new PropertyMetadata(default(DataTemplate)));



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
        private void DeleteChip_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("It is the custom routed event of your custom control");
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
            var _wkChips2 = d as WKChipsManager;
            if (_wkChips2 != null)
            {
                var users = e.NewValue as IEnumerable;
                if (_wkChips2.listBox != null)
                {
               
                    _wkChips2.SetItemSource(users);
                    
                }
                   
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
            //_chipsItem = (ItemsControl)GetTemplateChild("ChipsItems");
            listBox = (ListBox)GetTemplateChild("ListItems");
            _listTo = (ListBox)GetTemplateChild("ListTo");
            collectionView = CollectionViewSource.GetDefaultView(ItemsSource);
            collectionView.Filter = CustomerFilter;
            if (GroupNameProp != null && GroupNameProp.Count() > 0)
            {
                GroupNameProp.ToList().ForEach(
                    x =>
                        collectionView.GroupDescriptions.Add(new PropertyGroupDescription(x)));
            }
            pupListBox = (Popup)GetTemplateChild("pupListBox");
            listBox.ItemsSource = collectionView;
            listBox.DisplayMemberPath = _viewElement;
            listBox.SelectionChanged += ListBoxItems_SelectionChanged;
            _searchBox = (TextBox)GetTemplateChild("SearchBox");
            _searchBox.TextChanged += _searchBox_TextChanged;
            _searchBox.GotFocus += _searchBox_GotFocus;
            _searchBox.LostFocus += _searchBox_LostFocus;
            pupListBox.Loaded += PupListBox_Loaded;
            pupListBox.KeyUp += KeyboardManagement;
            // _searchBox.Focus();
            //this._chipsItem = (ItemsControl)GetTemplateChild("ChipsItems");
            //    //var el = this._chipsItem.ItemTemplate;
            //this._chipsItem.ItemTemplate = ChipEditorTemplate;
            EventManager.RegisterClassHandler(typeof(WKChip),
                                                WKChip.DeleteChipEvent,
                                                new RoutedEventHandler(ChipItem_DeleteChip), true);
            EventManager.RegisterClassHandler(typeof(WKChip),
                                               WKChip.CreatedChipEvent,
                                               new RoutedEventHandler(ChipItem_AddChip), true);
            UpdateStates(false);

        }

        private void KeyboardManagement(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    SelectedItems.Add(listBox.SelectedItem);

                    break;
                case Key.Up:
                    break;
                case Key.Down:
                    break;
                case Key.Escape:
                    break;

            }
        }

        private void ChipItem_AddChip(object sender, RoutedEventArgs e)
        {
            var item = sender as WKChip;
            if (SelectedItemsEditable.Contains(item.Content))
            {
                item.IsEditable = true;
                item.ShowAddButtonClick = true;
            }
            else
            {
                item.IsEditable = false;
            }


        }

        private void _searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateStates(true);
        }

        private void _searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateStates(true);
        }

        private void PupListBox_Loaded(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(_searchBox);
            if (null != w)
            {
                w.LocationChanged += delegate (object senderw, EventArgs argsw)
                {
                    var offset = pupListBox.HorizontalOffset;
                    pupListBox.HorizontalOffset = offset + 1;
                    pupListBox.HorizontalOffset = offset;
                };
            }
        }

        private void ListBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                SelectedItems.Insert(SelectedItems.Count!=0?SelectedItems.Count-1:0,listBox.SelectedItem);
                _searchBox.Text = null;
            }








            //var chipItem = new WKChip();
            ////TODO CHANGE FOR MANAGE GENERIC TYPES
            //var user = listBox.SelectedItem as User;
            //if (user != null)
            //{
            //    chipItem.InfoUser = user.Name;
            //    _listTo.Items.Add(chipItem);
            //    chipItem.DeleteChip += ChipItem_DeleteChip;
            //    //_listTo.Items.Add();
            //    _searchBox.Text = default(string);
            //}
            //e.Handled = true;
            ////listBox.SelectedIndex = -1;
            //Console.WriteLine("Selected index changed");
        }

        private void ChipItem_DeleteChip(object sender, RoutedEventArgs e)
        {
            //lstChip.Remove(sender as WKChip);
            //_listTo.Items.Remove(sender as WKChip);
            // SelectedItems.Remove();
            var el = sender as WKChip;
            SelectedItems.Remove(el.Content);
        }

        private void UpdateStates(bool useTransitions)
        {
            if (_searchBox.IsFocused)
            {
                if (ShowPopupOnFocus || !(string.IsNullOrEmpty(_searchBox.Text)))
                {
                    VisualStateManager.GoToState(this, "PopupOpenOnTextEdit", useTransitions);
                    Console.WriteLine("show");
                }
                else
                {
                    VisualStateManager.GoToState(this, "PopupOpenOnFocus", useTransitions);
                    Console.WriteLine("hide");
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "PopupOpenOnFocus", useTransitions);
                Console.WriteLine("hide");
            }

        }


        private void _searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_searchBox.Text) && _searchBox.Text.Last().Equals(';'))
            {
                Type t = ItemsSource.GetType().GetGenericArguments()[0];
                var el = Activator.CreateInstance(t);
                el.GetType().GetProperty(ElementiDaVisualizzare).SetValue(el, _searchBox.Text.Substring(0, _searchBox.Text.Count() - 1));
                SelectedItemsEditable.Add(el);
                SelectedItems.Insert(SelectedItems.Count != 0 ? SelectedItems.Count - 1 : 0, el);
                _searchBox.Text = string.Empty;
            }
            FilterString = _searchBox.Text;
            UpdateStates(true);
        }
    }
    public class MultiBooleanToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values,
                                Type targetType,
                                object parameter,
                                System.Globalization.CultureInfo culture)
        {
            bool visible = true;
            foreach (object value in values)
                if (value is bool)
                    visible = visible && (bool)value;
            return visible;
        }

        public object[] ConvertBack(object value,
                                    Type[] targetTypes,
                                    object parameter,
                                    System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}