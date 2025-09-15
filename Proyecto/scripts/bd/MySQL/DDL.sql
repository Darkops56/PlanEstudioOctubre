DROP DATABASE if EXISTS bdqa<_Eventos;

CREATE DATABASE bd_Eventos;

use bd_Eventos;

-- Tabla Cliente
CREATE TABLE Cliente (
    DNI INT PRIMARY KEY,
    nombreCompleto VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Telefono VARCHAR(30),
    Contrasena VARCHAR(100) NOT NULL
);

-- Tabla TipoEvento
CREATE TABLE TipoEvento (
    idTipoEvento INT AUTO_INCREMENT PRIMARY KEY,
    tipoEvento VARCHAR(50) NOT NULL
);

-- Tabla Eventos
CREATE TABLE Eventos (
    idEvento INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    idTipoEvento INT NOT NULL,
    fechaInicio DATETIME NOT NULL,
    fechaFin DATETIME NOT NULL,
    FOREIGN KEY (idTipoEvento) REFERENCES TipoEvento(idTipoEvento)
);

-- Tabla Funcion
CREATE TABLE Funcion (
    idFuncion INT AUTO_INCREMENT PRIMARY KEY,
    idEvento INT NOT NULL,
    fecha DATETIME NOT NULL,
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
    capacidad TINYINT UNSIGNED NOT NULL,
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
    Stock TINYINT UNSIGNED NOT NULL,
    Tipo VARCHAR(50) NOT NULL
);

-- Tabla Entrada
CREATE TABLE Entrada (
    idEntrada INT AUTO_INCREMENT PRIMARY KEY,
    Precio INT NOT NULL,
    idEvento INT NOT NULL,
    idTarifa INT NOT NULL,
    FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento),
    FOREIGN KEY (idTarifa) REFERENCES Tarifa(idTarifa)
);

-- Tabla RegistroCompra
CREATE TABLE RegistroCompra (
    idRegistro INT AUTO_INCREMENT PRIMARY KEY,
    idCliente INT NOT NULL,
    idEntrada INT NOT NULL,
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (idCliente) REFERENCES Cliente(DNI),
    FOREIGN KEY (idEntrada) REFERENCES Entrada(idEntrada)
);

-- Tabla QR
CREATE TABLE QR (
    idQR INT AUTO_INCREMENT PRIMARY KEY,
    url VARCHAR(255) NOT NULL,
    duracion TINYINT UNSIGNED NOT NULL,
    VCard TEXT
);
