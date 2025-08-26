using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TaskManager.Shared.Dto_s;

namespace TaskManager.Logic.Services;

public class EmailVerification
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    public EmailVerification(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
        _httpClient = httpClient ??
            throw new ArgumentNullException(nameof(httpClient));
    }
    public async Task<EmailValidationDto?> VerifyWithDetail(string emailAddress)
    {
        string uri = $"{_configuration["AbstractApi:Url"]}&email={emailAddress}";
        var response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            EmailValidationDto? emailValidationDto = JsonSerializer.Deserialize<EmailValidationDto>(jsonString);
            Console.WriteLine(jsonString);
            return emailValidationDto;
        }
        return null;
    }
    public async Task<bool> Verify(string emailAddress)
    {
        string uri = $"{_configuration["AbstractApi:Url"]}&email={emailAddress}";
        var response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            EmailValidationDto? emailValidationDto = JsonSerializer.Deserialize<EmailValidationDto>(jsonString);
            if (emailValidationDto!.deliverability == "UNDELIVERABLE")
            {
                return false;
            }
        }
        return true;
    }
}
