USE 5to_Eventos;


-- ============================================================
-- TIPO DE EVENTO
-- ============================================================
INSERT INTO TipoEvento (tipoEvento)
VALUES 
('Formales'), ('Informales'), ('Externos'), ('Internos'),
('Corporativos'), ('Empresariales'), ('Sociales'), ('Ocio'),
('Entretenimiento'), ('Deportivos'), ('Causa'), ('Politicos'),
('Religiosos'), ('Formativos'), ('Virtuales'), ('Hibridos');

USE 5to_Eventos;

-- ============================================================
-- CLIENTES
-- ============================================================
INSERT INTO Cliente (DNI, nombreCompleto, Telefono) VALUES
(40111222, 'Lucía Fernández', '1122334455'),
(39222333, 'Carlos Gómez', '1144556677'),
(40333444, 'Sofía Ramírez', '1133221144'),
(37888999, 'Julián Morales', '1199887766');

-- ============================================================
-- USUARIOS
-- ============================================================
INSERT INTO Usuario (DNI, Email, Apodo, Contrasena, Roles) VALUES
(40111222, 'luciaf@gmail.com', 'LuciaF', 'hash_contra1', 'Usuario'),
(39222333, 'cgomez@eventcorp.com', 'CGomez', 'hash_contra2', 'Organizador'),
(40333444, 'sofiaramirez@admin.com', 'SofiAdmin', 'hash_contra3', 'Administrador'),
(37888999, 'jmorales@acceso.com', 'JuliAcceso', 'hash_contra4', 'ControlAcceso');
-- ============================================================
-- EVENTOS
-- ============================================================
INSERT INTO Evento (Nombre, idTipoEvento, Estado, fechaInicio, fechaFin) VALUES
('Tech & Innovation Summit 2025', 1, 'Activo', '2025-11-15 09:00:00', '2025-11-17 18:00:00'),
('Taller de Programación con IA', 2, 'Activo', '2025-12-01 10:00:00', '2025-12-01 16:00:00'),
('Expo Emprendedores Regional', 3, 'Pendiente', '2026-01-10 09:00:00', '2026-01-12 20:00:00'),
('Concierto de Rock Solidario', 4, 'Activo', '2025-12-20 20:00:00', '2025-12-20 23:30:00');

-- ============================================================
-- FUNCIONES (cada evento puede tener varias funciones)
-- ============================================================
INSERT INTO Funcion (idEvento, Nombre, Estado, Fecha) VALUES
(1, 'Día 1 - Innovación y Startups', 'Activa', '2025-11-15 09:00:00'),
(1, 'Día 2 - Tecnología Sostenible', 'Activa', '2025-11-16 09:00:00'),
(2, 'Sesión Única', 'Activa', '2025-12-01 10:00:00'),
(3, 'Día Inaugural', 'Programada', '2026-01-10 09:00:00'),
(4, 'Función Principal', 'Activa', '2025-12-20 20:00:00');

-- ============================================================
-- LOCALES
-- ============================================================
INSERT INTO Local (Nombre, Ubicacion) VALUES
('Centro de Convenciones Buenos Aires', 'Av. Figueroa Alcorta 2200, CABA'),
('Auditorio Tecnológico', 'Av. Rivadavia 12345, CABA'),
('Parque Industrial Norte', 'Ruta 9 km 75, Escobar'),
('Estadio Arena Sur', 'Av. San Martín 999, La Plata');

-- ============================================================
-- SECTORES
-- ============================================================
INSERT INTO Sector (idLocal, Capacidad) VALUES
(1, 300),
(1, 200),
(2, 100),
(4, 500);

-- ============================================================
-- SECTOR_EVENTO (asociaciones muchos a muchos)
-- ============================================================
INSERT INTO Sector_Evento (idSector, idEvento) VALUES
(1, 1),
(2, 1),
(3, 2),
(4, 4);

-- ============================================================
-- TARIFAS (asociadas a funciones)
-- ============================================================
INSERT INTO Tarifa (idFuncion, Stock, Precio, Estado, Tipo) VALUES
(1, 200, 15000, 'Disponible', 'General'),
(1, 100, 25000, 'Disponible', 'VIP'),
(2, 250, 16000, 'Disponible', 'General'),
(3, 80, 12000, 'Disponible', 'Taller'),
(5, 500, 10000, 'Disponible', 'Campo');

-- ============================================================
-- ORDENES DE COMPRA
-- ============================================================
INSERT INTO OrdenesCompra (idUsuario, Fecha, Total, metodoPago, estado) VALUES
(1, '2025-11-01 10:00:00', 15000, 'Tarjeta de Crédito', 'Pagado'),
(1, '2025-11-02 15:30:00', 25000, 'MercadoPago', 'Pendiente'),
(2, '2025-11-05 09:00:00', 32000, 'Transferencia', 'Pagado');

-- ============================================================
-- ENTRADAS (relación entre tarifas y órdenes)
-- ============================================================
INSERT INTO Entrada (idTarifa, idOrdenCompra, Estado, PrecioPagado) VALUES
(1, 1, 'Activa', 15000),
(2, 2, 'Reservada', 25000),
(4, 3, 'Activa', 12000);

-- ============================================================
-- REFRESH TOKENS
-- ============================================================
INSERT INTO RefreshTokens (Token, Email, Expiration) VALUES
('tk123456aaa', 'luciaf@gmail.com', '2025-12-01 00:00:00'),
('tk999999bbb', 'cgomez@eventcorp.com', '2025-12-15 00:00:00');
