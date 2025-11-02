import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from "./components/ProtectedRoute/ProtectedRoute";
import Home from './pages/Home';
import Login from "./pages/Login";
import Register from "./pages/Register";
import Clientes from '@pages/Clientes';
import Usuarios from '@pages/Usuarios';

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <Home />
              </ProtectedRoute>
            }
          />
          <Route
            path="/clientes"
            element={
              <ProtectedRoute>
                <Clientes />
              </ProtectedRoute>
            }
          />
          <Route
          path='/usuarios'
          element = {
            <ProtectedRoute>
              <Usuarios/>
            </ProtectedRoute>
          }
          />
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;
