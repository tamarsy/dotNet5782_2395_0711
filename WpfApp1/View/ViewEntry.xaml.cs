using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.View
{
    /// <summary>
    /// Interaction logic for ViewEntry.xaml
    /// </summary>
    public partial class ViewEntry : Page
    {
        ViewModel.ViewEntryModel _viewEntryModel;
        public ViewEntry(Action<object> changePage)
        {
            _viewEntryModel = new ViewModel.ViewEntryModel(changePage, ()=> changePage(this));
            DataContext = _viewEntryModel;
            InitializeComponent();
        }
    }
}
