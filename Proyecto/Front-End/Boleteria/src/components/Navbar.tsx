export default function Navbar() {
  return (
    <nav className="bg-indigo-700 text-white p-4 shadow-md">
      <div className="container mx-auto flex justify-between items-center">
        <h1 className="text-2xl font-bold">🎟️ CineVirtual</h1>
        <ul className="flex gap-6">
          <li><a href="#" className="hover:text-indigo-200">Inicio</a></li>
          <li><a href="#" className="hover:text-indigo-200">Cartelera</a></li>
          <li><a href="#" className="hover:text-indigo-200">Contactos</a></li>
        </ul>
      </div>
    </nav>
  );
}