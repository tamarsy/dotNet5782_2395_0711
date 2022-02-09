using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    class ViewEntryModel:ViewModelBase
    {
        public int? Id { set; get; } = null;
        string phone;
        public string Phone { set { phone = OnlyNumStr(value); } get { return phone; } }
        public double? Latitude { get; set; } = null;
        public double? Longitude { get; set; } = null;
        public ICommand InCd
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    try
                    {
                        Customer customer = BLApi.FactoryBL.GetBL().GetCustomer((int)Id);
                        if (o is string name)
                            if (customer.Name.Equals(name))
                                ChangePage(new View.Menue((int)Id, Close));
                    }
                    catch (ObjectNotExistException e) { MessageBox.Show("Wrung id or name: " + e.Message); }
                    catch (Exception) { MessageBox.Show("Error"); }
                });
            }
        }

        public ICommand Comp
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    ChangePage(new View.Menue(Close));
                });
            }
        }

        public ICommand UpCd
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    try
                    {
                        if (o is string name)
                        {
                            BLApi.FactoryBL.GetBL().AddCustomer(
                                id: (int)Id,
                                name: name,
                                phone: Phone.ToString(),
                                location: new Location((double)Latitude, (double)Longitude)
                            );
                        }
                        MessageBox.Show("successfully Add your dedails\nWelcome!");
                        ChangePage(new View.Menue((int)Id, Close));
                    }
                    catch (ObjectAlreadyExistException e) { MessageBox.Show($"Id: {Id} already exist: \n" + e.Message); }
                    catch (Exception) { MessageBox.Show("Error"); }
                });
            }
        }

        public Action<object> ChangePage { get; }

        public ViewEntryModel(Action<object> changePage, Action close)
        {
            ChangePage = changePage;
            Close = close;
        }
    }
}
