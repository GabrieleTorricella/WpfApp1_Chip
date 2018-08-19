using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class DataTempalteSelcetorItemsControlChipSelector : DataTemplateSelector
    {
        public DataTemplate ChipDataTemplate { get; set; }
        public DataTemplate AddChipDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
                   DependencyObject container)
        {
          if(item.GetType() == typeof(AddChipTemplate))
            {
                return AddChipDataTemplate;
            }

            return ChipDataTemplate;
        }
    }
    public class AddChipTemplate
    {

    }
}
