using XtxServer.DTOs;

namespace XtxServer.Services;

public interface IAddressService
{
    Task<List<AddressItem>> GetAddressesAsync(string userId);
    Task<AddressItem?> GetAddressByIdAsync(string id, string userId);
    Task<AddressItem> AddAddressAsync(string userId, AddressRequest request);
    Task<AddressItem?> UpdateAddressAsync(string id, string userId, AddressRequest request);
    Task<bool> DeleteAddressAsync(string id, string userId);
}
