import { useNavigate } from "react-router-dom";
import { useEffect, useMemo, useState } from "react";
import { createColumnHelper, useReactTable, getCoreRowModel, flexRender } from "@tanstack/react-table";
import type { Usuario } from "@models/Usuario";
import api from "../api/api";
import Layout from "@components/Layout/Layout";


export default function Usuarios () {
    const [usuarios, setUsuarios] = useState<Usuario[]>([]);
    const nav = useNavigate();

    useEffect(() => {
        api.get<Usuario[]>("/usuarios")
            .then(res => setUsuarios(
                Array.isArray(res.data) ? res.data : [res.data]
            ))
            .catch(err => console.log("error al obtener los usuarios", err));
    }, []);
    console.log(usuarios);
    

    const handleDelete = async (idusuario: number) => {
        if (!confirm("¿Seguro que deseas eliminar este usuario?")) return;
        try {
        await api.delete(`/Usuarios/${idusuario}`);
        setUsuarios((prev) => prev.filter((u) => u.idUsuario !== idusuario));
        alert("Usuario eliminado correctamente");
        } catch (error) {
        console.error("Error al eliminar usuario:", error);
        alert("Error al eliminar el usuario");
        }
    }

    const handleEdit = (idusuario: number) => {
        if (idusuario === null || idusuario === undefined) {
            alert("no se puede editar un usuario con datos indefinidos.");
        }
        if (idusuario !== undefined) {
            nav("/editUsuario");
        }
    };

    const columnHelper = createColumnHelper<Usuario>();
        const columns = useMemo(
        () => [
            columnHelper.accessor("idUsuario", {
            header: "idUsuario",
            cell: info => info.getValue(),
            }),
            columnHelper.accessor("cliente.dni", {
            header: "DNI",
            cell: info => info.getValue(),
            }),
            columnHelper.accessor("apodo", {
            header: "apodo",
            cell: info => info.getValue(),
            }),
            columnHelper.accessor("email", {
            header: "email",
            cell: info => info.getValue(),
            }),
            columnHelper.accessor("role", {
                header: "Role",
                cell: info => info.getValue()
            })
        ],
        []
        );
  
    const table = useReactTable({
      data: usuarios,
      columns,
      getCoreRowModel: getCoreRowModel(),
    });

    return (
    <div className="bg-gray-900 text-gray-200 min-h-screen flex flex-col">
      <Layout>
        <main className="flex-1 p-8 overflow-x-auto">
          <h1 className="text-3xl font-bold mb-6 text-center">Listado de Usuarios</h1>

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
                          onClick={() => handleEdit(row.original.idUsuario)}
                          className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded-md text-sm transition"
                        >
                          Editar
                        </button>
                        <button
                          onClick={() => handleDelete(row.original.idUsuario)}
                          className="bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded-md text-sm transition"
                        >
                          Eliminar
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}



                {usuarios.length === 0 && (
                  <tr>
                    <td colSpan={columns.length} className="text-center py-4 text-gray-400">
                      No hay usuarios disponibles...
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