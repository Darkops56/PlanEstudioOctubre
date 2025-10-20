import { useState } from 'react';
import Navbar from '../Navbar/Navbar';
import { LayoutProps } from '../../types/LayoutProps';
import Sidebar from '@components/Sidebar/Sidebar';

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const [open, setOpen] = useState(false);

  return (
    <div className="min-h-screen flex flex-col bg-gray-900 text-white relative">
      <Navbar open={open} setOpen={setOpen} />
      <Sidebar open={open} setOpen={setOpen} />
      <main className={`flex-1 transition-all duration-300 ${open ? "ml-64" : "ml-0"}`}>
        {children}
      </main>
    </div>
  );
};

export default Layout;
