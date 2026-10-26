using VoltageSystem.Models;

namespace VoltageSystem.Services;

public static class CurrentUser
{
    public static User? Data {get; private set;}
    
    public static void Set(User user)
    {
        Data = user;
    }

    public static void Clear()
    {
        Data = null;
    }
}

