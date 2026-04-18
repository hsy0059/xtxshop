using XtxServer.DTOs;

namespace XtxServer.Services;

public interface IOrderService
{
    Task<OrderPreResult> GetOrderPreAsync(string userId);
    Task<OrderPreResult> GetOrderPreNowAsync(string userId, OrderPreNowRequest request);
    Task<OrderPreResult> GetOrderRepurchaseAsync(string userId, string orderId);
    Task<OrderCreateResult> CreateOrderAsync(string userId, OrderCreateRequest request);
    Task<OrderResult?> GetOrderByIdAsync(string userId, string orderId);
    Task<OrderListResult> GetOrderListAsync(string userId, OrderListParams request);
    Task<OrderResult?> ConfirmReceiptAsync(string userId, string orderId);
    Task<OrderResult?> CancelOrderAsync(string userId, string orderId, OrderCancelRequest request);
    Task<bool> DeleteOrderAsync(string userId, OrderDeleteRequest request);
    Task<OrderLogisticResult?> GetOrderLogisticsAsync(string userId, string orderId);
    Task<OrderResult?> SimulateConsignmentAsync(string userId, string orderId);
}
