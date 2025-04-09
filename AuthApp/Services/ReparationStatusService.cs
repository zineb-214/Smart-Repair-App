using AuthApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApp.Services
{
    public class ReparationStatusService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ReparationStatusService> _logger;

        public ReparationStatusService(IServiceProvider services, ILogger<ReparationStatusService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reparation Status Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<Context>();

                        var today = DateTime.Today;
                        var yesterday = today.AddDays(1);

                        var detailsToUpdate = await context.DetailsReparations
                            .Where(d => d.DateFinEstime.Date <= yesterday && d.Statut != "Terminé")
                            .ToListAsync();

                        foreach (var detail in detailsToUpdate)
                        {
                            if (detail.DateFinEstime.Date == yesterday)
                            {
                                detail.Statut = "En Cours";
                                _logger.LogInformation($"Updated status to En Cours for detail ID: {detail.Id}");
                            }
                            else
                            {
                                detail.Statut = "Terminé";
                                _logger.LogInformation($"Updated status to Terminé for detail ID: {detail.Id}");
                            }
                        }

                        if (detailsToUpdate.Any())
                        {
                            await context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating reparation statuses");
                }

                //had line ltest lbga chi 7ed ytesti
                 await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                //await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}