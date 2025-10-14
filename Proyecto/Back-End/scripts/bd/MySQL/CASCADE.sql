-- Usuario → OrdenesCompra
ALTER TABLE OrdenesCompra
DROP FOREIGN KEY FK_OrdenesCompra_idUsuario,
ADD CONSTRAINT FK_OrdenesCompra_idUsuario
FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)
ON DELETE CASCADE;

-- OrdenesCompra → Entrada
ALTER TABLE Entrada
DROP FOREIGN KEY Entrada_ibfk_2,
ADD CONSTRAINT FK_Entrada_idOrdenCompra
FOREIGN KEY (idOrdenCompra) REFERENCES OrdenesCompra(idOrdenCompra)
ON DELETE CASCADE;

-- Funcion → Tarifa
ALTER TABLE Tarifa
DROP FOREIGN KEY Tarifa_ibfk_1,
ADD CONSTRAINT FK_Tarifa_idFuncion
FOREIGN KEY (idFuncion) REFERENCES Funcion(idFuncion)
ON DELETE CASCADE;

-- Eventos → Funcion
ALTER TABLE Funcion
DROP FOREIGN KEY Funcion_ibfk_1,
ADD CONSTRAINT FK_Funcion_idEvento
FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento)
ON DELETE CASCADE;

-- Sector_Evento → Sector y Eventos
ALTER TABLE Sector_Evento
DROP FOREIGN KEY Sector_Evento_ibfk_1,
ADD CONSTRAINT FK_SectorEvento_idSector
FOREIGN KEY (idSector) REFERENCES Sector(idSector)
ON DELETE CASCADE;

ALTER TABLE Sector_Evento
DROP FOREIGN KEY Sector_Evento_ibfk_2,
ADD CONSTRAINT FK_SectorEvento_idEvento
FOREIGN KEY (idEvento) REFERENCES Eventos(idEvento)
ON DELETE CASCADE;

-- Cliente → Usuario
ALTER TABLE Usuario
DROP FOREIGN KEY FK_UsuarioCliente,
ADD CONSTRAINT FK_UsuarioCliente
FOREIGN KEY (DNI) REFERENCES Cliente(DNI)
ON DELETE CASCADE;
