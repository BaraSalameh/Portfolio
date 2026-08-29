using Domain.Entities;

namespace Application.Common.Services.Interface;

public interface IPasswordService
{
    string Hash(User user, string password);
    PasswordVerificationOutcome Verify(User user, string passwordHash, string providedPassword);
    void PerformDummyVerification(string providedPassword);
}

public enum PasswordVerificationOutcome
{
    Failed,
    Success,
    SuccessRehashNeeded
}
