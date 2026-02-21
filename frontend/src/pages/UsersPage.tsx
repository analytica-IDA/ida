import { Plus, Search, Filter, Edit2, Trash2, X, User as UserIcon, Lock, Shield, MapPin, Loader2 } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';
import { maskCPF, maskPhone, unmask } from '../utils/maskUtils';
import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

interface User {
  id: number;
  nome: string;
  login: string;
  email: string;
  cargo: string;
  roleDescription: string;
  area: string;
  flAtivo: boolean;
  dtUltimaAtualizacao: string;
}

interface Role {
  id: number;
  nome: string;
}

interface Cargo {
  id: number;
  nome: string;
}

interface Area {
  id: number;
  nome: string;
}

interface Pessoa {
  id: number;
  nome: string;
  cpf: string;
  email: string;
  telefone: string;
}

export default function UsersPage() {
  const [isModalOpen, setModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');

  // Queries
  const { data: users, isLoading } = useQuery<User[]>({
    queryKey: ['users'],
    queryFn: async () => {
      const { data } = await api.get('/user');
      return data;
    },
  });

  const { data: roles } = useQuery<Role[]>({
    queryKey: ['roles'],
    queryFn: async () => {
      const { data } = await api.get('/user/roles');
      return data;
    },
  });

  const { data: cargos } = useQuery<Cargo[]>({
    queryKey: ['cargos'],
    queryFn: async () => {
      const { data } = await api.get('/cargo');
      return data;
    },
  });

  const { data: areas } = useQuery<Area[]>({
    queryKey: ['areas'],
    queryFn: async () => {
      const { data } = await api.get('/area');
      return data;
    },
  });

  const { data: pessoas } = useQuery<Pessoa[]>({
    queryKey: ['pessoas'],
    queryFn: async () => {
      const { data } = await api.get('/pessoa');
      return data;
    },
  });

  const filteredUsers = users?.filter(u => 
    u.nome.toLowerCase().includes(searchTerm.toLowerCase()) || 
    u.login.toLowerCase().includes(searchTerm.toLowerCase()) || 
    u.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <motion.div 
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      className="space-y-8 pb-10"
    >
      <div className="flex flex-col lg:flex-row lg:items-end justify-between gap-6">
        <div>
          <h2 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Usuários</h2>
          <p className="text-neutral-500 dark:text-neutral-400 font-medium mt-1">
            {isLoading ? 'Carregando usuários...' : `Total de ${users?.length || 0} usuários ativos no sistema`}
          </p>
        </div>
        <div className="flex items-center gap-3">
          <button className="flex items-center justify-center gap-2 px-6 py-3 bg-white dark:bg-neutral-900 border border-neutral-200 dark:border-neutral-800 text-neutral-700 dark:text-neutral-300 rounded-2xl font-bold hover:bg-neutral-50 dark:hover:bg-neutral-800 transition-all shadow-sm">
            <Filter size={18} />
            <span>Filtros</span>
          </button>
          <button 
            onClick={() => setModalOpen(true)}
            className="flex items-center justify-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-2xl font-black hover:bg-blue-700 transition-all shadow-lg shadow-blue-500/20 active:scale-95"
          >
            <Plus size={18} strokeWidth={3} />
            <span>Criar Novo Usuário</span>
          </button>
        </div>
      </div>

      <div className="bg-white dark:bg-neutral-900 rounded-[2rem] shadow-xl shadow-neutral-200/50 dark:shadow-none border border-neutral-100 dark:border-neutral-800 overflow-hidden">
        <div className="p-6 border-b border-neutral-100 dark:border-neutral-800/50 flex flex-col md:flex-row md:items-center justify-between gap-4 bg-neutral-50/30 dark:bg-neutral-800/10">
          <div className="relative flex-1 max-w-md group">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
            <input 
              type="text" 
              placeholder="Buscar por nome, login ou email..." 
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-12 pr-4 py-3 bg-white dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-medium text-sm"
            />
          </div>
          <div className="flex items-center gap-2 text-xs font-bold text-neutral-400 uppercase tracking-widest px-2">
            Ordenar por: <span className="text-blue-600 dark:text-blue-400 cursor-pointer hover:underline ml-1">Mais recentes</span>
          </div>
        </div>

        <div className="overflow-x-auto min-h-[300px]">
          <table className="w-full">
            <thead className="bg-neutral-50/50 dark:bg-neutral-900/50 text-[10px] font-black text-neutral-400 uppercase tracking-[0.15em]">
              <tr>
                <th className="px-8 py-5 text-left">Código / Usuário</th>
                <th className="px-8 py-5 text-left">Atribuição</th>
                <th className="px-8 py-5 text-left">Situação</th>
                <th className="px-8 py-5 text-left">Área</th>
                <th className="px-8 py-5 text-center">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800">
              {isLoading ? (
                <tr>
                  <td colSpan={5} className="px-8 py-20 text-center">
                    <div className="flex flex-col items-center gap-3">
                      <Loader2 className="w-8 h-8 text-blue-500 animate-spin" />
                      <span className="text-sm font-bold text-neutral-400">Consultando base de dados...</span>
                    </div>
                  </td>
                </tr>
              ) : filteredUsers?.length === 0 ? (
                <tr>
                  <td colSpan={5} className="px-8 py-20 text-center">
                    <span className="text-sm font-bold text-neutral-400">Nenhum usuário encontrado</span>
                  </td>
                </tr>
              ) : (
                filteredUsers?.map(user => (
                  <UserRow 
                    key={user.id}
                    user={user}
                  />
                ))
              )}
            </tbody>
          </table>
        </div>
        
        <div className="p-6 bg-neutral-50/30 dark:bg-neutral-800/10 border-t border-neutral-100 dark:border-neutral-800 flex items-center justify-between">
            <span className="text-sm font-semibold text-neutral-500">Mostrando {filteredUsers?.length || 0} de {users?.length || 0} registros</span>
            <div className="flex gap-2">
                <button className="px-4 py-2 rounded-lg border border-neutral-200 dark:border-neutral-700 text-sm font-bold opacity-50 cursor-not-allowed">Anterior</button>
                <button className="px-4 py-2 rounded-lg border border-neutral-200 dark:border-neutral-700 text-sm font-bold hover:bg-neutral-100 dark:hover:bg-neutral-800 transition-colors">Próximo</button>
            </div>
        </div>
      </div>

      <AnimatePresence>
        {isModalOpen && (
          <RegisterModal 
            onClose={() => setModalOpen(false)} 
            roles={roles || []} 
            cargos={cargos || []}
            areas={areas || []}
            pessoas={pessoas || []}
          />
        )}
      </AnimatePresence>
    </motion.div>
  );
}

function UserRow({ user }: { user: User }) {
  const avatarColors = [
    "bg-blue-100 text-blue-600 dark:bg-blue-900/30 dark:text-blue-400",
    "bg-neutral-100 text-neutral-600 dark:bg-neutral-800 dark:text-neutral-400",
  ];
  
  const colorIndex = user.id % avatarColors.length;

  return (
    <tr className="group hover:bg-neutral-50/50 dark:hover:bg-neutral-800/30 transition-colors">
      <td className="px-8 py-6">
        <div className="flex items-center gap-4">
          <span className="text-xs font-black text-neutral-400 min-w-[2.5rem]">#{user.id}</span>
          <div className={cn("w-12 h-12 rounded-2xl flex items-center justify-center font-black text-lg transition-transform group-hover:scale-110", avatarColors[colorIndex])}>
            {user.nome.charAt(0)}
          </div>
          <div className="flex flex-col">
            <span className="font-bold text-neutral-900 dark:text-neutral-100 leading-tight">{user.nome}</span>
            <div className="flex items-center gap-2 mt-1">
                <span className="text-xs font-semibold text-neutral-400">@{user.login}</span>
                <span className="w-1 h-1 bg-neutral-300 rounded-full"></span>
                <span className="text-xs font-semibold text-neutral-400 truncate max-w-[150px]">{user.email}</span>
            </div>
          </div>
        </div>
      </td>
      <td className="px-8 py-6">
        <div className="flex flex-col">
          <span className="text-sm font-bold text-neutral-700 dark:text-neutral-300">{user.cargo}</span>
          <span className="text-[10px] uppercase tracking-wider text-neutral-400 font-bold mt-1">{user.roleDescription}</span>
        </div>
      </td>
      <td className="px-8 py-6">
        <span className={cn(
          "px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-wider",
          user.flAtivo ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400" : "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400"
        )}>
          {user.flAtivo ? 'Ativo' : 'Inativo'}
        </span>
      </td>
      <td className="px-8 py-6">
        <span className="text-sm font-semibold text-neutral-500">{user.area}</span>
      </td>
      <td className="px-8 py-6">
        <div className="flex justify-center gap-2 transition-opacity">
          <button className="p-2 hover:bg-blue-50 dark:hover:bg-blue-900/30 text-blue-600 rounded-xl transition-colors">
            <Edit2 size={16} />
          </button>
          <button className="p-2 hover:bg-red-50 dark:hover:bg-red-900/30 text-red-600 rounded-xl transition-colors">
            <Trash2 size={16} />
          </button>
        </div>
      </td>
    </tr>
  );
}

function RegisterModal({ onClose, roles, cargos, areas, pessoas }: { onClose: () => void; roles: Role[]; cargos: Cargo[]; areas: Area[]; pessoas: Pessoa[] }) {
  const [formData, setFormData] = useState({
    idPessoa: 0,
    login: '',
    senha: '',
    idCargo: cargos[0]?.id || 0,
    idArea: areas[0]?.id || 0,
  });

  const [isQuickCargoOpen, setQuickCargoOpen] = useState(false);
  const [isQuickAreaOpen, setQuickAreaOpen] = useState(false);
  const [isQuickPessoaOpen, setQuickPessoaOpen] = useState(false);
  
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (newUser: typeof formData) => api.post('/user/register', newUser),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['users'] });
      onClose();
    },
    onError: (error: any) => {
        alert(error.response?.data?.message || 'Erro ao registrar usuário');
    }
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    mutation.mutate(formData);
  };

  return (
    <motion.div 
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      exit={{ opacity: 0 }}
      className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-neutral-900/60 backdrop-blur-sm"
    >
      <motion.div
        initial={{ scale: 0.9, y: 20 }}
        animate={{ scale: 1, y: 0 }}
        exit={{ scale: 0.9, y: 20 }}
        className="bg-white dark:bg-neutral-900 w-full max-w-2xl rounded-[2.5rem] shadow-2xl overflow-hidden border border-neutral-100 dark:border-neutral-800"
      >
        <div className="p-8 border-b border-neutral-100 dark:border-neutral-800/50 flex items-center justify-between bg-neutral-50/50 dark:bg-neutral-800/20">
          <div className="flex items-center gap-4">
            <div className="w-12 h-12 rounded-2xl bg-blue-600 flex items-center justify-center text-white shadow-lg shadow-blue-500/20">
              <Plus size={24} strokeWidth={3} />
            </div>
            <div>
              <h3 className="text-2xl font-black tracking-tight text-neutral-900 dark:text-white">Novo Usuário</h3>
              <p className="text-sm font-medium text-neutral-500">Preencha as informações para o novo acesso</p>
            </div>
          </div>
          <button 
            onClick={onClose}
            className="p-3 hover:bg-neutral-100 dark:hover:bg-neutral-800 rounded-2xl transition-all text-neutral-400 hover:text-neutral-900 dark:hover:text-white"
          >
            <X size={24} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-8 space-y-8">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="md:col-span-2 space-y-2">
              <label className="text-xs font-black uppercase tracking-widest text-neutral-400 ml-1">Vincular Pessoa</label>
              <div className="flex gap-2">
                <div className="relative group flex-1">
                  <UserIcon className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
                  <select 
                    required
                    value={formData.idPessoa}
                    onChange={e => setFormData({...formData, idPessoa: Number(e.target.value)})}
                    className="w-full pl-12 pr-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-bold text-neutral-800 dark:text-white appearance-none"
                  >
                    <option value="">Selecione a Pessoa...</option>
                    {pessoas.map(p => (
                      <option key={p.id} value={p.id}>{p.nome} ({p.cpf})</option>
                    ))}
                  </select>
                </div>
                <button 
                  type="button"
                  onClick={() => setQuickPessoaOpen(true)}
                  className="p-4 bg-blue-50 dark:bg-blue-900/30 text-blue-600 rounded-2xl hover:bg-blue-100 transition-colors"
                  title="Cadastrar Nova Pessoa"
                >
                  <Plus size={20} />
                </button>
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-xs font-black uppercase tracking-widest text-neutral-400 ml-1">Login de Acesso</label>
              <div className="relative group">
                <Shield className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
                <input 
                  required
                  type="text" 
                  value={formData.login}
                  onChange={e => setFormData({...formData, login: e.target.value})}
                  className="w-full pl-12 pr-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-bold text-neutral-800 dark:text-white placeholder:text-neutral-400"
                  placeholder="Ex: joao.silva"
                />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-xs font-black uppercase tracking-widest text-neutral-400 ml-1">Senha Inicial</label>
              <div className="relative group">
                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
                <input 
                  required
                  type="password" 
                  value={formData.senha}
                  onChange={e => setFormData({...formData, senha: e.target.value})}
                  className="w-full pl-12 pr-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-bold text-neutral-800 dark:text-white placeholder:text-neutral-400"
                  placeholder="••••••••"
                />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-xs font-black uppercase tracking-widest text-neutral-400 ml-1">Cargo / Função</label>
              <div className="flex gap-2">
                <div className="relative group flex-1">
                  <Shield className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
                  <select 
                    required
                    value={formData.idCargo}
                    onChange={e => setFormData({...formData, idCargo: Number(e.target.value)})}
                    className="w-full pl-12 pr-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-bold text-neutral-800 dark:text-white appearance-none"
                  >
                    <option value="">Selecione...</option>
                    {cargos.map(cargo => (
                      <option key={cargo.id} value={cargo.id}>{cargo.nome}</option>
                    ))}
                  </select>
                </div>
                <button 
                  type="button"
                  onClick={() => setQuickCargoOpen(true)}
                  className="p-4 bg-blue-50 dark:bg-blue-900/30 text-blue-600 rounded-2xl hover:bg-blue-100 transition-colors"
                  title="Criar Novo Cargo"
                >
                  <Plus size={20} />
                </button>
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-xs font-black uppercase tracking-widest text-neutral-400 ml-1">Área de Atuação</label>
              <div className="flex gap-2">
                <div className="relative group flex-1">
                  <MapPin className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
                  <select 
                    required
                    value={formData.idArea}
                    onChange={e => setFormData({...formData, idArea: Number(e.target.value)})}
                    className="w-full pl-12 pr-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-bold text-neutral-800 dark:text-white appearance-none"
                  >
                    <option value="">Selecione...</option>
                    {areas.map(area => (
                      <option key={area.id} value={area.id}>{area.nome}</option>
                    ))}
                  </select>
                </div>
                <button 
                  type="button"
                  onClick={() => setQuickAreaOpen(true)}
                  className="p-4 bg-blue-50 dark:bg-blue-900/30 text-blue-600 rounded-2xl hover:bg-blue-100 transition-colors"
                  title="Criar Nova Área"
                >
                  <Plus size={20} />
                </button>
              </div>
            </div>
          </div>

          <div className="flex items-center gap-4 pt-4">
            <button 
              type="button"
              onClick={onClose}
              className="flex-1 py-4 bg-neutral-100 dark:bg-neutral-800 text-neutral-600 dark:text-neutral-400 rounded-2xl font-black hover:bg-neutral-200 dark:hover:bg-neutral-700 transition-all"
            >
              Cancelar
            </button>
            <button 
              type="submit"
              disabled={mutation.isPending}
              className="flex-[2] py-4 bg-blue-600 text-white rounded-2xl font-black hover:bg-blue-700 transition-all shadow-lg shadow-blue-500/20 active:scale-95 flex items-center justify-center gap-2"
            >
              {mutation.isPending ? <Loader2 className="animate-spin" size={20} /> : <Plus size={20} strokeWidth={3} />}
              <span>{mutation.isPending ? 'Cadastrando...' : 'Confirmar Cadastro'}</span>
            </button>
          </div>
        </form>

        <AnimatePresence>
          {isQuickCargoOpen && (
            <QuickCargoModal roles={roles} onClose={() => setQuickCargoOpen(false)} />
          )}
          {isQuickAreaOpen && (
            <QuickAreaModal onClose={() => setQuickAreaOpen(false)} />
          )}
          {isQuickPessoaOpen && (
            <QuickPessoaModal onClose={() => setQuickPessoaOpen(false)} />
          )}
        </AnimatePresence>
      </motion.div>
    </motion.div>
  );
}

