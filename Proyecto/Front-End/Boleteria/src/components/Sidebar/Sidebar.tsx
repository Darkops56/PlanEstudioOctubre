import { Link } from "react-router-dom";
import { SidebarProps } from "../../types/SidebarProps";

const menuItems = [
    { name: "Inicio", path: "/" },
    { name: "Clientes", path: "/Clientes" },
    { name: "Usuarios", path: "/Usuarios" },
    { name: "Eventos", path: "/Eventos" },
    { name: "Funciones", path: "/Funciones" },
    { name: "OrdenCompra", path: "/ordencompra" },
    { name: "Locales", path: "/locales" },
    { name: "Sectores", path: "/Sectores" },
    { name: "Contactos", path: "/contacto" },
  ];

const Sidebar: React.FC<SidebarProps> = ({ open, setOpen }) => {
  return (
    <div
      className={`fixed top-0 left-0 w-64 h-full bg-gray-800 z-50 transform transition-transform duration-300
                  ${open ? "translate-x-0" : "-translate-x-full"}`}
    >
      <ul className="flex flex-col p-4 space-y-4">
        {menuItems.map((item) => (
          <li key={item.name}>
            <Link
              to={item.path}
              className="block py-2 px-3 rounded hover:bg-gray-700"
              onClick={() => setOpen(false)}
            >
              {item.name}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Sidebar;