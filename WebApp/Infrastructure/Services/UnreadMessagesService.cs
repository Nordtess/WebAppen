using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApp.Infrastructure.Data;

namespace WebApp.Infrastructure.Services;

public interface IUnreadMessagesService
{
    Task<int> GetUnreadCountAsync(ClaimsPrincipal user);
}

public sealed class UnreadMessagesService : IUnreadMessagesService
{
    private readonly ApplicationDbContext _db;

    public UnreadMessagesService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetUnreadCountAsync(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return 0;

        return await _db.UserMessages.AsNoTracking()
            .Where(m => m.RecipientUserId == userId && !m.IsRead)
            .CountAsync();
    }
}
