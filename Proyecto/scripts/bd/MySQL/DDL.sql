DROP DATABASE if EXISTS bd_Eventos;

CREATE DATABASE bd_Eventos;

use bd_Eventos;

CREATE TABLE Cliente (
    DNI int NOT NULL, 
    nombreCompleto VARCHAR(50),
    email varchar(50),
    telefono varchar(50),
    contrasena varchar(50),
    CONSTRAINT PK_dni PRIMARY KEY (DNI)
);
CREATE TABLE Entrada (
    idEntrada INT NOT NULL,
    idEvento INT NOT NULL,
    idTarifa INT NOT NULL,
    precio INT NOT NULL,
    CONSTRAINT FK_EventoEntrada FOREIGN KEY (idEvento) REFERENCES Evento (idEvento),
    FOREIGN KEY (idTarifa) REFERENCES Tarifa (idTarifa)
);
CREATE TABLE 