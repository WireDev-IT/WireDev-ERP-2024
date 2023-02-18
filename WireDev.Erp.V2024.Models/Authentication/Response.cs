namespace WireDev.Erp.V1.Models.Authentication
{
    public class Response
    {
        public Response(bool success, string? message, object? data = null)
        {
            Status = success ? "Success" : "Error";
            Message = message;
            Data = data;
        }

        public string Status { get; }
        public string? Message { get; }
        public object? Data { get; }

        public static bool ConvertStatus(string status)
        {
            return status switch
            {
                "Success" => true,
                _ => false,
            };
        }
    }
}