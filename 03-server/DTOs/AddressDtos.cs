namespace XtxServer.DTOs;

public class AddressRequest
{
    public string Receiver { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string ProvinceCode { get; set; } = string.Empty;
    public string CityCode { get; set; } = string.Empty;
    public string CountyCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int IsDefault { get; set; } = 0;
}

public class AddressItem
{
    public string Id { get; set; } = string.Empty;
    public string Receiver { get; set; } = string.Empty;
    public string Contact { get; set; } = string.Empty;
    public string ProvinceCode { get; set; } = string.Empty;
    public string CityCode { get; set; } = string.Empty;
    public string CountyCode { get; set; } = string.Empty;
    public string FullLocation { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int IsDefault { get; set; } = 0;
}
