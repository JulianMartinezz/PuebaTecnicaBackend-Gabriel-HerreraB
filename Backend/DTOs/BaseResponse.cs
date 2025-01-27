namespace Backend.DTOs
{
     // Represents a standard response structure for system operations, 
     // including information about the operation's success, message, returned data, 
     // HTTP status code, and total rows (used for pagination).
    public class BaseResponse<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int? Code { get; set; }
        public int? TotalRows { get; set; }
        public string? Exception { get; set; }
    }

}
