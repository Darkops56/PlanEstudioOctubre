import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import api from "api/api";
import { Cliente } from "@models/Cliente";

export default function Register() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    nombreCompleto: "",
    Telefono: "",
    DNI: 0,
    Apodo: "",
    email: "",
    Contrasena: "",
    confirmarPassword: "",
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  // Maneja los cambios de los inputs
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  // Maneja el envío del formulario
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    // Validaciones simples
    if (formData.Contrasena !== formData.confirmarPassword) {
      setError("Las contraseñas no coinciden");
      return;
    }

    try {
      setLoading(true);

      const clienteResponse = await axios.post("http://localhost:5002/api/clientes", {
        DNI: Number(formData.DNI),
        nombreCompleto: formData.nombreCompleto,
        Telefono: formData.Telefono,
      });

      if (!clienteResponse.data) {
        throw new Error("No se pudo crear el cliente.");
      }
      
      const usuarioResponse = await axios.post("http://localhost:5002/api/auth/register", {
        Apodo: formData.Apodo,
        Email: formData.email,
        Contrasena: formData.Contrasena,
        DNI: Number(formData.DNI),
      });

      if (usuarioResponse.status === 200 || usuarioResponse.status === 201) {
        alert("Registro exitoso ✅");
        navigate("/login");
      } else {
        throw new Error("Error al registrar el usuario.");
      }

      localStorage.setItem("token", usuarioResponse.data.token);

      navigate("/login");
    } catch (err: any) {
      console.error(err);
      setError(
        err.response?.data?.message ||
        err.message ||
        "Error al registrarse. Verifique los datos e intente nuevamente."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-900">
      <div className="bg-gray-800 p-8 rounded-2xl shadow-xl w-full max-w-md text-white">
        <h2 className="text-3xl font-bold text-center mb-6">Crear Cuenta</h2>

        <form onSubmit={handleSubmit} className="space-y-4">

          {/* Datos de Cliente */}
          <input
            type="text"
            name="nombreCompleto"
            placeholder="Nombre completo"
            value={formData.nombreCompleto}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />
          <input
            type="text"
            name="Telefono"
            placeholder="Teléfono"
            value={formData.Telefono}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />
          <input
            type="number"
            name="DNI"
            placeholder="DNI"
            value={formData.DNI}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />

          {/* Datos de Usuario */}
          <input
            type="text"
            name="Apodo"
            placeholder="Apodo"
            value={formData.Apodo}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />

          <input
            type="email"
            name="email"
            placeholder="Correo electrónico"
            value={formData.email}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />

          <input
            type="password"
            name="Contrasena"
            placeholder="Contraseña"
            value={formData.Contrasena}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />

          <input
            type="password"
            name="confirmarPassword"
            placeholder="Confirmar contraseña"
            value={formData.confirmarPassword}
            onChange={handleChange}
            className="w-full p-3 rounded-lg bg-gray-700 border border-gray-600 focus:ring-2 focus:ring-indigo-500 outline-none"
            required
          />

          {/* Mensajes de error */}
          {error && <p className="text-red-400 text-center">{error}</p>}

          <button
            type="submit"
            className="w-full bg-indigo-600 hover:bg-indigo-700 text-white py-3 rounded-lg font-semibold transition-all duration-200"
            disabled={loading}
          >
            {loading ? "Registrando..." : "Registrarse"}
          </button>

          <p className="mt-4 text-center text-gray-400">
            ¿Ya tienes una cuenta?{" "}
            <span
              onClick={() => navigate("/login")}
              className="text-indigo-400 hover:text-indigo-500 cursor-pointer"
            >
              Inicia sesión
            </span>
          </p>
        </form>
      </div>
    </div>
  );
}
