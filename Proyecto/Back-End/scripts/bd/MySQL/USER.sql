DROP USER IF EXISTS 'usuario'@'localhost';
DROP USER IF EXISTS 'organizador'@'localhost';
DROP USER IF EXISTS 'administrador'@'localhost';
DROP USER IF EXISTS 'controlacceso'@'localhost';


-- 1️⃣ USUARIO: asiste a eventos, consulta, se registra, etc.

CREATE USER 'usuario'@'localhost' IDENTIFIED BY 'User123!';
GRANT SELECT, INSERT, UPDATE ON 5to_Eventos.Usuario TO 'usuario'@'localhost';
GRANT SELECT ON 5to_Eventos.Evento TO 'usuario'@'localhost';
GRANT SELECT ON 5to_Eventos.TipoEvento TO 'usuario'@'localhost';
GRANT SELECT ON 5to_Eventos.Funcion TO 'usuario'@'localhost';
GRANT SELECT ON 5to_Eventos.Tarifa TO 'usuario'@'localhost';
GRANT INSERT, UPDATE ON 5to_Eventos.OrdenesCompra TO 'usuario'@'localhost';
GRANT INSERT, UPDATE ON 5to_Eventos.Entrada TO 'usuario'@'localhost';
GRANT INSERT, SELECT ON 5to_Eventos.QR TO 'usuario'@'localhost';

-- El usuario puede registrarse, consultar eventos, inscribirse (OrdenCompra, Entrada),
-- y recibir su QR, pero no administrar ni borrar datos del sistema.



-- 2️⃣ ORGANIZADOR: administra eventos y genera reportes

CREATE USER 'organizador'@'localhost' IDENTIFIED BY 'Org123!';
GRANT SELECT, INSERT, UPDATE, DELETE ON 5to_Eventos.Evento TO 'organizador'@'localhost';
GRANT SELECT ON 5to_Eventos.TipoEvento TO 'organizador'@'localhost';
GRANT SELECT, INSERT, UPDATE, DELETE ON 5to_Eventos.Funcion TO 'organizador'@'localhost';
GRANT SELECT, INSERT, UPDATE, DELETE ON 5to_Eventos.Tarifa TO 'organizador'@'localhost';
GRANT SELECT ON 5to_Eventos.OrdenesCompra TO 'organizador'@'localhost';
GRANT SELECT ON 5to_Eventos.Entrada TO 'organizador'@'localhost';
GRANT SELECT ON 5to_Eventos.QR TO 'organizador'@'localhost';

-- Puede crear, editar o eliminar eventos, ver informes o exportar datos,
-- pero no gestionar usuarios ni acceder a datos administrativos.



-- 3️⃣ ADMINISTRADOR: gestiona usuarios, roles, auditorías y backups

CREATE USER 'administrador'@'localhost' IDENTIFIED BY 'Admin123!';
GRANT ALL ON 5to_Eventos.* TO 'administrador'@'localhost';

-- El administrador tiene control total (alta, roles, logs, restauraciones, etc.)



-- 4️⃣ CONTROL DE ACCESO: valida entradas (QR) y registra asistencias


CREATE USER 'controlacceso'@'localhost' IDENTIFIED BY 'Acceso123!';
GRANT SELECT, UPDATE ON 5to_Eventos.Entrada TO 'controlacceso'@'localhost';
GRANT SELECT, UPDATE ON 5to_Eventos.QR TO 'controlacceso'@'localhost';


-- Este rol puede validar accesos mediante el escaneo de QR,
-- marcando entradas como utilizadas o válidas, pero sin modificar otros datos.

FLUSH PRIVILEGES;