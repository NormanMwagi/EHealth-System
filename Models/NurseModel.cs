namespace EHealth.Models
{
    public class NurseModel
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
}
