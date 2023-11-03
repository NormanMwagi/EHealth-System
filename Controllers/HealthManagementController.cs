using EHealth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SHERIA.Helpers;
using SHERIA.Models;
using System.Collections;
using System.Data;
using static SHERIA.Controllers.ClientManagementController;

namespace EHealth.Controllers
{
    public class HealthManagementController : Controller
    {
        private IWebHostEnvironment ihostingenvironment;
        private ILoggerManager iloggermanager;
        private DBHandler dbhandler;
        public HealthManagementController(ILoggerManager logger, IWebHostEnvironment environment, DBHandler mydbhandler)
        {
            iloggermanager = logger;
            ihostingenvironment = environment;
            dbhandler = mydbhandler;
        }
        public class doctorsrecord
        {
            public Int64 id { get; set; }
            public string? name { get; set; }
            public Int64? department_id { get; set; }
            public string? phone_no { get; set; }
            public string? email { get; set; }
            public Int64 age { get; set; }
            public string? gender { get; set; }
            public Int64 hospital_id { get; set; }
        }
        public class hospitalrecord
        {
            public Int64 id { get; set; }
            public string? hospital_name { get; set; }
            public string? phone_no { get; set; }
            public string? email { get; set; }
            public string? physical_location { get; set; }
        }
        public class departmentrecord
        {
            public Int64 id { get; set; }
            public string? department_name { get; set; }
            public string? description { get; set; }
            public Int64 hospital_id { get; set; }
            public Int64 no_of_employees { get; set; }
        }
        public class nurserecord
        {
            public Int64 id { get; set; }
            public string? name { get; set; }
            public string? gender { get; set; }
            public Int64 age { get; set; }
            public string? phone_no { get; set; }
            public string? email { get; set; }
            public Int64 hospital_id { get; set; }
            public Int64 department_id { get; set; }
        }
        public class patientrecord
        {
            public Int64 id { get; set; }
            public Int64 hospital_id { get; set; }
            public Int64 department_id { get; set; }
            public Int64 doctor_id { get; set; }
            public string? name { get; set; }
            public Int64 age { get; set; }
            public string? residency { get; set; }
            public string? phone_no { get; set; }
            public string? insurance { get; set; }
            public Int64? insurance_no { get; set; }
            public string? health_issue { get; set; }
            public DateTime book_appointment_date { get; set; }
           
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Doctors()
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                ViewBag.MenuLayout = HttpContext.Session.GetString("menulayout");
                MenuHandler menuhandler = new MenuHandler(dbhandler);
                IEnumerable<MenuModel> menu = menuhandler.GetMenu(Convert.ToInt16(HttpContext.Session.GetString("profileid")), HttpContext.Request.Path);
                return View(menu);
            }
        }