function QuickCargoModal({ roles, onClose }: { roles: Role[], onClose: () => void }) {
  const [nome, setNome] = useState('');
  const [idRole, setIdRole] = useState(roles[0]?.id || 0);
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: (data: any) => api.post('/cargo', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['cargos'] });
      onClose();
    },
    onError: (error: any) => {
      alert(error.response?.data?.message || 'Erro ao criar cargo');
    }
  });

  return (
    <div className="fixed inset-0 z-[60] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <motion.div initial={{ scale: 0.9 }} animate={{ scale: 1 }} className="bg-white dark:bg-neutral-900 p-8 rounded-[2rem] shadow-2xl w-full max-w-sm border border-neutral-200 dark:border-neutral-800">
        <h4 className="text-xl font-bold mb-6">Novo Cargo</h4>
        <div className="space-y-4">
          <input 
            type="text" placeholder="Nome do cargo" value={nome} onChange={e => setNome(e.target.value)}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold"
          />
          <select 
            value={idRole} onChange={e => setIdRole(Number(e.target.value))}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold"
          >
            {roles.map(r => <option key={r.id} value={r.id}>{r.nome}</option>)}
          </select>
          <div className="flex gap-2 pt-2">
            <button onClick={onClose} className="flex-1 py-3 bg-neutral-100 dark:bg-neutral-800 rounded-xl font-bold">Cancelar</button>
            <button onClick={() => mutation.mutate({ nome, idRole })} className="flex-1 py-3 bg-blue-600 text-white rounded-xl font-bold">Criar</button>
          </div>
        </div>
      </motion.div>
    </div>
  );
}

