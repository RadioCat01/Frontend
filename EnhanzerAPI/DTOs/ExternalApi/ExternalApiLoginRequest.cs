namespace EnhanzerAPI.DTOs.ExternalApi
{
    public class ExternalApiLoginRequest
    {
        public string API_Action { get; set; } = string.Empty;
        public string Device_Id { get; set; } = string.Empty;
        public string Sync_Time { get; set; } = string.Empty;
        public string Company_Code { get; set; } = string.Empty;
        public ApiBody API_Body { get; set; } = new();
    }

    public class ApiBody
    {
        public string Username { get; set; } = string.Empty;
        public string Pw { get; set; } = string.Empty;
    }
}
