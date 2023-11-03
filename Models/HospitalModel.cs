
namespace EHealth.Models
{
    public class HospitalModel
    {
        public Int64 id { get; set; }
        public string? hospital_name { get; set; }
        public string? phone_no { get; set; }
        public string? email { get; set; }
        public string? physical_location { get; set; }
        public DateTime created_on { get; set; }
        public Int64 created_by { get; set; }
        public bool deleted { get; set; }
        public DateTime deleted_on { get; set; }
        public Int64 deleted_by { get; set; }
        public bool approved { get; set; }
        public DateTime approved_on { get; set; }
        public Int64 approved_by { get; set; }
    }
}
