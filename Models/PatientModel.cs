namespace EHealth.Models
{
    public class PatientModel
    {
        public Int64 id { get; set; }
        public Int64 hospital_id { get; set; }
        public Int64 department_id { get; set; }
        public Int64 doctor_id { get; set; }
        public string? name { get; set; }
        public Int64 age { get; set; }
        public string? residency { get; set; }
        public string? phone_no { get; set; }
        public string insurance { get; set; } = default !;
        public Int64? insurance_no { get; set; }
        public string? health_issue { get; set; }
        public DateTime book_appointment_date { get; set; }

    }
}
