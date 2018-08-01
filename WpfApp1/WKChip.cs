﻿using System;
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
    public class WKChip : Control
    {
        private bool? _isEditable = false;
        private TextBox _mailUserEditable = new TextBox();
        private TextBlock _mailUser = new TextBlock();
        public static readonly DependencyProperty IsEditableProperty;

        public bool? IsEditable
        {
            get => (bool?)base.GetValue(WKChip.IsEditableProperty);
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
            if (d is WKChip item)
            {
                var result = e.NewValue != null ? e.NewValue as bool? : false;
                if (item._mailUserEditable == null)
                    item._isEditable = result;
                else
                    item._isEditable = result;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mailUserEditable = (TextBox)GetTemplateChild("MailUserEditable");
            _mailUser = (TextBlock)GetTemplateChild("MailUser");
            _mailUserEditable.Visibility = (bool)!_isEditable ? Visibility.Hidden : Visibility.Visible;
            _mailUser.Visibility = (bool)_isEditable ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
