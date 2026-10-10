using PrimeraApp.Models;
namespace PrimeraApp.Services;

public static class CurrentUser
{
    public static User currentuser? {get; private set;}
    
    public static void Set(User user)
    {
        currentuser = user;
    }

    public static void Clear()
    {
        currentuser = null;
    }
}

