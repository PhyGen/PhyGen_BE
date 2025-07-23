using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NotificationService
{
    /// <summary>
    /// Gửi notification qua Firebase.
    /// </summary>
    /// <param name="title">Tiêu đề noti.</param>
    /// <param name="body">Nội dung noti.</param>
    /// <param name="data">Dữ liệu tùy chỉnh (ví dụ: transactionId cho xác minh).</param>
    /// <param name="target">Token device (cho cá nhân) hoặc topic (cho quảng cáo, ví dụ "promotion").</param>
    /// <param name="isTopic">True nếu target là topic (quảng cáo), false nếu là token cá nhân.</param>
    public async Task<string> SendNotificationAsync(string title, string body, Dictionary<string, string> data = null, string target = null, bool isTopic = false)
    {
        if (string.IsNullOrEmpty(target))
        {
            throw new ArgumentException("Target (token hoặc topic) không được để trống.");
        }

        var message = new Message()
        {
            Notification = new Notification()
            {
                Title = title,
                Body = body
            },
            Data = data ?? new Dictionary<string, string>(),
        };

        if (isTopic)
        {
            message.Topic = target; // Ví dụ: "promotion" cho quảng cáo
        }
        else
        {
            message.Token = target; // Token device cho xác minh giao dịch
        }

        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response; // Trả về message ID nếu thành công
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi gửi noti: {ex.Message}");
            throw;
        }
    }
}