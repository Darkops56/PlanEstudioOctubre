-- Insert Cliente
INSERT INTO Cliente (DNI, nombreCompleto, Telefono) VALUES
(101, 'Juan Pérez', '1123456789'),
(102, 'María López', '1134567890'),
(103, 'Carlos Fernández', '1145678901'),
(104, 'Ana Gómez', '1156789012'),
(105, 'Luis Martínez', '1167890123');

-- Insert Usuario
INSERT INTO Usuario (DNI, Apodo, Contrasena) VALUES
(101, 'juancho', '1234'),
(102, 'maru', 'abcd'),
(103, 'carlitos', 'pass123'),
(104, 'anitag', 'qwerty'),
(105, 'lucho', '9876');

-- Insert TipoEvento
INSERT INTO TipoEvento (tipoEvento) VALUES
('Concierto'),
('Teatro'),
('Deporte'),
('Conferencia'),
('Festival');

-- Insert Eventos
INSERT INTO Eventos (Nombre, idTipoEvento, fechaInicio, fechaFin) VALUES
('Recital Rock Nacional', 1, '2025-10-01 20:00:00', '2025-10-01 23:30:00'),
('Obra Romeo y Julieta', 2, '2025-10-05 18:00:00', '2025-10-05 20:30:00'),
('Partido Fútbol Amistoso', 3, '2025-10-10 21:00:00', '2025-10-10 23:00:00'),
('Congreso Tecnología IA', 4, '2025-11-01 09:00:00', '2025-11-01 18:00:00'),
('Festival de Jazz', 5, '2025-11-15 19:00:00', '2025-11-15 23:00:00');

-- Insert Funcion
INSERT INTO Funcion (idEvento, fecha) VALUES
(1, '2025-10-01 20:00:00'),
(2, '2025-10-05 18:00:00'),
(3, '2025-10-10 21:00:00'),
(4, '2025-11-01 09:00:00'),
(5, '2025-11-15 19:00:00');

-- Insert Local
INSERT INTO Local (Nombre, Ubicacion) VALUES
('Luna Park', 'Av. Madero 420, CABA'),
('Teatro Colón', 'Cerrito 628, CABA'),
('Estadio Monumental', 'Av. Figueroa Alcorta 7597, CABA'),
('Centro de Convenciones', 'Av. Figueroa Alcorta 2099, CABA'),
('Parque Sarmiento', 'Av. Ricardo Balbín 4750, CABA');

-- Insert Sector
INSERT INTO Sector (idLocal, capacidad) VALUES
(1, 5000),
(2, 1500),
(3, 70000),
(4, 2000),
(5, 10000);

-- Insert Sector_Evento
INSERT INTO Sector_Evento (idSector, idEvento) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

-- Insert Tarifa
INSERT INTO Tarifa (Stock, Tipo) VALUES
(100, 'General'),
(50, 'VIP'),
(200, 'Campo'),
(300, 'Platea'),
(150, 'Preferencial');

-- Insert Entrada
INSERT INTO Entrada (Precio, idEvento, idTarifa) VALUES
(5000, 1, 6),
(12000, 1, 7),
(3500, 2, 8),
(8000, 3, 9),
(10000, 5, 10);

-- Insert RegistroCompra
INSERT INTO RegistroCompra (idCliente, idEntrada, Fecha) VALUES
(101, 6, '2025-09-10 14:30:00'),
(102, 7, '2025-09-11 16:00:00'),
(103, 8, '2025-09-12 18:45:00'),
(104, 9, '2025-09-13 20:15:00'),
(105, 10, '2025-09-14 19:30:00');