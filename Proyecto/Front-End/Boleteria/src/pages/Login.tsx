import { useState } from "react";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await login(email, password);
      navigate("/"); // Redirige directamente después de login
    } catch (error) {
      alert("Credenciales inválidas");
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-900 text-white">
      <form
        onSubmit={handleSubmit}
        className="bg-gray-800 p-8 rounded-lg shadow-lg w-full max-w-sm"
      >
        <h2 className="text-2xl font-bold mb-6 text-center">Iniciar sesión</h2>

        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full mb-4 px-3 py-2 rounded bg-gray-700 border border-gray-600"
        />

        <input
          type="password"
          placeholder="Contraseña"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full mb-6 px-3 py-2 rounded bg-gray-700 border border-gray-600"
        />

        <button
          type="submit"
          className="w-full bg-indigo-600 hover:bg-indigo-700 py-2 rounded"
        >
          Entrar
        </button>
      </form>

      <p className="mt-4 text-center text-gray-400">
        ¿No tienes una cuenta?{" "}
        <span
          onClick={() => navigate("/register")}
          className="text-indigo-400 hover:text-indigo-500 cursor-pointer"
        >
          Regístrate aquí
        </span>
      </p>
    </div>
  );
}
