﻿using Diploma.Domain.Models;
using Diploma.EntityFramework.Services.StudentProviders;
using Diploma.EntityFramework.Services.TeacherProviders;
using Diploma.WPF.ViewModels;
using System;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;

namespace Diploma.WPF.Commands
{
    public class LoginCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public LoginViewModel _viewModel;
        private IStudentService studentService;
        private ITeacherService teacherService;

        public LoginCommand(LoginViewModel viewModel)
        {
            studentService = new DatabaseStudentService(new EntityFramework.SchoolDbContextFactory());
            teacherService = new DatabaseTeacherService(new EntityFramework.SchoolDbContextFactory());  
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return (string.IsNullOrWhiteSpace(_viewModel.Username) || _viewModel.Username.Length < 3
                || _viewModel.Password == null || _viewModel.Password.Length < 3) ? false : true;
        }

        public void Execute(object? parameter)
        {
            bool buttonStudentIsChecked = (bool)parameter;

            if (buttonStudentIsChecked)
            {
                var isValidUser = studentService.AuthenticateStudent(new System.Net.NetworkCredential(_viewModel.Username, _viewModel.Password));
                if (isValidUser)
                {
                    //Student student = new Student()
                    //{
                    //    Username = _viewModel.Username,
                    //    Password = _viewModel.Password,
                    //};

                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(_viewModel.Username), null);
                    _viewModel.IsViewVisible = false;
                }
                else
                {
                    _viewModel.ErrorMessage = "* Invalid username or password";
                }
            }
            else
            {
                var isValidUser = teacherService.AuthenticateTeacher(new System.Net.NetworkCredential(_viewModel.Username, _viewModel.Password));
                if (isValidUser)
                {
                    //Teacher teacher = new Teacher()
                    //{
                    //    Username = _viewModel.Username,
                    //    Password = _viewModel.Password,
                    //};

                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(_viewModel.Username), null);
                    _viewModel.IsViewVisible = false;
                }
                else
                {
                    _viewModel.ErrorMessage = "* Invalid username or password";
                }

            }

            
        }
    }
}
