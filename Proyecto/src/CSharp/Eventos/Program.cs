using Eventos.Entidades;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Cliente> clientes = new List<Cliente>
{
    new Cliente(10000001, "Juan Pérez", "juan@example.com", "111111111", "pass123"),
    new Cliente(10000002, "María López", "maria@example.com", "222222222", "clave456"),
    new Cliente(10000003, "Carlos Gómez", "carlos@example.com", "333333333", "qwerty"),
    new Cliente(10000004, "Laura Torres", "laura@example.com", "444444444", "123456"),
    new Cliente(10000005, "Sofía Díaz", "sofia@example.com", "555555555", "sofia2025"),
    new Cliente(10000006, "Pedro Ruiz", "pedro@example.com", "666666666", "pedrito"),
    new Cliente(10000007, "Ana Martínez", "ana@example.com", "777777777", "anita77"),
    new Cliente(10000008, "Diego Fernández", "diego@example.com", "888888888", "dieguito"),
    new Cliente(10000009, "Valentina Castro", "valentina@example.com", "999999999", "valen99"),
    new Cliente(10000010, "Mateo Sánchez", "mateo@example.com", "101010101", "mateo101")
};

List<TipoEvento> tiposEvento = new List<TipoEvento>
{
    new TipoEvento(1, "Concierto"),
    new TipoEvento(2, "Teatro"),
    new TipoEvento(3, "Deporte"),
    new TipoEvento(4, "Cine"),
    new TipoEvento(5, "Festival"),
    new TipoEvento(6, "Conferencia"),
    new TipoEvento(7, "Exposición"),
    new TipoEvento(8, "Taller"),
    new TipoEvento(9, "Fiesta"),
    new TipoEvento(10, "Charla")
};


List<Evento> eventos = new List<Evento>
{
    new Evento(1, "Rock Nacional", tiposEvento[0], DateTime.Now.AddDays(10), DateTime.Now.AddDays(11)),
    new Evento(2, "Obra de Teatro Infantil", tiposEvento[1], DateTime.Now.AddDays(5), DateTime.Now.AddDays(6)),
    new Evento(3, "Partido de Fútbol", tiposEvento[2], DateTime.Now.AddDays(15), DateTime.Now.AddDays(15)),
    new Evento(4, "Estreno de Película", tiposEvento[3], DateTime.Now.AddDays(1), DateTime.Now.AddDays(1)),
    new Evento(5, "Festival de Jazz", tiposEvento[4], DateTime.Now.AddDays(20), DateTime.Now.AddDays(22)),
    new Evento(6, "Conferencia Tech", tiposEvento[5], DateTime.Now.AddDays(7), DateTime.Now.AddDays(7)),
    new Evento(7, "Exposición Arte Moderno", tiposEvento[6], DateTime.Now.AddDays(30), DateTime.Now.AddDays(40)),
    new Evento(8, "Taller de Fotografía", tiposEvento[7], DateTime.Now.AddDays(12), DateTime.Now.AddDays(13)),
    new Evento(9, "Fiesta Electrónica", tiposEvento[8], DateTime.Now.AddDays(3), DateTime.Now.AddDays(4)),
    new Evento(10, "Charla Motivacional", tiposEvento[9], DateTime.Now.AddDays(8), DateTime.Now.AddDays(8))
};

List<Tarifa> tarifas = new List<Tarifa>
{
    new Tarifa(1, 100, "General"),
    new Tarifa(2, 50, "VIP"),
    new Tarifa(3, 30, "Estudiante"),
    new Tarifa(4, 20, "Jubilado"),
    new Tarifa(5, 10, "Niños"),
    new Tarifa(6, 80, "Premium"),
    new Tarifa(7, 60, "Palco"),
    new Tarifa(8, 40, "Lateral"),
    new Tarifa(9, 70, "Preferencial"),
    new Tarifa(10, 25, "Promo")
};

List<Entrada> entradas = new List<Entrada>
{
    new Entrada(1, 5000, eventos[0], tarifas[0]),
    new Entrada(2, 10000, eventos[0], tarifas[1]),
    new Entrada(3, 3000, eventos[1], tarifas[2]),
    new Entrada(4, 2000, eventos[1], tarifas[3]),
    new Entrada(5, 1500, eventos[3], tarifas[4]),
    new Entrada(6, 8000, eventos[5], tarifas[5]),
    new Entrada(7, 6000, eventos[6], tarifas[6]),
    new Entrada(8, 4000, eventos[7], tarifas[7]),
    new Entrada(9, 7000, eventos[8], tarifas[8]),
    new Entrada(10, 2500, eventos[9], tarifas[9])
};

