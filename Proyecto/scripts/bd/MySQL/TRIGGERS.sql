DELIMITER $$

CREATE TRIGGER BefInsCliente
BEFORE INSERT ON Cliente
FOR EACH ROW
BEGIN
    -- Si la contraseña no está ya en formato hash, la encripta
    SET NEW.Contrasena = SHA2(NEW.Contrasena, 256);
END $$;


DELIMITER $$
CREATE TRIGGER BefUpdCliente
BEFORE UPDATE ON Cliente
FOR EACH ROW
BEGIN
    -- Solo hashea si se cambió la contraseña
    IF NEW.Contrasena <> OLD.Contrasena THEN
        SET NEW.Contrasena = SHA2(NEW.Contrasena, 256);
    END IF;
END $$;