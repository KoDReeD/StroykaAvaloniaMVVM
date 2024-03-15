using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DemoMVVM.Context;
using Microsoft.EntityFrameworkCore;

namespace DemoMVVM.ViewModels;

public class AuthViewModel : INotifyPropertyChanged
{
    private string _login = "";
    public string Login
    {
        get { return _login; }
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }
    
    private string _password = "";
    public string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }
    
    private string _loginMessage = "";
    public string LoginMessage
    {
        get { return _loginMessage; }
        set
        {
            _loginMessage = value;
            OnPropertyChanged();
        }
    } 
    
    private string _passwordMessage = "";
    public string PasswordMessage
    {
        get { return _passwordMessage; }
        set
        {
            _passwordMessage = value;
            OnPropertyChanged();
        }
    }

    private async Task Submit()
    {
        var user = await StroykaMvvmContext.GetContext().Users
            .FirstOrDefaultAsync(x => x.Userlogin == Login && x.Userpassword == Password);

        if (user == null)
        {
            Login = "";
            Password = "";
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}