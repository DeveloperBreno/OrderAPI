using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Token;

public class JwtSecurityKey
{
    public static SymmetricSecurityKey Create(string secret)
    {
        if (secret.Length < 32)
        {
            throw new ArgumentOutOfRangeException(nameof(secret), "The key must be at least 256 bits (32 characters) long.");
        }
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
    }
}
