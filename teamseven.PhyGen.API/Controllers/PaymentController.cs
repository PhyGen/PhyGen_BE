using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Net.payOS.Types;
using teamseven.PhyGen.Controllers;
using teamseven.PhyGen.Repository.Dtos;
using teamseven.PhyGen.Repository.Models;
using teamseven.PhyGen.Repository.Repository;
using teamseven.PhyGen.Repository.Repository.Interfaces;
using teamseven.PhyGen.Services.Object.Requests;
using teamseven.PhyGen.Services.Services;
using teamseven.PhyGen.Services.Services.ServiceProvider;

namespace teamseven.PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;  // Thêm Repository cho UserSubscription
        private readonly ILogger<ChapterController> _logger;

        public PaymentController(ILogger<ChapterController> logger, IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment(CreatePaymentRequest request)
        {
            if (request == null || request.UserId <= 0 || request.Amount <= 0)
            {
                return BadRequest(new { Message = "Invalid request data" });
            }

            // Tạo orderCode unique (sẽ dùng làm PaymentGatewayTransactionId)
            long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // 24/07/2025 03:18 PM +07

            // Tạo ItemData cho PayOS
            var item = new ItemData(request.ItemName, request.Quantity, (int)request.Amount); // Chuyển decimal sang int
            var items = new List<ItemData> { item };

            // Cấu hình PaymentData
            var paymentData = new PaymentData(
                orderCode,
                (int)request.Amount, // Chuyển decimal sang int vì PayOS yêu cầu
                request.Description,
                items,
                "https://your-domain.com/cancel",
                "https://your-domain.com/success"
            );
            // Gọi service để tạo payment link
            CreatePaymentResult result = await _serviceProvider.PayOSService.CreatePaymentLink(paymentData);

            // Chuẩn bị UserSubscriptionRequest để gọi AddSubscriptionAsync
            var subscriptionRequest = new UserSubscriptionRequest
            {
                UserId = request.UserId,
                SubscriptionTypeId = request.SubscriptionTypeId ?? 1, // Giả sử mặc định, bạn có thể lấy từ request
                EndDate = DateTime.UtcNow.AddMonths(1), // Giả sử gói 1 tháng từ 24/07/2025
                Amount = request.Amount,
                PaymentGatewayTransactionId = orderCode.ToString()
            };

            // Lưu thông tin vào UserSubscription qua service
            await _serviceProvider.UserSubscriptionService.AddSubscriptionAsync(subscriptionRequest);

            // Trả về URL cho FE
            return Ok(new { CheckoutUrl = result.checkoutUrl });
        }

        [HttpPost("payos-webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] WebhookType webhookBody)
        {
            Console.WriteLine("[Webhook] Received webhook from PayOS"); 
            try
            {
                WebhookData data = webhookBody.data;
                // Nếu thanh toán thành công (code == "00")
                if (data.code == "00")
                {

                    var userSubscription = await _serviceProvider.UserSubscriptionService.GetByPaymentGatewayTransactionIdAsync(data.orderCode.ToString());
                    if (userSubscription != null)
                    {
                        Console.WriteLine("Found subscription: " + userSubscription.Id + data.amount+ userSubscription.UserId);

                        var user = await _serviceProvider.UserService.GetUserByIdAsync(userSubscription.UserId);
                        if (user != null)
                        {
                            Console.WriteLine("hi");

                            // Xử lý Balance: Nếu null thì gán 0 trước khi cộng amount
                            if (!user.Balance.HasValue)
                            {
                                user.Balance = 0m; // Chuyển null thành 0
                            }
                            user.Balance += (decimal)data.amount; // Giả sử Balance là decimal
                            await _serviceProvider.UserService.UpdateUserAsync(user);
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    // DTO cho request
    public class CreatePaymentRequest
    {
        public int UserId { get; set; }
        public int? SubscriptionTypeId { get; set; }  // Thêm field này để chọn loại subscription
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }  // Sử dụng decimal để khớp với UserSubscription
        public string Description { get; set; }
    }
}