        [HttpGet]
        public ContentResult GetRecords(string module, string param = "normal")
        {
            ArrayList details = new ArrayList();
            DataTable datatable = new DataTable();
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            if (param == "unapproved")
                datatable = dbhandler.GetUnapprovedRecords(module);
            else
            {
                if (param.Contains("|"))
                {
                    string[] parameters = param.Split('|');
                    switch (parameters.Length)
                    {
                        case 1:
                            datatable = dbhandler.GetRecords(module, parameters[0]);
                            break;

                        case 2:
                            datatable = dbhandler.GetRecords(module, parameters[0], parameters[1]);
                            break;

                        default:
                            datatable = dbhandler.GetRecords(module);
                            break;
                    }
                }
                else
                {
                    switch (module)
                    {
                        case "roles":
                            datatable = dbhandler.GetRecords(module);
                            break;

                        case "role_unallocated_permissions":
                        case "role_allocated_permissions":
                        case "user_unallocated_roles":
                        case "user_allocated_roles":
                            datatable = dbhandler.GetRecordsById(module, Convert.ToInt16(param));
                            break;

                        default:
                            datatable = dbhandler.GetRecords(module);
                            break;
                    }
                }
            }

            if (datatable.Rows.Count > 0)
            {
                Dictionary<string, object> row;
                foreach (DataRow dr in datatable.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in datatable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
            }
            return Content(JsonConvert.SerializeObject(rows, Formatting.Indented) /*serializer.Serialize(rows)*/, "application/json");
        }

        [HttpPost]
        public ActionResult CreateDoctors(doctorsrecord record)
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                if (record.name == null)
                    return Content("Invalid name");
                if (record.department_id == null)
                    return Content("Invalid name");

                try
                {
                    DoctorsModel existingrecord = dbhandler.GetDoctors().Find(mymodel => mymodel.id == record.id)!;
                    if (existingrecord != null)
                    {
                        DoctorsModel mymodel = new DoctorsModel
                        {
                            id = existingrecord.id,
                            name = record.name,
                            department_id = record.department_id,
                            phone_no = record.phone_no,
                            email = record.email,
                            age = record.age,
                            gender = record.gender,
                            hospital_id = record.hospital_id,

                        };

                        if (dbhandler.UpdateDoctors(mymodel))
                        {
                            // CaptureAuditTrail("Updated name", "name: " + mymodel.name);

                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not update DOCTOR, kindly contact system admin");
                    }
                    else
                    {
                        DoctorsModel mymodel = new DoctorsModel
                        {

                            name = record.name,
                            department_id = record.department_id,
                            phone_no = record.phone_no,
                            email = record.email,
                            age = record.age,
                            gender = record.gender,
                            hospital_id = record.hospital_id,

                        };

                        if (dbhandler.AddDoctors(mymodel))
                        {
                            // CaptureAuditTrail("Created topic", "Created topic: " + mymodel.name);
                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not create topic, kindly contact system admin");
                    }
                }
                catch
                {
                    return Content("Could not create topic, kindly contact system admin");
                }
            }
        }


        [RBAC]
        public ActionResult Delete(/*[FromBody] JObject jobject*/int id, string module)
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                //Int32 id = Convert.ToInt32(jobject["id"]);
                //string module = jobject["module"].ToString();

                switch (module)
                {
                    case "roles":
                        RolesModel profilesmodel = dbhandler.GetRoles().Find(mymodel => mymodel.id == id)!;
                        if (profilesmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted role", "Deleted role: " + profilesmodel.role_name);
                        }
                        break;

                    case "portal_users":
                        PortalUsersModel usersmodel = dbhandler.GetPortalUsers().Find(mymodel => mymodel.id == id)!;
                        if (usersmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted user", "Deleted user: " + usersmodel.name);
                        }
                        break;

                    case "permissions":
                        PermissionsModel permissionsmodel = dbhandler.GetPermissions().Find(mymodel => mymodel.id == id)!;
                        if (permissionsmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted permission", "Deleted permission: " + permissionsmodel.permission_name);
                        }
                        break;

                    case "hospital_record":
                        HospitalModel hospitalmodel = dbhandler.GetHospital().Find(mymodel => mymodel.id == id)!;
                        if (hospitalmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted client", "Deleted client: " + clientrecordmodel.client_name);
                        }
                        break;

                    case "department_record":
                        DepartmentsModel departmentsmodel = dbhandler.GetDepartment().Find(mymodel => mymodel.id == id)!;
                        if (departmentsmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted client", "Deleted client: " + clientrecordmodel.client_name);
                        }
                        break;

                    case "doctors_record":
                        DoctorsModel doctorsmodel = dbhandler.GetDoctors().Find(mymodel => mymodel.id == id)!;
                        if (doctorsmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted client", "Deleted client: " + clientrecordmodel.client_name);
                        }
                        break;

                    case "nurse_record":
                        NurseModel nursemodel = dbhandler.GetNurse().Find(mymodel => mymodel.id == id)!;
                        if (nursemodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted client", "Deleted client: " + clientrecordmodel.client_name);
                        }
                        break;

                    case "patient_record":
                        PatientModel patientmodel = dbhandler.GetPatient().Find(mymodel => mymodel.id == id)!;
                        if (patientmodel != null)
                        {
                            dbhandler.DeleteRecord(id, Convert.ToInt16(HttpContext.Session.GetString("userid")), module);
                            //CaptureAuditTrail("Deleted client", "Deleted client: " + clientrecordmodel.client_name);
                        }
                        break;



                    default:
                        break;
                }

                return GetRecords(module);
            }
        }

        [HttpPost]
        public ActionResult CreateHospital(hospitalrecord record)
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                if (record.hospital_name == null)
                    return Content("Invalid name");
                if (record.phone_no == null)
                    return Content("Invalid name");

                try
                {
                    HospitalModel existingrecord = dbhandler.GetHospital().Find(mymodel => mymodel.id == record.id)!;
                    if (existingrecord != null)
                    {
                        HospitalModel mymodel = new HospitalModel
                        {
                            id = existingrecord.id,
                            hospital_name = record.hospital_name,
                            phone_no = record.phone_no,
                            email = record.email,
                            physical_location = record.physical_location,
                            
                        };

                        if (dbhandler.UpdateHospital(mymodel))
                        {
                            // CaptureAuditTrail("Updated name", "name: " + mymodel.name);

                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not update Hospital, kindly contact system admin");
                    }
                    else
                    {
                        HospitalModel mymodel = new HospitalModel
                        {

                            hospital_name = record.hospital_name,
                            phone_no = record.phone_no,
                            email = record.email,
                            physical_location = record.physical_location,
                            created_by = Convert.ToInt64( HttpContext.Session.GetString("userid"))

                            
                        };

                        if (dbhandler.AddHospital(mymodel))
                        {
                            // CaptureAuditTrail("Created topic", "Created topic: " + mymodel.name);
                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not create topic, kindly contact system admin");
                    }
                }
                catch
                {
                    return Content("Could not create topic, kindly contact system admin");
                }
            }
        }

        public IActionResult Hospital()
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                ViewBag.MenuLayout = HttpContext.Session.GetString("menulayout");
                MenuHandler menuhandler = new MenuHandler(dbhandler);
                IEnumerable<MenuModel> menu = menuhandler.GetMenu(Convert.ToInt16(HttpContext.Session.GetString("profileid")), HttpContext.Request.Path);
                return View(menu);
            }
        }

        public IActionResult Department()
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                ViewBag.MenuLayout = HttpContext.Session.GetString("menulayout");
                MenuHandler menuhandler = new MenuHandler(dbhandler);
                IEnumerable<MenuModel> menu = menuhandler.GetMenu(Convert.ToInt16(HttpContext.Session.GetString("profileid")), HttpContext.Request.Path);
                return View(menu);
            }
        }

        [HttpPost]
        public ActionResult CreateDepartment(departmentrecord record)
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                if (record.department_name == null)
                    return Content("Invalid name");
                if (record.description == null)
                    return Content("Invalid name");

                try
                {
                    DepartmentsModel existingrecord = dbhandler.GetDepartment().Find(mymodel => mymodel.id == record.id)!;
                    if (existingrecord != null)
                    {
                        DepartmentsModel mymodel = new DepartmentsModel
                        {
                            id = existingrecord.id,
                            department_name = record.department_name,
                            description = record.description,
                            hospital_id = record.hospital_id,
                            no_of_employees = record.no_of_employees,

                        };

                        if (dbhandler.UpdateDepartment(mymodel))
                        {
                            // CaptureAuditTrail("Updated name", "name: " + mymodel.name);

                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not update Hospital, kindly contact system admin");
                    }
                    else
                    {
                        DepartmentsModel mymodel = new DepartmentsModel
                        {

                            department_name = record.department_name,
                            description = record.description,
                            hospital_id = record.hospital_id,
                            no_of_employees = record.no_of_employees,
                            created_by = Convert.ToInt64(HttpContext.Session.GetString("userid"))

                        };

                        if (dbhandler.AddDepartment(mymodel))
                        {
                            // CaptureAuditTrail("Created topic", "Created topic: " + mymodel.name);
                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not create topic, kindly contact system admin");
                    }
                }
                catch
                {
                    return Content("Could not create topic, kindly contact system admin");
                }
            }
        }

        public IActionResult Nurse()
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                ViewBag.MenuLayout = HttpContext.Session.GetString("menulayout");
                MenuHandler menuhandler = new MenuHandler(dbhandler);
                IEnumerable<MenuModel> menu = menuhandler.GetMenu(Convert.ToInt16(HttpContext.Session.GetString("profileid")), HttpContext.Request.Path);
                return View(menu);
            }
        }


        [HttpPost]
        public ActionResult CreateNurse(nurserecord record)
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                if (record.name == null)
                    return Content("Invalid name");
                if (record.gender == null)
                    return Content("Invalid name");

                try
                {
                    NurseModel existingrecord = dbhandler.GetNurse().Find(mymodel => mymodel.id == record.id)!;
                    if (existingrecord != null)
                    {
                        NurseModel mymodel = new NurseModel
                        {
                            id = existingrecord.id,
                            name = record.name,
                            gender = record.gender,
                            age = record.age,
                            phone_no = record.phone_no,
                            email = record.email,
                            hospital_id = record.hospital_id,
                            department_id = record.department_id,

                        };

                        if (dbhandler.UpdateNurse(mymodel))
                        {
                            // CaptureAuditTrail("Updated name", "name: " + mymodel.name);

                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not update NURSE, kindly contact system admin");
                    }
                    else
                    {
                        NurseModel mymodel = new NurseModel
                        {

                            name = record.name,
                            gender = record.gender,
                            age = record.age,
                            phone_no = record.phone_no,
                            email = record.email,
                            hospital_id = record.hospital_id,
                            department_id = record.department_id,


                        };

                        if (dbhandler.AddNurse(mymodel))
                        {
                            // CaptureAuditTrail("Created topic", "Created topic: " + mymodel.name);
                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not create topic, kindly contact system admin");
                    }
                }
                catch
                {
                    return Content("Could not create topic, kindly contact system admin");
                }
            }
        }

        public IActionResult Patient()
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                ViewBag.MenuLayout = HttpContext.Session.GetString("menulayout");
                MenuHandler menuhandler = new MenuHandler(dbhandler);
                IEnumerable<MenuModel> menu = menuhandler.GetMenu(Convert.ToInt16(HttpContext.Session.GetString("profileid")), HttpContext.Request.Path);
                return View(menu);
            }
        }

        [HttpPost]
        public ActionResult CreatePatient(patientrecord record)
        {
            if (HttpContext.Session.GetString("name") == null)
                return RedirectToAction("AdminLogin", "AppAuth");
            else
            {
                if (record.name == null)
                    return Content("Invalid name");
                if (record.age == null)
                    return Content("Invalid name");

                try
                {
                    PatientModel existingrecord = dbhandler.GetPatient().Find(mymodel => mymodel.id == record.id)!;
                    if (existingrecord != null)
                    {
                        PatientModel mymodel = new PatientModel
                        {
                            id = existingrecord.id,
                            hospital_id = record.hospital_id,
                            department_id = record.department_id,
                            doctor_id = record.doctor_id,
                            name = record.name,
                            age = record.age,
                            residency = record.residency,
                            phone_no = record.phone_no,
                            insurance = record.insurance,
                            insurance_no = record.insurance_no,
                            health_issue = record.health_issue,
                            book_appointment_date = record.book_appointment_date,
                            

                        };

                        if (dbhandler.UpdatePatient(mymodel))
                        {
                            // CaptureAuditTrail("Updated name", "name: " + mymodel.name);

                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not update NURSE, kindly contact system admin");
                    }
                    else
                    {
                        PatientModel mymodel = new PatientModel
                        {
                            hospital_id = record.hospital_id,
                            department_id = record.department_id,
                            doctor_id = record.doctor_id,
                            name = record.name,
                            age = record.age,
                            residency = record.residency,
                            phone_no = record.phone_no,
                            insurance = record.insurance,
                            insurance_no = record.insurance_no,
                            health_issue = record.health_issue,
                            book_appointment_date = record.book_appointment_date,


                        };

                        if (dbhandler.AddPatient(mymodel))
                        {
                            // CaptureAuditTrail("Created topic", "Created topic: " + mymodel.name);
                            ModelState.Clear();
                            return Content("Success");
                        }
                        else
                            return Content("Could not create topic, kindly contact system admin");
                    }
                }
                catch
                {
                    return Content("Could not create topic, kindly contact system admin");
                }
            }
        }


    }
}
