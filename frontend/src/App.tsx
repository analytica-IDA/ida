import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import LoginPage from './pages/LoginPage';
import DashboardLayout from './layouts/DashboardLayout';
import UsersPage from './pages/UsersPage';
import { useEffect, useState } from 'react';

const queryClient = new QueryClient();

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const token = localStorage.getItem('token');
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  return <>{children}</>;
}

function App() {
  const [theme, setTheme] = useState(localStorage.getItem('theme') || 'dark');

  useEffect(() => {
    document.documentElement.classList.toggle('dark', theme === 'dark');
    localStorage.setItem('theme', theme);
  }, [theme]);

  const toggleTheme = () => setTheme(theme === 'dark' ? 'light' : 'dark');

  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route 
            path="/" 
            element={
              <ProtectedRoute>
                <DashboardLayout toggleTheme={toggleTheme} theme={theme} />
              </ProtectedRoute>
            }
          >
            <Route index element={<Navigate to="/users" replace />} />
            <Route path="users" element={<UsersPage />} />
            <Route path="dashboard" element={<div>Dashboard Placeholder</div>} />
            <Route path="notifications" element={<div>Notifications Placeholder</div>} />
            <Route path="settings" element={<div>Settings Placeholder</div>} />
          </Route>
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </QueryClientProvider>
  );
}

export default App;
