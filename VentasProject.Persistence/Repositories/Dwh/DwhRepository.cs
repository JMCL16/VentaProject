using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasProject.Application.Dtos;
using VentasProject.Application.Interfaces;
using VentasProject.Application.Repositories.Dwh;
using VentasProject.Application.Services;
using VentasProject.Domain.Entities.Dwh.Dimensions;
using VentasProject.Persistence.Repositories.Db.Context;
using VentasProject.Persistence.Repositories.Dwh.Context;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VentasProject.Persistence.Repositories.Dwh
{
    public class DwhRepository : IDwhRepository
    {
        private readonly DwhSalesContext _context;
        private readonly ILogger<DwhRepository> _logger;

        public DwhRepository(DwhSalesContext context, ILogger<DwhRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result> LoadDimsDataAsync(DimDtos dimDtos)
        {
            var result = new Result();
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _logger.LogInformation("Iniciando carga masiva de Dimensiones...");

                    if (dimDtos.Customers != null && dimDtos.Customers.Any())
                    {
                        var customersEntities = dimDtos.Customers.Select(c => new DimCustomers
                        {
                            CustomerId = c.CustomerId,
                            CustomerName = c.CustomerName,
                            City = c.City,
                            Country = c.Country
                        }).ToList();

                        await _context.DimCustomers.AddRangeAsync(customersEntities);
                    }

                    if (dimDtos.Products != null && dimDtos.Products.Any())
                    {
                        var productsEntities = dimDtos.Products.Select(p => new DimProducts
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName ?? "Sin Nombre",
                            Category = p.Category ?? "Sin Categoria",
                            ListPrice = p.ListPrice
                        }).ToList();

                        await _context.DimProducts.AddRangeAsync(productsEntities);
                    }

                    if (dimDtos.Dates != null && dimDtos.Dates.Any())
                    {
                        await LoadDatesInternalAsync(dimDtos.Dates);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    result.IsSuccess = true;
                    result.Message = $"Carga Exitosa. Clientes: {dimDtos.Customers?.Count}, Productos: {dimDtos.Products?.Count}";
                    _logger.LogInformation(result.Message);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error en la carga de dimensiones. Se hizo Rollback.");
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                }
            });

            return result;
        }
        public async Task CleanDataAsync()
        {
            _logger.LogInformation("Iniciando limpieza del Data Warehouse...");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Fact].[FactVentas]");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[Dim_Customer]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[Dim_Customer]', RESEED, 0)");

  
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[Dim_Products]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[Dim_Products]', RESEED, 0)");
                await transaction.CommitAsync();

                _logger.LogInformation("Data Warehouse limpio.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error durante la limpieza del DWH.");
                throw;
            }
        }

        private async Task LoadDatesInternalAsync(List<DatesDto> dateDtos)
        {
            _logger.LogInformation("Procesando {Count} fechas...", dateDtos.Count);

            var existingDateIds = await _context.DimDates
                .Select(d => d.DateId)
                .ToListAsync();

            var existingSet = new HashSet<int>(existingDateIds);
            var datesToInsert = new List<DimDates>();

            foreach (var dto in dateDtos)
            {
                if (!existingSet.Contains(dto.DateId))
                {
                    datesToInsert.Add(new DimDates
                    {
                        DateId = dto.DateId,
                        Date = dto.FullDate,
                        Anio = dto.Year,
                        Trimestre = dto.Quarter,
                        Mes = dto.Month,
                        NombreMes = dto.MonthName,
                        Semana = dto.Week,
                        DiaMes = dto.DayOfMonth,
                        DiaSemana = dto.DayOfWeek,
                        NombreDia = dto.DayName
                    });
                    existingSet.Add(dto.DateId);
                }
            }

            if (datesToInsert.Any())
            {
                await _context.DimDates.AddRangeAsync(datesToInsert);
                _logger.LogInformation("Se detectaron {Count} fechas nuevas.", datesToInsert.Count);
            }
        }
    }
}
