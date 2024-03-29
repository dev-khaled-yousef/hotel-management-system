using Hotel_DataAccess;
using System;
using System.Data;

namespace Hotel_Business
{
    public class clsRoomService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public short? RoomServiceID { get; set; }
        public string RoomServiceTitle { get; set; }
        public float Fees { get; set; }
        public string Description { get; set; }

        public clsRoomService()
        {
            this.RoomServiceID = null;
            this.RoomServiceTitle = string.Empty;
            this.Fees = -1f;
            this.Description = string.Empty;
            Mode = enMode.AddNew;
        }

        private clsRoomService(short? RoomServiceID, string RoomServiceTitle, 
            float Fees, string Description)
        {
            this.RoomServiceID = RoomServiceID;
            this.RoomServiceTitle = RoomServiceTitle;
            this.Fees = Fees;
            this.Description = Description;
            Mode = enMode.Update;
        }

        private bool _AddNewRoomService()
        {
            this.RoomServiceID = clsRoomServiceData.AddNewRoomService(this.RoomServiceTitle, this.Fees, this.Description);

            return (this.RoomServiceID.HasValue);
        }

        private bool _UpdateRoomService()
        {
            return clsRoomServiceData.UpdateRoomService(this.RoomServiceID, this.RoomServiceTitle, this.Fees, this.Description);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewRoomService())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateRoomService();
            }

            return false;
        }

        public static clsRoomService Find(short? RoomServiceID)
        {
            string RoomServiceTitle = string.Empty;
            float Fees = -1f;
            string Description = string.Empty;

            bool IsFound = clsRoomServiceData.GetRoomServiceInfoByID(RoomServiceID, ref RoomServiceTitle, ref Fees, ref Description);

            return (IsFound) ? (new clsRoomService(RoomServiceID, RoomServiceTitle, Fees, Description)) : null;
        }

        public static bool DeleteRoomService(short? RoomServiceID)
        {
            return clsRoomServiceData.DeleteRoomService(RoomServiceID);
        }

        public static bool DoesRoomServiceExist(short? RoomServiceID)
        {
            return clsRoomServiceData.DoesRoomServiceExist(RoomServiceID);
        }

        public static bool DoesRoomServiceExist(string RoomServiceTitle)
        {
            return clsRoomServiceData.DoesRoomServiceExist(RoomServiceTitle);
        }

        public static DataTable GetAllRoomServices()
        {
            return clsRoomServiceData.GetAllRoomServices();
        }

    }

}