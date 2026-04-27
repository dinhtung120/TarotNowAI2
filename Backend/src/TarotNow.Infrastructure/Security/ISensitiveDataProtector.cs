namespace TarotNow.Infrastructure.Security;

public interface ISensitiveDataProtector
{
    string Protect(string? plaintext);

    string Unprotect(string? protectedValue);
}
