import type { Cliente } from "../models/Cliente";

interface CartaClienteProps{
    cliente: Cliente;
}

export default function CartaEvento({ cliente }: CartaClienteProps) {
  return (
    <div className="bg-white rounded-xl shadow-md overflow-hidden hover:scale-105 transition-transform">
      
      <div className="p-4">
        <h3 className="text-lg font-semibold">{cliente.nombreCompleto}</h3>
        <p className="text-gray-500 text-sm">{cliente.DNI}</p>
        <p className="text-gray-500 text-sm">{cliente.Telefono}</p>
        <button className="mt-3 w-full bg-indigo-600 text-white py-2 rounded-lg hover:bg-indigo-700">
          Ver detalles
        </button>
      </div>
    </div>
  );
}