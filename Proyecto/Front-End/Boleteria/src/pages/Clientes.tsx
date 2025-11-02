import { useEffect, useState, useMemo } from "react";
import { createColumnHelper, useReactTable, getCoreRowModel, flexRender } from "@tanstack/react-table";
import api from "../api/api";
import type { Cliente } from "../models/Cliente";
import Layout from "@components/Layout/Layout";
import { useNavigate } from "react-router-dom";

export default function Clientes() {
  const [cliente, setCliente] = useState<Cliente[]>([]);
  const nav = useNavigate();
  useEffect(() => {
    api.get<Cliente[]>("/Clientes")
      .then(res => setCliente(Array.isArray(res.data) ? res.data : [res.data]))
      .catch(err => console.log("Error al obtener clientes", err));
  }, []);

  const handleDelete = async (dni: number) => {
    if (!confirm("¿Seguro que deseas eliminar este cliente?")) return;

    try {
      await api.delete(`/Clientes/${dni}`);
      setCliente((prev) => prev.filter((c) => c.dni !== dni));
      alert("Cliente eliminado correctamente");
    } catch (error) {
      console.error("Error al eliminar cliente:", error);
      alert("Error al eliminar el cliente");
    }
  };

  const handleEdit = (dni: number) => {
    alert(`Editar cliente con DNI: ${dni}`);
    nav("/editCliente");
  };

  const columnHelper = createColumnHelper<Cliente>();
  const columns = useMemo(
    () => [
      columnHelper.accessor("dni", {
        header: "DNI",
        cell: info => info.getValue(),
      }),
      columnHelper.accessor("nombreCompleto", {
        header: "Nombre Completo",
        cell: info => info.getValue(),
      }),
      columnHelper.accessor("telefono", {
        header: "Teléfono",
        cell: info => info.getValue(),
      }),
    ],
    []
  );

  const table = useReactTable({
    data: cliente,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });


  return (
    <div className="bg-gray-900 text-gray-200 min-h-screen flex flex-col">
      <Layout>
        <main className="flex-1 p-8 overflow-x-auto">
          <h1 className="text-3xl font-bold mb-6 text-center">Listado de Clientes</h1>

          <div className="overflow-x-auto rounded-lg border border-gray-700 shadow-md">
            <table className="min-w-full border-collapse">
              <thead className="bg-gray-800">
                {table.getHeaderGroups().map(headerGroup => (
                  <tr key={headerGroup.id}>
                    {headerGroup.headers.map(header => (
                      <th
                        key={header.id}
                        className="px-4 py-2 text-left font-semibold uppercase border-b border-gray-700"
                      >
                        {flexRender(header.column.columnDef.header, header.getContext())}
                      </th>
                    ))}
                  </tr>
                ))}
              </thead>

              <tbody>
                {table.getRowModel().rows.map(row => (
                  <tr key={row.id} className="hover:bg-gray-800 transition-colors">
                    {row.getVisibleCells().map(cell => (
                      <td key={cell.id} className="px-4 py-2 border-b border-gray-700">
                        {flexRender(cell.column.columnDef.cell, cell.getContext())}
                      </td>
                    ))}
                    <td className="px-4 py-2 border-b border-gray-700">
                      <div className="flex gap-2">
                        <button
                          onClick={() => handleEdit(row.original.dni)}
                          className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded-md text-sm transition"
                        >
                          Editar
                        </button>
                        <button
                          onClick={() => handleDelete(row.original.dni)}
                          className="bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded-md text-sm transition"
                        >
                          Eliminar
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}



                {cliente.length === 0 && (
                  <tr>
                    <td colSpan={columns.length} className="text-center py-4 text-gray-400">
                      No hay clientes disponibles...
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </main>
      </Layout>
    </div>
  );

}