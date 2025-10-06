DROP DATABASE IF EXISTS 5to_Eventos;
CREATE DATABASE 5to_Eventos CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

USE 5to_Eventos;

SOURCE ./DDL.sql;

SELECT '✅ Instalación completada correctamente' AS Resultado;