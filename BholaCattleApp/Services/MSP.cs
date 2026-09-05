using BholaCattleApp.Models;
using BholaCattleApp.ViewModels;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

                    cmd.Parameters.Add("vResult", OracleDbType.Int64).Direction = ParameterDirection.Output;
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

        #region Animal
        public static DataTable GetGenderOptions()
        {
            DataTable dt = new DataTable();
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("pr_get_gender", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter retval = cmd.Parameters.Add("retval", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetGender failed: {ex.Message}");
            }
            return dt;
        }
        public static DataTable GetSpeciesOptions()
        {
            DataTable dt = new DataTable();
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("pr_get_species", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    OracleParameter retval = cmd.Parameters.Add("retval", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetGender failed: {ex.Message}");
            }
            return dt;
        }
        public static DataTable GetAnimalRecords(int offset, int limit)
        {
            DataTable dt = new DataTable();
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("pr_get_animalrecord", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("voffset", OracleDbType.Int64).Value = offset;
                    cmd.Parameters.Add("vlimit", OracleDbType.Int64).Value = limit;
                    cmd.Parameters.Add("retval", OracleDbType.RefCursor, ParameterDirection.Output);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAnimalRecords failed: {ex.Message}");
            }
            return dt;
        }

        public static bool DeleteAnimal(int animalId,  out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("pr_delete_animal", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = animalId;
                    //cmd.Parameters.Add("vusername", OracleDbType.Varchar2, 100).Value = username;
                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    message = msgParam.Value?.ToString();
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = $"Delete failed: {ex.Message}";
                return false;
            }
        }
        public static bool DeleteAnimals(IEnumerable<int> animalIds,  out string message)
        {
            int successCount = 0;
            var errors = new List<string>();

            foreach (var id in animalIds)
            {
                if (DeleteAnimal(id,  out string rowMessage))
                {
                    successCount++;
                }
                else
                {
                    errors.Add($"ID {id}: {rowMessage}");
                }
            }

            if (errors.Count == 0)
            {
                message = $"{successCount} record(s) deleted.";
                return true;
            }

            message = $"{successCount} deleted, {errors.Count} failed:\n" + string.Join("\n", errors);
            return errors.Count < successCount; // treat as overall success if most rows succeeded
        }

        public static string AddEditAnimal(Animal animal, string username, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Animal",Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = animal.AnimalID > 0 ? (object)animal.AnimalID : DBNull.Value;
                    cmd.Parameters.Add("vtagnumber", OracleDbType.Int64).Value = animal.TagNumber;
                    cmd.Parameters.Add("vname", OracleDbType.Varchar2).Value = animal.Name;
                    cmd.Parameters.Add("vfk_speciesid", OracleDbType.Int64).Value = animal.SpeciesID;
                    cmd.Parameters.Add("vbreed", OracleDbType.Varchar2).Value = animal.Breed;
                    cmd.Parameters.Add("vfk_genderid", OracleDbType.Int64).Value = animal.GenderID;
                    cmd.Parameters.Add("vdob", OracleDbType.Date).Value = animal.DateOfBirth;
                    cmd.Parameters.Add("vusername", OracleDbType.Varchar2, 100).Value = username;
                    
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
                return message = $"Animal Updating exception.{ex.Message}";
            }
        }

        #endregion

        #region Animal Status
        public static DataTable GetAnimalOptions()
        {
            Connection.RecheckConnection();

            using (var cmd = new OracleCommand(
                "SELECT animalid, tagnumber, name, tagnumber || ' - ' || name AS display " +
                "FROM animals WHERE delstatus = 0 ORDER BY name", Connection._connection))
            using (var adapter = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetStatusOptions()
        {
            Connection.RecheckConnection();

            using (var cmd = new OracleCommand(
                "SELECT statusid, name FROM status ORDER BY name", Connection._connection))
            using (var adapter = new OracleDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public static DataTable GetAnimalStatusRecords(int offset, int limit)
        {
            Connection.RecheckConnection();

            using (OracleCommand cmd = new OracleCommand("pr_get_animalstatusrecord", Connection._connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("voffset", OracleDbType.Int32).Value = offset;
                cmd.Parameters.Add("vlimit", OracleDbType.Int32).Value = limit;

                var cursorParam = new OracleParameter("retval", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(cursorParam);

                using (var adapter = new OracleDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
        public static bool DeleteAnimalStatus(int animalStatusId,  out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("pr_delete_animalstatus", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("vanimalstatusid", OracleDbType.Int64).Value = animalStatusId;
                    cmd.Parameters.Add("vusername", OracleDbType.Varchar2, 100).Value = "Dev";
                    
                    var msgParam = new OracleParameter("vmessage", OracleDbType.Varchar2, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();
                    message = msgParam.Value?.ToString();
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = $"Delete failed: {ex.Message}";
                return false;
            }
        }
        public static bool DeleteAnimalStatus(IEnumerable<int> animalStatusIds,  out string message)
        {
            int successCount = 0;
            var errors = new List<string>();

            foreach (var id in animalStatusIds)
            {
                if (DeleteAnimalStatus(id, out string rowMessage))
                {
                    successCount++;
                }
                else
                {
                    errors.Add($"ID {id}: {rowMessage}");
                }
            }

            if (errors.Count == 0)
            {
                message = $"{successCount} record(s) deleted.";
                return true;
            }

            message = $"{successCount} deleted, {errors.Count} failed:\n" + string.Join("\n", errors);
            return errors.Count < successCount; // treat as overall success if most rows succeeded
        }
        public static string AddEditStatusAnimal(StatusAnimal animalStatus,string username, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_animalstatus", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("vanimalstatusid", OracleDbType.Int64).Value = animalStatus.AnimalStatusID > 0 ? (object)animalStatus.AnimalStatusID : DBNull.Value ;
                    cmd.Parameters.Add("vfk_animalid", OracleDbType.Int64).Value = animalStatus.AnimalID;
                    cmd.Parameters.Add("vfk_statusid", OracleDbType.Int64).Value = animalStatus.StatusID;
                    cmd.Parameters.Add("vstartdate", OracleDbType.Date).Value = animalStatus.StartDate;
                    cmd.Parameters.Add("venddate", OracleDbType.Date).Value = animalStatus.EndDate == null ? (object)DBNull.Value : animalStatus.EndDate;
                    cmd.Parameters.Add("vnotes", OracleDbType.Varchar2).Value = animalStatus.Notes;
                    cmd.Parameters.Add("vusername", OracleDbType.Varchar2, 100).Value = username;

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
        #endregion

        public static string AddEditHeifer(Heifer heifer, out string message)
        {
            try
            {
                Connection.RecheckConnection();

                using (OracleCommand cmd = new OracleCommand("AddEdit_Heifer", Connection._connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("vheiferid", OracleDbType.Int64).Value = heifer.HeiferHistoryID == null ? (object)DBNull.Value : heifer.HeiferID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = heifer.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int64).Value = heifer.StatusID;
                    cmd.Parameters.Add("vheatdate", OracleDbType.Date).Value = heifer.StartDate;
                    cmd.Parameters.Add("vweight", OracleDbType.Int64).Value = heifer.Weight;
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

                    //cmd.Parameters.Add("vpregnancyid", OracleDbType.Int64).Value = pregnancy.PregnantHistoryID == null ? (object)DBNull.Value : pregnancy.PregnancyID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = pregnancy.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int64).Value = pregnancy.StatusID;
                    //cmd.Parameters.Add("vpregnancydate", OracleDbType.Date).Value = pregnancy.PregnantDate == null ? (object)DBNull.Value : pregnancy.PregnancyDate;
                    //cmd.Parameters.Add("vdeliverdate", OracleDbType.Date).Value = pregnancy.DeliverDate == null ? (object)DBNull.Value : pregnancy.DeliverDate;
                    cmd.Parameters.Add("vcalfgender", OracleDbType.Varchar2).Value = pregnancy.Gender;
                    //cmd.Parameters.Add("vcalfanimalid", OracleDbType.Int64).Value = pregnancy.Result == null ? (object)DBNull.Value : pregnancy.CalfAnimalID;
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

                    //cmd.Parameters.Add("vmilkingid", OracleDbType.Int64).Value = milking.MilkingHistoryID == null ? (object)DBNull.Value : milking.MilkingID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = milking.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int64).Value = milking.StatusID;
                    cmd.Parameters.Add("vmilkingdate", OracleDbType.Date).Value = milking.MilkingDate;
                    cmd.Parameters.Add("vmorningqty", OracleDbType.Int64).Value = milking.MorningQty;
                    cmd.Parameters.Add("veveningqty", OracleDbType.Int64).Value = milking.EveningQty;
                    cmd.Parameters.Add("vnightqty", OracleDbType.Int64).Value = milking.NightQty;

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

                    //cmd.Parameters.Add("vfeedid", OracleDbType.Int64).Value = feed.FeedingHistoryID == null ? (object)DBNull.Value : feed.FeedID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = feed.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int64).Value = feed.StatusID;
                    cmd.Parameters.Add("vstartdate", OracleDbType.Date).Value = feed.StartDate;
                    cmd.Parameters.Add("venddate", OracleDbType.Date).Value = feed.EndDate == null ? (object)DBNull.Value : feed.EndDate;
                    cmd.Parameters.Add("vtype", OracleDbType.Varchar2).Value = feed.Type;
                    cmd.Parameters.Add("vquantity", OracleDbType.Int64).Value = feed.Quantity;
                    cmd.Parameters.Add("vtotalcount", OracleDbType.Int64).Value = feed.TotalCount;

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

                    //cmd.Parameters.Add("vmedicineid", OracleDbType.Int64).Value = medicine.MedicineHistoryID == null ? (object)DBNull.Value : medicine.MedicineID;
                    cmd.Parameters.Add("vanimalid", OracleDbType.Int64).Value = medicine.AnimalID;
                    cmd.Parameters.Add("vstatusid", OracleDbType.Int64).Value = medicine.StatusID;
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

                    //cmd.Parameters.Add("vtransactionid", OracleDbType.Int64).Value = transaction.TransactionHistoryID == null ? (object)DBNull.Value : transaction.TransactionID;
                    cmd.Parameters.Add("vtrandate", OracleDbType.Date).Value = transaction.TransDate;
                    cmd.Parameters.Add("vtrantype", OracleDbType.Varchar2).Value = transaction.Type;
                    cmd.Parameters.Add("vfk_itemid", OracleDbType.Int64).Value = transaction.ItemID;
                    cmd.Parameters.Add("vquantity", OracleDbType.Int64).Value = transaction.Qty;
                    cmd.Parameters.Add("vunitprice", OracleDbType.Int64).Value = transaction.Price;
                    cmd.Parameters.Add("vtotalamount", OracleDbType.Int64).Value = transaction.TotalAmount;
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

                    //cmd.Parameters.Add("vvaccineid", OracleDbType.Int64).Value = vaccine.VaccineHistoryID == null ? (object)DBNull.Value : vaccine.VaccineID;
                    cmd.Parameters.Add("vname", OracleDbType.Varchar2).Value = vaccine.Name;
                    cmd.Parameters.Add("vqty", OracleDbType.Int64).Value = vaccine.Qty;
                    cmd.Parameters.Add("vprice", OracleDbType.Int64).Value = vaccine.Price;

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
