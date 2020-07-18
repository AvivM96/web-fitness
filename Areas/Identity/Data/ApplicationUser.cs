using System;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }

    public ApplicationUser()
    {
    }
}
