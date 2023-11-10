using System.ComponentModel.DataAnnotations;

namespace SecureCoding.Models;

public class Customer
{
    [RegularExpression(@"^[A-Z]+[a-zA-Z]*$",
        ErrorMessage = "First Name must contain only letters")]
    [Display(Name = "First Name")]
    [StringLength(25, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;


    [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", 
        ErrorMessage = "Last Name must contain only letters")]
    [Display(Name = "Last Name")]
    //[StringLength(25, MinimumLength = 3)]
    [Length(3, 25)]
    public string LastName { get; set; } = string.Empty;


    [DeniedValues("Male", "Female", 
        ErrorMessage = "Only 'Male' and 'Female' are allowed")]
    public string Gender { get; set; } = string.Empty;
}