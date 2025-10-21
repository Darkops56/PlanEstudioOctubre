import { createContext, useContext, useState, ReactNode } from "react";
import api from "../api/api";
import { AuthContextType  } from "../types/AuthContextProps";

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<string | null>(localStorage.getItem("user"));

  const login = async (email: string, password: string) => {
    const res = await api.post("/login", { email, password });
    const { token, usuario } = res.data;

    localStorage.setItem("token", token);
    localStorage.setItem("user", usuario.email);
    setUser(usuario.email);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isAuthenticated: !!user }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth debe usarse dentro de AuthProvider");
  return context;
};