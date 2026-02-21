import { Outlet, Link, useNavigate, useLocation } from 'react-router-dom';
import { LayoutDashboard, Users, Settings, LogOut, Sun, Moon, Menu, Bell, ChevronLeft, UserCircle, Briefcase, MapPin, Building2, BarChart3, Key, Mail, Fingerprint, Phone } from 'lucide-react';
import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

interface LayoutProps {
  toggleTheme: () => void;
  theme: string;
}

const iconMap: Record<string, any> = {
  "Página Inicial": <LayoutDashboard size={20} />,
  "Dashboard": <LayoutDashboard size={20} />,
  "Gerenciamento de Cliente": <Building2 size={20} />,
  "Gerenciamento de Pessoa": <UserCircle size={20} />,
  "Gerenciamento de Cargo": <Briefcase size={20} />,
  "Gerenciamento de Área": <MapPin size={20} />,
  "Gerenciamento de Usuário": <Users size={20} />,
  "Relatórios": <BarChart3 size={20} />,
  "Configurações": <Settings size={20} />,
};

const routeMap: Record<string, string> = {
  "Página Inicial": "/",
  "Dashboard": "/dashboard",
  "Gerenciamento de Cliente": "/clientes",
  "Gerenciamento de Pessoa": "/pessoas",
  "Gerenciamento de Cargo": "/cargos",
  "Gerenciamento de Área": "/areas",
  "Gerenciamento de Usuário": "/users",
  "Relatórios": "/reports",
  "Configurações": "/settings",
};

const menuOrder = [
  "Página Inicial",
  "Dashboard",
  "Gerenciamento de Cliente",
  "Gerenciamento de Pessoa",
  "Gerenciamento de Cargo",
  "Gerenciamento de Área",
  "Gerenciamento de Usuário",
  "Relatórios",
  "Configurações"
];

