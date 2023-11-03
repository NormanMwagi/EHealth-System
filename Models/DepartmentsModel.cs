
namespace EHealth.Models
{
    public class DepartmentsModel
    {
        public Int64 id { get; set; }
        public string? department_name { get; set; }
        public DateTime created_on { get; set; }
        public Int64 created_by { get; set; }
        public string? description { get; set; }
        public Int64 hospital_id { get; set; }
        public Int64 no_of_employees { get; set; }
    }
}
