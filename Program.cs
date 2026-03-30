using System.Runtime.CompilerServices;
using InventarioApp.Models;
using InventarioApp.Services;

var service = new InventoryService();
bool activo = true;

while (activo)
{
    MostrarMenu();
    string option = Console.ReadLine()?.ToLower() ?? "";

    switch (option)
    {
        case "1":
            AgregarProducto();
            break;
        case "2":
            ListarProductos();
            break;
        case "3":
            BuscarPorId();
            break;
        case "4":
            EliminarProducto();
            break;
        case "5":
            BuscarPorCategoria();
            break;
        case "6":
            MostrarResumen();
            break;
        case "7":
            MostrarStockBajo();
            break;
        case "8":
            MostrarEstadisticas();
            break;
        case "9":
            ExportarCsv();
            break;
        case "10":
            activo = false;
            Console.WriteLine("Saliendo del programa...");
            break;
        default:
            Console.WriteLine("Opcion no valida");
            break;
    }

    void MostrarMenu()
    {
        Console.WriteLine("====================== InventarioApp====================");
        Console.WriteLine("1. Agregar Producto");
        Console.WriteLine("2. Listar Productos");
        Console.WriteLine("3. Buscar Producto por ID");
        Console.WriteLine("4. Eliminar Producto");
        Console.WriteLine("5. Buscar Producto por Categoria");
        Console.WriteLine("6. Mostrar Resumen");
        Console.WriteLine("7. Mostrar Stock Bajo");
        Console.WriteLine("8. Mostrar Estadisticas");
        Console.WriteLine("9. Exportar CSV");
        Console.WriteLine("10. Salir");
        Console.WriteLine("Ingrese una opcion: ");
    }

    void AgregarProducto()
    {
        Console.WriteLine("Ingrese el nombre del producto: ");
        string nombre = Console.ReadLine() ?? "";

        Console.WriteLine("Ingrese el precio del producto: ");
        decimal precio = decimal.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Ingrese la cantidad del producto: ");
        int cantidad = int.Parse(Console.ReadLine() ?? "0");

        Console.WriteLine("Ingrese la categoria del producto (Electronica, Ropa, Alimentos, Hogar, Deportes, Libros, Muebles, Otros): ");
        string categoriaStr = Console.ReadLine() ?? "Otros";

        if (Enum.TryParse<CategoriaProducto>(categoriaStr, ignoreCase: true, out var categoria))
        {
            service.AgregarProducto(nombre, precio, cantidad, categoria);
            Console.WriteLine("Producto agregado correctamente");
        } else {
            Console.WriteLine("Categoria no valida");
        }    
    }

    void ListarProductos()
    {
        var productos = service.ObtenerTodosLosProductos();
        if (!productos.Any())
        {
            Console.WriteLine("\nNo hay productos en el inventario");
            return;
        }

        Console.WriteLine("\nLista de productos:");
        foreach (var producto in productos)
        {
            Console.WriteLine($"ID: {producto.Id} - Nombre: {producto.Nombre} - Precio: {producto.Precio:C2} - Cantidad: {producto.Cantidad} - Categoria: {producto.Categoria}");
        }
    }

    void BuscarPorId()
    {
        Console.WriteLine("\nIngrese el ID del producto: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        var producto = service.ObtenerProductoPorID(id);
        if (producto != null)
        {
            Console.WriteLine($"Producto encontrado: {producto.Nombre} - Precio: {producto.Precio:C2} - Cantidad: {producto.Cantidad} - Categoria: {producto.Categoria}");
        } else {
            Console.WriteLine("Producto no encontrado");
        }
    }

    void EliminarProducto()
    {
        Console.WriteLine("\nIngrese el ID del producto a eliminar: ");
        int id = int.Parse(Console.ReadLine() ?? "0");
        
        var producto = service.ObtenerProductoPorID(id);
        if (producto != null)
        {
            service.EliminarProducto(id);
            Console.WriteLine("Producto eliminado correctamente");
        } else {
            Console.WriteLine("Producto no encontrado");
        }
    }

    void BuscarPorCategoria()
    {
        Console.WriteLine("\nIngrese la categoria del producto: ");
        Console.WriteLine("\nCategorias: (Electronica, Ropa, Alimentos, Hogar, Deportes, Libros, Muebles, Otros)");
        string categoriaStr = Console.ReadLine() ?? "Otros";

        if (Enum.TryParse<CategoriaProducto>(categoriaStr, ignoreCase: true, out var categoria))
        {
            var productos = service.BuscarPorCategoria(categoria);
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos en esta categoria");
                return;
            }

            Console.WriteLine($"Productos en la categoria {categoria}:");
            foreach (var producto in productos)
            {
                Console.WriteLine($"ID: {producto.Id} - Nombre: {producto.Nombre} - Precio: {producto.Precio:C2} - Cantidad: {producto.Cantidad} - Categoria: {producto.Categoria}");
            }
        } else {
            Console.WriteLine("Categoria no valida");
        }
    }

    void MostrarResumen()
    {
        var resumen = service.GenerarResumen(); 
        Console.WriteLine(resumen);
    }

    void MostrarStockBajo()
    {
        var reporte = service.GenerarReporteStockBajo();
        Console.WriteLine(reporte);
    }

    void MostrarEstadisticas()
    {
        Console.WriteLine("\nEstadisticas del inventario:");
        Console.WriteLine($"Valor total del inventario: {service.ObtenerValorTotalInventario():C2}");
        Console.WriteLine($"Precio promedio: {service.ObtenerPrecioPromedio():C2}");
        
        var masCaro = service.ObtenerProductoMasCaro();
        if (masCaro != null)
        {
            Console.WriteLine($"Producto mas caro: {masCaro.Nombre} - Precio: {masCaro.Precio:C2}");
        }
    }

    void ExportarCsv()
    {
        var csv = service.ExportarCsv();
        Console.WriteLine(csv);
    }
}


