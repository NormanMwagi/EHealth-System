namespace SHERIA.Models
{
    public class TasksModel
    {
        public Int64 id { get; set; }
        public string? task_name { get; set; }
        public Int64? matter_id { get; set; }
        public DateTime due_date { get; set; }
        public Int64 user_id { get; set; }
        public string? priority { get; set; }
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
