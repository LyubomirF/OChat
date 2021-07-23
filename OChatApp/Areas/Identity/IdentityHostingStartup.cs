﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OChatApp.Areas.Identity.Data;
using OChatApp.Data;

[assembly: HostingStartup(typeof(OChatApp.Areas.Identity.IdentityHostingStartup))]
namespace OChatApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<OChatAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("OChatAppContextConnection")));

                services.AddDefaultIdentity<OChatAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<OChatAppContext>();
            });
        }
    }
}