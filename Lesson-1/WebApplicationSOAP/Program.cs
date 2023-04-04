using Microsoft.AspNetCore.Builder;
using SoapCore;
using System.ServiceModel;
using WebApplicationSOAP.Services;
using WebApplicationSOAP.Services.Impl;

namespace WebApplicationSOAP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSoapCore();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddSingleton<ISoapService, SoapService>();
            builder.Services.AddSingleton<IRepositoryContext, BooksContext>();
            builder.Services.AddSingleton<IRepositoryBook, BooksRepository>();

            builder.Services.AddMvc();
            builder.Services.AddRouting();

            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.UseSoapEndpoint<ISoapService>(
                    "/Services.asmx",
                    new SoapEncoderOptions(),
                    SoapSerializer.XmlSerializer);
            });

            app.Run();
        }
    }
}