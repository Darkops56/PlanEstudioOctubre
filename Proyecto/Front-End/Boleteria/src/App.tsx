import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout/Layout';
import Home from './pages/Home';
import Clientes from '@pages/Clientes';

const App: React.FC = () => {
  return (
    <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/Clientes" element={<Clientes />} />
        </Routes>
    </Router>
  );
};

export default App;
