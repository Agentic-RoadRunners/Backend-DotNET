
using System.Threading.Tasks;

namespace SafeRoad.Core.Interfaces.Services;

public interface IEmailService
{
    Task SendPasswordResetAsync(string email, string resetToken);
    Task SendEmailVerificationAsync(string email, string verificationToken);
}