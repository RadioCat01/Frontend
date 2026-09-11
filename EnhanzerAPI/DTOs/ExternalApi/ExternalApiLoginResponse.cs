namespace EnhanzerAPI.DTOs.ExternalApi
{
    public class ExternalApiLoginResponse
    {
        public int Status_Code { get; set; }
        public string Sync_Time { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<UserData> Response_Body { get; set; } = new();
    }

    public class UserData
    {
        public List<UserLocation> User_Locations { get; set; } = new();
    }

    public class UserLocation
    {
        public string Location_Code { get; set; } = string.Empty;
        public string Location_Name { get; set; } = string.Empty;
    }
}
