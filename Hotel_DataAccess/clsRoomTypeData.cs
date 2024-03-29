using DataAccessToolkit;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Hotel_DataAccess
{
    public class clsRoomTypeData
    {
        public static bool GetRoomTypeInfoByID(byte? RoomTypeID, ref string RoomTypeTitle,
            ref byte Capacity, ref float PricePerNight, ref string Description)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetRoomTypeInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomTypeID", (object)RoomTypeID ?? DBNull.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                IsFound = true;

                                RoomTypeTitle = (string)reader["RoomTypeTitle"];
                                Capacity = (byte)reader["Capacity"];
                                PricePerNight = Convert.ToSingle(reader["PricePerNight"]);
                                Description = (reader["Description"] != DBNull.Value) ? (string)reader["Description"] : null;
                            }
                            else
                            {
                                // The record was not found
                                IsFound = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;

                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                IsFound = false;

                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("General Exception", ex);
            }

            return IsFound;
        }

        public static bool GetRoomTypeInfoByTitle(string RoomTypeTitle, ref byte? RoomTypeID,
            ref byte Capacity, ref float PricePerNight, ref string Description)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetRoomTypeInfoByTitle", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomTypeTitle", RoomTypeTitle);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                IsFound = true;

                                RoomTypeID = (reader["RoomTypeID"] != DBNull.Value) ? (byte?)Convert.ToByte(reader["RoomTypeID"]) : null;
                                Capacity = (byte)reader["Capacity"];
                                PricePerNight = Convert.ToSingle(reader["PricePerNight"]);
                                Description = (reader["Description"] != DBNull.Value) ? (string)reader["Description"] : null;
                            }
                            else
                            {
                                // The record was not found
                                IsFound = false;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;

                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                IsFound = false;

                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("General Exception", ex);
            }

            return IsFound;
        }

        public static byte? AddNewRoomType(string RoomTypeTitle, byte Capacity, float PricePerNight, string Description)
        {
            // This function will return the new person id if succeeded and null if not
            byte? RoomTypeID = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_AddNewRoomType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomTypeTitle", RoomTypeTitle);
                        command.Parameters.AddWithValue("@Capacity", Capacity);
                        command.Parameters.AddWithValue("@PricePerNight", PricePerNight);
                        command.Parameters.AddWithValue("@Description", (object)Description ?? DBNull.Value);

                        SqlParameter outputIdParam = new SqlParameter("@NewRoomTypeID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        RoomTypeID = Convert.ToByte(outputIdParam.Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("General Exception", ex);
            }

            return RoomTypeID;
        }

        public static bool UpdateRoomType(byte? RoomTypeID, string RoomTypeTitle, byte Capacity, float PricePerNight, string Description)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_UpdateRoomType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomTypeID", (object)RoomTypeID ?? DBNull.Value);
                        command.Parameters.AddWithValue("@RoomTypeTitle", RoomTypeTitle);
                        command.Parameters.AddWithValue("@Capacity", Capacity);
                        command.Parameters.AddWithValue("@PricePerNight", PricePerNight);
                        command.Parameters.AddWithValue("@Description", (object)Description ?? DBNull.Value);

                        RowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("General Exception", ex);
            }

            return (RowAffected > 0);
        }

        public static bool DeleteRoomType(byte? RoomTypeID)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_DeleteRoomType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomTypeID", (object)RoomTypeID ?? DBNull.Value);

                        RowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("General Exception", ex);
            }

            return (RowAffected > 0);
        }

        public static bool DoesRoomTypeExist(byte? RoomTypeID)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_DoesRoomTypeExist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RoomTypeID", (object)RoomTypeID ?? DBNull.Value);

                        // @ReturnVal could be any name, and we don't need to add it to the SP, just use it here in the code.
                        SqlParameter returnParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParameter);

                        command.ExecuteNonQuery();

                        IsFound = (int)returnParameter.Value == 1;
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;

                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                IsFound = false;

                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
                loggerToEventViewer.LogError("General Exception", ex);
            }

            return IsFound;
        }

        public static DataTable GetAllRoomTypes()
        {
            return clsDataAccessHelper.GetAll("SP_GetAllRoomTypes");
        }

        public static DataTable GetAllRoomTypesTitle()
        {
            return clsDataAccessHelper.GetAll("SP_GetAllRoomTypesTitle");
        }
    }
}