/*
using InventarioApp.Infrastructure;
using InventarioApp.Models;
using InventarioApp.Factories;

var productos = new List<Producto>
{
    ProductoFactory.Crear(nombre: "Laptop Dell XPS 13", precio: 1200, cantidad: 5, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Mouse Logitech MX Master", precio: 99, cantidad: 20, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Teclado Mecánico", precio: 150, cantidad: 3, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Silla Ergonómica Herman Miller", precio: 500, cantidad: 8, CategoriaProducto.Muebles),
    ProductoFactory.Crear(nombre: "Escritorio Stand-up", precio: 300, cantidad: 2, CategoriaProducto.Muebles),
    ProductoFactory.Crear(nombre: "Monitor LG 27英寸", precio: 250, cantidad: 4, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Impresora HP LaserJet", precio: 300, cantidad: 3, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Altavoz Bluetooth JBL", precio: 100, cantidad: 10, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Teclado Mecánico", precio: 150, cantidad: 3, CategoriaProducto.Electronica),
    ProductoFactory.Crear(nombre: "Silla Ergonómica Herman Miller", precio: 500, cantidad: 8, CategoriaProducto.Muebles),
    ProductoFactory.Crear(nombre: "Escritorio Stand-up", precio: 300, cantidad: 2, CategoriaProducto.Muebles),
};

var generador = new ReportGenerator(productos);

Console.WriteLine(generador.GenerarReporte());
Console.WriteLine("\n");

Console.WriteLine(generador.GeneraReporteStockBajo());
Console.WriteLine("\n");

Console.WriteLine(generador.GenerarTopProductos());
Console.WriteLine("\n");

Console.WriteLine(generador.ExportarCsv());
Console.WriteLine("\n");

Console.WriteLine(generador.ExportarResumenJson());
*/

