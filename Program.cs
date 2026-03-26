// ============================================================
// SISTEMA DE INVENTARIO - Clase 1.1
// Estado: Mensaje de bienvenida
// ============================================================

using System.Reflection;

var assembly = Assembly.GetExecutingAssembly();
var version = assembly.GetName().Version;


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

int cantidadProductos = 0;
decimal valorTotalDelInventario = 0.00m;
bool sistemaActivo = true;
string nombreSistema = "Sistema de Gestion de Inventario";

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


//MostarBanner();

Console.Write("Ingrese un comando o ingrese salir para terminar: ");
string? entrada = Console.ReadLine();

if (string.IsNullOrWhiteSpace(entrada) || entrada.ToLower() == "salir")
{
    Console.WriteLine("Saliendo del programa...");
    Environment.Exit(0);
}

/*
Console.WriteLine();
Console.WriteLine("Estructura del proyecto:");
Console.WriteLine("Configuracion .csproj");
Console.WriteLine("Carpet src/ creada");
Console.WriteLine("Metadatos configurados");
Console.WriteLine();
Console.WriteLine("Proximo paso: Agregar argumentos CL y configuracion de repositorio en github");
Console.WriteLine("==========================================");
*/
// Funciones
/*
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
*/
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
