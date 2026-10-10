using PrimeraApp.Models;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.IO.Ports;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace PrimeraApp.Services;

public static class DatabaseService
{
    private static string database_path{get;} = "/home/Alex/CSharp/PrimeraApp/database.db";
    

    public static List<User> UserQuery(string name)
    {
        using var connection = new SqliteConnection($"Data Source={database_path}");
        using var command = connection.CreateCommand();
        connection.Open();

        command.CommandText= """
            SELECT ID, Name, Password
            FROM USERS
            WHERE Name = $name;
        """;

        command.Parameters.AddWithValue("$name", name);

        List<User> list = new List<User>();
        using var reader = command.ExecuteReader();
        
        while(reader.Read())
        {
            list.Add(new User {Id = reader.GetInt32(0), Name = reader.GetString(1), Password = reader.GetString(2)});
        }

        return list;
    }
}




