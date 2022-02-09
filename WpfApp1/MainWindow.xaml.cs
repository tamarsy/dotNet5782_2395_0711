using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationService.Navigate(new View.ViewEntry(NavigatePage));
        }

        private void NavigatePage(object o)
        {
            mainFrame.NavigationService.Navigate(o);
        }

        private void Close_Progrem(object sender, EventArgs e)
        {
            Close();
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}

        //private void is_num(object sender, TextChangedEventArgs e)
        //{
        //    if (e.Source is TextBox t)
        //    {
        //        if (t.CaretIndex != 0 && t.Text.Length > 0 && !char.IsDigit(t.Text[t.CaretIndex - 1]))
        //        {
        //            MessageBox.Show("ERROR enter only number for Id");
        //            int ii = e.Changes.ElementAt(0).AddedLength;
        //            t.Text = t.Text.Remove(t.CaretIndex - ii, ii);
        //        }
        //    }
        //}


