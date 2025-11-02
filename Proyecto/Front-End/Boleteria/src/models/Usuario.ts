import type { Cliente } from "./Cliente";

export interface Usuario{
    idUsuario: number,
    cliente: Cliente,
    apodo: string,
    email: string,
    contrasena: string,
    role: string
}