import { useEffect, useState } from "react";
import api from "../api/api";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";
import CartaEvento from "../components/CartaEvento";
import type { Cliente } from "../models/Cliente";

export default function Home() {
  const [cliente, setCliente] = useState<Cliente[]>([]);

  useEffect(() => {
    api.get<Cliente[]>("/clientes")
      .then(res => setCliente(res.data))
      .catch(err => console.error("Error al obtener Clientes:", err));
  }, []);

  return (
    <div className="bg-gray-100 min-h-screen flex flex-col">
      <Navbar />

      <main className="flex-1 container mx-auto px-6 py-10">
        <h1 className="text-3xl font-bold text-center mb-8 text-gray-800">
          Cartelera de Hoy 🎬
        </h1>

        <div className="grid gap-8 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
          {cliente.map((c) => (
            <CartaEvento key={c.nombreCompleto} cliente={c} />
          ))}
        </div>
      </main>
      

      <Footer />
    </div>
  );
}
