import { useEffect, useState, useMemo } from "react";
import { createColumnHelper, useReactTable, getCoreRowModel, flexRender } from "@tanstack/react-table";
import api from "../api/api";
import type { Cliente } from "../models/Cliente";
import Layout from "@components/Layout/Layout";

export default function Clientes() {
    const [cliente, setCliente] = useState<Cliente[]>([]);

    useEffect(() => {
        api.get<Cliente[]>("/Clientes")
            .then(res => setCliente(Array.isArray(res.data) ? res.data : [res.data]))
            .catch(err => console.log("Error al obtener clientes", err));
    }, []);


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