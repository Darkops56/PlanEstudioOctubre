import { createContext, useContext, useState, ReactNode, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import api from "../api/api";
import { AuthContextType  } from "../types/AuthContextProps";

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<string | null>(null);

 
  const login = async (email: string, password: string) => {
    
    const res = await api.post("/auth/login", { 
      Email: email,
      Contrasena: password 
    }, {
      headers: { "Content-Type": "application/json" }
    });

    const { token, refreshToken, usuario } = res.data;

    localStorage.setItem("token", token);
    localStorage.setItem("refreshToken", refreshToken);

    setUser(usuario?.Email || null);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
  };

  const getUserEmail = (): string | null => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return decoded.name || null;
    } catch {
      return null;
    }
  };

  const isAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
    if (!token) return false;

    try {
      const decoded: any = jwtDecode(token);
      // Comparar expiración
      if (decoded.exp && Date.now() >= decoded.exp * 1000) {
        logout();
        return false;
      }
      return true;
    } catch {
      return false;
    }
  };

  

  return (
    <AuthContext.Provider value={{ login, logout, getUserEmail, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth debe usarse dentro de AuthProvider");
  return context;
};