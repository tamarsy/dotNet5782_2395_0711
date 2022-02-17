using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PL.Model
{
    class StationModel
    {
        public Action Addcommand
        {
            get
            {
                return () =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().AddStation(Id, Name, new BO.Location(Latitude, Longitude), NumCargeSlot);
                        MessageBox.Show("Successfully add station");
                    }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("failed add station: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                };
            }
        }
        public Action UpDateComand
        {
            get
            {
                return () =>
                {
                    try
                    {
                        BLApi.FactoryBL.GetBL().UpdateStation(Id, Name, NumCargeSlot);
                        MessageBox.Show("Successfully update station details");
                    }
                    catch (BO.ObjectAlreadyExistException e) { MessageBox.Show("failed update station: " + e.Message); }
                    catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                };
            }
        }
        public Action Delete
        {
            get => () =>
                 {
                     try
                     {
                         BLApi.FactoryBL.GetBL().DeleteStation(Id);
                         MessageBox.Show("Successfully delete");

                     }
                     catch (BO.ObjectNotExistException e) { MessageBox.Show("can't update station details:" + e.Message); }
                     catch (Exception e) { MessageBox.Show("ERROR" + e.Message); }
                 };
        }
        public ListCollectionView DronesInCharge;
        public bool IsDetailsPanelVisibility { set; get; }
        public string Details { get; set; }
        public double Latitude { get; internal set; }
        public double Longitude { get; internal set; }
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int NumCargeSlot { get; internal set; }
    }
}
