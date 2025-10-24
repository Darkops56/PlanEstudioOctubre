-- ============================================================
-- INSTALL.SQL
-- Script maestro de instalación para el sistema de eventos
-- Autor: Sebastián Zerpa
-- Fecha: Octubre 2025
-- ============================================================

-- 📌 NOTA:
-- Este script debe ejecutarse con un usuario que tenga permisos de root o DBA.
-- Asegúrate de que los archivos DDL.sql, INSERT.sql y USER.sql
-- estén en la misma carpeta y sean accesibles para el cliente MySQL.

-- ------------------------------------------------------------
-- 1️⃣ Eliminar base y usuarios previos (para reinstalación limpia)
-- ------------------------------------------------------------
DROP DATABASE IF EXISTS 5to_Eventos;

DROP USER IF EXISTS 'usuario'@'localhost';
DROP USER IF EXISTS 'organizador'@'localhost';
DROP USER IF EXISTS 'administrador'@'localhost';
DROP USER IF EXISTS 'controlacceso'@'localhost';

-- ------------------------------------------------------------
-- 2️⃣ Crear base de datos y estructura
-- ------------------------------------------------------------
SOURCE DDL.sql;

-- ------------------------------------------------------------
-- 3️⃣ Insertar datos iniciales
-- ------------------------------------------------------------
SOURCE INSERT.sql;

-- ------------------------------------------------------------
-- 4️⃣ Crear usuarios y asignar privilegios
-- ------------------------------------------------------------
SOURCE USER.sql;

-- ------------------------------------------------------------
-- 5️⃣ Confirmar cambios
-- ------------------------------------------------------------
FLUSH PRIVILEGES;

-- ------------------------------------------------------------
-- ✅ Mensaje final
-- ------------------------------------------------------------
SELECT '✅ Instalación completa del sistema 5to_Eventos' AS Estado;