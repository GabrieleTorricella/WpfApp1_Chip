using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    ///     <MyNamespace:WKChip/>
    ///
    /// </summary>
    /// 
  
    [TemplatePart(Name = "_photoUser", Type = typeof(Ellipse))]
    [TemplatePart(Name = "BtnProfilePicture", Type = typeof(Button))]
    [TemplateVisualState(Name = "AddButtonState", GroupName = "CircleImageState")]
    [TemplateVisualState(Name = "VisualButtonStateWithImage", GroupName = "CircleImageState")]
    [TemplateVisualState(Name = "VisualButtonStateWithoutImage", GroupName = "CircleImageState")]
    public class WKChip : ContentControl
    {
        private bool? _isEditable = false;
        private TextBox _mailUserEditable;
        private ContentPresenter _infoUser = new ContentPresenter();
        public static readonly DependencyProperty IsEditableProperty;
        private Button delete = new Button();
        private Ellipse _photoUser;
        public static readonly RoutedEvent DeleteChipEvent = EventManager.RegisterRoutedEvent("DeleteChip", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WKChip));
        public static readonly RoutedEvent CreatedChipEvent = EventManager.RegisterRoutedEvent("CreatedChip", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WKChip));
        public static readonly DependencyProperty InfoUserChangedProperty;
        public static readonly RoutedEvent AddButtonClickChipEvent = EventManager.RegisterRoutedEvent("AddButtonClickChip", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WKChip));
        private VisualStateGroup _circleImageState;
        private Button BtnProfilePicture;

        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }



        public bool ShowAddButtonClick
        {
            get { return (bool)GetValue(ShowAddButtonClickProperty); }
            set { SetValue(ShowAddButtonClickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAddButtonClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAddButtonClickProperty =
            DependencyProperty.Register("ShowAddButtonClick", typeof(bool), typeof(WKChip), new PropertyMetadata(default(bool), new PropertyChangedCallback(WKChip.ShowAddButtonCallback)));

        private static void ShowAddButtonCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChip;
            if (item != null)
            {
                item.UpdateState(true);
            }
        }

        // Using a DependencyProperty as the backing store for ImageName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageNameProperty =
            DependencyProperty.Register("ImageName", typeof(string), typeof(WKChip), new PropertyMetadata(default(string),new PropertyChangedCallback(WKChip.ImageNameCallback)));

        private static void ImageNameCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChip;
            if (item != null)
            {
                item.UpdateState(true);
            }
        }

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(WKChip), new PropertyMetadata(default(string),new PropertyChangedCallback(WKChip.PropertyNameCallback)));

        private static void PropertyNameCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChip;
            if (item != null)
            {
                item.SetBinding();
               
              
            }
        }

        private void SetBinding()
        {
            Binding myBinding = new Binding(PropertyName);


            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this._mailUserEditable.SetBinding(TextBlock.TextProperty, myBinding);
        }

        private static void DataTemplateChipPropertyCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as WKChip;
            if (item!=null)
            {
                item.ContentTemplate = e.NewValue as DataTemplate;
            }
            
        }

        public bool? IsEditable
        {
            get
            {
                return (bool?)base.GetValue(WKChip.IsEditableProperty);
            }
            set
            {
                if (value == null)
                {
                    base.ClearValue(WKChip.IsEditableProperty);
                    return;
                }
                base.SetValue(WKChip.IsEditableProperty, value);
            }
        }

        static WKChip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WKChip), new FrameworkPropertyMetadata(typeof(WKChip)));

            WKChip.IsEditableProperty =
                DependencyProperty.Register(
                    "IsEditable",
                    typeof(bool?),
                    typeof(WKChip),
                    new FrameworkPropertyMetadata(null, new PropertyChangedCallback(WKChip.OnIsEditableChanged)));
            
        }     

        private static void OnIsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is WKChip)
            {
                var item = d as WKChip;
                var result = e.NewValue != null ? e.NewValue as bool? : false;
                if (item._mailUserEditable == null)
                    item.IsEditable = result;
                else
                    item.IsEditable = result;
            }                     
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mailUserEditable = (TextBox)GetTemplateChild("MailUserEdit");
            _infoUser = (ContentPresenter)GetTemplateChild("InformationUser");
            //_mailUserEditable.Visibility = (bool)!_isEditable ? Visibility.Hidden : Visibility.Visible;
            //_infoUser.Visibility = (bool)_isEditable ? Visibility.Collapsed : Visibility.Visible;
            delete = (Button)GetTemplateChild("Delete");
            //delete.Click += new RoutedEventHandler(delete_Click);

            delete.Click += Delete_Click;
            this.Loaded += Loaded_Event;
            //_mailUserEditable.SetBinding(TextBlock.TextProperty, new Binding("Mail"));
            //_photoUser = (Ellipse)GetTemplateChild("PhotoUser");
            BtnProfilePicture = (Button)GetTemplateChild("BtnProfilePicture");
            BtnProfilePicture.Click += AddButton_Click;
            UpdateState(false);
            _circleImageState = (VisualStateGroup)GetTemplateChild("CircleImageState");
            LoadImageSource();
        }



        private void LoadImageSource()
        {
            if (BtnProfilePicture!=null &&this.ShowAddButtonClick)
            {
                var image = new BitmapImage();
                try
                {
                    image.BeginInit();
                    
                    {
                        var bytes = (byte[])new ImageConverter().ConvertTo(Properties.Resources.blue_plus_icon_6, typeof(byte[]));
                        image.StreamSource = new MemoryStream(bytes);
                    }
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    image.Freeze();
                }
                catch (FileNotFoundException ex)
                {
                    throw ex;
                }

                BtnProfilePicture.Background = new ImageBrush { ImageSource = image };
            }
            else
            {

                if (!string.IsNullOrEmpty(ImageName) && this.Content.GetType().GetProperty(ImageName, typeof(byte[])).GetValue(this.Content) != null)
                {
                    var image = new BitmapImage();
                    try
                    {
                        image.BeginInit();

                        {
                          
                            image.StreamSource = new MemoryStream((byte [])this.Content.GetType().GetProperty(ImageName, typeof(byte[])).GetValue(this.Content));
                        }
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        image.Freeze();
                    }
                    catch (FileNotFoundException ex)
                    {
                        throw ex;
                    }

                    BtnProfilePicture.Background = new ImageBrush { ImageSource = image };
                }
                else
                {
                    BtnProfilePicture.Background = new SolidColorBrush {};
                }
            }           
        }    

        private void Loaded_Event(object sender, RoutedEventArgs e)
        {
            RaiseAddChipEvent();
        }

        public event RoutedEventHandler AddChip
        {
            add { AddHandler(DeleteChipEvent, value); }
            remove { RemoveHandler(DeleteChipEvent, value); }
        }
        void RaiseAddChipEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(WKChip.CreatedChipEvent);
            RaiseEvent(newEventArgs);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            RaiseDeleteChipEvent();
        }
        
        public event RoutedEventHandler DeleteChip
        {
            add { AddHandler(DeleteChipEvent, value); }
            remove { RemoveHandler(DeleteChipEvent, value); }
        }
        void RaiseDeleteChipEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(WKChip.DeleteChipEvent);
            RaiseEvent(newEventArgs);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_circleImageState.CurrentState.Name.Equals("AddButtonState"))
            {
                RaiseAddButtonClickChipEvent();
            }
          
        }

        public event RoutedEventHandler AddButtonClickChip
        {
            add { AddHandler(AddButtonClickChipEvent, value); }
            remove { RemoveHandler(AddButtonClickChipEvent, value); }
        }
        void RaiseAddButtonClickChipEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(WKChip.AddButtonClickChipEvent);
            RaiseEvent(newEventArgs);
        }

        private void UpdateState(bool useTransition)
        {
            if (ShowAddButtonClick)
            {
                VisualStateManager.GoToState(this, "AddButtonState", useTransition);
            }
            else
            {
                if (!string.IsNullOrEmpty(ImageName) && this.Content.GetType().GetProperty(ImageName,typeof(byte[])).GetValue(this.Content) != null)
                {
                    VisualStateManager.GoToState(this, "VisualButtonStateWithImage", useTransition);
                }
                else
                {
                    VisualStateManager.GoToState(this, "VisualButtonStateWithoutImage", useTransition);
                }
                
            }
            
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "FocusedElementState", true);
            Console.WriteLine("got focus " + ((UserCase)this.Content).Name);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "NotFocusedElementState", true);
    
        }
    }
}
