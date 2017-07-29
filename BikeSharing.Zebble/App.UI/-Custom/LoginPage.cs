﻿using Domain;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Zebble;

namespace UI.Pages
{
    partial class LoginPage
    {


        private string _userName;
        private string _password;
        private bool _isValid;
      
       

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;             
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
            }
        }

        private async void SignInAsync()
        {
            IsValid = true;
            bool isValid = Validate();
            bool isAuthenticated = false;

            if (isValid)
            {
                try
                {
                    var _authenticationService = new AuthenticationService();
                    isAuthenticated = await _authenticationService.LoginAsync(UserName, Password);
                }              
                catch (Exception ex) when (ex is WebException )
                {
                  //  Debug.WriteLine($"[SignIn] Error signing in: {ex}");
                    await Alert.Show("Error","Communication error");
                }
                catch (Exception ex)
                {
                  //  Debug.WriteLine($"[SignIn] Error signing in: {ex}");
                }
            }
            else
            {
                var file = Device.IO.File("Session.txt");
                if (file.Exists())
                {
                    var data = file.ReadAllText();
                    Settings.UserId = Convert.ToInt32(data);
                    if (Settings.UserId != 0)
                    isAuthenticated = true;
                }


                IsValid = false;
            }

            if (isAuthenticated)
            {
                await Nav.Go<HomePage>();
            }
            
        }
        
        private bool Validate()
        {
            _userName =  usernameTextInput?.Text;
            _password = passwordTextInput?.Text;
            bool isValidUser = _userName.HasValue();
            bool isValidPassword = _password.HasValue();

            return isValidUser && isValidPassword;
        }

      

       // private void AddValidations()
       // {
         //   _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Username should not be empty" });
         //   _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password should not be empty" });
       // }
    }

}