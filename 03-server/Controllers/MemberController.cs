using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XtxServer.DTOs;
using XtxServer.Services;

namespace XtxServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAddressService _addressService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IPayService _payService;

    public MemberController(
        IUserService userService,
        IAddressService addressService,
        ICartService cartService,
        IOrderService orderService,
        IPayService payService)
    {
        _userService = userService;
        _addressService = addressService;
        _cartService = cartService;
        _orderService = orderService;
        _payService = payService;
    }

    private string GetUserId()
    {
        return HttpContext.Items["UserId"]?.ToString() ?? string.Empty;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<ProfileDetail>>> GetProfile()
    {
        var userId = GetUserId();
        var result = await _userService.GetProfileAsync(userId);
        if (result == null)
        {
            return NotFound(ApiResponse<ProfileDetail>.Error("用户不存在"));
        }
        return Ok(ApiResponse<ProfileDetail>.Success(result));
    }

    [HttpPut("profile")]
    public async Task<ActionResult<ApiResponse<ProfileDetail>>> UpdateProfile([FromBody] ProfileUpdateRequest request)
    {
        var userId = GetUserId();
        var result = await _userService.UpdateProfileAsync(userId, request);
        if (result == null)
        {
            return NotFound(ApiResponse<ProfileDetail>.Error("用户不存在"));
        }
        return Ok(ApiResponse<ProfileDetail>.Success(result, "修改成功"));
    }

    [HttpGet("address")]
    public async Task<ActionResult<ApiResponse<List<AddressItem>>>> GetAddresses()
    {
        var userId = GetUserId();
        var result = await _addressService.GetAddressesAsync(userId);
        return Ok(ApiResponse<List<AddressItem>>.Success(result));
    }

    [HttpGet("address/{id}")]
    public async Task<ActionResult<ApiResponse<AddressItem>>> GetAddress(string id)
    {
        var userId = GetUserId();
        var result = await _addressService.GetAddressByIdAsync(id, userId);
        if (result == null)
        {
            return NotFound(ApiResponse<AddressItem>.Error("地址不存在"));
        }
        return Ok(ApiResponse<AddressItem>.Success(result));
    }

    [HttpPost("address")]
    public async Task<ActionResult<ApiResponse<AddressItem>>> AddAddress([FromBody] AddressRequest request)
    {
        var userId = GetUserId();
        var result = await _addressService.AddAddressAsync(userId, request);
        return Ok(ApiResponse<AddressItem>.Success(result, "添加成功"));
    }

    [HttpPut("address/{id}")]
    public async Task<ActionResult<ApiResponse<AddressItem>>> UpdateAddress(string id, [FromBody] AddressRequest request)
    {
        var userId = GetUserId();
        var result = await _addressService.UpdateAddressAsync(id, userId, request);
        if (result == null)
        {
            return NotFound(ApiResponse<AddressItem>.Error("地址不存在"));
        }
        return Ok(ApiResponse<AddressItem>.Success(result, "修改成功"));
    }

    [HttpDelete("address/{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteAddress(string id)
    {
        var userId = GetUserId();
        var result = await _addressService.DeleteAddressAsync(id, userId);
        if (!result)
        {
            return NotFound(ApiResponse.Error("地址不存在"));
        }
        return Ok(ApiResponse.Success("删除成功"));
    }

    [HttpGet("cart")]
    public async Task<ActionResult<ApiResponse<List<CartItemDto>>>> GetCart()
    {
        var userId = GetUserId();
        var result = await _cartService.GetCartAsync(userId);
        return Ok(ApiResponse<List<CartItemDto>>.Success(result));
    }

    [HttpPost("cart")]
    public async Task<ActionResult<ApiResponse>> AddToCart([FromBody] CartAddRequest request)
    {
        var userId = GetUserId();
        await _cartService.AddToCartAsync(userId, request);
        return Ok(ApiResponse.Success("添加成功"));
    }

    [HttpPut("cart/{skuId}")]
    public async Task<ActionResult<ApiResponse>> UpdateCartItem(string skuId, [FromBody] CartUpdateRequest request)
    {
        var userId = GetUserId();
        await _cartService.UpdateCartItemAsync(userId, skuId, request);
        return Ok(ApiResponse.Success("修改成功"));
    }

    [HttpDelete("cart")]
    public async Task<ActionResult<ApiResponse>> DeleteCartItems([FromBody] CartDeleteRequest request)
    {
        var userId = GetUserId();
        await _cartService.DeleteCartItemsAsync(userId, request);
        return Ok(ApiResponse.Success("删除成功"));
    }

    [HttpPut("cart/selected")]
    public async Task<ActionResult<ApiResponse>> UpdateCartSelected([FromBody] CartSelectedRequest request)
    {
        var userId = GetUserId();
        await _cartService.UpdateCartSelectedAsync(userId, request);
        return Ok(ApiResponse.Success("修改成功"));
    }

    [HttpGet("order/pre")]
    public async Task<ActionResult<ApiResponse<OrderPreResult>>> GetOrderPre()
    {
        var userId = GetUserId();
        var result = await _orderService.GetOrderPreAsync(userId);
        return Ok(ApiResponse<OrderPreResult>.Success(result));
    }

    [HttpGet("order/pre/now")]
    public async Task<ActionResult<ApiResponse<OrderPreResult>>> GetOrderPreNow([FromQuery] OrderPreNowRequest request)
    {
        var userId = GetUserId();
        var result = await _orderService.GetOrderPreNowAsync(userId, request);
        return Ok(ApiResponse<OrderPreResult>.Success(result));
    }

    [HttpGet("order/repurchase/{id}")]
    public async Task<ActionResult<ApiResponse<OrderPreResult>>> GetOrderRepurchase(string id)
    {
        var userId = GetUserId();
        var result = await _orderService.GetOrderRepurchaseAsync(userId, id);
        return Ok(ApiResponse<OrderPreResult>.Success(result));
    }

    [HttpPost("order")]
    public async Task<ActionResult<ApiResponse<OrderCreateResult>>> CreateOrder([FromBody] OrderCreateRequest request)
    {
        var userId = GetUserId();
        var result = await _orderService.CreateOrderAsync(userId, request);
        return Ok(ApiResponse<OrderCreateResult>.Success(result, "提交成功"));
    }

    [HttpGet("order")]
    public async Task<ActionResult<ApiResponse<OrderListResult>>> GetOrderList([FromQuery] OrderListParams request)
    {
        var userId = GetUserId();
        var result = await _orderService.GetOrderListAsync(userId, request);
        return Ok(ApiResponse<OrderListResult>.Success(result));
    }

    [HttpGet("order/{id}")]
    public async Task<ActionResult<ApiResponse<OrderResult>>> GetOrder(string id)
    {
        var userId = GetUserId();
        var result = await _orderService.GetOrderByIdAsync(userId, id);
        if (result == null)
        {
            return NotFound(ApiResponse<OrderResult>.Error("订单不存在"));
        }
        return Ok(ApiResponse<OrderResult>.Success(result));
    }

    [HttpPut("order/{id}/receipt")]
    public async Task<ActionResult<ApiResponse<OrderResult>>> ConfirmReceipt(string id)
    {
        var userId = GetUserId();
        var result = await _orderService.ConfirmReceiptAsync(userId, id);
        if (result == null)
        {
            return BadRequest(ApiResponse<OrderResult>.Error("确认收货失败"));
        }
        return Ok(ApiResponse<OrderResult>.Success(result, "确认收货成功"));
    }

    [HttpPut("order/{id}/cancel")]
    public async Task<ActionResult<ApiResponse<OrderResult>>> CancelOrder(string id, [FromBody] OrderCancelRequest request)
    {
        var userId = GetUserId();
        var result = await _orderService.CancelOrderAsync(userId, id, request);
        if (result == null)
        {
            return BadRequest(ApiResponse<OrderResult>.Error("取消订单失败"));
        }
        return Ok(ApiResponse<OrderResult>.Success(result, "取消订单成功"));
    }

    [HttpDelete("order")]
    public async Task<ActionResult<ApiResponse>> DeleteOrder([FromBody] OrderDeleteRequest request)
    {
        var userId = GetUserId();
        var result = await _orderService.DeleteOrderAsync(userId, request);
        if (!result)
        {
            return BadRequest(ApiResponse.Error("删除订单失败"));
        }
        return Ok(ApiResponse.Success("删除成功"));
    }

    [HttpGet("order/{id}/logistics")]
    public async Task<ActionResult<ApiResponse<OrderLogisticResult>>> GetOrderLogistics(string id)
    {
        var userId = GetUserId();
        var result = await _orderService.GetOrderLogisticsAsync(userId, id);
        if (result == null)
        {
            return BadRequest(ApiResponse<OrderLogisticResult>.Error("获取物流信息失败"));
        }
        return Ok(ApiResponse<OrderLogisticResult>.Success(result));
    }

    [HttpGet("order/consignment/{id}")]
    public async Task<ActionResult<ApiResponse<OrderResult>>> SimulateConsignment(string id)
    {
        var userId = GetUserId();
        var result = await _orderService.SimulateConsignmentAsync(userId, id);
        if (result == null)
        {
            return BadRequest(ApiResponse<OrderResult>.Error("模拟发货失败"));
        }
        return Ok(ApiResponse<OrderResult>.Success(result, "模拟发货成功"));
    }
}
