using PrimeraApp.Models;
using System;
using System.Collections.Generic;

namespace PrimeraApp.Services;

public enum LoginStatus
{
    Success = 0, 
    UserNotFound = 1, 
    WrongPassword = 2
}

public static class UserService
{
    public static LoginStatus(User user)
    {
        
        LoginStatus loginstatus = CheckCredentials(user.name, user.password);

        if(loginstatus == 0)
        {
            CurrentUser.Set(user);
        }

        return loginstatus;
           
    }

    public static void Logout()
    {
        CurrentUser.Clear();
    }

    private static LoginStatus CheckCredentials(string name, string password)
    {

        List<User> list = DatabaseService.UserQuery(name);
        if(list.Count == 0)
        {
            LoginStatus loginstatus = LoginStatus.UserNotFound;
            return loginstatus;
        }
        else
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].Password == password)
                {
                    LoginStatus loginstatus = LoginStatus.Success;
                    return loginstatus;
                }
            }
            LoginStatus loginstatus = LoginStatus.WrongPassword;
            return loginstatus;
        }
    }
//    public static void SaveToDatabase()
//    {
//
//    }
//    public static void Login()
//    {
//
//    }
//



}
