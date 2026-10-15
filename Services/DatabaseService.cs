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


    public static void SaveRun(List<double> voltages, int id)
    {
        using var connection = new SqliteConnection($"Data Source ={database_path}");
        connection.Open();
        using var transaction = connection.BeginTransaction();

        var create = connection.CreateCommand();

        create.CommandText= """
            CREATE TABLE IF NOT EXISTS RunData (
                    Id  INTEGER PRIMARY KEY AUTOINCREMENT,
                    RunId   INTEGER,
                    SourceId    INTEGER,
                    Voltage     REAL,
                    TimeStamp   TEXT
                    );
        """;
        create.ExecuteNonQuery();

        var idcommand = connection.CreateCommand();
        idcommand.Transaction = transaction;
        idcommand.CommandText = "SELECT COALESCE(MAX(RunId), 0) +  1 FROM RunData;";
        long runid = (long)idcommand.ExecuteScalar()!;


        foreach(double voltage in voltages)
        {
            var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = """
                INSERT INTO RunData(RunId, SourceId, Voltage, Timestamp)
                VALUES($run, $source, $voltage, $timestamp);
            """;
            cmd.Parameters.AddWithValue($"run", runid);
            cmd.Parameters.AddWithValue($"source", id);
            cmd.Parameters.AddWithValue($"volt", voltage);
            cmd.Parameters.AddWithValue($"timestamp", DateTime.Now.ToString("o"));
            cmd.ExecuteNonQuery();

        }

        transaction.Commit();

    }
}