List<Local> locales = new List<Local>
{
    new Local(1, "Teatro Colón", "CABA"),
    new Local(2, "Luna Park", "CABA"),
    new Local(3, "Estadio Monumental", "CABA"),
    new Local(4, "Movistar Arena", "CABA"),
    new Local(5, "Cine Gaumont", "CABA"),
    new Local(6, "Centro Cultural Recoleta", "CABA"),
    new Local(7, "Usina del Arte", "CABA"),
    new Local(8, "Estadio Único", "La Plata"),
    new Local(9, "Teatro Argentino", "La Plata"),
    new Local(10, "Club Atenas", "La Plata")
};

List<Sector> sectores = new List<Sector>
{
    new Sector(1, locales[0], 50),
    new Sector(2, locales[1], 100),
    new Sector(3, locales[2], 200),
    new Sector(4, locales[3], 150),
    new Sector(5, locales[4], 80),
    new Sector(6, locales[5], 60),
    new Sector(7, locales[6], 70),
    new Sector(8, locales[7], 180),
    new Sector(9, locales[8], 90),
    new Sector(10, locales[9], 120)
};

List<Funcion> funciones = new List<Funcion>
{
    new Funcion(1, eventos[0], DateTime.Now.AddDays(10)),
    new Funcion(2, eventos[1], DateTime.Now.AddDays(5)),
    new Funcion(3, eventos[2], DateTime.Now.AddDays(15)),
    new Funcion(4, eventos[3], DateTime.Now.AddDays(1)),
    new Funcion(5, eventos[4], DateTime.Now.AddDays(20)),
    new Funcion(6, eventos[5], DateTime.Now.AddDays(7)),
    new Funcion(7, eventos[6], DateTime.Now.AddDays(30)),
    new Funcion(8, eventos[7], DateTime.Now.AddDays(12)),
    new Funcion(9, eventos[8], DateTime.Now.AddDays(3)),
    new Funcion(10, eventos[9], DateTime.Now.AddDays(8))
};

List<RegistroCompra> registros = new List<RegistroCompra>
{
    new RegistroCompra(1, clientes[0].DNI, entradas[0].idEntrada, DateTime.Now),
    new RegistroCompra(2, clientes[1].DNI, entradas[1].idEntrada, DateTime.Now.AddDays(-1)),
    new RegistroCompra(3, clientes[2].DNI, entradas[2].idEntrada, DateTime.Now.AddDays(-2)),
    new RegistroCompra(4, clientes[3].DNI, entradas[3].idEntrada, DateTime.Now.AddDays(-3)),
    new RegistroCompra(5, clientes[4].DNI, entradas[4].idEntrada, DateTime.Now.AddDays(-4)),
    new RegistroCompra(6, clientes[5].DNI, entradas[5].idEntrada, DateTime.Now.AddDays(-5)),
    new RegistroCompra(7, clientes[6].DNI, entradas[6].idEntrada, DateTime.Now.AddDays(-6)),
    new RegistroCompra(8, clientes[7].DNI, entradas[7].idEntrada, DateTime.Now.AddDays(-7)),
    new RegistroCompra(9, clientes[8].DNI, entradas[8].idEntrada, DateTime.Now.AddDays(-8)),
    new RegistroCompra(10, clientes[9].DNI, entradas[9].idEntrada, DateTime.Now.AddDays(-9))
};

/* List<QR> qrs = new List<QR>
{
    new QR { url = "https://miapp.com/qr/1" },
    new QR { url = "https://miapp.com/qr/2" },
    new QR { url = "https://miapp.com/qr/3" },
    new QR { url = "https://miapp.com/qr/4" },
    new QR { url = "https://miapp.com/qr/5" },
    new QR { url = "https://miapp.com/qr/6" },
    new QR { url = "https://miapp.com/qr/7" },
    new QR { url = "https://miapp.com/qr/8" },
    new QR { url = "https://miapp.com/qr/9" },
    new QR { url = "https://miapp.com/qr/10" }
}; */

app.Urls.Add("http://localhost:5090");

app.MapGet("/api/locales", () =>
{
    return locales is null ? Results.NotFound(locales) : Results.Ok(locales);
});

app.Run();

