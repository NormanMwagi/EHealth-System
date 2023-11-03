using EHealth.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SHERIA.Helpers;
using System.Data;
using System.Data.SqlClient;


namespace SHERIA.Models
{
    public class DBHandler : IDisposable
    {
        public readonly IConfiguration config;
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        private SqlConnection connection;
        private string connectionstring;

        public DBHandler(string connstring)
        {
            connection = new SqlConnection(connstring);
            this.connection.Open();
            connectionstring = connstring;
        }

        public void Dispose()
        {
            connection.Close();
        }

        #region Databases
        public enum DataBaseObject
        {
            HostDB,
            BrokerDB
        }

        public string GetDataBaseConnection(DataBaseObject databaseobject)
        {
            string connection_string = connectionstring; //config["ConnectionStrings:DefaultConnection"];
            switch (databaseobject)
            {
                case DataBaseObject.HostDB:
                    connection_string = connectionstring; //config["ConnectionStrings:DefaultConnection"];
                    break;
                default:
                    connection_string = connectionstring; //config["ConnectionStrings:DefaultConnection"];
                    break;
            }
            return connection_string;
        }
        #endregion

        #region Client

        //Client
        public List<ClientRecordModel> GetClientRecord()
        {
            List<ClientRecordModel> recordlist = new List<ClientRecordModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("client_record");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new ClientRecordModel
                    {
                        id = Convert.ToInt32(dr["id"]),
                        client_type = Convert.ToString(dr["client_type"]),
                        client_name = Convert.ToString(dr["client_name"]),
                        phone_number = Convert.ToString(dr["phone_number"]),
                        email = Convert.ToString(dr["email"]),
                        id_number = Convert.ToString(dr["id_number"]),
                        kra_pin = Convert.ToString(dr["kra_pin"]),
                        physical_address = Convert.ToString(dr["physical_address"]),
                        postal_address = Convert.ToString(dr["postal_address"]),
                        industry = Convert.ToString(dr["industry"]),
                        remarks = Convert.ToString(dr["remarks"]),

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetClientRecords | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public Int64 AddClient(ClientRecordModel model)
        {
            try
            {
                Int64 i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_client_record", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@client_type", model.client_type);
                    cmd.Parameters.AddWithValue("@client_name", model.client_name);
                    cmd.Parameters.AddWithValue("@phone_number", model.phone_number);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@id_number", model.id_number);
                    cmd.Parameters.AddWithValue("@kra_pin", model.kra_pin);
                    cmd.Parameters.AddWithValue("@physical_address", model.physical_address);
                    cmd.Parameters.AddWithValue("@postal_address", model.postal_address);
                    cmd.Parameters.AddWithValue("@industry", model.industry);
                    cmd.Parameters.AddWithValue("@remarks", model.remarks);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);
                    cmd.ExecuteNonQuery();

                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                return i;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddClient | Exception ->" + ex.Message);
                return 0;
            }
        }

