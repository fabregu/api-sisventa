using BLL.SistemaVenta.Servicios;
using BLL.SistemaVenta.Servicios.Contrato;
using DAL.SistemaVenta.DBContext;
using DAL.SistemaVenta.Repositorios;
using DAL.SistemaVenta.Repositorios.Contrato;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utility.SistemaVenta;

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

            services.AddAutoMapper(typeof(AutoMaperProfile));

            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IMenuService, MenuService>();
        }
    }
}
