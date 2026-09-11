namespace EnhanzerAPI.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<LocationDetailDto> Locations { get; set; } = new();
    }
}
