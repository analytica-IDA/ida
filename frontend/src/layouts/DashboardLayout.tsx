import { Outlet, Link, useNavigate, useLocation } from 'react-router-dom';
import { LayoutDashboard, Users, Settings, LogOut, Sun, Moon, Menu, Bell, ChevronLeft } from 'lucide-react';
import { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

interface LayoutProps {
  toggleTheme: () => void;
  theme: string;
}

export default function DashboardLayout({ toggleTheme, theme }: LayoutProps) {
  const [isSidebarOpen, setSidebarOpen] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-neutral-50 dark:bg-neutral-950 text-neutral-900 dark:text-neutral-100 flex transition-colors duration-500 font-sans">
      {/* Sidebar */}
      <aside 
        className={cn(
          "relative h-screen bg-white dark:bg-neutral-900 border-r border-neutral-200 dark:border-neutral-800 transition-all duration-300 ease-in-out z-20 flex flex-col shadow-xl dark:shadow-none",
          isSidebarOpen ? "w-72" : "w-20"
        )}
      >
        <div className="h-20 flex items-center justify-between px-6 border-b border-neutral-100 dark:border-neutral-800/50">
          <AnimatePresence mode="wait">
            {isSidebarOpen && (
              <motion.div
                initial={{ opacity: 0, x: -10 }}
                animate={{ opacity: 1, x: 0 }}
                exit={{ opacity: 0, x: -10 }}
                className="flex items-center gap-3"
              >
                <div className="w-8 h-8 rounded-lg bg-gradient-to-tr from-blue-600 to-indigo-600 flex items-center justify-center text-white font-black shadow-lg shadow-blue-500/20">
                  G
                </div>
                <span className="text-xl font-bold tracking-tight bg-gradient-to-r from-neutral-900 to-neutral-600 dark:from-white dark:to-neutral-400 bg-clip-text text-transparent">
                  Analytica IDA
                </span>
              </motion.div>
            )}
          </AnimatePresence>
          <button 
            onClick={() => setSidebarOpen(!isSidebarOpen)}
            className="p-2 hover:bg-neutral-100 dark:hover:bg-neutral-800 rounded-xl transition-colors text-neutral-500"
          >
            {isSidebarOpen ? <ChevronLeft size={20} /> : <Menu size={20} />}
          </button>
        </div>

        <nav className="flex-1 px-4 py-8 space-y-2 overflow-y-auto custom-scrollbar">
          <SidebarItem 
            icon={<LayoutDashboard size={20} />} 
            label="Dashboard" 
            to="/dashboard" 
            isOpen={isSidebarOpen} 
            active={location.pathname === '/dashboard'}
          />
          <SidebarItem 
            icon={<Users size={20} />} 
            label="Usuários" 
            to="/users" 
            isOpen={isSidebarOpen} 
            active={location.pathname === '/users'}
          />
          <SidebarItem 
            icon={<Bell size={20} />} 
            label="Notificações" 
            to="/notifications" 
            isOpen={isSidebarOpen} 
            active={location.pathname === '/notifications'}
          />
          <SidebarItem 
            icon={<Settings size={20} />} 
            label="Configurações" 
            to="/settings" 
            isOpen={isSidebarOpen} 
            active={location.pathname === '/settings'}
          />
        </nav>

        <div className="p-4 space-y-3 bg-neutral-50/50 dark:bg-neutral-800/20 border-t border-neutral-100 dark:border-neutral-800">
          <button 
            onClick={toggleTheme} 
            className="w-full flex items-center p-3 hover:bg-white dark:hover:bg-neutral-800 rounded-xl transition-all duration-200 shadow-sm hover:shadow group border border-transparent hover:border-neutral-200 dark:hover:border-neutral-700"
          >
            <div className={cn(
              "p-1.5 rounded-lg transition-colors",
              theme === 'dark' ? "bg-amber-100 text-amber-600" : "bg-indigo-100 text-indigo-600"
            )}>
              {theme === 'dark' ? <Sun size={18} /> : <Moon size={18} />}
            </div>
            {isSidebarOpen && <span className="ml-3 font-medium text-sm">Alternar Tema</span>}
          </button>
          <button 
            onClick={handleLogout} 
            className="w-full flex items-center p-3 text-red-500 hover:bg-red-50 dark:hover:bg-red-950/30 rounded-xl transition-all duration-200 group"
          >
            <div className="p-1.5 rounded-lg bg-red-50 dark:bg-red-950/50 group-hover:bg-red-100 dark:group-hover:bg-red-900/50 transition-colors">
              <LogOut size={18} />
            </div>
            {isSidebarOpen && <span className="ml-3 font-medium text-sm">Sair do Sistema</span>}
          </button>
        </div>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col h-screen overflow-hidden">
        <header className="h-20 bg-white/80 dark:bg-neutral-900/80 backdrop-blur-md border-b border-neutral-200 dark:border-neutral-800 flex items-center justify-between px-8 z-10 sticky top-0">
          <div className="flex flex-col">
            <h2 className="text-xl font-bold tracking-tight text-neutral-800 dark:text-neutral-100">
              {location.pathname === '/users' ? 'Gestão de Usuários' : 'Visão Geral'}
            </h2>
            <span className="text-xs text-neutral-400 font-medium">Bem-vindo de volta, Administrador</span>
          </div>
          
          <div className="flex items-center gap-6">
            <div className="flex items-center bg-neutral-100 dark:bg-neutral-800 p-1 rounded-full border border-neutral-200 dark:border-neutral-700">
               <button className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-full transition-all text-neutral-500 relative">
                <Bell size={18} />
                <span className="absolute top-2 right-2 w-2 h-2 bg-blue-500 rounded-full border-2 border-white dark:border-neutral-800"></span>
              </button>
            </div>
            <div className="h-8 w-[1px] bg-neutral-200 dark:bg-neutral-800"></div>
            <div className="flex items-center gap-3 group cursor-pointer">
              <div className="text-right hidden sm:block">
                <p className="text-sm font-bold leading-none">Paulo Takeda</p>
                <p className="text-[10px] text-neutral-500 font-medium mt-1 uppercase tracking-wider">Administrador</p>
              </div>
              <div className="w-10 h-10 rounded-2xl bg-gradient-to-br from-blue-500 to-indigo-600 flex items-center justify-center font-bold text-white shadow-lg shadow-indigo-500/20 group-hover:scale-105 transition-transform">
                PT
              </div>
            </div>
          </div>
        </header>

        <main className="flex-1 overflow-y-auto p-4 sm:p-8 bg-neutral-50 dark:bg-neutral-950 custom-scrollbar w-full">
          <div className="w-full space-y-8">
            <Outlet />
          </div>
        </main>
      </div>
    </div>
  );
}

function SidebarItem({ icon, label, to, isOpen, active }: { icon: any; label: string; to: string; isOpen: boolean; active?: boolean }) {
  return (
    <Link 
      to={to} 
      className={cn(
        "flex items-center p-3 rounded-xl transition-all duration-300 group relative",
        active 
          ? "bg-blue-600 text-white shadow-lg shadow-blue-500/30" 
          : "text-neutral-500 hover:bg-neutral-100 dark:hover:bg-neutral-800 hover:text-neutral-900 dark:hover:text-neutral-100"
      )}
    >
      <div className={cn("transition-transform duration-300 group-hover:scale-110", active ? "text-white" : "text-neutral-400 group-hover:text-blue-500")}>
        {icon}
      </div>
      <AnimatePresence>
        {isOpen && (
          <motion.span 
            initial={{ opacity: 0, x: -10 }}
            animate={{ opacity: 1, x: 0 }}
            className="ml-3 font-semibold text-sm whitespace-nowrap"
          >
            {label}
          </motion.span>
        )}
      </AnimatePresence>
      {active && (
        <motion.div 
          layoutId="sidebar-active"
          className="absolute left-[-1rem] w-1.5 h-6 bg-blue-600 rounded-r-full"
        />
      )}
    </Link>
  );
}

