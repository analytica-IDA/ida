import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Lock, User, Eye, EyeOff, Loader2, ShieldCheck } from 'lucide-react';
import { motion } from 'framer-motion';
import api from '../services/api';

export default function LoginPage() {
  const [login, setLogin] = useState('');
  const [senha, setSenha] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      const response = await api.post('/user/login', { login, senha });
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('user', JSON.stringify(response.data.user));
      navigate('/users');
    } catch (error: any) {
      alert(error.response?.data?.message || 'Erro ao realizar login. Verifique suas credenciais.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen w-full flex items-center justify-center bg-neutral-50 dark:bg-neutral-950 p-6 selection:bg-blue-500/30 selection:text-blue-200 overflow-hidden relative">
      {/* Dynamic Background Elements */}
      <div className="absolute inset-0 z-0 overflow-hidden">
        <div className="absolute top-[-10%] left-[-10%] w-[50%] h-[50%] bg-blue-500/10 blur-[120px] rounded-full animate-pulse"></div>
        <div className="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] bg-indigo-600/10 blur-[100px] rounded-full delay-700"></div>
      </div>

      <motion.div 
        initial={{ opacity: 0, scale: 0.95 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.5, ease: "easeOut" }}
        className="w-full max-w-md z-10"
      >
        <div className="bg-white/80 dark:bg-neutral-900/80 backdrop-blur-xl border border-neutral-200 dark:border-neutral-800 p-10 rounded-[2.5rem] shadow-2xl dark:shadow-none">
          <div className="text-center mb-10">
            <motion.div 
              initial={{ opacity: 0, y: -20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.2 }}
              className="w-16 h-16 bg-gradient-to-tr from-blue-600 to-indigo-600 rounded-2xl mx-auto mb-6 flex items-center justify-center text-white shadow-xl shadow-blue-500/30"
            >
              <ShieldCheck size={32} strokeWidth={2.5} />
            </motion.div>
            <h1 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white mb-3">Analytica IDA</h1>
            <p className="text-neutral-500 dark:text-neutral-400 font-medium tracking-tight">Painel Administrativo Segurado</p>
          </div>

          <form onSubmit={handleLogin} className="space-y-6">
            <div className="space-y-2">
              <label className="text-sm font-bold text-neutral-700 dark:text-neutral-300 ml-1">Usuário</label>
              <div className="relative group">
                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors">
                  <User size={18} />
                </div>
                <input
                  type="text"
                  value={login}
                  onChange={(e) => setLogin(e.target.value)}
                  className="w-full pl-12 pr-4 py-4 bg-neutral-100 dark:bg-neutral-800/50 border border-transparent focus:border-blue-500/50 focus:bg-white dark:focus:bg-neutral-800 rounded-2xl outline-none transition-all duration-300 font-medium"
                  placeholder="Seu login de acesso"
                  required
                />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-bold text-neutral-700 dark:text-neutral-300 ml-1">Senha</label>
              <div className="relative group">
                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors">
                  <Lock size={18} />
                </div>
                <input
                  type={showPassword ? 'text' : 'password'}
                  value={senha}
                  onChange={(e) => setSenha(e.target.value)}
                  className="w-full pl-12 pr-12 py-4 bg-neutral-100 dark:bg-neutral-800/50 border border-transparent focus:border-blue-500/50 focus:bg-white dark:focus:bg-neutral-800 rounded-2xl outline-none transition-all duration-300 font-medium"
                  placeholder="••••••••"
                  required
                />
                <button
                  type="button"
                  onClick={() => setShowPassword(!showPassword)}
                  className="absolute right-4 top-1/2 -translate-y-1/2 text-neutral-400 hover:text-neutral-600 dark:hover:text-neutral-200 transition-colors"
                >
                  {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
            </div>

            <div className="flex items-center justify-between px-1">
              <label className="flex items-center gap-2 cursor-pointer group">
                <div className="relative flex items-center">
                  <input type="checkbox" className="w-5 h-5 rounded-lg border-neutral-300 dark:border-neutral-700 text-blue-600 focus:ring-blue-500/30 transition-all cursor-pointer accent-blue-600" />
                </div>
                <span className="text-sm font-semibold text-neutral-500 dark:text-neutral-400 group-hover:text-neutral-700 dark:group-hover:text-neutral-200 transition-colors">Lembrar</span>
              </label>
              <button type="button" className="text-sm font-bold text-blue-600 hover:text-blue-700 dark:text-blue-400 transition-colors">Recuperar acesso</button>
            </div>

            <button
              type="submit"
              disabled={isLoading}
              className="w-full py-4 bg-blue-600 hover:bg-blue-700 text-white font-black rounded-2xl transition-all shadow-xl shadow-blue-500/20 active:scale-[0.98] disabled:opacity-50 flex items-center justify-center gap-3 group"
            >
              {isLoading ? (
                <Loader2 className="animate-spin" size={20} />
              ) : (
                <>
                  <span>Entrar no Sistema</span>
                  <motion.div
                    animate={{ x: [0, 4, 0] }}
                    transition={{ repeat: Infinity, duration: 1.5 }}
                  >
                    →
                  </motion.div>
                </>
              )}
            </button>
          </form>
        </div>

        <p className="mt-8 text-center text-neutral-400 dark:text-neutral-600 text-sm font-medium">
          © 2024 Analytica IDA • Todos os direitos reservados
        </p>
      </motion.div>
    </div>
  );
}

