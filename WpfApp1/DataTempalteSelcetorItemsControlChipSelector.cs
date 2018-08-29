using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApp1
{
    public class DataTempalteSelcetorItemsControlChipSelector : DataTemplateSelector
    {
        public DataTemplate ChipDataTemplate { get; set; }
        public DataTemplate AddChipDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
                   DependencyObject container)
        {
            if (item.GetType() == typeof(AddChipTemplate))
            {
                return AddChipDataTemplate;
            }

            return ChipDataTemplate;
        }
    }
    public class AddChipTemplate
    {

    }

    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupNameTemplate
        { get; set; }

        public DataTemplate GroupCategoryTemplate
        { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ContentPresenter cp = container as ContentPresenter;

            if (cp != null)
            {
                CollectionViewGroup cvg = cp.Content as CollectionViewGroup;

                if (cvg.Items.Count > 0)
                {
                    UserCase stinfo = cvg.Items[0] as UserCase;

                    if (stinfo != null)
                        return GroupCategoryTemplate;
                    else
                        return GroupNameTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
