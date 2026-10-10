using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Microsoft.Data.Sqlite;
using System.IO.Ports;
namespace PrimeraApp;


public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        if(VerificarUsuario(UserText.Text, PasswordText.Text ))
        {
            Saludar();
        }
    }

    private void Saludar()
    {
        VoltageWindow voltagewindow = new VoltageWindow();
        voltagewindow.Show();
        this.Close();
    }

    private bool VerificarUsuario(string nombre, string contraseña)
    {
        using var connection = new SqliteConnection("Data Source=database.db");
        using var command = connection.CreateCommand();
        connection.Open();
        command.CommandText= """
            SELECT ID, Name, Password
            FROM USERS
            WHERE Name = $nombre;
        """;
        command.Parameters.AddWithValue("$nombre", nombre);
        using var reader = command.ExecuteReader();
        if(reader.Read())
        {
            if (reader.GetString(2) == contraseña)
                {
                    return true;
                }
            else
            {
                while(reader.Read())
                {
                    if (reader.GetString(2) == contraseña)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }
        else
        {
            return false;
        }

        
         
    }
}


