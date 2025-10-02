DROP DATABASE if EXISTS 5to_Eventos

CREATE DATABASE 5to_Eventos;

use 5to_Eventos;

-- Tabla Cliente
CREATE TABLE Cliente (
    DNI INT PRIMARY KEY,
    nombreCompleto VARCHAR(100) NOT NULL,
    Telefono VARCHAR(30)
);
-- Tabla Usuario
CREATE TABLE Usuario(
    idUsuario INT PRIMARY KEY,
    DNI INT NOT NULL,
    Apodo VARCHAR(45),
    Contrasena VARCHAR(45),
    Roles VARCHAR(45),
    CONSTRAINT FK_UsuarioCliente FOREIGN KEY (DNI) REFERENCES Cliente(DNI)
);

-- Tabla TipoEvento
CREATE TABLE TipoEvento (
    idTipoEvento INT AUTO_INCREMENT PRIMARY KEY,
    tipoEvento VARCHAR(50) NOT NULL
);

-- Tabla Evento
CREATE TABLE Evento (
    idEvento INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    idTipoEvento INT NOT NULL,
    Estado VARCHAR(45) NOT NULL,
    fechaInicio DATETIME NOT NULL,
    fechaFin DATETIME NOT NULL,
    FOREIGN KEY (idTipoEvento) REFERENCES TipoEvento(idTipoEvento)
);

-- Tabla Funcion
CREATE TABLE Funcion (
    idFuncion INT AUTO_INCREMENT PRIMARY KEY,
    idEvento INT NOT NULL,
    Estado VARCHAR(45) NOT NULL,
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento)
);

-- Tabla Local
CREATE TABLE Local (
    idLocal INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Ubicacion VARCHAR(150) NOT NULL
);

-- Tabla Sector
CREATE TABLE Sector (
    idSector INT AUTO_INCREMENT PRIMARY KEY,
    idLocal INT NOT NULL,
    Capacidad INT UNSIGNED NOT NULL,
    FOREIGN KEY (idLocal) REFERENCES Local(idLocal)
);

-- Tabla intermedia Sector_Evento (muchos a muchos)
CREATE TABLE Sector_Evento (
    idSector INT NOT NULL,
    idEvento INT NOT NULL,
    PRIMARY KEY (idSector, idEvento),
    FOREIGN KEY (idSector) REFERENCES Sector(idSector),
    FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento)
);

-- Tabla Tarifa
CREATE TABLE Tarifa (
    idTarifa INT AUTO_INCREMENT PRIMARY KEY,
    idFuncion INT,
    Stock INT,
    Precio INT NOT NULL,
    Estado BOOLEAN,
    Tipo VARCHAR(50) NOT NULL,
    Foreign Key (idFuncion) REFERENCES Funcion(idFuncion)
);

-- Tabla OrdenesCompra
CREATE TABLE OrdenesCompra(
    idOrdenCompra INT NOT NULL PRIMARY KEY,
    idUsuario INT NOT NULL,
    Fecha DATETIME NOT NULL,
    Total INT NOT NULL,
    metodoPago VARCHAR(45) NOT NULL,
    estado VARCHAR(45) NOT NULL,
    Foreign Key (idUsuario) REFERENCES Usuario (idUsuario)
);
CREATE TABLE StockReservaciones (
    IdStockReservacion INT AUTO_INCREMENT PRIMARY KEY,
    idTarifa INT NOT NULL,
    Cantidad INT NOT NULL,
    FechaReserva DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ExpiraEn DATETIME NOT NULL,
    idOrdenCompra INT NOT NULL,
    FOREIGN KEY (idTarifa) REFERENCES Tarifa(idTarifa),
    FOREIGN KEY (idOrdenCompra) REFERENCES OrdenesCompra(idOrdenCompra)
);
-- Tabla Entrada
CREATE TABLE Entrada (
    idEntrada INT AUTO_INCREMENT PRIMARY KEY,
    idTarifa INT NOT NULL,
    idOrdenCompra INT NOT NULL,
    Estado VARCHAR(45) NOT NULL,
    PrecioPagado INT NOT NULL,
    Foreign Key (idOrdenCompra) REFERENCES OrdenesCompra(idOrdenCompra),
    FOREIGN KEY (idTarifa) REFERENCES Tarifa(idTarifa)
);

-- Tabla RegistroCompra
/* CREATE TABLE RegistroCompra (
    idRegistro INT AUTO_INCREMENT PRIMARY KEY,
    idUsuario INT NOT NULL,
    idEntrada INT NOT NULL,
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (idUsuario) REFERENCES Usuario (idUsuario),
    FOREIGN KEY (idEntrada) REFERENCES Entrada(idEntrada)
); */
-- Tabla QR
CREATE TABLE QR (
    idQR INT AUTO_INCREMENT PRIMARY KEY,
    url VARCHAR(255) NOT NULL,
    Duracion TINYINT UNSIGNED NOT NULL,
    VCard TEXT
);
CREATE TABLE RefreshTokens (
    IdRefreshTokens INT AUTO_INCREMENT PRIMARY KEY,
    Token VARCHAR(200) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Expiration DATETIME NOT NULL,
    Foreign Key (Email) REFERENCES Usuario (Email)
);
