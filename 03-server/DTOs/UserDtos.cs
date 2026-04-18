namespace XtxServer.DTOs;

public class LoginRequest
{
    public string Code { get; set; } = string.Empty;
    public string? EncryptedData { get; set; }
    public string? Iv { get; set; }
}

public class SimpleLoginRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
}

public class LoginResult
{
    public string Id { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Avatar { get; set; }
    public string Account { get; set; } = string.Empty;
}

public class ProfileDetail
{
    public string Id { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Nickname { get; set; }
    public string Account { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? Birthday { get; set; }
    public string FullLocation { get; set; } = string.Empty;
    public string? Profession { get; set; }
}

public class ProfileUpdateRequest
{
    public string? Nickname { get; set; }
    public string? Gender { get; set; }
    public string? Birthday { get; set; }
    public string? Profession { get; set; }
    public string? ProvinceCode { get; set; }
    public string? CityCode { get; set; }
    public string? CountyCode { get; set; }
}
