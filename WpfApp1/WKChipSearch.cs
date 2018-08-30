using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApp1"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApp1;assembly=WpfApp1"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:WKChipSearch/>
    ///
    /// </summary>
    
    /// <summary>
    /// Chip selector
    /// </summary>
    [TemplatePart(Name = "pupListBox", Type = typeof(Popup))]
    [TemplatePart(Name = "_searchBox", Type = typeof(TextBox))]
    [TemplateVisualState(Name = "PopupOpenOnTextEdit", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "PopupOpenOnFocus", GroupName = "ValueStates")]
    public class WKChipSearch : Control, INotifyPropertyChanged
    {
        private TextBox _searchBox = new TextBox();
        private Popup _pupListBox;
        private bool _isFocusedSearchBox;
        private string _filterString = string.Empty;        
        private ICollectionView _collectionView;       
        private ListBox _listBox = new ListBox();
        private string _viewElement = default(string);

        public static readonly RoutedEvent SelectedChipEvent = EventManager.RegisterRoutedEvent("SelectedChip", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WKChipSearch));
        public event PropertyChangedEventHandler PropertyChanged;        

        static WKChipSearch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WKChipSearch), new FrameworkPropertyMetadata(typeof(WKChipSearch)));
        }
        public bool ShowPopupOnFocus
        {
            get { return (bool)GetValue(ShowPopupOnFocusProperty); }
            set { SetValue(ShowPopupOnFocusProperty, value); }
        }

        public string FilterString
        {
            get { return _filterString; }
            set { _filterString = value; NotifyPropertyChanged("FilterString"); _collectionView.Refresh(); }
        }      

        // Using a DependencyProperty as the backing store for ShowPopupOnFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPopupOnFocusProperty =
            DependencyProperty.Register("ShowPopupOnFocus", typeof(bool), typeof(WKChipSearch), new PropertyMetadata(default(bool)));

        public DataTemplate DataTemplateItemsListbox
        {
            get { return (DataTemplate)GetValue(DataTemplateItemsListboxProperty); }
            set { SetValue(DataTemplateItemsListboxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataTemplateItemsListbox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataTemplateItemsListboxProperty =
            DependencyProperty.Register("DataTemplateItemsListbox", typeof(DataTemplate), typeof(WKChipSearch), new PropertyMetadata(default(DataTemplate), WKChipSearch.DataTemplateItemsListboxCallback));

        private static void DataTemplateItemsListboxCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ChipsSelector = d as WKChipSearch;
            if (ChipsSelector != null)
            {
                var template = e.NewValue as DataTemplate;
                ChipsSelector.SetDataTemplate(template);
            }
        }

        public IEnumerable ItemsSource  
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSourceProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(WKChipSearch), new PropertyMetadata(null, new PropertyChangedCallback(WKChipSearch.OnItemsSourceChanged)));



        public IEnumerable<string> SearchProperties
        {
            get { return (IEnumerable<string>)GetValue(SearchPropertiesProperty); }
            set { SetValue(SearchPropertiesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchProperties.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchPropertiesProperty =
            DependencyProperty.Register("SearchProperties", typeof(IEnumerable<string>), typeof(WKChipSearch), new PropertyMetadata(null));
       

        public IEnumerable<string> GroupNames
        {
            get { return (IEnumerable<string>)GetValue(GroupNamesProperty); }
            set { SetValue(GroupNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNamesProperty =
            DependencyProperty.Register("GroupNames", typeof(IEnumerable<string>), typeof(WKChipSearch), new PropertyMetadata(null, WKChipSearch.GroupNamesCallBack));

        private static void GroupNamesCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as WKChipSearch;
            sender.SetGroupDescriptions();
        }

        private void SetGroupDescriptions()
        {
            if (GroupNames != null && GroupNames.Count() > 0)
            {
                GroupNames.ToList().ForEach(
                    x =>
                        _collectionView.GroupDescriptions.Add(new PropertyGroupDescription(x)));
            }
        }

        public string ElementiDaVisualizzare
        {
            get { return (string)GetValue(ElementiDaVisualizzareProperty); }
            set { SetValue(ElementiDaVisualizzareProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementiDaVisualizzare.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementiDaVisualizzareProperty =
            DependencyProperty.Register("ElementiDaVisualizzare", typeof(string), typeof(WKChipSearch), new PropertyMetadata(null, new PropertyChangedCallback(WKChipSearch.ElementiDaVisualizzareCallback)));


        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _wkChipSearch = d as WKChipSearch;
            if (_wkChipSearch != null)
            {
                var users = e.NewValue as IEnumerable;
                if (_wkChipSearch._listBox != null)
                {
                    _wkChipSearch.SetItemSource(users);
                }
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _listBox = (ListBox)GetTemplateChild("ListItems");           
            if(ItemsSource != null)
            {
                _collectionView = CollectionViewSource.GetDefaultView(ItemsSource);
                _collectionView.Filter = CustomerFilter;
            }
            _pupListBox = (Popup)GetTemplateChild("pupListBox");
            _listBox.ItemsSource = _collectionView;
            _listBox.DisplayMemberPath = _viewElement;
            //_listBox.SelectionChanged += ListBox_SelectionChanged;
            _listBox.GotFocus += ListBox_GotFocus;
            _listBox.LostFocus += ListBox_LostFocus;
            _listBox.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;


            _searchBox = (TextBox)GetTemplateChild("SearchBox");
            _searchBox.TextChanged += SearchBox_TextChanged;
            _searchBox.GotFocus += SearchBox_GotFocus;
            _searchBox.LostFocus += SearchBox_LostFocus;
            _pupListBox.Loaded += PupListBox_Loaded;
            _pupListBox.KeyUp += PupListBox_KeyUp;

            EventManager.RegisterClassHandler(typeof(ListBoxItem), ListBoxItem.MouseLeftButtonDownEvent, new RoutedEventHandler(this.MouseLeftButtonDownClassHandler));

            if (GroupNames != null && GroupNames.Count() > 0)
            {
                GroupNames.ToList().ForEach(
                    x =>
                        _collectionView.GroupDescriptions.Add(new PropertyGroupDescription(x)));
            }

            UpdateStates(false);
        }

        private void MouseLeftButtonDownClassHandler(object sender, RoutedEventArgs e)
        {
            if (_listBox.SelectedIndex != -1)
            {
                RaiseSelectedChipEvent(_listBox.SelectedItem, false);
                _searchBox.Text = null;
            }
        }    

        private void ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("listbox lost focus");
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (_listBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                _listBox.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, new Action(DelayedAction));
            }
        }

        void DelayedAction()
        {
            var i = _listBox.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
            if(i != null)
            {
                Console.WriteLine("attempt to give focus to listitem");
                i.Focus();
            }            
        }

        private void ListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("listbox got focus");
        }

        private void PupListBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    RaiseSelectedChipEvent(_listBox.SelectedItem, false);

                    break;
                case Key.Up:
                    break;
                case Key.Down:
                    break;
                case Key.Escape:
                    break;

            }
        }

        private void PupListBox_Loaded(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(_searchBox);
            if (null != w)
            {
                w.LocationChanged += delegate (object senderw, EventArgs argsw)
                {
                    var offset = _pupListBox.HorizontalOffset;
                    _pupListBox.HorizontalOffset = offset + 1;
                    _pupListBox.HorizontalOffset = offset;
                };
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("udate state from searchbox lostfocus");
            //UpdateStates(true);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //UpdateStates(true);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_searchBox.Text) && _searchBox.Text.Last().Equals(';'))
            {                
                Type t = ItemsSource.GetType().GetGenericArguments()[0];
                var el = Activator.CreateInstance(t);
                el.GetType().GetProperty(ElementiDaVisualizzare).SetValue(el, _searchBox.Text.Substring(0, _searchBox.Text.Count() - 1));
                RaiseSelectedChipEvent(el, true);
                _searchBox.Text = string.Empty;
            }
            FilterString = _searchBox.Text;
            Console.WriteLine("udate state from searchbox text changed");
            UpdateStates(true);
        }

        //private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (_listBox.SelectedIndex != -1)
        //    {
        //        RaiseSelectedChipEvent(_listBox.SelectedItem, false);                
        //        _searchBox.Text = null;
        //    }
        //}

        /// <summary>
        /// Metodo che serve per inserire i valori nella proprietà ItemsSource del TreeView
        /// </summary>
        /// <param name="e">Lista da inserire</param>
        private void SetItemSource(IEnumerable e)
        {
            this._listBox.ItemsSource = e;
            if (this.ItemsSource != null)
            {
                this._collectionView = CollectionViewSource.GetDefaultView(this.ItemsSource);
                this._collectionView.Filter = CustomerFilter;
            }
        }

        private void SetDataTemplate(DataTemplate e)
        {
            this._listBox.ItemTemplate = e;          
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

        /// <summary>
        /// Call back di ElementiDaVisualizzareProperty. Gestisce la dependency property
        /// </summary>
        /// <param name="d">Fa riferimento alla classe "Padre"</param>
        /// <param name="e">Fa riferimento ai cambiamenti</param>
        private static void ElementiDaVisualizzareCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChipSearch;
            if (item != null)
            {
                var viewElements = e.NewValue as string;                        
                item._viewElement = viewElements;                
            }
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

        public event RoutedEventHandler SelectedChip
        {
            add { AddHandler(SelectedChipEvent, value); }
            remove { RemoveHandler(SelectedChipEvent, value); }
        }
        void RaiseSelectedChipEvent(object selectedItem, bool isEditableItem)
        {
            CustomEventArgs newCustomEventArgs = new CustomEventArgs(WKChipSearch.SelectedChipEvent, selectedItem, isEditableItem);                        
            RaiseEvent(newCustomEventArgs);
        }
    }     

    public class CustomEventArgs : RoutedEventArgs
    {         
        public bool IsEditableItem
        {
            get; private set;
        }

        public CustomEventArgs(RoutedEvent routedEvent, object source, bool isEditableItem)
        {
            base.RoutedEvent = routedEvent;
            base.Source = source;
            this.IsEditableItem = isEditableItem;
        }
    } 
}