function QuickAreaModal({ onClose }: { onClose: () => void }) {
  const [nome, setNome] = useState('');
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: (data: any) => api.post('/area', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['areas'] });
      onClose();
    },
    onError: (error: any) => {
      alert(error.response?.data?.message || 'Erro ao criar área');
    }
  });

  return (
    <div className="fixed inset-0 z-[60] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <motion.div initial={{ scale: 0.9 }} animate={{ scale: 1 }} className="bg-white dark:bg-neutral-900 p-8 rounded-[2rem] shadow-2xl w-full max-w-sm border border-neutral-200 dark:border-neutral-800">
        <h4 className="text-xl font-bold mb-6">Nova Área</h4>
        <div className="space-y-4">
          <input 
            type="text" placeholder="Nome da área" value={nome} onChange={e => setNome(e.target.value)}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold"
          />
          <div className="flex gap-2 pt-2">
            <button onClick={onClose} className="flex-1 py-3 bg-neutral-100 dark:bg-neutral-800 rounded-xl font-bold">Cancelar</button>
            <button onClick={() => mutation.mutate({ nome })} className="flex-1 py-3 bg-blue-600 text-white rounded-xl font-bold">Criar</button>
          </div>
        </div>
      </motion.div>
    </div>
  );
}

function QuickPessoaModal({ onClose }: { onClose: () => void }) {
  const [formData, setFormData] = useState({ nome: '', email: '', cpf: '', telefone: '' });
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: (data: any) => {
      const payload = {
        ...data,
        cpf: unmask(data.cpf),
        telefone: unmask(data.telefone)
      };
      return api.post('/pessoa', payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pessoas'] });
      onClose();
    },
    onError: (error: any) => {
      alert(error.response?.data?.message || error.response?.data?.title || 'Erro ao cadastrar pessoa');
    }
  });

  return (
    <div className="fixed inset-0 z-[60] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <motion.div initial={{ scale: 0.9 }} animate={{ scale: 1 }} className="bg-white dark:bg-neutral-900 p-8 rounded-[2rem] shadow-2xl w-full max-w-md border border-neutral-200 dark:border-neutral-800">
        <h4 className="text-xl font-bold mb-6">Nova Pessoa</h4>
        <div className="space-y-4">
          <input 
            type="text" placeholder="Nome completo" value={formData.nome} onChange={e => setFormData({ ...formData, nome: e.target.value })}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
          />
          <input 
            type="email" placeholder="E-mail" value={formData.email} onChange={e => setFormData({ ...formData, email: e.target.value })}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
          />
          <div className="grid grid-cols-2 gap-4">
            <input 
              type="text" placeholder="CPF" 
              value={maskCPF(formData.cpf)} 
              onChange={e => setFormData({ ...formData, cpf: unmask(e.target.value) })}
              className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
            />
            <input 
              type="text" placeholder="Telefone" 
              value={maskPhone(formData.telefone)} 
              onChange={e => setFormData({ ...formData, telefone: unmask(e.target.value) })}
              className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
            />
          </div>
          <div className="flex gap-2 pt-2">
            <button onClick={onClose} className="flex-1 py-3 bg-neutral-100 dark:bg-neutral-800 rounded-xl font-bold">Cancelar</button>
            <button onClick={() => mutation.mutate(formData)} className="flex-1 py-3 bg-blue-600 text-white rounded-xl font-bold">Cadastrar</button>
          </div>
        </div>
      </motion.div>
    </div>
  );
}

