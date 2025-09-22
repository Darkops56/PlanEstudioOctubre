```mermaid
erDiagram
    Cliente {
        int DNI PK
        varchar(45) nombreCompleto
        varchar(45) Telefono
    }
    Usuario{
        int idUsuario PK
        varchar(45) apodo
        varchar(60) Email
        varchar(45) Contrasena
        int DNI FK
    }

    TipoEvento {
        int idTipoEvento PK
        varchar(45) tipoEvento
    }

    Eventos {
        int idEvento PK
        varchar(45) Nombre
        datetime fechaInicio
        datetime fechaFin
        int idTipoEvento FK
    }

    Funcion {
        int idFuncion PK
        int idEvento FK
        datetime fecha
    }

    Local {
        int idLocal PK
        varchar(45) Nombre
        varchar(80) Ubicacion
    }

    Sector {
        int idSector PK
        int idLocal FK
        tinyint capacidad
    }

    Sector_Evento {
        int idSector PK, FK
        int idEvento PK, FK
    }

    Tarifa {
        int idTarifa PK
        tinyint Stock
        varchar(30) Tipo
    }

    Entrada {
        int idEntrada PK
        int Precio
        int idFuncion FK
        int idTarifa FK
    }

    RegistroCompra {
        int idRegistro PK
        int idUsuario FK
        int idEntrada FK
        datetime Fecha
    }

    QR {
        int idQR PK
        varchar(255) url
        tinyint duracion
        text VCard
    }
    TipoEvento ||--|{ Eventos : ""
    Funcion ||--o{ Entrada : ""
    Eventos ||--o{ Funcion : ""
    Entrada ||--|{ RegistroCompra : ""
    Cliente ||--o| Usuario : ""
    Usuario ||--o{ RegistroCompra : ""
    Eventos ||--|{ Sector_Evento : ""
    Sector ||--|{ Sector_Evento : ""
    Local ||--|{ Sector : ""
    Tarifa ||--|{ Entrada : ""
