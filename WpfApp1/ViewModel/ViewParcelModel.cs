using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PL.ViewModel
{
    partial class ViewParcelModel : ViewModelBase
    {
        private Model.ParcelModel parcelModel;

        public string Details { get { return parcelModel.Details; } set { parcelModel.Details = value; } }

        public ICommand CloseCd
        {
            get
            {
                return new DelegateCommand((o) =>
                {
                    Close();
                });
            }
        }

        private void InitializeData(int parcelId)
        {
            parcelModel.Details = BLApi.FactoryBL.GetBL().GetParcel(parcelId).ToString();
            if (BLApi.FactoryBL.GetBL().GetParcel(parcelId).DroneDelivery == default)
                parcelModel.DD_ButtonVisibility = Visibility.Hidden;
        }


        public ViewParcelModel(int parcelId, Action upDateParcelsWindow, Action close)
        {
            Close = close;
            parcelModel = new Model.ParcelModel();
            parcelModel.UpDatePWindow = upDateParcelsWindow;
            parcelModel.ParcelId = parcelId;
            parcelModel.DetailsPanelVisibility = true;
            InitializeData(parcelId);
        }

    }
}
