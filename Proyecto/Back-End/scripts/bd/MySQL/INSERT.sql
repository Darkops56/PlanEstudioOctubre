USE 5to_Eventos;

-- CLIENTE + USUARIO
INSERT INTO Cliente (DNI, nombreCompleto, Telefono)
VALUES (12345678, 'Sebastian Zerpa', '1122334455');

INSERT INTO Usuario (DNI, Email, Apodo, Contrasena, Roles)
VALUES (12345678, 'micho@gmail.com', 'MichoRhodesia', '1234contra', 'Usuario');

INSERT INTO Usuario (DNI, Email, Apodo, Contrasena, Roles)
VALUES (12345678, 'zerpa@gmail.com', 'EñAdmin', 'admincontra', 'Admin');


-- TIPOS DE EVENTO

INSERT INTO TipoEvento (tipoEvento)
VALUES 
('Formales'), ('Informales'), ('Externos'), ('Internos'),
('Corporativos'), ('Empresariales'), ('Sociales'), ('Ocio'),
('Entretenimiento'), ('Deportivos'), ('Causa'), ('Politicos'),
('Religiosos'), ('Formativos'), ('Virtuales'), ('Hibridos');


-- EVENTO

INSERT INTO Evento (Nombre, idTipoEvento, Estado, fechaInicio, fechaFin)
VALUES ('Concierto Rock Nacional', 9, 'Publicado', '2025-12-10 20:00:00', '2025-12-10 23:30:00');


-- FUNCIONES

INSERT INTO Funcion (idEvento, Nombre, Estado, Fecha)
VALUES 
(1, 'Función Principal', 'Publicado', '2025-12-10 20:00:00'),
(1, 'Función Secundaria', 'Publicado', '2025-12-11 20:00:00');


-- LOCAL + SECTOR

INSERT INTO Local (Nombre, Ubicacion)
VALUES ('Estadio Central', 'Av. Siempre Viva 123, Buenos Aires');

INSERT INTO Sector (idLocal, Capacidad)
VALUES (1, 1000), (1, 500), (1, 200);

INSERT INTO Sector_Evento (idSector, idEvento)
VALUES (1,1), (2,1), (3,1);


-- TARIFAS

INSERT INTO Tarifa (idFuncion, Stock, Precio, Estado, Tipo)
VALUES
(1, 500, 5000, TRUE, 'General'),
(1, 100, 8000, TRUE, 'Vip'),
(2, 200, 3000, TRUE, 'Descuento');


-- ORDENES DE COMPRA

INSERT INTO OrdenesCompra (idUsuario, Fecha, Total, metodoPago, estado)
VALUES (1, '2025-12-01 15:30:00', 13000, 'Credito', 'Pagado');


-- STOCK RESERVACIONES

INSERT INTO StockReservaciones (idTarifa, Cantidad, ExpiraEn, idOrdenCompra)
VALUES 
(1, 2, '2025-12-05 23:59:59', 1),
(2, 1, '2025-12-05 23:59:59', 1);


-- ENTRADAS

INSERT INTO Entrada (idTarifa, idOrdenCompra, Estado, PrecioPagado)
VALUES
(1, 1, 'Pagado', 5000),
(1, 1, 'Pagado', 5000),
(2, 1, 'Pagado', 8000);


-- QR

INSERT INTO QR (idEntrada, url, token, ExpiraEn, VCard)
VALUES
(1, 'http://localhost:5001/api/entradas/1/validar?token=abc123', 'abc123', '2025-12-11 23:59:59', 'VCARD DATA 1'),
(2, 'http://localhost:5001/api/entradas/2/validar?token=def456', 'def456', '2025-12-11 23:59:59', 'VCARD DATA 2'),
(3, 'http://localhost:5001/api/entradas/3/validar?token=ghi789', 'ghi789', '2025-12-11 23:59:59', 'VCARD DATA 3');


-- REFRESH TOKENS

INSERT INTO RefreshTokens (Token, Email, Expiration)
VALUES ('refreshToken123', 'micho@gmail.com', '2025-12-31 23:59:59');

