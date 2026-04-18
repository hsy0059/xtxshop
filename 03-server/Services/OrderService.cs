using Microsoft.EntityFrameworkCore;
using XtxServer.Data;
using XtxServer.DTOs;
using XtxServer.Entities;

namespace XtxServer.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderPreResult> GetOrderPreAsync(string userId)
    {
        var cartItems = await _context.CartItems
            .Include(c => c.Sku)
            .Include(c => c.Goods)
            .Where(c => c.UserId == userId && c.Selected)
            .ToListAsync();

        var goods = cartItems.Select(c => new OrderPreGoods
        {
            Id = c.GoodsId ?? string.Empty,
            Name = c.GoodsName ?? c.Goods?.Name ?? string.Empty,
            Picture = c.Picture ?? string.Empty,
            Count = c.Count,
            SkuId = c.SkuId,
            AttrsText = c.AttrsText ?? string.Empty,
            Price = c.Price.ToString("F2"),
            PayPrice = c.NowPrice.ToString("F2"),
            TotalPrice = (c.Price * c.Count).ToString("F2"),
            TotalPayPrice = (c.NowPrice * c.Count).ToString("F2")
        }).ToList();

        var totalPrice = cartItems.Sum(c => c.Price * c.Count);
        var postFee = totalPrice >= 99 ? 0 : 5;

        var addresses = await _context.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
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

        return new OrderPreResult
        {
            Goods = goods,
            Summary = new OrderSummary
            {
                TotalPrice = totalPrice,
                PostFee = postFee,
                TotalPayPrice = totalPrice + postFee
            },
            UserAddresses = addresses
        };
    }

    public async Task<OrderPreResult> GetOrderPreNowAsync(string userId, OrderPreNowRequest request)
    {
        var sku = await _context.Skus
            .Include(s => s.Goods)
            .FirstOrDefaultAsync(s => s.Id == request.SkuId);

        if (sku == null) throw new Exception("SKU不存在");

        var count = int.Parse(request.Count);
        var goods = new List<OrderPreGoods>
        {
            new OrderPreGoods
            {
                Id = sku.GoodsId,
                Name = sku.Goods?.Name ?? string.Empty,
                Picture = sku.Picture ?? sku.Goods?.MainPicture ?? string.Empty,
                Count = count,
                SkuId = sku.Id,
                AttrsText = string.Empty,
                Price = sku.Price.ToString("F2"),
                PayPrice = sku.Price.ToString("F2"),
                TotalPrice = (sku.Price * count).ToString("F2"),
                TotalPayPrice = (sku.Price * count).ToString("F2")
            }
        };

        var totalPrice = sku.Price * count;
        var postFee = totalPrice >= 99 ? 0 : 5;

        var addresses = await _context.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
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

        return new OrderPreResult
        {
            Goods = goods,
            Summary = new OrderSummary
            {
                TotalPrice = totalPrice,
                PostFee = postFee,
                TotalPayPrice = totalPrice + postFee
            },
            UserAddresses = addresses
        };
    }

    public async Task<OrderPreResult> GetOrderRepurchaseAsync(string userId, string orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null) throw new Exception("订单不存在");

        var goods = new List<OrderPreGoods>();
        foreach (var item in order.Items)
        {
            goods.Add(new OrderPreGoods
            {
                Id = item.SpuId ?? string.Empty,
                Name = item.Name ?? string.Empty,
                Picture = item.Image ?? string.Empty,
                Count = item.Quantity,
                SkuId = item.SkuId,
                AttrsText = item.AttrsText ?? string.Empty,
                Price = item.CurPrice.ToString("F2"),
                PayPrice = item.CurPrice.ToString("F2"),
                TotalPrice = (item.CurPrice * item.Quantity).ToString("F2"),
                TotalPayPrice = (item.CurPrice * item.Quantity).ToString("F2")
            });
        }

        var totalPrice = order.Items.Sum(i => i.CurPrice * i.Quantity);
        var postFee = totalPrice >= 99 ? 0 : 5;

        var addresses = await _context.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
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

        return new OrderPreResult
        {
            Goods = goods,
            Summary = new OrderSummary
            {
                TotalPrice = totalPrice,
                PostFee = postFee,
                TotalPayPrice = totalPrice + postFee
            },
            UserAddresses = addresses
        };
    }

    public async Task<OrderCreateResult> CreateOrderAsync(string userId, OrderCreateRequest request)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == request.AddressId && a.UserId == userId);

        if (address == null) throw new Exception("地址不存在");

        var totalMoney = 0m;
        var orderItems = new List<OrderItem>();

        foreach (var goods in request.Goods)
        {
            var sku = await _context.Skus
                .Include(s => s.Goods)
                .FirstOrDefaultAsync(s => s.Id == goods.SkuId);

            if (sku == null) continue;

            totalMoney += sku.Price * goods.Count;

            orderItems.Add(new OrderItem
            {
                SkuId = goods.SkuId,
                SpuId = sku.GoodsId,
                Name = sku.Goods?.Name,
                AttrsText = string.Empty,
                Quantity = goods.Count,
                CurPrice = sku.Price,
                Image = sku.Picture ?? sku.Goods?.MainPicture
            });
        }

        var postFee = totalMoney >= 99 ? 0 : 5;

        var order = new Order
        {
            UserId = userId,
            OrderState = 1,
            ReceiverContact = address.Receiver,
            ReceiverMobile = address.Contact,
            ReceiverAddress = $"{address.FullLocation} {address.AddressDetail}",
            DeliveryTimeType = request.DeliveryTimeType,
            BuyerMessage = request.BuyerMessage,
            PayType = request.PayType,
            PayChannel = request.PayChannel,
            TotalMoney = totalMoney,
            PostFee = postFee,
            PayMoney = totalMoney + postFee,
            Items = orderItems,
            Countdown = 1800
        };

        _context.Orders.Add(order);

        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId && c.Selected)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);

        await _context.SaveChangesAsync();

        return new OrderCreateResult { Id = order.Id };
    }

    public async Task<OrderResult?> GetOrderByIdAsync(string userId, string orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null) return null;

        var countdown = order.OrderState == 1 && order.CreateTime.AddMinutes(30) > DateTime.Now
            ? (int)(order.CreateTime.AddMinutes(30) - DateTime.Now).TotalSeconds
            : -1;

        return new OrderResult
        {
            Id = order.Id,
            OrderState = order.OrderState,
            Countdown = countdown,
            Skus = order.Items.Select(i => new OrderSkuItem
            {
                Id = i.Id,
                SpuId = i.SpuId ?? string.Empty,
                Name = i.Name ?? string.Empty,
                AttrsText = i.AttrsText ?? string.Empty,
                Quantity = i.Quantity,
                CurPrice = i.CurPrice,
                Image = i.Image ?? string.Empty
            }).ToList(),
            ReceiverContact = order.ReceiverContact ?? string.Empty,
            ReceiverMobile = order.ReceiverMobile ?? string.Empty,
            ReceiverAddress = order.ReceiverAddress ?? string.Empty,
            CreateTime = order.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            TotalMoney = order.TotalMoney,
            PostFee = order.PostFee,
            PayMoney = order.PayMoney
        };
    }

    public async Task<OrderListResult> GetOrderListAsync(string userId, OrderListParams request)
    {
        var query = _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId);

        if (request.OrderState > 0)
        {
            query = query.Where(o => o.OrderState == request.OrderState);
        }

        var total = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.CreateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var items = orders.Select(o =>
        {
            var countdown = o.OrderState == 1 && o.CreateTime.AddMinutes(30) > DateTime.Now
                ? (int)(o.CreateTime.AddMinutes(30) - DateTime.Now).TotalSeconds
                : -1;

            return new OrderItemDto
            {
                Id = o.Id,
                OrderState = o.OrderState,
                Countdown = countdown,
                Skus = o.Items.Select(i => new OrderSkuItem
                {
                    Id = i.Id,
                    SpuId = i.SpuId ?? string.Empty,
                    Name = i.Name ?? string.Empty,
                    AttrsText = i.AttrsText ?? string.Empty,
                    Quantity = i.Quantity,
                    CurPrice = i.CurPrice,
                    Image = i.Image ?? string.Empty
                }).ToList(),
                ReceiverContact = o.ReceiverContact ?? string.Empty,
                ReceiverMobile = o.ReceiverMobile ?? string.Empty,
                ReceiverAddress = o.ReceiverAddress ?? string.Empty,
                CreateTime = o.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                TotalMoney = o.TotalMoney,
                PostFee = o.PostFee,
                PayMoney = o.PayMoney,
                TotalNum = o.Items.Sum(i => i.Quantity)
            };
        }).ToList();

        return new OrderListResult
        {
            Counts = total,
            Items = items,
            Page = request.Page,
            Pages = (int)Math.Ceiling((double)total / request.PageSize),
            PageSize = request.PageSize
        };
    }

    public async Task<OrderResult?> ConfirmReceiptAsync(string userId, string orderId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null || order.OrderState != 3) return null;

        order.OrderState = 4;
        order.ReceiveTime = DateTime.Now;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetOrderByIdAsync(userId, orderId);
    }

    public async Task<OrderResult?> CancelOrderAsync(string userId, string orderId, OrderCancelRequest request)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null || order.OrderState != 1) return null;

        order.OrderState = 6;
        order.CancelReason = request.CancelReason;
        order.CancelTime = DateTime.Now;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetOrderByIdAsync(userId, orderId);
    }

    public async Task<bool> DeleteOrderAsync(string userId, OrderDeleteRequest request)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId && request.Ids.Contains(o.Id))
            .ToListAsync();

        foreach (var order in orders)
        {
            if (order.OrderState != 4 && order.OrderState != 5 && order.OrderState != 6)
            {
                continue;
            }
            _context.Orders.Remove(order);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<OrderLogisticResult?> GetOrderLogisticsAsync(string userId, string orderId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null || (order.OrderState != 3 && order.OrderState != 4 && order.OrderState != 5))
            return null;

        return new OrderLogisticResult
        {
            Company = new LogisticCompany
            {
                Name = "顺丰速运",
                Number = $"SF{order.Id[..8].ToUpper()}",
                Tel = "95338"
            },
            Count = order.Items.Sum(i => i.Quantity),
            List = new List<LogisticItem>
            {
                new LogisticItem
                {
                    Id = "1",
                    Text = "您的订单已签收，感谢使用",
                    Time = order.ReceiveTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                new LogisticItem
                {
                    Id = "2",
                    Text = "您的订单正在派送中",
                    Time = order.ShipTime?.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss")
                },
                new LogisticItem
                {
                    Id = "3",
                    Text = "您的订单已发货",
                    Time = order.ShipTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss")
                }
            }
        };
    }

    public async Task<OrderResult?> SimulateConsignmentAsync(string userId, string orderId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null || order.OrderState != 2) return null;

        order.OrderState = 3;
        order.ShipTime = DateTime.Now;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return await GetOrderByIdAsync(userId, orderId);
    }
}
