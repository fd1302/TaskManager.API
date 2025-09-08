using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Siahroud.Logic.Services;

public class SMSService
{
    private readonly IConfiguration _configuration;
    private readonly string _url;
    private readonly string _username;
    private readonly string _apiKey;
    private readonly string _lineNumber;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;
    public SMSService(IConfiguration configuration, IMemoryCache memoryCache, IHttpClientFactory httpClient)
    {
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
        _memoryCache = memoryCache ??
            throw new ArgumentNullException(nameof(memoryCache));
        _httpClient = httpClient.CreateClient();
        _url = _configuration["SmsSettings:Url"]!;
        _username = _configuration["SmsSettings:Username"]!;
        _apiKey = _configuration["SmsSettings:ApiKey"]!;
        _lineNumber = _configuration["SmsSettings:LineNumber"]!;
    }

    public async Task<string> PassRecoveryMessageAsync(string phoneNumber)
    {
        string recoveryCode = GenerateCode();
        string message = $"Password recovery code:{recoveryCode}\nDon't share this with anyone.";
        _memoryCache.Set(phoneNumber, recoveryCode, TimeSpan.FromMinutes(5));
        string url = $"{_url}?username={_username}&password={_apiKey}&line={_lineNumber}&mobile={phoneNumber}&text={message}";
        try
        {
            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Password recover sms error: {ex}");
            return $"Error: {ex.Message}";
        }
    }
    public bool CheckCode(string phoneNumber, string inputCode)
    {
        if (_memoryCache.TryGetValue(phoneNumber, out string? storedCode))
        {
            if (storedCode == inputCode)
            {
                _memoryCache.Remove(phoneNumber);
                return true;
            }
        }
        return false;
    }
    private string GenerateCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    }
    public string PasswordRercoveryPageToken(string phoneNumber)
    {
        var token = Guid.NewGuid().ToString("N");
        _memoryCache.Set($"reset_token:{token}", phoneNumber, TimeSpan.FromMinutes(5));
        return token;
    }


    //private void CodeTimer()
    //{
    //    _timer = new System.Timers.Timer(3000000);
    //    _timer.Elapsed += (senders, args) => ClearCode();
    //    _timer.AutoReset = false;
    //    _timer.Start();
    //}
}
