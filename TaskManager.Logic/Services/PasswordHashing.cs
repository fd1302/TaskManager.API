using System.Security.Cryptography;
using System.Text;

namespace TaskManager.Logic.Services;

public class PasswordHashing
{
    public string HashingPass(string password)
    {
        byte[] saltBytes = new byte[16];
        using(var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        password += Convert.ToBase64String(saltBytes);
        var passBytes = Encoding.UTF8.GetBytes(password);
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hashBytes = sha.ComputeHash(passBytes);
            StringBuilder sb = new StringBuilder();
            foreach(byte s in hashBytes)
            {
                sb.Append(s.ToString("x2"));
            }
            return sb.ToString();
        }
    }
    public string Sha256HashPass(string password)
    {
        var salt = new byte[16];
        salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

        
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(saltedPassword);
            return $"{BitConverter.ToString(salt).Replace("-", "")}:{BitConverter.ToString(hashBytes).Replace("-", "")}";
        }
    }
    public bool Verify(string password, string storedHashWithSalt)
    {
        var parts = storedHashWithSalt.Split(':');
        if (parts.Length != 2)
        {
            throw new FormatException("Stored hash must be in 'salt:hash' format.");
        }

        string saltHex = parts[0];
        string hashHex = parts[1];

        
        byte[] salt = new byte[saltHex.Length / 2];
        for (int i = 0; i < salt.Length; i++)
            salt[i] = Convert.ToByte(saltHex.Substring(i * 2, 2), 16);

        
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
        Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] computedHash = sha256.ComputeHash(saltedPassword);
            string computedHashHex = BitConverter.ToString(computedHash).Replace("-", "");

            
            return StringComparer.OrdinalIgnoreCase.Compare(computedHashHex, hashHex) == 0;
        }
    }
}
