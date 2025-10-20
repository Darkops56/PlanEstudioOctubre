import { useState } from "react";
import { Link } from 'react-router-dom';
import { NavbarProps } from "../../types/NavbarProps";

const Navbar: React.FC<NavbarProps> = ({ open, setOpen}) => {

  return (
    <nav className="bg-gray-900 text-white px-4 py-3 flex items-center justify-between relative">
      <div className="text-xl font-bold">Boleteria Virtual</div>

      {/* Hamburger */}
      <button
        className="focus:outline-none"
        onClick={() => setOpen(!open)}
      >
        <div className="space-y-1">
          <span className="block w-6 h-0.5 bg-white"></span>
          <span className="block w-6 h-0.5 bg-white"></span>
          <span className="block w-6 h-0.5 bg-white"></span>
        </div>
      </button>
    </nav>
  );
};

export default Navbar;