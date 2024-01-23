﻿using BugFixer.Application.Services.Implementations;
using BugFixer.Application.Services.Interfaces;
using BugFixer.DataLayer.Repositories;
using BugFixer.domain.InterFaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.IoC
{
    public class DependencyContainer
    {
        public static void RejosterService(IServiceCollection services)
        {
            #region services

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IStateService, StateService>();

            #endregion

            #region repositories

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();

            services.AddScoped<IStateRepository, StateRepository>();

            #endregion


            #region tools

            #endregion
        }
    }
}
