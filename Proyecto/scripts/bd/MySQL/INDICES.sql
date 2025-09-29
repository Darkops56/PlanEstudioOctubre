-- Índices en FKs y columnas de búsqueda frecuente

-- Cliente
CREATE INDEX idx_Cliente_nombreCompleto ON Cliente(nombreCompleto);

-- Usuario
CREATE INDEX idx_Usuario_Email ON Usuario(Email);
CREATE INDEX idx_Usuario_DNI ON Usuario(DNI);

-- Eventos
CREATE INDEX idx_Eventos_Nombre ON Eventos(Nombre);
CREATE INDEX idx_Eventos_idTipoEvento ON Eventos(idTipoEvento);

-- Funcion
CREATE INDEX idx_Funcion_idEvento ON Funcion(idEvento);
CREATE INDEX idx_Funcion_Fecha ON Funcion(Fecha);

-- Local
CREATE INDEX idx_Local_Nombre ON Local(Nombre);

-- Sector
CREATE INDEX idx_Sector_idLocal ON Sector(idLocal);

-- Sector_Evento
CREATE INDEX idx_SectorEvento_idEvento ON Sector_Evento(idEvento);
CREATE INDEX idx_SectorEvento_idSector ON Sector_Evento(idSector);

-- Tarifa
CREATE INDEX idx_Tarifa_idFuncion ON Tarifa(idFuncion);
CREATE INDEX idx_Tarifa_Tipo ON Tarifa(Tipo);

-- OrdenesCompra
CREATE INDEX idx_OrdenesCompra_idUsuario ON OrdenesCompra(idUsuario);
CREATE INDEX idx_OrdenesCompra_Fecha ON OrdenesCompra(Fecha);

-- StockReservaciones
CREATE INDEX idx_StockReservaciones_idTarifa ON StockReservaciones(idTarifa);
CREATE INDEX idx_StockReservaciones_idOrdenCompra ON StockReservaciones(idOrdenCompra);
CREATE INDEX idx_StockReservaciones_Fecha ON StockReservaciones(FechaReserva);

-- Entrada
CREATE INDEX idx_Entrada_idTarifa ON Entrada(idTarifa);
CREATE INDEX idx_Entrada_idOrdenCompra ON Entrada(idOrdenCompra);
