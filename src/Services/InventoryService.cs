using InventarioApp.Models;
using InventarioApp.Repositories;
using InventarioApp.Infrastructure;
using InventarioApp.Factories;

namespace InventarioApp.Services;

public class InventoryService
{
    private readonly InMemoryProductoRepository _repository;
    private readonly JsonInventoryStorage _storage;
    private readonly string _rutaInventario;

    public InventoryService(string rutaInventario = "inventario.json")
    {
        _repository = new InMemoryProductoRepository();
        _storage = new JsonInventoryStorage();
        _rutaInventario = rutaInventario;

        CargarInventario();
    }

    public void CargarInventario()
    {
        if (!File.Exists(_rutaInventario))
        {
            var productos = _storage.Cargar(_rutaInventario);
            foreach (Producto producto in productos)
            {
                _repository.Agregar(producto);
            }
        }
    }

    public void AgregarProducto(string nombre, decimal precio, int cantidad, CategoriaProducto categoria)
    {
        var producto = ProductoFactory.Crear(nombre, precio, cantidad, categoria);
        _repository.Agregar(producto);
        _Persistir();
    }

    public IEnumerable<Producto> ObtenerTodosLosProductos()
    {
        return _repository.ObtenerTodos();
    }

    public Producto? ObtenerProductoPorID(int id)
    {
        return _repository.ObtenerPorID(id);
    }

    public void ActualizarProducto(int id, string nombre, decimal precio, int cantidad, CategoriaProducto categoria)
    {
        var producto = _repository.ObtenerPorID(id);
        if (producto != null) {
            producto.Nombre = nombre;
            producto.Precio = precio;
            producto.Cantidad = cantidad;
            producto.Categoria = categoria;
            _repository.Actualizar(producto);
            _Persistir();
        }
    }

    public void EliminarProducto(int id)
    {
        _repository.Eliminar(id);
        _Persistir();
    }

    public IEnumerable<Producto> BuscarPorCategoria(CategoriaProducto categoria)
    {
        return _repository.BuscarPorCategoria(categoria);
    }

    public IEnumerable<Producto> BuscarPorNombre(string nombre)
    {
        return _repository.BuscarPorNombre(nombre);
    }

    public IEnumerable<Producto> ObtenerProductosBajoStock(int stockMinimo = 5)
    {
        return _repository.BuscarPorRangoPrecio(0, stockMinimo);
    }

    public decimal ObtenerValorTotalInventario()
    {
        return _repository.ObtenerValorTotalInventario();
    }

    public decimal ObtenerPrecioPromedio()
    {
        return _repository.ObtenerPrecioPromedio();
    }

    public Producto? ObtenerProductoMasCaro()
    {
        return _repository.ObtenerProductoMasCaro();
    }

    // Metodo Reportes
    public string GenerarResumen()
    {
        var productos = _repository.ObtenerTodos();
        var generador = new ReportGenerator(productos);
        return generador.GenerarReporte();
    }

    public string GenerarReporteStockBajo(int stockMinimo = 5)
    {
        var productos = _repository.ObtenerTodos();
        var generador = new ReportGenerator(productos);
        return generador.GeneraReporteStockBajo(stockMinimo);
    }

    public string GenerarTopProductos(int cantidad = 5)
    {
        var productos = _repository.ObtenerTodos();
        var generador = new ReportGenerator(productos);
        return generador.GenerarTopProductos(cantidad);
    }

    public string ExportarCsv()
    {
        var productos = _repository.ObtenerTodos();
        var generador = new ReportGenerator(productos);
        return generador.ExportarCsv();
    }

    private void _Persistir()
    {
        _storage.CrearBackup(_rutaInventario);
        var productos = _repository.ObtenerTodos().ToList();
        _storage.Guardar(productos, _rutaInventario);
    }
}