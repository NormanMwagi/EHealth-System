namespace SHERIA.Models
{
    public class MattersModel
    {
        public Int64 id { get; set; }
        public string? matter_name { get; set; }
        public string? matter_number { get; set; }
        public Int64 user_id { get; set; }
        public Int64 client_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime close_date { get; set; }
        public string? practice_area { get; set; }
        public string? matter_status { get; set; }
        public string? matter_billing { get; set; }
        public string? description { get; set; }
        public Int64 created_by { get; set; }
        public DateTime created_on { get; set; }
        public bool approved { get; set; }
        public Int64 approved_by { get; set; }
        public DateTime approved_on { get; set; }
        public bool deleted { get; set; }
        public Int64 deleted_by { get; set; }
        public DateTime deleted_on { get; set; }

    }
}
