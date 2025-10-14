
DELIMITER $$
CREATE PROCEDURE cancelarEntrada(INT unIdEntrada, INT unIdTarifa)
BEGIN
    START TRANSACTION;
        UPDATE  Tarifa
        SET     Stock = Stock + 1
        WHERE   idTarifa = unIdTarifa;

        UPDATE  Entrada
        SET     Estado = 'Cancelado'
        WHERE   idEntrada = unIdEntrada;
    COMMIT;
END $$
DELIMITER ;