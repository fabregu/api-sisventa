﻿
namespace DTO.SistemaVenta
{
    public class DashboardDTO
    {
        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public List<VentasSemanaDTO>? VentasUltimaSemana { get; set; }
    }
}
