-- primero tipoEvento
INSERT INTO TipoEvento (tipoEvento) VALUES
('Concierto'), ('Teatro'), ('Deporte'), ('Conferencia'), ('Festival');

-- después eventos
INSERT INTO Eventos (Nombre, idTipoEvento, fechaInicio, fechaFin) VALUES
('Rock en Vivo', 6, '2025-10-01 20:00:00', '2025-10-01 23:30:00'),
('Obra Romeo y Julieta', 7, '2025-10-05 18:00:00', '2025-10-05 20:30:00'),
('Final de Fútbol', 8, '2025-11-02 15:00:00', '2025-11-02 17:30:00'),
('Charla Motivacional', 9, '2025-09-20 10:00:00', '2025-09-20 12:00:00'),
('Festival de Jazz', 10, '2025-12-10 18:00:00', '2025-12-10 23:59:00');

-- locales
INSERT INTO Local (Nombre, Ubicacion) VALUES
('Teatro Colón', 'Buenos Aires'),
('Luna Park', 'Buenos Aires'),
('Estadio Monumental', 'Buenos Aires'),
('Centro de Convenciones', 'Córdoba'),
('Parque Sarmiento', 'Córdoba');

-- sectores
INSERT INTO Sector (idLocal, capacidad) VALUES
(6, 50),
(7, 200),
(8, 500),
(9, 100),
(10, 300);

-- ahora recién sector_evento
INSERT INTO Sector_Evento (idSector, idEvento) VALUES
(16, 20),
(17, 19),
(18, 18),
(19, 17),
(20, 16);

-- funciones (dependen de eventos)
INSERT INTO Funcion (idEvento, fecha) VALUES
(16, '2025-10-01 20:00:00'),
(17, '2025-10-05 18:00:00'),
(18, '2025-11-02 15:00:00'),
(19, '2025-09-20 10:00:00'),
(20, '2025-12-10 18:00:00');