        public bool UpdateClient(ClientRecordModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_client_record", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@client_type", model.client_type);
                    cmd.Parameters.AddWithValue("@client_name", model.client_name);
                    cmd.Parameters.AddWithValue("@phone_number", model.phone_number);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@id_number", model.id_number);
                    cmd.Parameters.AddWithValue("@kra_pin", model.kra_pin);
                    cmd.Parameters.AddWithValue("@physical_address", model.physical_address);
                    cmd.Parameters.AddWithValue("@postal_address", model.postal_address);
                    cmd.Parameters.AddWithValue("@industry", model.industry);
                    cmd.Parameters.AddWithValue("@remarks", model.remarks);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateClient | Exception ->" + ex.Message);
                return false;
            }
        }

        //EndClient

        //ClientFiles

        public bool AddClientFiles(ClientFilesModel model)
        {
            try
            {
                Int64 i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_client_file", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@client_id", model.client_id);
                    cmd.Parameters.AddWithValue("@file_name", model.file_name);

                    cmd.ExecuteNonQuery();
                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i > 0)

                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddPost | Exception ->" + ex.Message);
                return false;
            }
        }
        //Doctors
        public List<DoctorsModel> GetDoctors()
        {
            List<DoctorsModel> recordlist = new List<DoctorsModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("doctors_record");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new DoctorsModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        name = Convert.ToString(dr["name"]),
                        department_id = Convert.ToInt64(dr["department_id"]),
                        phone_no = Convert.ToString(dr["phone_no"]),
                        email = Convert.ToString(dr["email"]),
                        age = Convert.ToInt64(dr["age"]),
                        gender = Convert.ToString(dr["gender"]),
                        hospital_id = Convert.ToInt64(dr["hospital_id"])


                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetSubjectTypes | Exception ->" + ex.Message);
            }

            return recordlist;
        }
        public bool AddDoctors(DoctorsModel model)
        {
            try
            {
                Int64 i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_doctor", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.Parameters.AddWithValue("@department_id", model.department_id);
                    cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@age", model.age);
                    cmd.Parameters.AddWithValue("@gender", model.gender);
                    cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);

                    cmd.ExecuteNonQuery();

                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddDoctor | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateDoctors(DoctorsModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_doctor", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.Parameters.AddWithValue("@department_id", model.department_id);
                    cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@age", model.age);
                    cmd.Parameters.AddWithValue("@gender", model.gender);
                    cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);

                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateDoctor | Exception ->" + ex.Message);
                return false;
            }
        }
        //Hospital
        public List<HospitalModel> GetHospital()
        {
            List<HospitalModel> recordlist = new List<HospitalModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("hospital_record");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new HospitalModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        hospital_name = Convert.ToString(dr["hospital_name"]),
                        phone_no = Convert.ToString(dr["phone_no"]),
                        email = Convert.ToString(dr["email"]),
                        physical_location = Convert.ToString(dr["physical_location"])
                        

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetSubjectTypes | Exception ->" + ex.Message);
            }

            return recordlist;
        }
        public bool AddHospital(HospitalModel model)
        {
            try
            {
                Int64 i = 0;

                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_hospital", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@hospital_name", model.hospital_name);
                    cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@physical_location", model.physical_location);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);

                    cmd.ExecuteNonQuery();
                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddHospital | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateHospital(HospitalModel mymodel, DataBaseObject databaseobject = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(databaseobject)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_hospital", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", mymodel.id);
                        cmd.Parameters.AddWithValue("@hospital_name", mymodel.hospital_name);
                        cmd.Parameters.AddWithValue("@phone_no", mymodel.phone_no);
                        cmd.Parameters.AddWithValue("@email", mymodel.email);
                        cmd.Parameters.AddWithValue("@physical_location", mymodel.physical_location);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                return false;
            }
        }
        //EndHospital
        //Department
        public List<DepartmentsModel> GetDepartment()
        {
            List<DepartmentsModel> recordlist = new List<DepartmentsModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("department_record");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new DepartmentsModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        department_name = Convert.ToString(dr["department_name"]),
                        description = Convert.ToString(dr["description"]),
                        hospital_id = Convert.ToInt64(dr["hospital_id"]),
                        no_of_employees = Convert.ToInt64(dr["no_of_employees"])


                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetSubjectTypes | Exception ->" + ex.Message);
            }

            return recordlist;
        }
        public bool AddDepartment(DepartmentsModel model)
        {
            try
            {
                Int64 i = 0;

                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("[add_department]", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@department_name", model.department_name);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);
                    cmd.Parameters.AddWithValue("@no_of_employees", model.no_of_employees);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);

                    cmd.ExecuteNonQuery();
                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddHospital | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateDepartment(DepartmentsModel model, DataBaseObject databaseobject = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(databaseobject)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_department", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", model.id);
                        cmd.Parameters.AddWithValue("@department_name", model.department_name);
                        cmd.Parameters.AddWithValue("@description", model.description);
                        cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);
                        cmd.Parameters.AddWithValue("@no_of_employees", model.no_of_employees);

                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                return false;
            }
        }
        //Nurse
        public List<NurseModel> GetNurse()
        {
            List<NurseModel> recordlist = new List<NurseModel>();
            
            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("nurse_record");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new NurseModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        name = Convert.ToString(dr["name"]),
                        gender = Convert.ToString(dr["gender"]),
                        age = Convert.ToInt64(dr["age"]),
                        phone_no = Convert.ToString(dr["phone_no"]),
                        email = Convert.ToString(dr["email"]),
                        hospital_id = Convert.ToInt64(dr["hospital_id"]),
                        department_id = Convert.ToInt64(dr["department_id"])

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetSubjectTypes | Exception ->" + ex.Message);
            }

            return recordlist;
        }
        public bool AddNurse(NurseModel model)
        {
            try
            {
                Int64 i = 0;

                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("[add_nurse]", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.Parameters.AddWithValue("@gender", model.gender);
                    cmd.Parameters.AddWithValue("@age", model.age);
                    cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);
                    cmd.Parameters.AddWithValue("@department_id", model.department_id);

                    cmd.ExecuteNonQuery();
                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddNurse | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateNurse(NurseModel model, DataBaseObject databaseobject = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(databaseobject)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_nurse", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", model.id);
                        cmd.Parameters.AddWithValue("@name", model.name);
                        cmd.Parameters.AddWithValue("@gender", model.gender);
                        cmd.Parameters.AddWithValue("@age", model.age);
                        cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                        cmd.Parameters.AddWithValue("@email", model.email);
                        cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);
                        cmd.Parameters.AddWithValue("@department_id", model.department_id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                return false;
            }
        }
        //patient
        public List<PatientModel> GetPatient()
        {
            List<PatientModel> recordlist = new List<PatientModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("patient_record");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new PatientModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        name = Convert.ToString(dr["name"]),
                        age = Convert.ToInt64(dr["age"]),
                        residency = Convert.ToString(dr["residency"]),
                        phone_no = Convert.ToString(dr["phone_no"]),
                        insurance = Convert.ToString(dr["insurance"]),
                        insurance_no = Convert.ToInt64(dr["insurance_no"]),
                        health_issue = Convert.ToString(dr["health_issue"]),
                        book_appointment_date = Convert.ToDateTime(dr["book_appointment_date"]),
                        hospital_id = Convert.ToInt64(dr["hospital_id"]),
                        department_id = Convert.ToInt64(dr["department_id"]),
                        doctor_id = Convert.ToInt64(dr["doctor_id"])

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetSubjectTypes | Exception ->" + ex.Message);
            }

            return recordlist;
        }
        public bool AddPatient(PatientModel model)
        {
            try
            {
                Int64 i = 0;

                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("[add_patient]", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.Parameters.AddWithValue("@age", model.age);
                    cmd.Parameters.AddWithValue("@residency", model.residency);
                    cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                    cmd.Parameters.AddWithValue("@insurance", model.insurance);
                    cmd.Parameters.AddWithValue("@insurance_no", model.insurance_no);
                    cmd.Parameters.AddWithValue("@health_issue", model.health_issue);
                    cmd.Parameters.AddWithValue("@book_appointment_date", model.book_appointment_date);
                    cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);
                    cmd.Parameters.AddWithValue("@department_id", model.department_id);
                    cmd.Parameters.AddWithValue("@doctor_id", model.doctor_id);

                    cmd.ExecuteNonQuery();
                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddPatient | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdatePatient(PatientModel model, DataBaseObject databaseobject = DataBaseObject.HostDB)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(databaseobject)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_patient", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", model.id);
                        cmd.Parameters.AddWithValue("@name", model.name);
                        cmd.Parameters.AddWithValue("@age", model.age);
                        cmd.Parameters.AddWithValue("@residency", model.residency);
                        cmd.Parameters.AddWithValue("@phone_no", model.phone_no);
                        cmd.Parameters.AddWithValue("@insurance", model.insurance);
                        cmd.Parameters.AddWithValue("@insurance_no", model.insurance_no);
                        cmd.Parameters.AddWithValue("@health_issue", model.health_issue);
                        cmd.Parameters.AddWithValue("@book_appointment_date", model.book_appointment_date);
                        cmd.Parameters.AddWithValue("@hospital_id", model.hospital_id);
                        cmd.Parameters.AddWithValue("@department_id", model.department_id);
                        cmd.Parameters.AddWithValue("@doctor_id", model.doctor_id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                return false;
            }
        }

        //Matters
        public List<MattersModel> GetMatters()
        {
            List<MattersModel> recordlist = new List<MattersModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("matters");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new MattersModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        matter_name = Convert.ToString(dr["matter_name"]),
                        matter_number = Convert.ToString(dr["matter_number"]),
                        user_id = Convert.ToInt64(dr["user_id"]),
                        client_id = Convert.ToInt64(dr["client_id"]),
                        start_date = Convert.ToDateTime(dr["start_date"]),
                        close_date = Convert.ToDateTime(dr["close_date"]),
                        practice_area = Convert.ToString(dr["matter_name"]),
                        matter_status = Convert.ToString(dr["matter_name"]),
                        matter_billing = Convert.ToString(dr["matter_name"]),
                        description = Convert.ToString(dr["matter_name"]),
                        created_by = Convert.ToInt64(dr["matter_name"])

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetSubjectTypes | Exception ->" + ex.Message);
            }

            return recordlist;
        }


        public bool AddMatters(MattersModel model)
        {
            try
            {
                Int64 i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_matter", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@matter_name", model.matter_name);
                    cmd.Parameters.AddWithValue("@matter_number", model.matter_number);
                    cmd.Parameters.AddWithValue("@user_id", model.user_id);
                    cmd.Parameters.AddWithValue("@client_id", model.client_id);
                    cmd.Parameters.AddWithValue("@start_date", model.start_date);
                    cmd.Parameters.AddWithValue("@close_date", model.close_date);
                    cmd.Parameters.AddWithValue("@practice_area", model.practice_area);
                    cmd.Parameters.AddWithValue("@matter_status", model.matter_status);
                    cmd.Parameters.AddWithValue("@matter_billing", model.matter_billing);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);
                    cmd.ExecuteNonQuery();

                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddMatter | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateMatters(MattersModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_matter", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@matter_name", model.matter_name);
                    cmd.Parameters.AddWithValue("@matter_number", model.matter_number);
                    cmd.Parameters.AddWithValue("@user_id", model.user_id);
                    cmd.Parameters.AddWithValue("@client_id", model.client_id);
                    cmd.Parameters.AddWithValue("@start_date", model.start_date);
                    cmd.Parameters.AddWithValue("@close_date", model.close_date);
                    cmd.Parameters.AddWithValue("@practice_area", model.practice_area);
                    cmd.Parameters.AddWithValue("@matter_status", model.matter_status);
                    cmd.Parameters.AddWithValue("@matter_billing", model.matter_billing);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateMatter | Exception ->" + ex.Message);
                return false;
            }
        }

        //EndMatters 
        
        //Tasks

        public List<TasksModel> GetTasks()
        {
            List<TasksModel> recordlist = new List<TasksModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("tasks");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new TasksModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        task_name = Convert.ToString(dr["task_name"]),
                        matter_id = Convert.ToInt64(dr["matter_id"]),
                        due_date = Convert.ToDateTime(dr["due_date"]),
                        user_id = Convert.ToInt64(dr["user_id"]),
                        priority = Convert.ToString(dr["priority"]),
                        description = Convert.ToString(dr["description"]),

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetTasks | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddTasks(TasksModel model)
        {
            try
            {
                Int64 i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_tasks", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@task_name", model.task_name);
                    cmd.Parameters.AddWithValue("@matter_id", model.matter_id);
                    cmd.Parameters.AddWithValue("@due_date", model.due_date);
                    cmd.Parameters.AddWithValue("@user_id", model.user_id);
                    cmd.Parameters.AddWithValue("@priority", model.priority);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);
                    cmd.ExecuteNonQuery();

                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddTask | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateTasks(TasksModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_tasks", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@task_name", model.task_name);
                    cmd.Parameters.AddWithValue("@matter_id", model.matter_id);
                    cmd.Parameters.AddWithValue("@due_date", model.due_date);
                    cmd.Parameters.AddWithValue("@user_id", model.user_id);
                    cmd.Parameters.AddWithValue("@priority", model.priority);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdaTask | Exception ->" + ex.Message);
                return false;
            }
        }

        //EndTasks

        

        //Invoices

        public List<InvoiceModel> GetInvoices()
        {
            List<InvoiceModel> recordlist = new List<InvoiceModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("invoice");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new InvoiceModel
                    {
                        id = Convert.ToInt64(dr["id"]),
                        invoice_type = Convert.ToString(dr["invoice_type"]),
                        client_id = Convert.ToInt64(dr["client_id"]),
                        invoice_number = Convert.ToString(dr["invoice_number"]),
                        date_issued = Convert.ToDateTime(dr["date_issued"]),
                        due_date = Convert.ToDateTime(dr["due_date"]),
                        amount = Convert.ToString(dr["amount"]),
                        duration = Convert.ToString(dr["duration"]),
                        hourly_rate = Convert.ToString(dr["hourly_rate"]),
                        description = Convert.ToString(dr["description"]),

                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetInvoices | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddInvoices(InvoiceModel model)
        {
            try
            {
                Int64 i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_invoice", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@invoice_type", model.invoice_type);
                    cmd.Parameters.AddWithValue("@client_id", model.client_id);
                    cmd.Parameters.AddWithValue("@invoice_number", model.invoice_number);
                    cmd.Parameters.AddWithValue("@date_issued", model.date_issued);
                    cmd.Parameters.AddWithValue("@due_date", model.due_date);
                    cmd.Parameters.AddWithValue("@amount", model.amount);
                    cmd.Parameters.AddWithValue("@tax", model.tax);
                    cmd.Parameters.AddWithValue("@duration", model.duration);
                    cmd.Parameters.AddWithValue("@hourly_rate", model.hourly_rate);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);
                    cmd.ExecuteNonQuery();

                    i = Convert.ToInt64(cmd.Parameters["@id"].Value.ToString());
                }
                if (i >= 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddInvoice | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateInvoices(InvoiceModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_invoice", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@invoice_type", model.invoice_type);
                    cmd.Parameters.AddWithValue("@client_id", model.client_id);
                    cmd.Parameters.AddWithValue("@invoice_number", model.invoice_number);
                    cmd.Parameters.AddWithValue("@date_issued", model.date_issued);
                    cmd.Parameters.AddWithValue("@due_date", model.due_date);
                    cmd.Parameters.AddWithValue("@amount", model.amount);
                    cmd.Parameters.AddWithValue("@tax", model.tax);
                    cmd.Parameters.AddWithValue("@duration", model.duration);
                    cmd.Parameters.AddWithValue("@hourly_rate", model.hourly_rate);
                    cmd.Parameters.AddWithValue("@description", model.description);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateInvoice | Exception ->" + ex.Message);
                return false;
            }
        }

        //EndInvoices

        #endregion

        #region Adhoc
        public DataTable ValidateUserLogin(string user_type, string email_address)
        {
            DataTable dt = new DataTable();
            try
            {
                using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
                using SqlCommand cmd = new SqlCommand("validate_user_login", connect);
                using SqlDataAdapter sd = new SqlDataAdapter(cmd);
                connect.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", email_address);
                cmd.Parameters.AddWithValue("@profiletype", user_type);
                sd.Fill(dt);
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "ValidateUserLogin | Exception ->" + ex.Message);
            }

            return dt;
        }

        public bool UpdateProfile(Int16 record_id, Int16 profile_id, string attribute, string new_value)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_profile", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@record_id", record_id);
                    cmd.Parameters.AddWithValue("@profile_id", profile_id);
                    cmd.Parameters.AddWithValue("@attribute", attribute);
                    cmd.Parameters.AddWithValue("@new_value", new_value);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateProfile | Exception ->" + ex.Message);
                return false;
            }
        }

        public DataTable GetAdhocData(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
                using SqlCommand cmd = new SqlCommand(sql, connect);
                using SqlDataAdapter sd = new SqlDataAdapter(cmd);
                sd.Fill(dt);
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetAdhocData | Exception ->" + ex.Message);
            }
            return dt;
        }

        public DataTable GetRecords(string module, string param1 = "", string param2 = "")
        {
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
                using SqlCommand cmd = new SqlCommand("get_records", connect);
                using SqlDataAdapter sd = new SqlDataAdapter(cmd);
                connect.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@module", module);
                cmd.Parameters.AddWithValue("@param1", param1);
                cmd.Parameters.AddWithValue("@param2", param2);
                sd.Fill(dt);
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetRecords | Exception ->" + ex.Message);
            }

            return dt;
        }

        //public DataTable GetUnapprovedRecords(string module, string param1 = "")
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
        //        using SqlCommand cmd = new SqlCommand("get_records_unapproved", connect);
        //        using SqlDataAdapter sd = new SqlDataAdapter(cmd);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@module", module);
        //        if (param1 != "")
        //            cmd.Parameters.AddWithValue("@param1", param1);
        //        sd.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLogHelper.log_message_fields("ERROR", "GetUnapprovedRecords | Exception ->" + ex.Message);
        //    }

        //    return dt;
        //}


        public DataTable GetUnapprovedRecords(string module, string param1 = "", string param2 = "", DataBaseObject database = DataBaseObject.HostDB)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(database)))
                {
                    using (SqlCommand cmd = new SqlCommand("get_records_unapproved", connect))
                    {
                        using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@module", module);
                            cmd.Parameters.AddWithValue("@param1", param1);
                            cmd.Parameters.AddWithValue("@param2", param2);
                            sd.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                FileLogHelper.log_message_fields("ERROR", "GetUnapprovedRecords | Exception ->" + ex.Message);
            }

            return dt;
        }

        public DataTable GetRecordsById(string module, Int64 id, string param1 = "", string param2 = "")
        {
            DataTable dt = new DataTable();

            try
            {
                using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
                using SqlCommand cmd = new SqlCommand("get_records_by_id", connect);
                using SqlDataAdapter sd = new SqlDataAdapter(cmd);
                connect.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@module", module);
                cmd.Parameters.AddWithValue("@id", id);
                sd.Fill(dt);
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetRecordsById | Exception ->" + ex.Message);
            }

            return dt;
        }

        public string GetScalarItem(string sql)
        {
            string scalaritem = "";

            try
            {
                using SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB));
                using SqlCommand command = new SqlCommand(sql, connect);
                connect.Open();
                scalaritem = (string)(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetScalarItem | Exception ->" + ex.Message);
                scalaritem = "";
            }
            return scalaritem;
        }

        public bool ClearAppointment(string module,string param1 = "")
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("delete_appointment", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@module", module);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "DeleteAppointment | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool DeleteRecord(Int64 id, Int64 deleted_by, string module, string param1 = "")
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("delete_records", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@recordid", id);
                    cmd.Parameters.AddWithValue("@deleted_by", deleted_by);
                    cmd.Parameters.AddWithValue("@module", module);
                    if (param1 != "")
                        cmd.Parameters.AddWithValue("@param1", param1);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "DeleteRecord | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool ApproveRecord(Int64 id, Int64 approved_by, string module, string action_flag = "")
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("approve_records", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@record_id", id);
                    cmd.Parameters.AddWithValue("@approved_by", approved_by);
                    cmd.Parameters.AddWithValue("@module", module);
                    if (action_flag != "")
                        cmd.Parameters.AddWithValue("@action_flag", action_flag);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "ApproveRecord | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool AddAuditTrail(AuditTrailModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_audit_trail", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@user_name", model.user_name);
                    cmd.Parameters.AddWithValue("@action_type", model.action_type);
                    cmd.Parameters.AddWithValue("@action_description", model.action_description);
                    cmd.Parameters.AddWithValue("@page_accessed", model.page_accessed);
                    cmd.Parameters.AddWithValue("@client_ip_address", model.client_ip_address);
                    cmd.Parameters.AddWithValue("@session_id", model.session_id);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddAuditTrail | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool AllocateDeallocateRolePermission(string action, int role_id, int permission_id)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("allocate_deallocate_role_permission", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@role_id", role_id);
                    cmd.Parameters.AddWithValue("@permission_id", permission_id);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AllocateDeallocateRolePermission | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool AllocateDeallocateUserRole(string action, int user_id, int role_id)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("allocate_deallocate_user_role", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    cmd.Parameters.AddWithValue("@role_id", role_id);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AllocateDeallocateUserRole | Exception ->" + ex.Message);
                return false;
            }
        }
        #endregion

        #region Access Control
        //Roles
        public List<RolesModel> GetRoles()
        {
            List<RolesModel> recordlist = new List<RolesModel>();

            try
            {
                DataTable dt = new DataTable();
                dt = GetRecords("roles");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new RolesModel
                    {
                        id = Convert.ToInt32(dr["id"]),
                        role_name = Convert.ToString(dr["role_name"])!,
                        role_type = Convert.ToString(dr["role_type"])!,
                        remarks = Convert.ToString(dr["remarks"])!,
                        is_sys_admin = Convert.ToBoolean(dr["is_sys_admin"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetPortalRoles | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddRole(RolesModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_role", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@role_name", model.role_name);
                    cmd.Parameters.AddWithValue("@role_type", model.role_type);
                    cmd.Parameters.AddWithValue("@remarks", model.remarks);
                    cmd.Parameters.AddWithValue("@is_sys_admin", model.is_sys_admin);
                    

                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddRole | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateRole(RolesModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_role", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@role_name", model.role_name);
                    cmd.Parameters.AddWithValue("@role_type", model.role_type);
                    cmd.Parameters.AddWithValue("@remarks", model.remarks);
                    cmd.Parameters.AddWithValue("@is_sys_admin", model.is_sys_admin);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateRole | Exception ->" + ex.Message);
                return false;
            }
        }
        //End Roles

        //Role Menu Access
        public bool AddMenuAccess(string page_url, string main_menu_name, string sub_menu_name, int role_id, int can_access, int menu_order, int sub_menu_order)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_menu_access", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@role_id", role_id);
                    cmd.Parameters.AddWithValue("@main_menu_name", main_menu_name);
                    cmd.Parameters.AddWithValue("@sub_menu_name", sub_menu_name);
                    cmd.Parameters.AddWithValue("@page_url", page_url);
                    cmd.Parameters.AddWithValue("@can_access", can_access);
                    cmd.Parameters.AddWithValue("@menu_order", menu_order);
                    cmd.Parameters.AddWithValue("@sub_menu_order", sub_menu_order);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetPermissions | Exception ->" + ex.Message);
                return false;
            }
        }
        //End Role Menu Access

        //Permissions
        public List<PermissionsModel> GetPermissions()
        {
            List<PermissionsModel> recordlist = new List<PermissionsModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("permissions");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new PermissionsModel
                    {
                        id = Convert.ToInt32(dr["id"]),
                        permission_name = Convert.ToString(dr["permission_name"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetPermissions | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddPermission(PermissionsModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_permission", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@permission_name", model.permission_name);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddPermission | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdatePermission(PermissionsModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_permission", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@permission_name", model.permission_name);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdatePermission | Exception ->" + ex.Message);
                return false;
            }
        }
        //End Permissions

        //Role Permissions
        public List<RolePermissionModel> GetRolePermissions(int role_id)
        {
            List<RolePermissionModel> recordlist = new List<RolePermissionModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecordsById("role_allocated_permissions", role_id);

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new RolePermissionModel
                    {
                        id = Convert.ToInt32(dr["id"]),
                        role_id = Convert.ToInt32(dr["role_id"]),
                        permission_id = Convert.ToInt32(dr["permission_id"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetRolePermissions | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddRolePermission(RolePermissionModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_role_permission_mapping", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@role_id", model.role_id);
                    cmd.Parameters.AddWithValue("@permission_id", model.permission_id);

                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddRolePermission | Exception ->" + ex.Message);
                return false;
            }
        }
        //End Role Permissions

        //Users
        public List<PortalUsersModel> GetPortalUsers()
        {
            List<PortalUsersModel> recordlist = new List<PortalUsersModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("portal_users");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new PortalUsersModel
                    {
                        id = Convert.ToInt32(dr["id"]),
                        role_id = Convert.ToInt32(dr["role_id"]),
                        mobile = Convert.ToString(dr["mobile"])!,
                        email = Convert.ToString(dr["email"])!,
                        name = Convert.ToString(dr["name"])!,
                        password = Convert.ToString(dr["password"])!,
                        avatar = Convert.ToString(dr["avatar"])!,
                        locked = Convert.ToBoolean(dr["locked"]),
                        google_authenticate = Convert.ToBoolean(dr["google_authenticate"]),
                        sec_key = Convert.ToString(dr["sec_key"])!
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetUserRoles | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddPortalUser(PortalUsersModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_portal_user", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@role_id", model.role_id);
                    cmd.Parameters.AddWithValue("@mobile", model.mobile);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.Parameters.AddWithValue("@password", model.password);
                    cmd.Parameters.AddWithValue("@avatar", model.avatar);
                    cmd.Parameters.AddWithValue("@locked", model.locked);
                    cmd.Parameters.AddWithValue("@google_authenticate", model.google_authenticate);
                    cmd.Parameters.AddWithValue("@created_by", model.created_by);
                    cmd.Parameters.AddWithValue("@sec_key", model.sec_key);

                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddPortalUser | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdatePortalUser(PortalUsersModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_portal_user", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@role_id", model.role_id);
                    cmd.Parameters.AddWithValue("@mobile", model.mobile);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.Parameters.AddWithValue("@password", model.password);
                    cmd.Parameters.AddWithValue("@avatar", model.avatar);
                    cmd.Parameters.AddWithValue("@locked", model.locked);
                    cmd.Parameters.AddWithValue("@google_authenticate", model.google_authenticate);
                    cmd.Parameters.AddWithValue("@sec_key", model.sec_key);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdatePortalUser | Exception ->" + ex.Message);
                return false;
            }
        }
        //End Users

        //User Roles
        public List<UserRoleModel> GetUserRoles(int user_id)
        {
            List<UserRoleModel> recordlist = new List<UserRoleModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecordsById("user_allocated_roles", user_id);

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new UserRoleModel
                    {
                        id = Convert.ToInt32(dr["id"]),
                        user_id = Convert.ToInt32(dr["user_id"]),
                        role_id = Convert.ToInt32(dr["role_id"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetUserRoles | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddUserRole(UserRoleModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_user_role_mapping", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@user_id", model.user_id);
                    cmd.Parameters.AddWithValue("@role_id", model.role_id);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddUserRole | Exception ->" + ex.Message);
                return false;
            }
        }
        //End User Roles
        #endregion

        #region Settings
        // Parameters
        public List<ParametersModel> GetParameters()
        {
            List<ParametersModel> recordlist = new List<ParametersModel>();

            try
            {
                DataTable dt = new DataTable();

                dt = GetRecords("parameters");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new ParametersModel
                    {
                        id = Convert.ToInt16(dr["Id"]),
                        item_key = Convert.ToString(dr["item_key"]),
                        item_value = Convert.ToString(dr["item_value"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetParameters | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddParameter(ParametersModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("add_parameter", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@item_key", model.item_key);
                    cmd.Parameters.AddWithValue("@item_value", model.item_value);
                    cmd.Parameters.AddWithValue("@comments", model.comments);
                    //cmd.Parameters.AddWithValue("@created_by", model.created_by);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddParameter | Exception ->" + ex.Message);
                return false;
            }
        }

        public bool UpdateParameter(ParametersModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using SqlCommand cmd = new SqlCommand("update_parameter", connect);
                    connect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", model.id);
                    cmd.Parameters.AddWithValue("@item_key", model.item_key);
                    cmd.Parameters.AddWithValue("@item_value", model.item_value);
                    cmd.Parameters.AddWithValue("@comments", model.comments);
                    i = (int)cmd.ExecuteNonQuery();
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateParameter | Exception ->" + ex.Message);
                return false;
            }
        }
        //End Parameters
        #endregion

        #region Reports
        public List<ReportsModel> GetReports()
        {
            List<ReportsModel> recordlist = new List<ReportsModel>();

            try
            {
                DataTable dt = new DataTable();
                dt = GetRecords("reports");

                foreach (DataRow dr in dt.Rows)
                {
                    recordlist.Add(
                    new ReportsModel
                    {
                        id = Convert.ToInt16(dr["id"]),
                        name = Convert.ToString(dr["name"])!,
                        view_name = Convert.ToString(dr["view_name"])!,
                        enabled = Convert.ToInt16(dr["enabled"])
                    });
                }
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "GetReports | Exception ->" + ex.Message);
            }

            return recordlist;
        }

        public bool AddReport(ReportsModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using (SqlCommand cmd = new SqlCommand("add_report", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.AddWithValue("@name", model.name);
                        cmd.Parameters.AddWithValue("@view_name", model.view_name);
                        cmd.Parameters.AddWithValue("@enabled", Convert.ToBoolean(model.enabled));
                        cmd.Parameters.AddWithValue("@created_by", model.created_by);

                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "AddReport | Exception ->" + ex.Message);
                return false;
            }
        }

       

        public bool UpdateReport(ReportsModel model)
        {
            try
            {
                int i = 0;
                using (SqlConnection connect = new SqlConnection(GetDataBaseConnection(DataBaseObject.HostDB)))
                {
                    using (SqlCommand cmd = new SqlCommand("update_report", connect))
                    {
                        connect.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id", model.id);
                        cmd.Parameters.AddWithValue("@name", model.name);
                        cmd.Parameters.AddWithValue("@view_name", model.view_name);
                        cmd.Parameters.AddWithValue("@enabled", Convert.ToBoolean(model.enabled));

                        i = (int)cmd.ExecuteNonQuery();
                    }
                }

                if (i >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                FileLogHelper.log_message_fields("ERROR", "UpdateReport | Exception ->" + ex.Message);
                return false;
            }
        }
        #endregion

        






    }
}