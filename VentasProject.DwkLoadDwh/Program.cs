using Microsoft.EntityFrameworkCore;
using VentasProject.Application.Interfaces;
using VentasProject.Application.Repositories.Csv;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Application.Services;
using VentasProject.DwkLoadDwh;
using VentasProject.DwkLoadDwh.Extensions;
using VentasProject.Persistence.Repositories.Csv;
using VentasProject.Persistence.Repositories.Dwh;
using VentasProject.Persistence.Repositories.Dwh.Context;

namespace VentasProject.DwkLoadDwh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddCsvRepositories(builder.Configuration);

            builder.Services.AddApplicationServices();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();

        }
    }
}



