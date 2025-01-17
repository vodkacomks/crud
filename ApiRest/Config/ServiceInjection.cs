﻿using ApiRest.Auth.Core;
using ApiRest.Auth.Interfaces;
using ApiRest.Model;
using Core.Files;
using Core.Files.Interfaces;
using Core.User;
using Core.User.Interfaces;
using Core.User.UserRecovey;
using Core.User.UserRecovey.Interfaces;
using Core.User.Validators;
using DataAcess.Context;
using DataAcess.Interfaces;
using DataAcess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiRest.Config
{
    public class ServiceInjection
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection("DatabaseSettings");

            services.AddSingleton<IConfiguration>(configuration);
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));
            services.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            #region DataAccess

            services.AddSingleton<IDataContext>(serviceProvider => new DataContext(databaseSettings["BancoCRUDSqlConnection"]));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileUploadRepository, FileUploadRepository>();

            #endregion

            #region Core Services

            services.AddScoped<ICreateUser, CreateUser>();
            services.AddScoped<IDeleteUser, DeleteUser>();
            services.AddScoped<IGetAllUser, GetAllUser>();
            services.AddScoped<IGetUserByEmail, GetUserByEmail>();
            services.AddScoped<IGetUserById, GetUserById>();
            services.AddScoped<IGetUserByName, GetUserByName>();
            services.AddScoped<IGetUserBySocialNumber, GetUserBySocialNumber>();
            services.AddScoped<IUpdateUser, UpdateUser>();
            services.AddScoped<IEmailValidate, EmailValidate>();
            services.AddScoped<IValidateSocialNumber, ValidateSocialNumber>();
            services.AddScoped<IGetUserByLogin, GetUserByLogin>();
            services.AddScoped<IUpdatePassword, UpdatePassword>();
            services.AddScoped<INewPassowordUserRecovery, NewPassowordUserRecovery>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IPasswordValidator, PasswordValidator>();
            services.AddScoped<IFileUpload, FileUpload>();
            services.AddScoped<IGetFileByName, GetFileByName>();

            #endregion
        }
    }
}
