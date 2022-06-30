// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Solutions.CosmosDB.Security.ManagedIdentity;
using Microsoft.TokenService.BlockchainNetworkManager;
using Microsoft.TokenService.KeyManagement;
using Microsoft.TokenService.LedgerClient.Client;
using Microsoft.TokenService.PartyManager;
using Microsoft.TokenService.UserManager;
using AutoMapper;
using Newtonsoft.Json.Converters;

namespace Microsoft.TokenService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                 .AddNewtonsoftJson(jsonOptions =>
                 {
                     jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter());
                 });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microsoft Token Service API Endpoint", Version = "v1.0.0" });
                c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
            });

            services.AddSwaggerGenNewtonsoftSupport();

            //add DI
            services.AddSingleton<CosmosConnectionStrings>(x =>
            {
                return ConnectionStringAccessor.Create(Configuration["App:SubscriptionId"], Configuration["App:ResourceGroupName"], Configuration["App:DatabaseAccountName"])
                    .GetConnectionStringsAsync(Configuration["App:ManagedIdentityId"]).GetAwaiter().GetResult();
            });

            services.AddSingleton<AzureKeyVaultKeyClient>(x =>
            {
                return AzureKeyVaultKeyClient.Create()
                                             .Build(Configuration["KeyVault:KeyVaultUrl"],
                                                     Configuration["App:SubscriptionId"],
                                                     Configuration["App:ResourceGroupName"],
                                                     Configuration["App:ManagedIdentityId"]);
            });

            services.AddTransient<IBlockchainNetworkManager, BlockchainNetworks>();
            services.AddTransient<IPartyManager, Parties>();
            services.AddTransient<IUserManager, Users>();
            services.AddTransient<IERC721TokenClient, ERC721TokenClient>();
            services.AddTransient<TransactionIndexer.TransactionIndexerService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }



            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwagger(c =>
            {
                c.RouteTemplate =
                    "api-docs/{documentName}/swagger.json";

            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NonFungible Token Service API V1");

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LedgerClient.Model.TransactionReciept,TransactionIndexer.Model.TransactionRecieptInfo>();
        }
    }
}
