using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        object selectedView;
        private ViewListDrone droneListView;
        public MainWindowViewModel()
        {
            DroneListView = new ViewListDrone();
        }
        public object SelectedView
        {
            get => selectedView;
            set
            {
                selectedView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedView)));
            }
        }
        public ViewListDrone DroneListView
        {
            get => droneListView;
            set
            {
                droneListView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DroneListView)));
            }
        }
        public List<object> Tabs { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
