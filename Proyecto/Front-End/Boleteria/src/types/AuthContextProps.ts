export interface AuthContextType {
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  getUserEmail: () => string | null;
  isAuthenticated: () => boolean;
}