using Clinic.Service.Data;
using ClinicServiceV2.Services;
using ClinicServiceV2.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;

namespace ClinicServiceV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Database

            builder.Services.AddDbContext<ClinicServiceDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            #endregion

            #region Configure Kestrel

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5100, listnerOptions =>
                {
                    listnerOptions.Protocols = HttpProtocols.Http2;
                });

                options.Listen(IPAddress.Any, 5101, listnerOptions =>
                {
                    listnerOptions.Protocols = HttpProtocols.Http1;
                });
            });

            #endregion

            #region Configure Grpc

            builder.Services.AddGrpc()
                .AddJsonTranscoding();

            #endregion

            #region Configure Swagger

            //https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-openapi?view=aspnetcore-7.0

            builder.Services.AddGrpcSwagger();
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "ClinicService", Version = "v1" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ClinicServiceV2.xml");
                options.IncludeXmlComments(filePath);
                options.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

            #endregion

            #region Add Services

            builder.Services.AddAuthorization();

            #endregion

            #region Configure JWT

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthenticateService.SecretKey)),
                        ValidateIssuerSigningKey = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            #endregion

            var app = builder.Build();

            #region Configure App

            app.UseRouting();

            app.UseGrpcWeb(new GrpcWebOptions
            {
                DefaultEnabled = true,

            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<ClinicService>()
                .EnableGrpcWeb();
            app.MapGrpcService<AuthService>()
                .EnableGrpcWeb();
            app.Map("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.nicrosoft.com/fwlink/?linjid=2086909");

            #endregion

            app.Run();
        }
    }
}