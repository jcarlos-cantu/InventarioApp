namespace InventarioApp.Repositories;

using InventarioApp.Models;

/// <summary>
/// Interfaz para el repositorio de productos
/// </summary>
public interface IProductoRepository
{

    /// <summary>
    /// Agrega un producto al repositorio
    /// </summary>
    /// <param name="producto">El producto a agregar</param>
    void Agregar(Producto producto);

    /// <summary>
    /// Obtiene un producto por su ID
    /// </summary>
    /// <param name="id">El ID del producto</param>
    /// <returns>El producto encontrado o null si no existe</returns>
    Producto? ObtenerPorID(int id);

    /// <summary>
    /// Obtiene todos los productos del repositorio
    /// </summary>
    /// <returns>Una colección de productos</returns>
    IEnumerable<Producto> ObtenerTodos();

    /// <summary>
    /// Actualiza un producto en el repositorio
    /// </summary>
    /// <param name="producto">El producto a actualizar</param>
    /// <returns>True si el producto se actualizó correctamente, false si no existe</returns>
    bool Actualizar(Producto producto);

    /// <summary>
    /// Elimina un producto del repositorio
    /// </summary>
    /// <param name="id">El ID del producto a eliminar</param>
    /// <returns>True si el producto se eliminó correctamente, false si no existe</returns>
    bool Eliminar(int id);

    /// <summary>
    /// Obtiene la cantidad de productos en el repositorio
    /// </summary>
    /// <returns>La cantidad de productos</returns>
    int CantidadProductos { get; }
}
