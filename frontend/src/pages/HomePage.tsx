import { motion } from 'framer-motion';
import { Users, Building2, UserCircle, BarChart3, Settings, ArrowRight, Loader2 } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';

// Initial state for stats while loading
const initialStats = [
  { label: 'Usuários Ativos', key: 'activeUsers', icon: Users, color: 'text-blue-600', bg: 'bg-blue-100 dark:bg-blue-900/30' },
  { label: 'Clientes Parceiros', key: 'partnerClients', icon: Building2, color: 'text-blue-600', bg: 'bg-blue-50 dark:bg-blue-900/10' },
  { label: 'Pessoas Cadastradas', key: 'registeredPersons', icon: UserCircle, color: 'text-blue-600', bg: 'bg-blue-50 dark:bg-blue-900/10' },
  { label: 'Relatórios Gerados', key: 'reportsGenerated', icon: BarChart3, color: 'text-blue-600', bg: 'bg-blue-50 dark:bg-blue-900/10' },
];

const shortcuts = [
  { title: 'Gerenciar Usuários', description: 'Administre contas e permissões de acesso.', icon: Users, to: '/users', color: 'blue' },
  { title: 'Base de Pessoas', description: 'Dados cadastrais unificados de colaboradores.', icon: UserCircle, to: '/pessoas', color: 'blue' },
  { title: 'Portfólio de Clientes', description: 'Gestão de contratos e áreas de atuação.', icon: Building2, to: '/clientes', color: 'blue' },
  { title: 'Configurações', description: 'Parametrização global do sistema IDA.', icon: Settings, to: '/settings', color: 'neutral' },
];

export default function HomePage() {
  const navigate = useNavigate();

  const { data: statsData, isLoading } = useQuery({
    queryKey: ['dashboard-stats'],
    queryFn: async () => {
      const { data } = await api.get('/dashboard/stats');
      return data;
    },
  });

  return (
    <div className="space-y-12 animate-in fade-in duration-700">
      {/* Hero Section */}
      <section className="relative overflow-hidden rounded-[2.5rem] bg-gradient-to-br from-blue-600 to-blue-800 p-8 md:p-16 text-white shadow-2xl shadow-blue-500/20">
        <div className="relative z-10 max-w-2xl space-y-6">
          <motion.div 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/10 backdrop-blur-md border border-white/20 text-sm font-medium"
          >
            <span className="w-2 h-2 rounded-full bg-blue-400 animate-pulse" />
            Bem-vindo ao Ecossistema Analytica
          </motion.div>
          <motion.h1 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.1 }}
            className="text-4xl md:text-6xl font-black tracking-tight leading-[1.1]"
          >
            Sua central de inteligência para <span className="text-blue-200">gestão de dados</span>.
          </motion.h1>
          <motion.p 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.2 }}
            className="text-lg text-blue-100/80 leading-relaxed font-medium"
          >
            O módulo IDA consolida pessoas, clientes e usuários em uma plataforma unificada de alta performance.
          </motion.p>
          <motion.div 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.3 }}
            className="pt-4"
          >
            <button 
              onClick={() => navigate('/dashboard')}
              className="group flex items-center gap-3 bg-white text-blue-700 px-8 py-4 rounded-2xl font-bold hover:bg-blue-50 transition-all shadow-xl shadow-black/10 text-lg"
            >
              Explorar Dashboard
              <ArrowRight className="group-hover:translate-x-1 transition-transform" />
            </button>
          </motion.div>
        </div>

        {/* Abstract Background Design */}
        <div className="absolute top-0 right-0 w-1/2 h-full opacity-10 pointer-events-none">
          <div className="absolute top-[-20%] right-[-10%] w-[120%] h-[150%] rounded-[50%] border-[40px] border-white rotate-12" />
        </div>
      </section>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {initialStats.map((stat, idx) => (
          <motion.div
            key={stat.label}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.1 * idx }}
            className="bg-white dark:bg-neutral-900 p-6 rounded-3xl border border-neutral-200 dark:border-neutral-800 shadow-sm flex items-center gap-5 translate-z-0"
          >
            <div className={`w-14 h-14 rounded-2xl ${stat.bg} ${stat.color} flex items-center justify-center shrink-0`}>
              <stat.icon size={28} />
            </div>
            <div>
              {isLoading ? (
                <div className="h-8 flex items-center">
                    <Loader2 className="w-5 h-5 animate-spin text-neutral-300" />
                </div>
              ) : (
                <p className="text-3xl font-black text-neutral-900 dark:text-white leading-none mb-1">
                    {(statsData as any)?.[stat.key] ?? 0}
                </p>
              )}
              <p className="text-sm font-semibold text-neutral-500 dark:text-neutral-400">{stat.label}</p>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Quick Access */}
      <section className="space-y-6">
        <div className="flex items-center justify-between">
          <h2 className="text-2xl font-bold tracking-tight">Atalhos Disponíveis</h2>
          <div className="h-1 flex-1 mx-8 bg-neutral-100 dark:bg-neutral-800 rounded-full" />
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {shortcuts.map((shortcut, idx) => (
            <motion.div
              key={shortcut.title}
              initial={{ opacity: 0, scale: 0.95 }}
              animate={{ opacity: 1, scale: 1 }}
              transition={{ delay: 0.2 + idx * 0.05 }}
              onClick={() => navigate(shortcut.to)}
              className="group cursor-pointer bg-white dark:bg-neutral-900 p-8 rounded-[2rem] border border-neutral-200 dark:border-neutral-800 hover:border-blue-500/50 hover:shadow-2xl transition-all duration-500 relative overflow-hidden"
            >
              <div className="flex items-start justify-between relative z-10">
                <div className={`p-4 rounded-2xl bg-${shortcut.color}-50 dark:bg-${shortcut.color}-900/20 text-${shortcut.color}-600 group-hover:scale-110 transition-transform duration-500`}>
                  <shortcut.icon size={32} />
                </div>
                <div className="w-12 h-12 rounded-full border border-neutral-200 dark:border-neutral-800 flex items-center justify-center text-neutral-400 group-hover:text-blue-500 group-hover:border-blue-500/30 transition-all">
                  <ArrowRight size={20} className="group-hover:translate-x-1 transition-transform" />
                </div>
              </div>
              <div className="mt-8 relative z-10">
                <h3 className="text-xl font-bold mb-2 group-hover:text-blue-600 transition-colors uppercase tracking-tight">{shortcut.title}</h3>
                <p className="text-neutral-500 dark:text-neutral-400 font-medium">{shortcut.description}</p>
              </div>
              
              {/* Subtle hover decorations */}
              <div className="absolute bottom-0 right-0 w-32 h-32 bg-gradient-to-br from-transparent to-blue-500/5 translate-x-16 translate-y-16 group-hover:translate-x-8 group-hover:translate-y-8 transition-transform duration-700 rounded-full" />
            </motion.div>
          ))}
        </div>
      </section>
    </div>
  );
}
