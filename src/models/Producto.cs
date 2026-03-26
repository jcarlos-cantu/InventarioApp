namespace InventarioApp.Models;

public class Producto
{
    private string _nombre = "";
    private decimal _precio;
    private int _cantidad;
    
    public int Id { get; set; }

    public string Nombre 
    { 
        get => _nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El nombre no puede ser nulo o vacío", nameof(value));
            }
            _nombre = value.Trim();
        } 
    }
    public decimal Precio
    {
        get => _precio;
        set{
            if(value < 0)
            {
                throw new ArgumentException("El precio no puede ser negativo", nameof(value));
            }
            _precio = value;
        } 
    }
    public int Cantidad
    {
        get => _cantidad;
        set{
            if(value < 0)
            {
                throw new ArgumentException("La cantidad no puede ser negativa", nameof(value));
            }
            _cantidad = value;
        } 
    }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public CategoriaProducto Categoria { get; set; }
    public EstadoProducto Estado { get; set; } = EstadoProducto.Activo;

    public decimal ValorTotal => Precio * Cantidad;

    public override string ToString() => $"[{Id}] {Nombre} - {Precio:C2} - {Cantidad} - {ValorTotal:C2}";
}