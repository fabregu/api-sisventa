using DAL.SistemaVenta.DBContext;
using DAL.SistemaVenta.Repositorios;
using DAL.SistemaVenta.Repositorios.Contrato;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC.SistemaVenta
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            // Inyección de dependencias para el contexto de la base de datos
            services.AddDbContext<DbventaContext>(options => { 
                options.UseSqlServer(configuration.GetConnectionString("cadenaSql"));
            
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();
        }
    }
}
