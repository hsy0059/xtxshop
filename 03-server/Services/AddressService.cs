using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Services;

public class AddressService : IAddressService
{
    private readonly AppDbContext _context;

    public AddressService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AddressItem>> GetAddressesAsync(string userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenByDescending(a => a.CreatedAt)
            .Select(a => new AddressItem
            {
                Id = a.Id,
                Receiver = a.Receiver,
                Contact = a.Contact,
                ProvinceCode = a.ProvinceCode,
                CityCode = a.CityCode,
                CountyCode = a.CountyCode,
                FullLocation = a.FullLocation,
                Address = a.AddressDetail,
                IsDefault = a.IsDefault
            })
            .ToListAsync();
    }

    public async Task<AddressItem?> GetAddressByIdAsync(string id, string userId)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (address == null) return null;

        return new AddressItem
        {
            Id = address.Id,
            Receiver = address.Receiver,
            Contact = address.Contact,
            ProvinceCode = address.ProvinceCode,
            CityCode = address.CityCode,
            CountyCode = address.CountyCode,
            FullLocation = address.FullLocation,
            Address = address.AddressDetail,
            IsDefault = address.IsDefault
        };
    }

    public async Task<AddressItem> AddAddressAsync(string userId, AddressRequest request)
    {
        if (request.IsDefault == 1)
        {
            var defaultAddresses = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault == 1)
                .ToListAsync();
            foreach (var addr in defaultAddresses)
            {
                addr.IsDefault = 0;
            }
        }

        var address = new Address
        {
            UserId = userId,
            Receiver = request.Receiver,
            Contact = request.Contact,
            ProvinceCode = request.ProvinceCode,
            CityCode = request.CityCode,
            CountyCode = request.CountyCode,
            FullLocation = $"{request.ProvinceCode} {request.CityCode} {request.CountyCode}",
            AddressDetail = request.Address,
            IsDefault = request.IsDefault
        };

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        return new AddressItem
        {
            Id = address.Id,
            Receiver = address.Receiver,
            Contact = address.Contact,
            ProvinceCode = address.ProvinceCode,
            CityCode = address.CityCode,
            CountyCode = address.CountyCode,
            FullLocation = address.FullLocation,
            Address = address.AddressDetail,
            IsDefault = address.IsDefault
        };
    }

    public async Task<AddressItem?> UpdateAddressAsync(string id, string userId, AddressRequest request)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (address == null) return null;

        if (request.IsDefault == 1)
        {
            var defaultAddresses = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault == 1 && a.Id != id)
                .ToListAsync();
            foreach (var addr in defaultAddresses)
            {
                addr.IsDefault = 0;
            }
        }

        address.Receiver = request.Receiver;
        address.Contact = request.Contact;
        address.ProvinceCode = request.ProvinceCode;
        address.CityCode = request.CityCode;
        address.CountyCode = request.CountyCode;
        address.FullLocation = $"{request.ProvinceCode} {request.CityCode} {request.CountyCode}";
        address.AddressDetail = request.Address;
        address.IsDefault = request.IsDefault;
        address.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return new AddressItem
        {
            Id = address.Id,
            Receiver = address.Receiver,
            Contact = address.Contact,
            ProvinceCode = address.ProvinceCode,
            CityCode = address.CityCode,
            CountyCode = address.CountyCode,
            FullLocation = address.FullLocation,
            Address = address.AddressDetail,
            IsDefault = address.IsDefault
        };
    }

    public async Task<bool> DeleteAddressAsync(string id, string userId)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (address == null) return false;

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }
}
