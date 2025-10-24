import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import * as React from "react";
import { ProtectedRouteProps } from "../../types/ProtectedRouteProps";

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated } = useAuth();
  return isAuthenticated() ? children : <Navigate to="/login" replace />;
}