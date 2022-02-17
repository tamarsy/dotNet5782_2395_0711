using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Menue.xaml
    /// </summary>
    public partial class Menue : Page
    {
        ViewModel.ViewMenueModel _menu;
        public Menue(Action close)
        {
            _menu = new ViewModel.ViewMenueModelCompany(AddTab, RemoveTab, close);
            DataContext = _menu;
            InitializeComponent();
        }

        public Menue(int Id, Action close)
        {
            _menu = new ViewModel.ViewMenueModelCustomer(AddTab, RemoveTab, Id, close);
            DataContext = _menu;
            InitializeComponent();
        }


        private void AddTab(object newTab)
        {
            if (newTab is TabItem tabItem)
            {
                int i = FindIndexOf((string)tabItem.Header);
                if (i < 0)
                {
                    i = MainTabs.Items.Add(tabItem);
                }
                _menu.SelectedTab = i;
            }
        }

        private int FindIndexOf(string header)
        {
            int i = 0;
            foreach (TabItem item in MainTabs.Items)
            {
                if (item.Header.Equals(header))
                    return i;
                ++i;
            }
            return -1;
        }


        private void RemoveTab(object o)
        {
            if (o is string header)
            {
                int i = FindIndexOf(header);
                if (i >= 0 && i < MainTabs.Items.Count)
                    MainTabs.Items.RemoveAt(i);
            }
        }
    }
}
