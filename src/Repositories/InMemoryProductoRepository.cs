namespace InventarioApp.Repositories;

using InventarioApp.Models;
public class InMemoryProductoRepository : IProductoRepository
{
    private readonly List<Producto> _productos = new();

    private int _proximoId = 1;

    public void Agregar(Producto producto)
    {
        producto.Id = _proximoId++;
        _productos.Add(producto);
    }

    public Producto? ObtenerPorID(int id)
    {
        return _productos.FirstOrDefault((Producto p) => p.Id == id);
    }

    public IEnumerable<Producto> ObtenerTodos()
    {
        return _productos.AsReadOnly();
    }

    public bool Actualizar(Producto producto)
    {
        Producto? existente = ObtenerPorID(producto.Id);
        if (existente == null) return false;

        existente.Nombre = producto.Nombre;
        existente.Precio = producto.Precio;
        existente.Cantidad = producto.Cantidad;
        existente.Categoria = producto.Categoria;
        existente.Estado = producto.Estado;
        return true;
    }

    public bool Eliminar(int id)
    {
        var producto = ObtenerPorID(id);
        if (producto == null) return false;

        return _productos.Remove(producto);
    }

    public int CantidadProductos => _productos.Count;

    public IEnumerable<Producto> BuscarPorCategoria(CategoriaProducto categoria)
    {
        return _productos.Where((Producto p) => p.Categoria == categoria);
    }

    public IEnumerable<Producto> BuscarPorNombre(string nombre)
    {
        return _productos.Where((Producto p) => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Producto> BuscarPorRangoPrecio(decimal precioMinimo, decimal precioMaximo)
    {
        return _productos.Where((Producto p) => p.Precio >= precioMinimo && p.Precio <= precioMaximo);
    }

    public IEnumerable<string> ObtenerNombres()
    {
        return _productos.Select((Producto p) => p.Nombre);
    }

    public bool HayStockBajo()
    {
        return _productos.Any((Producto p) => p.Cantidad < 5);
    }

    public IEnumerable<Producto> ObtenerOrdenadosPorPrecio()
    {
        return _productos.OrderBy((Producto p) => p.Precio);
    }

    public IEnumerable<Producto> ObtenerTopPorPrecio(int cantidad)
    {
        return _productos.OrderByDescending((Producto p) => p.Precio).Take(cantidad);
    }

    public IEnumerable<IGrouping<CategoriaProducto, Producto>> AgruparPorCategoria()
    {
        return _productos.GroupBy((Producto p) => p.Categoria);
    }

    public Dictionary<CategoriaProducto, int> ObtenerCantidadPorCategoria()
    {
        return _productos
        .GroupBy((Producto p) => p.Categoria)
        .ToDictionary((IGrouping<CategoriaProducto, Producto> g) => g.Key, (IGrouping<CategoriaProducto, Producto> g) => g.Count());
    }

    public decimal ObtenerValorTotalInventario()
    {
        return _productos.Sum((Producto p) => p.ValorTotal);
    }

    public decimal ObtenerPrecioPromedio()
    {
        if (_productos.Count == 0) return 0;
        return _productos.Average((Producto p) => p.Precio);
    }

    public Producto? ObtenerProductoMasCaro()
    {
        return _productos.MaxBy((Producto p) => p.Precio);
    }

    public Dictionary<CategoriaProducto, decimal> ObtenerValorPorCategoria()
    {
        return _productos
        .GroupBy((Producto p) => p.Categoria)
        .ToDictionary((IGrouping<CategoriaProducto, Producto> g) => g.Key, (IGrouping<CategoriaProducto, Producto> g) => g.Sum((Producto p) => p.ValorTotal));
    }
}