export default function DashboardLayout({ toggleTheme, theme }: LayoutProps) {
  const [isSidebarOpen, setSidebarOpen] = useState(true);
  const [isUserMenuOpen, setUserMenuOpen] = useState(false);
  const [isChangePasswordOpen, setChangePasswordOpen] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  const { data: userProfile } = useQuery({
    queryKey: ['user-me'],
    queryFn: async () => {
      const { data } = await api.get('/user/me');
      return data;
    },
  });

  const { data: menuItems, isLoading } = useQuery<string[]>({
    queryKey: ['menu'],
    queryFn: async () => {
      const { data } = await api.get('/user/menu');
      return data;
    },
  });

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  // Close dropdown on click outside
  useEffect(() => {
    const handleClickOutside = (event: any) => {
      if (isUserMenuOpen && !event.target.closest('.user-menu-container')) {
        setUserMenuOpen(false);
      }
    };
    window.addEventListener('mousedown', handleClickOutside);
    return () => window.removeEventListener('mousedown', handleClickOutside);
  }, [isUserMenuOpen]);


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
                <div className="w-8 h-8 rounded-lg bg-blue-600 flex items-center justify-center text-white font-black shadow-lg shadow-blue-500/20">
                  G
                </div>
                <span className="text-xl font-bold tracking-tight text-neutral-900 dark:text-neutral-100">
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
          {!isLoading && [
            ...(menuItems || []).map(name => {
              if (name === "Gestão de Usuários") return "Gerenciamento de Usuário";
              if (name === "Gerenciamento de Áreas") return "Gerenciamento de Área";
              return name;
            })
          ]
          .filter((name, index, self) => self.indexOf(name) === index) // Unique
          .sort((a, b) => menuOrder.indexOf(a) - menuOrder.indexOf(b)) // Follow user order
          .map(appName => (
            <SidebarItem 
              key={appName}
              icon={iconMap[appName] || <LayoutDashboard size={20} />} 
              label={appName} 
              to={routeMap[appName] || "/"} 
              isOpen={isSidebarOpen} 
              active={location.pathname === routeMap[appName]}
            />
          ))}
          {isLoading && (
            <div className="flex justify-center py-10">
              <div className="w-5 h-5 border-2 border-blue-600 border-t-transparent rounded-full animate-spin"></div>
            </div>
          )}
        </nav>

      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col h-screen overflow-hidden">
        <header className="h-20 bg-white/80 dark:bg-neutral-900/80 backdrop-blur-md border-b border-neutral-200 dark:border-neutral-800 flex items-center justify-between px-8 z-10 sticky top-0">
          <div className="flex flex-col">
            <h2 className="text-xl font-bold tracking-tight text-neutral-800 dark:text-neutral-100">
              {Object.keys(routeMap).find(key => routeMap[key] === location.pathname) || 'Visão Geral'}
            </h2>
            <span className="text-xs text-neutral-400 font-medium">Bem-vindo de volta, Administrador</span>
          </div>
          
          <div className="flex items-center gap-4">
            <div className="flex items-center bg-neutral-100 dark:bg-neutral-800 p-1.5 rounded-full border border-neutral-200 dark:border-neutral-700">
               <button className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-full transition-all text-neutral-400 relative">
                <Bell size={18} />
                <span className="absolute top-2 right-2 w-2 h-2 bg-blue-500 rounded-full border-2 border-white dark:border-neutral-800"></span>
              </button>
            </div>
            
            <div className="h-8 w-[px] bg-neutral-200 dark:bg-neutral-800"></div>

            {/* Profile Dropdown */}
            <div className="relative user-menu-container">
              <button 
                onClick={() => setUserMenuOpen(!isUserMenuOpen)}
                className="flex items-center gap-3 group cursor-pointer p-1 rounded-2xl hover:bg-neutral-50 dark:hover:bg-neutral-800 transition-colors"
                id="user-menu-button"
              >
                <div className="text-right hidden sm:block">
                  <p className="text-sm font-bold leading-none">{userProfile?.nome || 'Carregando...'}</p>
                  <p className="text-[10px] text-neutral-500 font-medium mt-1 uppercase tracking-wider">{userProfile?.cargo || (userProfile?.role === 'admin' ? 'Administrador' : 'Usuário')}</p>
                </div>
                <div className="w-10 h-10 rounded-2xl bg-blue-600 flex items-center justify-center font-bold text-white shadow-lg shadow-blue-500/20 group-hover:scale-105 transition-transform">
                  {(userProfile?.nome || 'PT').split(' ').map((n: string) => n[0]).join('').substring(0, 2).toUpperCase()}
                </div>
              </button>

              <AnimatePresence>
                {isUserMenuOpen && (
                  <motion.div
                    initial={{ opacity: 0, y: 10, scale: 0.95 }}
                    animate={{ opacity: 1, y: 0, scale: 1 }}
                    exit={{ opacity: 0, y: 10, scale: 0.95 }}
                    className="absolute right-0 mt-3 w-64 bg-white dark:bg-neutral-900 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-2xl p-2 z-50 overflow-hidden"
                  >
                    <div className="px-4 py-3 mb-2 border-b border-neutral-100 dark:border-neutral-800">
                        <p className="text-xs font-bold text-neutral-400 uppercase tracking-widest mb-1">Perfil e Ações</p>
                        {userProfile && (
                            <div className="mt-2 space-y-1">
                                <div className="flex items-center gap-2 text-[10px] text-neutral-500 font-medium">
                                    <Mail size={12} />
                                    <span className="truncate">{userProfile.email}</span>
                                </div>
                                <div className="flex items-center gap-2 text-[10px] text-neutral-500 font-medium">
                                    <Fingerprint size={12} />
                                    <span>{userProfile.cpf}</span>
                                </div>
                                {userProfile.telefone && (
                                    <div className="flex items-center gap-2 text-[10px] text-neutral-500 font-medium">
                                        <Phone size={12} />
                                        <span>{userProfile.telefone}</span>
                                    </div>
                                )}
                            </div>
                        )}
                    </div>

                    <button 
                      onClick={() => { setChangePasswordOpen(true); setUserMenuOpen(false); }}
                      className="w-full flex items-center gap-3 p-3 hover:bg-neutral-50 dark:hover:bg-neutral-800 rounded-xl transition-all group"
                    >
                      <div className="p-2 rounded-lg transition-colors bg-neutral-100 dark:bg-neutral-800 group-hover:bg-blue-50 dark:group-hover:bg-blue-900/30 text-neutral-500 group-hover:text-blue-600">
                        <Key size={18} />
                      </div>
                      <div className="flex flex-col items-start">
                        <span className="text-sm font-bold text-neutral-700 dark:text-neutral-200">Alterar Senha</span>
                        <span className="text-[10px] text-neutral-400 font-semibold">Atualize sua credencial</span>
                      </div>
                    </button>

                    <button 
                      onClick={() => { toggleTheme(); setUserMenuOpen(false); }}
                      className="w-full flex items-center gap-3 p-3 hover:bg-neutral-50 dark:hover:bg-neutral-800 rounded-xl transition-all group"
                    >
                      <div className={cn(
                        "p-2 rounded-lg transition-colors bg-neutral-100 dark:bg-neutral-800 group-hover:bg-white dark:group-hover:bg-neutral-700",
                        theme === 'dark' ? "text-amber-500" : "text-blue-600"
                      )}>
                        {theme === 'dark' ? <Sun size={18} /> : <Moon size={18} />}
                      </div>
                      <div className="flex flex-col items-start">
                        <span className="text-sm font-bold text-neutral-700 dark:text-neutral-200">Tema</span>
                        <span className="text-[10px] text-neutral-400 font-semibold">{theme === 'dark' ? 'Mudar para modo claro' : 'Mudar para modo escuro'}</span>
                      </div>
                    </button>

                    <div className="h-px bg-neutral-100 dark:bg-neutral-800 my-2 mx-2"></div>

                    <button 
                      onClick={handleLogout}
                      className="w-full flex items-center gap-3 p-3 hover:bg-red-50 dark:hover:bg-red-950/30 text-red-500 rounded-xl transition-all group"
                    >
                      <div className="p-2 bg-red-100/50 dark:bg-red-950/50 rounded-lg group-hover:bg-red-100 dark:group-hover:bg-red-900/50 transition-colors">
                        <LogOut size={18} />
                      </div>
                      <div className="flex flex-col items-start">
                        <span className="text-sm font-bold">Encerrar Sessão</span>
                        <span className="text-[10px] text-red-400/70 font-semibold">Sair do sistema com segurança</span>
                      </div>
                    </button>
                  </motion.div>
                )}
              </AnimatePresence>
            </div>
          </div>
        </header>

        <AnimatePresence>
            {isChangePasswordOpen && (
                <ChangePasswordModal onClose={() => setChangePasswordOpen(false)} />
            )}
        </AnimatePresence>

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

function ChangePasswordModal({ onClose }: { onClose: () => void }) {
  const [senhaAtual, setSenhaAtual] = useState('');
  const [novaSenha, setNovaSenha] = useState('');
  const [isPending, setIsPending] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsPending(true);
    try {
      await api.post('/user/change-password', { senhaAtual, novaSenha });
      alert('Senha alterada com sucesso!');
      onClose();
    } catch (error: any) {
      alert(error.response?.data?.message || 'Erro ao alterar senha');
    } finally {
      setIsPending(false);
    }
  };

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
      <motion.div 
        initial={{ scale: 0.9, opacity: 0 }} animate={{ scale: 1, opacity: 1 }}
        className="bg-white dark:bg-neutral-900 w-full max-w-md rounded-[2.5rem] shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800"
      >
        <div className="p-8 border-b border-neutral-100 dark:border-neutral-800/50 flex items-center justify-between">
            <div className="flex items-center gap-4">
                <div className="w-12 h-12 rounded-2xl bg-blue-600 flex items-center justify-center text-white">
                    <Key size={24} />
                </div>
                <div>
                    <h3 className="text-xl font-bold">Alterar Senha</h3>
                    <p className="text-xs text-neutral-500 font-medium">Atualize suas credenciais de acesso</p>
                </div>
            </div>
        </div>

        <form onSubmit={handleSubmit} className="p-8 space-y-6">
            <div className="space-y-2">
                <label className="text-[10px] font-black uppercase tracking-widest text-neutral-400 ml-1">Senha Atual</label>
                <input 
                    type="password" required autoFocus value={senhaAtual} onChange={e => setSenhaAtual(e.target.value)}
                    className="w-full px-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 font-bold"
                    placeholder="••••••••"
                />
            </div>
            <div className="space-y-2">
                <label className="text-[10px] font-black uppercase tracking-widest text-neutral-400 ml-1">Nova Senha</label>
                <input 
                    type="password" required value={novaSenha} onChange={e => setNovaSenha(e.target.value)}
                    className="w-full px-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 font-bold"
                    placeholder="••••••••"
                />
            </div>

            <div className="flex gap-3 pt-2">
                <button type="button" onClick={onClose} className="flex-1 py-4 bg-neutral-100 dark:bg-neutral-800 rounded-2xl font-bold transition-all hover:bg-neutral-200">Cancelar</button>
                <button type="submit" disabled={isPending} className="flex-1 py-4 bg-blue-600 text-white rounded-2xl font-bold shadow-lg shadow-blue-500/20 active:scale-95 disabled:opacity-50 transition-all">
                    {isPending ? 'Salvando...' : 'Alterar Senha'}
                </button>
            </div>
        </form>
      </motion.div>
    </div>
  );
}
