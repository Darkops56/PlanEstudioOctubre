DROP DATABASE if EXISTS bdqa<_Eventos;

CREATE DATABASE bd_Eventos;

use bd_Eventos;

-- Tabla Cliente
CREATE TABLE Cliente (
    DNI INT PRIMARY KEY,
    nombreCompleto VARCHAR(100) NOT NULL,
    Telefono VARCHAR(30)
);
-- Tabla Usuario
CREATE TABLE Usuario(
    idUsuario INT PRIMARY KEY,
    DNI UNIQUE INT,
    Apodo VARCHAR(45),
    Contrasena VARCHAR(45),
    CONSTRAINT FK_UsuarioCliente FOREIGN KEY (DNI) REFERENCES Cliente (DNI)
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
    Stock INT,
    Precio INT NOT NULL,
    Estado BOOLEAN,
    Tipo VARCHAR(50) NOT NULL,
);

-- Tabla Entrada
CREATE TABLE Entrada (
    idEntrada INT AUTO_INCREMENT PRIMARY KEY,
    idFuncion INT NOT NULL,
    idTarifa INT NOT NULL,
    FOREIGN KEY (idTarifa) REFERENCES Tarifa(idTarifa),
    CONSTRAINT FK_EntradaFuncion FOREIGN KEY (idFuncion) REFERENCES Funcion (idFuncion)
);
-- Tabla OrdenesCompra
CREATE TABLE OrdenesCompra(

);

-- Tabla RegistroCompra
CREATE TABLE RegistroCompra (
    idRegistro INT AUTO_INCREMENT PRIMARY KEY,
    idUsuario INT NOT NULL,
    idEntrada INT NOT NULL,
    Fecha DATETIME NOT NULL,
    FOREIGN KEY (idUsuario) REFERENCES Usuario (idUsuario),
    FOREIGN KEY (idEntrada) REFERENCES Entrada(idEntrada)
);
-- Tabla QR
CREATE TABLE QR (
    idQR INT AUTO_INCREMENT PRIMARY KEY,
    url VARCHAR(255) NOT NULL,
    Duracion TINYINT UNSIGNED NOT NULL,
    VCard TEXT
);