/*
using InventarioApp.Factories;
using InventarioApp.Models;
using InventarioApp.Repositories;
using InventarioApp.Infrastructure;

Console.WriteLine("====================== InventarioApp====================");

var fileManager = new FileManager();
string contenido = "Inventario actualizado";
fileManager.Escribir(ruta: "inventario.txt", contenido);

string leerContenido = fileManager.Leer(ruta: "inventario.txt");
Console.WriteLine($"Contenido del archivo: {leerContenido}");

var repository = new InMemoryProductoRepository();

Producto laptop = ProductoFactory.Crear(nombre: "Laptop Dell XPS 13", precio: 1200, cantidad: 5, CategoriaProducto.Electronica);
Producto mouse = ProductoFactory.Crear(nombre: "Mouse Logitech MX Master", precio: 99, cantidad: 20, CategoriaProducto.Electronica);
Producto teclado = ProductoFactory.Crear(nombre: "Teclado Mecánico", precio: 150, cantidad: 3, CategoriaProducto.Electronica);
Producto silla = ProductoFactory.Crear(nombre: "Silla Ergonómica Herman Miller", precio: 500, cantidad: 8, CategoriaProducto.Muebles);
Producto escritorio = ProductoFactory.Crear(nombre: "Escritorio Stand-up", precio: 300, cantidad: 2, CategoriaProducto.Muebles);

repository.Agregar(laptop);
repository.Agregar(mouse);
repository.Agregar(teclado);
repository.Agregar(silla);
repository.Agregar(escritorio);

Console.WriteLine($"Productos agregados: {repository.CantidadProductos}");

IEnumerable<Producto> electronicos = repository.BuscarPorCategoria(CategoriaProducto.Electronica);
Console.WriteLine($"Productos electronicos: {electronicos.Count()}");

foreach (Producto producto in electronicos)
{
    Console.WriteLine($" {producto.Nombre} : {producto.Precio:C2}");
}

IEnumerable<Producto> conMouse = repository.BuscarPorNombre("mouse");
Console.WriteLine($"\nProductos con mouse: {conMouse.Count()}");

foreach (Producto producto in conMouse)
{
    Console.WriteLine($" {producto.Nombre} : {producto.Precio:C2}");
}

IEnumerable<string> nombres = repository.ObtenerNombres();
Console.WriteLine($"\nTodos los nombres de los productos: {string.Join(", ", nombres)}");

bool hayStockBajo = repository.HayStockBajo();
Console.WriteLine($"\nHay stock bajo: {hayStockBajo}");

/*
// ============================================================
// SISTEMA DE INVENTARIO - Clase 1.1
// Estado: Mensaje de bienvenida
// ============================================================

using System.Reflection;

var assembly = Assembly.GetExecutingAssembly();
var version = assembly.GetName().Version;
int cantidadProductos = 0;
decimal valorTotalDelInventario = 0.00m;
bool sistemaActivo = true;
string nombreSistema = "Sistema de Gestion de Inventario";

MostarBanner();

bool continuar = true;
while (continuar)
{
    MostrarMenu();
    string comando = LeerEntradaInventario("inventario: ");
    Console.WriteLine($"Comando ingresado: " {comando});
    //continuar = ProcesarComando(comando);
    comando = false;
}

bool ProcesarComando(string comando)
{
    switch (comando.ToLower())
    {
        case "listar":
            ListarProductos();
            return true;
        case "agregar":
            AgregarProducto();
            return true;
        case "buscar":
            BuscarProducto();
            return true;
        case "salir":
            return false;
        default:
            Console.WriteLine($"Error: comando desconocido '{comando}'");
            return true;
    }
}

void ListarProductos()
{
    Console.WriteLine($"Total: {cantidadProductos} productos en el inventario");
    Console.WriteLine($"Valor total: ${valorTotalDelInventario:N2}");
}

void AgregarProducto()
{
    Console.WriteLine("Agregar producto (Modulo 3)...");
}

void BuscarProducto()
{
    Console.WriteLine("Buscar producto (Modulo 4)...");
}

string LeerEntradaInventario(string prompt)
{
    string salida = "El prompt ingresado es: " + prompt;
    return salida;
}

if (args.Length > 0)
{
    switch (args[0].ToLower())
    {
        case "--help":
            MostrarAyuda();
            Environment.Exit(0);
            break;

        case "--version":
            Console.WriteLine($"InventarioApp Version: {version}");
            Environment.Exit(0);
            break;
            
        default:
            Console.WriteLine($"Error: comando desconocido '{args[0]}'");
            Console.WriteLine("Usa --help para ver las opciones disponibles");
            Environment.Exit(2);
            break;
    }
}



/*
string? nombre = null;
int longitud = nombre.Length;
Console.WriteLine($"La longitud del nombre es: {longitud}");

// Problema: readline puede devolver null
Console.Write("Ingrese un valor: ");
string? entrada = Console.ReadLine();
int? longitud = entrada?.Length;

// Solucion Operador coalescing ??
//string comando = string.IsNullOrEmpty(entrada) ? "salir" : entrada;
string comandoLimpio = string.IsNullOrWhiteSpace(entrada) ? "salir" : entrada.Trim().ToLower();
Console.WriteLine($"Longitud: {longitud ?? 0}");
Console.WriteLine($"Comando: {comandoLimpio}");


Console.WriteLine("Estado del sistema");
Console.WriteLine($"Nombre: {nombreSistema}");
Console.WriteLine($"Cantidad de productos registrados: {cantidadProductos}");
Console.WriteLine($"Valor total del inventario: ${valorTotalDelInventario:N2}");
Console.WriteLine($"Sistema activo: {(sistemaActivo ? "Si" : "No")}");
Console.Write("Ingrese una cantidad: ");
string? input = Console.ReadLine();


// Conversion segura TryParse
if (int.TryParse(input, out int cantidad))
{
    Console.Write($"Cantidad valida: {cantidad} \n");
    cantidadProductos = cantidad;
} else {
    Console.WriteLine("Error: Dene ingresar un numero entero");
}

Console.Write("Ingrese un precio: ");
string? inputPrecio = Console.ReadLine();
if (decimal.TryParse(inputPrecio, out decimal precio))
{
    Console.Write($"Precio valido: {precio} \n");
    valorTotalDelInventario = cantidad * precio;
    Console.WriteLine($"Valor total del inventario: ${valorTotalDelInventario:N2}");
} else {
    Console.WriteLine("Error: Dene ingresar un numero decimal");
}


// Loop de nullabilidad
Console.WriteLine("Comandos: listar, agregar, buscar, salir");
Console.WriteLine();

while (sistemaActivo)
{
    Console.Write("inventario: ");
    string? entrada = Console.ReadLine();

    string comando = string.IsNullOrWhiteSpace(entrada) ? "salir" : entrada.Trim().ToLower();
    switch (comando)
    {
        case "salir":
            Console.WriteLine("Saliendo del programa...");
            sistemaActivo = false;
            break;
        case "listar":
            Console.WriteLine($"Lista de productos: {cantidadProductos}");
            break;
        case "":
            break;
        default:
            Console.WriteLine($"Error: comando desconocido '{comando}'");
            Console.WriteLine("Comandos disponibles: listar, agregar, buscar, salir");
            break;
    }
}

/*
Console.Write("Ingrese un comando o ingrese salir para terminar: ");
string? comandoSalir = Console.ReadLine();

if (string.IsNullOrWhiteSpace(comandoSalir) || comandoSalir.ToLower() == "salir")
{
    Console.WriteLine("Saliendo del programa...");
    Environment.Exit(0);
}
*/
/*
Console.WriteLine();
Console.WriteLine("Estructura del proyecto:");
Console.WriteLine("Configuracion .csproj");
Console.WriteLine("Carpet src/ creada");
Console.WriteLine("Metadatos configurados");
Console.WriteLine();
Console.WriteLine("Proximo paso: Agregar argumentos CL y configuracion de repositorio en github");
Console.WriteLine("==========================================");

// Funciones

void MostarBanner()
{
    Console.WriteLine("==========================================");
    Console.WriteLine("    SISTEMA DE GESTIÓN DE INVENTARIO      ");
    Console.WriteLine("==========================================");
    Console.WriteLine();
    Console.WriteLine($"Version: {version}");
    Console.WriteLine($"Plataforma: {Environment.OSVersion}");
    Console.WriteLine($".NET Version: {Environment.Version}");
    Console.WriteLine();
}

void MostrarAyuda()
{
    Console.WriteLine("USO: InventarioApp [comando] [opciones]");
    Console.WriteLine();
    Console.WriteLine("COMANDOS:");
    Console.WriteLine("  --help, -h      Muestra esta ayuda");
    Console.WriteLine("  --version, -v   Muestra la version del programa");
    Console.WriteLine();
    Console.WriteLine("EJEMPLOS:");
    Console.WriteLine(" dotnet run -- --help");
    Console.WriteLine(" dotnet run -- --version");
}

void MostarMenu()
{
    Console.WriteLine("\nMenu Principal");
    Console.WriteLine("1. listar - Listar productos");
    Console.WriteLine("2. agregar - Agregar producto");
    Console.WriteLine("3. buscar - Buscar producto");
    Console.WriteLine("4. salir - Salir");
}
*/