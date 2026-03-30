using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using InventarioApp.Models;

namespace InventarioApp.Infrastructure;

public class ReportGenerator
{
    private readonly IEnumerable<Producto> _productos;

    public ReportGenerator(IEnumerable<Producto> productos)
    {
        _productos = productos ?? throw new ArgumentNullException(nameof(productos));
    }

    public string GenerarReporte()
    {
        var sb = new StringBuilder();

        sb.AppendLine("=== RESUMEN DE INVENTARIO ===");
        sb.AppendLine($"Total de productos: {_productos.Count()}");
        sb.AppendLine($"Valor total del inventario: {_productos.Sum(p => p.ValorTotal):C2}");

        var productosPorCategoria = _productos
            .GroupBy(p => p.Categoria)
            .Select(g => new { Categoria = g.Key, Cantidad = g.Count() });

        sb.AppendLine("\nProductos por categoria:");
        foreach (var item in productosPorCategoria)
        {
            sb.AppendLine($"- {item.Categoria}: {item.Cantidad}");
        }

        return sb.ToString();
    }

    public string GeneraReporteStockBajo(int minimo = 5)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"=== PRODUCTOS CON STOCK BAJO ({minimo}) ===");

        var stockBajo = _productos
            .Where(p => p.Cantidad < minimo)
            .OrderBy(p => p.Precio);

        if (!stockBajo.Any())
        {
            sb.AppendLine("No hay productos con stock bajo");
            return sb.ToString();
        }

        foreach (var producto in stockBajo)
        {
            sb.AppendLine(
                $"ID: {producto.Id} | Nombre: {producto.Nombre} | Stock: {producto.Cantidad} | ${producto.Precio:F2}"
            );
        }

        return sb.ToString();
    }

    public string GenerarTopProductos(int cantidad = 5)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"=== TOP {cantidad} PRODUCTOS POR PRECIO ===");

        var topProductos = _productos
            .OrderByDescending(p => p.Precio)
            .Take(cantidad);

        if (!topProductos.Any())
        {
            sb.AppendLine("No hay productos para mostrar");
            return sb.ToString();
        }

        int contador = 1;
        foreach (var producto in topProductos)
        {
            sb.AppendLine(
                $"{contador}. Nombre: {producto.Nombre} | Cantidad: {producto.Cantidad} | Valor: ${producto.ValorTotal:F2}"
            );
            contador++;
        }

        return sb.ToString();
    }

    public string ExportarCsv()
    {
        var sb = new StringBuilder();

        sb.AppendLine("ID,Nombre,Precio,Cantidad,Categoria,ValorTotal");

        foreach (var producto in _productos)
        {
            sb.AppendLine(
                $"{producto.Id},{producto.Nombre},{producto.Precio},{producto.Cantidad},{producto.Categoria},{producto.ValorTotal}"
            );
        }

        return sb.ToString();
    }

    public string ExportarResumenJson()
    {
        var resumen = new
        {
            TotalProductos = _productos.Count(),
            ValorTotalInventario = _productos.Sum(p => p.ValorTotal),
            ProductosPorCategoria = _productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key, Cantidad = g.Count() }),
            TopProductos = _productos
                .OrderByDescending(p => p.ValorTotal)
                .Take(5)
                .Select(p => new { p.Id, p.Nombre, p.Cantidad, ValorTotal = p.ValorTotal })
        };

        return JsonSerializer.Serialize(
            resumen,
            new JsonSerializerOptions { WriteIndented = true }
        );
    }
}