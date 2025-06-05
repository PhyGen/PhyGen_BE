using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using teamseven.PhyGen.Repository.Models;

public class DatabaseKeepAliveMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseKeepAliveMiddleware> _logger;
    private readonly TimeSpan _queryInterval = TimeSpan.FromMinutes(60); // Truy vấn mỗi 45 min
    private DateTime _lastQueryTime;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    private Task _backgroundTask;

    public DatabaseKeepAliveMiddleware(RequestDelegate next, IServiceProvider serviceProvider, ILogger<DatabaseKeepAliveMiddleware> logger)
    {
        _next = next;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _lastQueryTime = DateTime.MinValue;
        _backgroundTask = StartBackgroundQuery(_cts.Token); // Bắt đầu task nền
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Tiếp tục pipeline
        await _next(context);
    }

    private async Task StartBackgroundQuery(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (DateTime.UtcNow - _lastQueryTime >= _queryInterval)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<teamsevenphygendbContext>();
                        await dbContext.Database.ExecuteSqlRawAsync("SELECT 1");
                        _lastQueryTime = DateTime.UtcNow;
                        _logger.LogInformation("Database keep-alive query executed at {Time}", _lastQueryTime);
                        Console.WriteLine("Database keep-alive query executed");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute keep-alive query");
                Console.WriteLine($"Keep-alive query failed: {ex.Message}");
            }

            // Chờ 30 phút trước khi kiểm tra lại
            await Task.Delay(_queryInterval, cancellationToken);
        }
    }

    // Hủy task khi middleware bị dispose (tùy chọn)
    public void Dispose()
    {
        _cts.Cancel();
    }
}

// Extension method để đăng ký middleware
public static class DatabaseKeepAliveMiddlewareExtensions
{
    public static IApplicationBuilder UseDatabaseKeepAlive(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DatabaseKeepAliveMiddleware>();
    }
}