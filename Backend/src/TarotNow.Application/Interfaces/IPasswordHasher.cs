

namespace TarotNow.Application.Interfaces;

public interface IPasswordHasher
{
        string HashPassword(string password);

        bool VerifyPassword(string hash, string providedPassword);

        bool NeedsRehash(string hash);
}
