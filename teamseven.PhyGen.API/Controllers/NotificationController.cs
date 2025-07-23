using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Gửi thông báo quảng cáo tới tất cả người dùng đã subscribe topic "promotion".
    /// </summary>
    /// <param name="request">Thông tin thông báo quảng cáo.</param>
    /// <returns>Kết quả gửi thông báo.</returns>
    /// <response code="201">Thông báo quảng cáo được gửi thành công.</response>
    /// <response code="400">Tiêu đề hoặc nội dung trống.</response>
    /// <response code="500">Lỗi server khi gửi thông báo.</response>
    [Authorize(Policy = "SaleStaffPolicy")]
    [HttpPost("promotions")]
    [SwaggerOperation(Summary = "Gửi thông báo quảng cáo", Description = "Gửi thông báo quảng cáo tới topic 'promotion'.")]
    [ProducesResponseType(typeof(NotificationResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> SendPromotion([FromBody] NotificationRequest request)
    {
        if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
        {
            return BadRequest("Tiêu đề và nội dung không được để trống.");
        }

        try
        {
            var data = new Dictionary<string, string>
            {
                { "type", "promotion" },
                { "offer", request.Details ?? "Không có chi tiết ưu đãi" }
            };

            var messageId = await _notificationService.SendNotificationAsync(
                title: request.Title,
                body: request.Body,
                data: data,
                target: "promotion",
                isTopic: true
            );

            return StatusCode(201, new NotificationResponse
            {
                Message = "Thông báo quảng cáo đã gửi.",
                MessageId = messageId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi khi gửi thông báo quảng cáo: {ex.Message}");
        }
    }

    /// <summary>
    /// Gửi thông báo chung tới tất cả người dùng đã subscribe topic "announcements".
    /// </summary>
    /// <param name="request">Thông tin thông báo chung.</param>
    /// <returns>Kết quả gửi thông báo.</returns>
    /// <response code="201">Thông báo chung được gửi thành công.</response>
    /// <response code="400">Tiêu đề hoặc nội dung trống.</response>
    /// <response code="500">Lỗi server khi gửi thông báo.</response>
    [HttpPost("announcements")]
    [Authorize(Policy = "SaleStaffPolicy")]
    [SwaggerOperation(Summary = "Gửi thông báo chung", Description = "Gửi thông báo chung tới topic 'announcements'.")]
    [ProducesResponseType(typeof(NotificationResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> SendAnnouncement([FromBody] NotificationRequest request)
    {
        if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Body))
        {
            return BadRequest("Tiêu đề và nội dung không được để trống.");
        }

        try
        {
            var data = new Dictionary<string, string>
            {
                { "type", "announcement" },
                { "details", request.Details ?? "Không có chi tiết thêm" }
            };

            var messageId = await _notificationService.SendNotificationAsync(
                title: request.Title,
                body: request.Body,
                data: data,
                target: "announcements",
                isTopic: true
            );

            return StatusCode(201, new NotificationResponse
            {
                Message = "Thông báo chung đã gửi.",
                MessageId = messageId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi khi gửi thông báo chung: {ex.Message}");
        }
    }
}

public class NotificationRequest
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Details { get; set; }
}

public class NotificationResponse
{
    public string Message { get; set; }
    public string MessageId { get; set; }
}