using EmailRep.NET;
using System.ComponentModel.DataAnnotations;

namespace SecureCoding.Attributes;

public class ReputableEmailAttribute : ValidationAttribute
{
    public string GetErrorMessage() =>
        "Email address is rejected because of its reptation";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var email = value as string;

        var emailRepClient = (IEmailRepClient)validationContext.GetService(typeof(IEmailRepClient))!;

        if(IsRisky(email!, emailRepClient).GetAwaiter().GetResult())
            return new ValidationResult(GetErrorMessage());

        return ValidationResult.Success;
    }

    private static async Task<bool> IsRisky(string email, IEmailRepClient emailRepClient)
    {
        var reputation = await emailRepClient.QueryEmailAsync(email);

        return reputation.Details.Blacklisted ||
            reputation.Details.MaliciousActivity ||
            reputation.Details.MaliciousActivityRecent ||
            reputation.Details.Spam ||
            reputation.Details.SuspiciousTld;
    }
}