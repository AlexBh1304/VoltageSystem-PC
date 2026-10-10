using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Microsoft.Data.Sqlite;
using System.IO.Ports;
using PrimeraApp.Models;
using PrimeraApp.Services;
namespace PrimeraApp;


public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void MiBoton_Click(object? sender, RoutedEventArgs e)
    {
        User user = new User{Name = UserText.Text, Password = PasswordText.Text};       
        LoginStatus loginstatus = UserService.Login(user);

        switch(loginstatus)
        {
            case LoginStatus.Success:
                LoadVoltageWindow();
                break;
            case LoginStatus.UserNotFound:
                //UserNotFound
                StatusLabel.Content="User not found";
                break;
            case LoginStatus.WrongPassword:
                //WrongPassword
                StatusLabel.Content="Wrong Password";
                break;

        }

       
    }

    private void LoadVoltageWindow()
    {
        VoltageWindow voltagewindow = new VoltageWindow();
        voltagewindow.Show();
        this.Close();
    }

    
}


