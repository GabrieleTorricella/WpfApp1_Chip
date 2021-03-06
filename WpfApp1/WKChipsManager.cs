﻿using System;
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
using System.Windows.Media;

namespace WpfApp1
{


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

        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ElementiDaVisualizzareProperty;
        public static readonly DependencyProperty GroupNameProperty;
        public static readonly DependencyProperty ChipEditorTemplateProperty;



        private DataTemplate data = new DataTemplate();

        public event PropertyChangedEventHandler PropertyChanged;

        private List<WKChip> lstChip;
        private IList SelectedItemsEditable;

        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageNameProperty =
            DependencyProperty.Register("ImageNameChip", typeof(string), typeof(WKChipsManager), new PropertyMetadata(default(string)));


        public string ElementiDaVisualizzare
        {
            get { return (string)base.GetValue(WKChipsManager.ElementiDaVisualizzareProperty); }
            set { if (value == null) { base.ClearValue(WKChipsManager.ElementiDaVisualizzareProperty); return; } base.SetValue(WKChipsManager.ElementiDaVisualizzareProperty, value); }
        }

        public IEnumerable<string> GroupNames
        {
            get { return (IEnumerable<string>)base.GetValue(WKChipsManager.GroupNameProperty); }
            set { if (value == null) { base.ClearValue(WKChipsManager.GroupNameProperty); return; } base.SetValue(WKChipsManager.GroupNameProperty, value); }
        }

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(WKChipsManager), new PropertyMetadata(null, new PropertyChangedCallback(WKChipsManager.TestCallBack)));

        private static void TestCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public WKChipsManager()
        {
            SelectedItems = new ObservableCollection<object>() { new AddChipTemplate() };
            SelectedItemsEditable = new ObservableCollection<object>();
            Focusable = false;
        }
        static WKChipsManager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WKChipsManager), new FrameworkPropertyMetadata(typeof(WKChipsManager)));

            WKChipsManager.ItemsSourceProperty =
                DependencyProperty.Register("ItemsSource",
                typeof(IEnumerable),
                typeof(WKChipsManager),
                new FrameworkPropertyMetadata(null));

            WKChipsManager.ElementiDaVisualizzareProperty =
                DependencyProperty.Register(
                    "ElementiDaVisualizzare",
                    typeof(string),
                    typeof(WKChipsManager),
                    new FrameworkPropertyMetadata(null));

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

        public DataTemplate DataTemplateItemsListbox
        {
            get { return (DataTemplate)GetValue(DataTemplateItemsListboxProperty); }
            set { SetValue(DataTemplateItemsListboxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataTemplateItemsListbox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataTemplateItemsListboxProperty =
            DependencyProperty.Register("DataTemplateItemsListbox", typeof(DataTemplate), typeof(WKChipsManager), new PropertyMetadata(default(DataTemplate)));


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
            get { return (IEnumerable)base.GetValue(WKChipsManager.ItemsSourceProperty); }
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

        public IEnumerable<string> SearchProperties
        {
            get { return (IEnumerable<string>)GetValue(SearchPropertiesProperty); }
            set { SetValue(SearchPropertiesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchProperties.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchPropertiesProperty =
            DependencyProperty.Register("SearchProperties", typeof(IEnumerable<string>), typeof(WKChipsManager), new PropertyMetadata(null));

        public bool ShowPopupOnFocus
        {
            get { return (bool)GetValue(ShowPopupOnFocusProperty); }
            set { SetValue(ShowPopupOnFocusProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ShowPopupOnFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPopupOnFocusProperty =
            DependencyProperty.Register("ShowPopupOnFocus", typeof(bool), typeof(WKChipsManager), new PropertyMetadata(default(bool)));


        private void ItemSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _chipsItem = this.GetTemplateChild("ChipsItems") as ItemsControl;
            EventManager.RegisterClassHandler(typeof(WKChip),
                                                WKChip.DeleteChipEvent,
                                                new RoutedEventHandler(ChipItem_DeleteChip), true);
            EventManager.RegisterClassHandler(typeof(WKChip),
                                               WKChip.CreatedChipEvent,
                                               new RoutedEventHandler(ChipItem_AddChip), true);
            EventManager.RegisterClassHandler(typeof(WKChipSearch),
                                              WKChipSearch.SelectedChipEvent,
                                              new RoutedEventHandler(ChipItem_SelectedChip), true);

            EventManager.RegisterClassHandler(typeof(WKChip),
                                            WKChip.AddButtonClickChipEvent,
                                            new RoutedEventHandler(ChipItem_AddChip_NewWindow), true);

            EventManager.RegisterClassHandler(typeof(WKChip),
                                            WKChip.MouseLeftButtonDownEvent,
                                            new RoutedEventHandler(ChipItem_MouseLeftButtonDown),
                                            true
                                            );

            EventManager.RegisterClassHandler(typeof(WKChipsManager),
                                            WKChip.KeyDownEvent,
                                            new KeyEventHandler(ChipItem_KeyDown),
                                            true
                                            );
        }

        private void ChipItem_AddChip_NewWindow(object sender, RoutedEventArgs e)
        {
            Window w = new Window();
            w.Width = 300;
            w.Height = 100;

            w.Content = new TextBlock() { Text = "Nuovo utente" };
            w.Show();
        }

        private void ChipItem_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            WKChip chip = sender as WKChip;                      
            chip.Focus();
            Console.WriteLine("Focused element: " + ((WpfApp1.User)((System.Windows.Controls.ContentControl)(FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)))).Content).Name.ToString());
        }

        private void ChipItem_KeyDown(object sender, KeyEventArgs e)
        {
            var element = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(Application.Current.MainWindow));
            var fe = (element as FrameworkElement).DataContext as WpfApp1.AddChipTemplate;
            bool isAddChipTemplate = fe != null;            
            var focusedChip = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(Application.Current.MainWindow)) as WKChip;

            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    break;
                case Key.Up:
                    break;
                case Key.Down:
                    break;
                case Key.Escape:
                    break;
                case Key.Left:                                              
                    SetNextFocusedChip(sender, Key.Left);                    
                    break;
                case Key.Right:                  
                    break;
                case Key.Delete:                    
                    if(focusedChip != null && focusedChip is WKChip)
                    {
                        SelectedItems.Remove(focusedChip.Content);
                    }                    
                    break;
                case Key.Cancel:
                    if (focusedChip != null && focusedChip is WKChip)
                    {
                        SelectedItems.Remove(focusedChip.Content);
                    }
                    break;
                case Key.Back:
                    if (focusedChip != null && focusedChip is WKChip)
                    {
                        SelectedItems.Remove(focusedChip.Content);
                    }
                    break;
            }
            Console.WriteLine("Focused element: " + FocusManager.GetFocusedElement(FocusManager.GetFocusScope(Application.Current.MainWindow)).ToString());
        }
        
        private void SetNextFocusedChip(object senderObject, Key pressedKey)
        {
            var element = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(Application.Current.MainWindow));
            var fe = (element as FrameworkElement).DataContext as WpfApp1.AddChipTemplate;
            bool isAddChipTemplate = fe != null;
            IList<UserCase> chipsList = _chipsItem.Items.Cast<UserCase>().Take(this.SelectedItems.Count - 1).ToList();
            ContentPresenter cp = null;

            if (isAddChipTemplate && pressedKey == Key.Left)
            {
                TextBox tb = element as TextBox; 
                if(string.IsNullOrEmpty(tb.Text))
                {
                    cp = ((_chipsItem.ItemContainerGenerator.ContainerFromIndex(_chipsItem.Items.Count - 2))) as ContentPresenter;
                }                
            }          

            if (cp != null)
            {
                WKChip nextFocusedChip = (VisualTreeHelper.GetChild(cp, 0)) as WKChip;
                if (nextFocusedChip != null)
                {
                    nextFocusedChip.Focus();
                }
            }
        }       

        private void ChipItem_SelectedChip(object sender, RoutedEventArgs e)
        {
            var item = sender as WKChipSearch;
                CustomEventArgs customEventArgs = e as CustomEventArgs;

            if (customEventArgs.IsEditableItem)
            {
                SelectedItemsEditable.Add(e.OriginalSource);
            }
            SelectedItems.Insert(SelectedItems.Count != 0 ? SelectedItems.Count - 1 : 0, e.OriginalSource);                  
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

        private void ChipItem_DeleteChip(object sender, RoutedEventArgs e)
        {         
            var el = sender as WKChip;
            SelectedItems.Remove(el.Content);
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