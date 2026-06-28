using BholaCattleApp.Models;
using BholaCattleApp.ViewModels;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;

namespace BholaCattleApp.Services
{
    public class MSP
    {
        public static User LoginVerification(string username, string password, out string message)
        {
            message = "Unknown error";
            User user = null;
            try
            {
                using (OracleCommand cmd = new OracleCommand("Verify_Login",Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("vUsername", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("vPassword", OracleDbType.Varchar2).Value = password;

                    cmd.Parameters.Add("vResult", OracleDbType.Int32).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("vMessage", OracleDbType.Varchar2,200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("vRetval", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    var resultParam = cmd.Parameters["vResult"].Value;
                    int result = (resultParam == DBNull.Value || resultParam == null) ? 0 : ((OracleDecimal)resultParam).ToInt32();

                    var messageParam = cmd.Parameters["vMessage"].Value;
                    message = (messageParam == DBNull.Value || messageParam == null) ? "No message returned" : messageParam.ToString();

                    if (result == 1)
                    {
                        object obj = cmd.Parameters["vRetval"].Value;
                        if (obj != null)
                        {
                            using (OracleDataReader reader = ((OracleRefCursor)obj).GetDataReader())
                            {
                                if (reader.Read())
                                {
                                    user = new User
                                    {
                                        username = reader.IsDBNull(0) ? null : reader.GetString(0),
                                        firstname = reader.IsDBNull(1) ? null : reader.GetString(1),
                                        lastname = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    };
                                }
                            }
                        }
                        
                    }
                    
                    return user;
                }
            }
            catch (Exception ex)
            {
                message = $"Login execution failed.{ex.Message}";
                return null;
            }
        }

        public static string AddEditAnimal(Animal animal, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Animal",Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure; 

                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = animal.AnimalID == null ? (object)DBNull.Value : animal.AnimalID;
                    cmd.Parameters.Add("vtagnumber", OracleDbType.Varchar2).Value = animal.TagNumber;
                    cmd.Parameters.Add("vname", OracleDbType.Varchar2).Value = animal.Name;
                    cmd.Parameters.Add("vspecies", OracleDbType.Varchar2).Value = animal.Species;
                    cmd.Parameters.Add("vbreed", OracleDbType.Varchar2).Value = animal.Breed;
                    cmd.Parameters.Add("vgender", OracleDbType.Varchar2).Value = animal.Gender;
                    cmd.Parameters.Add("vdob", OracleDbType.Date).Value = animal.DateOfBirth;

                    var msgParam = new OracleParameter("vmsg", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmsg"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Animal Updating exception.{ex.Message}";
            }
        }

        public static string AddEditStatusAnimal(StatusAnimal animalStatus, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_AnimalStatus", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vanimalstatusid", OracleDbType.Int32).Value = animalStatus.StatusHistoryID == null ? (object)DBNull.Value : animalStatus.AnimalStatusID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = animalStatus.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int32).Value = animalStatus.StatusID;
                    cmd.Parameters.Add("vstartdate", OracleDbType.Date).Value = animalStatus.StartDate;
                    cmd.Parameters.Add("venddate", OracleDbType.Date).Value = animalStatus.EndDate == null ? (object)DBNull.Value : animalStatus.EndDate;
                    cmd.Parameters.Add("vnotes", OracleDbType.Varchar2).Value = animalStatus.Notes;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Animal Status Updating exception. {ex.Message}";
            }
        }

        public static string AddEditHeifer(Heifer heifer, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Heifer", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vheiferid", OracleDbType.Int32).Value = heifer.HeiferHistoryID == null ? (object)DBNull.Value : heifer.HeiferID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = heifer.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int32).Value = heifer.StatusID;
                    cmd.Parameters.Add("vheatdate", OracleDbType.Date).Value = heifer.StartDate;
                    cmd.Parameters.Add("vweight", OracleDbType.Int32).Value = heifer.Weight;
                    cmd.Parameters.Add("vnotes", OracleDbType.Varchar2).Value = heifer.Notes;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Heifer Updating exception. {ex.Message}";
            }
        }

        public static string AddEditPregnant(Pregnant pregnancy, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Pregnancy", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vpregnancyid", OracleDbType.Int32).Value = pregnancy.PregnantHistoryID == null ? (object)DBNull.Value : pregnancy.PregnancyID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = pregnancy.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int32).Value = pregnancy.StatusID;
                    cmd.Parameters.Add("vpregnancydate", OracleDbType.Date).Value = pregnancy.PregnantDate == null ? (object)DBNull.Value : pregnancy.PregnancyDate;
                    cmd.Parameters.Add("vdeliverdate", OracleDbType.Date).Value = pregnancy.DeliverDate == null ? (object)DBNull.Value : pregnancy.DeliverDate;
                    cmd.Parameters.Add("vcalfgender", OracleDbType.Varchar2).Value = pregnancy.Gender;
                    cmd.Parameters.Add("vcalfanimalid", OracleDbType.Int32).Value = pregnancy.Result == null ? (object)DBNull.Value : pregnancy.CalfAnimalID;
                    cmd.Parameters.Add("vnotes", OracleDbType.Varchar2).Value = pregnancy.Notes;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Pregnancy Updating exception. {ex.Message}";
            }
        }

        public static string AddEditMilking(Milking milking, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Milking", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vmilkingid", OracleDbType.Int32).Value = milking.MilkingHistoryID == null ? (object)DBNull.Value : milking.MilkingID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = milking.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int32).Value = milking.StatusID;
                    cmd.Parameters.Add("vmilkingdate", OracleDbType.Date).Value = milking.MilkingDate;
                    cmd.Parameters.Add("vmorningqty", OracleDbType.Int32).Value = milking.MorningQty;
                    cmd.Parameters.Add("veveningqty", OracleDbType.Int32).Value = milking.EveningQty;
                    cmd.Parameters.Add("vnightqty", OracleDbType.Int32).Value = milking.NightQty;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Milking Updating exception. {ex.Message}";
            }
        }

        public static string AddEditFeeding(Feeding feed, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Feed", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vfeedid", OracleDbType.Int32).Value = feed.FeedingHistoryID == null ? (object)DBNull.Value : feed.FeedID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = feed.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int32).Value = feed.StatusID;
                    cmd.Parameters.Add("vstartdate", OracleDbType.Date).Value = feed.StartDate;
                    cmd.Parameters.Add("venddate", OracleDbType.Date).Value = feed.EndDate == null ? (object)DBNull.Value : feed.EndDate;
                    cmd.Parameters.Add("vtype", OracleDbType.Varchar2).Value = feed.Type;
                    cmd.Parameters.Add("vquantity", OracleDbType.Int32).Value = feed.Quantity;
                    cmd.Parameters.Add("vtotalcount", OracleDbType.Int32).Value = feed.TotalCount;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Feed Updating exception. {ex.Message}";
            }
        }

        public static string AddEditMedicine(Medicine medicine, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Medicine", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vmedicineid", OracleDbType.Int32).Value = medicine.MedicineHistoryID == null ? (object)DBNull.Value : medicine.MedicineID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int32).Value = medicine.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int32).Value = medicine.StatusID;
                    cmd.Parameters.Add("vmeddate", OracleDbType.Date).Value = medicine.MedicineDate;
                    cmd.Parameters.Add("vname", OracleDbType.Varchar2).Value = medicine.Name;
                    cmd.Parameters.Add("vdosage", OracleDbType.Varchar2).Value = medicine.Dosage;
                    cmd.Parameters.Add("vnote", OracleDbType.Varchar2).Value = medicine.Note;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Medicine Updating exception. {ex.Message}";
            }
        }

        public static string AddEditTransactions(Transaction transaction, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Transactions", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vtransactionid", OracleDbType.Int32).Value = transaction.TransactionHistoryID == null ? (object)DBNull.Value : transaction.TransactionID;
                    cmd.Parameters.Add("vtrandate", OracleDbType.Date).Value = transaction.TransDate;
                    cmd.Parameters.Add("vtrantype", OracleDbType.Varchar2).Value = transaction.Type;
                    cmd.Parameters.Add("vfk_itemid", OracleDbType.Int32).Value = transaction.ItemID;
                    cmd.Parameters.Add("vquantity", OracleDbType.Int32).Value = transaction.Qty;
                    cmd.Parameters.Add("vunitprice", OracleDbType.Int32).Value = transaction.Price;
                    cmd.Parameters.Add("vtotalamount", OracleDbType.Int32).Value = transaction.TotalAmount;
                    cmd.Parameters.Add("vnotes", OracleDbType.Varchar2).Value = transaction.Note;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Transaction Updating exception. {ex.Message}";
            }
        }

        public static string AddEditVaccine(Vaccine vaccine, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Vaccine", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vvaccineid", OracleDbType.Int32).Value = vaccine.VaccineHistoryID == null ? (object)DBNull.Value : vaccine.VaccineID;
                    cmd.Parameters.Add("vname", OracleDbType.Varchar2).Value = vaccine.Name;
                    cmd.Parameters.Add("vqty", OracleDbType.Int32).Value = vaccine.Qty;
                    cmd.Parameters.Add("vprice", OracleDbType.Int32).Value = vaccine.Price;

                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2);
                    msgParam.Direction = ParameterDirection.Output;
                    msgParam.Size = 200;
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    return message = cmd.Parameters["vmessage"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                return message = $"Vaccine Updating exception. {ex.Message}";
            }
        }
    }
}
