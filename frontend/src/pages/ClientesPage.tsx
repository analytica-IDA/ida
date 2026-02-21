import { Plus, Search, Edit2, Trash2, Building2, Loader2, X, Users } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';
import { maskCNPJ, unmask } from '../utils/maskUtils';

interface Cliente {
  id: number;
  nome: string;
  cnpj: string;
}

interface User {
  id: number;
  nome: string;
}

interface Area {
  id: number;
  nome: string;
}

interface ClienteUsuario {
  idUsuario: number;
  nome: string;
  area: string;
  idArea: number;
}

export default function ClientesPage() {
  const [isModalOpen, setModalOpen] = useState(false);
  const [isUserModalOpen, setUserModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [editingCliente, setEditingCliente] = useState<Cliente | null>(null);
  const [targetCliente, setTargetCliente] = useState<Cliente | null>(null);
  const queryClient = useQueryClient();

  const { data: clientes, isLoading } = useQuery<Cliente[]>({
    queryKey: ['clientes'],
    queryFn: async () => {
      const { data } = await api.get('/cliente');
      return data;
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/cliente/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['clientes'] }),
  });

  const filteredClientes = clientes?.filter(c => 
    c.nome.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Clientes</h1>
          <p className="text-neutral-500 dark:text-neutral-400 mt-1">Gerencie as empresas e parceiros do sistema.</p>
        </div>
        <button 
          onClick={() => { setEditingCliente(null); setModalOpen(true); }}
          className="flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-5 py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 font-semibold"
        >
          <Plus size={20} />
          Novo Cliente
        </button>
      </div>

      <div className="flex items-center gap-4 bg-white dark:bg-neutral-900 p-4 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={20} />
          <input 
            type="text" 
            placeholder="Buscar por nome do cliente..." 
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border-none rounded-xl focus:ring-2 focus:ring-blue-500/50 transition-all outline-none"
          />
        </div>
      </div>

      <div className="bg-white dark:bg-neutral-900 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-neutral-50 dark:bg-neutral-800/50 border-b border-neutral-200 dark:border-neutral-800">
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Código</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Razão Social / Nome</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">CNPJ</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300 text-right">Ações</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800">
            {isLoading ? (
              <tr>
                <td colSpan={3} className="px-6 py-20 text-center">
                  <div className="flex flex-col items-center gap-3">
                    <Loader2 className="animate-spin text-blue-600" size={32} />
                    <span className="text-neutral-500">Carregando clientes...</span>
                  </div>
                </td>
              </tr>
            ) : filteredClientes?.map((cliente) => (
              <tr key={cliente.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors group">
                <td className="px-6 py-4 text-sm text-neutral-500">#{cliente.id}</td>
                <td className="px-6 py-4">
                  <div className="flex items-center gap-3">
                    <div className="w-8 h-8 rounded-lg bg-blue-100 dark:bg-blue-900/30 flex items-center justify-center text-blue-600">
                      <Building2 size={16} />
                    </div>
                    <span className="font-medium">{cliente.nome}</span>
                  </div>
                </td>
                <td className="px-6 py-4 text-sm font-medium text-neutral-700 dark:text-neutral-300">
                  {cliente.cnpj ? maskCNPJ(cliente.cnpj) : 'Não informado'}
                </td>
                <td className="px-6 py-4 text-right">
                  <div className="flex items-center justify-end gap-2 transition-opacity">
                    <button 
                      onClick={() => { setTargetCliente(cliente); setUserModalOpen(true); }}
                      className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-lg text-blue-600 hover:text-blue-700 transition-colors shadow-sm"
                      title="Gerenciar Usuários"
                    >
                      <Users size={18} />
                    </button>
                    <button 
                      onClick={() => { setEditingCliente(cliente); setModalOpen(true); }}
                      className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-lg text-neutral-500 hover:text-blue-600 transition-colors shadow-sm"
                    >
                      <Edit2 size={18} />
                    </button>
                    <button 
                      onClick={() => { if(confirm('Excluir cliente?')) deleteMutation.mutate(cliente.id); }}
                      className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-lg text-neutral-500 hover:text-red-600 transition-colors shadow-sm"
                    >
                      <Trash2 size={18} />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <AnimatePresence>
        {isModalOpen && (
          <ClienteModal 
            onClose={() => setModalOpen(false)} 
            cliente={editingCliente}
          />
        )}
        {isUserModalOpen && targetCliente && (
          <ClientUserModal 
            onClose={() => setUserModalOpen(false)} 
            cliente={targetCliente}
          />
        )}
      </AnimatePresence>
    </div>
  );
}

function ClientUserModal({ onClose, cliente }: { onClose: () => void; cliente: Cliente }) {
  const [selectedUser, setSelectedUser] = useState<number>(0);
  const [selectedArea, setSelectedArea] = useState<number>(0);
  const [isQuickAreaOpen, setQuickAreaOpen] = useState(false);
  const queryClient = useQueryClient();

  const { data: associations, isLoading: loadingAssoc } = useQuery<ClienteUsuario[]>({
    queryKey: ['cliente-usuarios', cliente.id],
    queryFn: async () => {
      const { data } = await api.get(`/cliente/${cliente.id}/usuarios`);
      return data;
    },
  });

  const { data: allUsers } = useQuery<User[]>({
    queryKey: ['users-simple'],
    queryFn: async () => {
      const { data } = await api.get('/user');
      return data;
    },
  });

  const { data: allAreas } = useQuery<Area[]>({
    queryKey: ['areas'],
    queryFn: async () => {
      const { data } = await api.get('/area');
      return data;
    },
  });

  const associateMutation = useMutation({
    mutationFn: (data: { idUsuario: number; idArea: number }) => api.post(`/cliente/${cliente.id}/usuarios`, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['cliente-usuarios', cliente.id] });
      setSelectedUser(0);
      setSelectedArea(0);
    },
  });

  const removeMutation = useMutation({
    mutationFn: (userId: number) => api.delete(`/cliente/${cliente.id}/usuarios/${userId}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['cliente-usuarios', cliente.id] }),
  });

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
      <motion.div 
        initial={{ opacity: 0, scale: 0.95 }} animate={{ opacity: 1, scale: 1 }} exit={{ opacity: 0, scale: 0.95 }}
        className="bg-white dark:bg-neutral-900 w-full max-w-2xl rounded-3xl shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800"
      >
        <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 flex items-center justify-between bg-neutral-50/50 dark:bg-neutral-800/50">
          <div>
            <h2 className="text-xl font-bold">Vincular Usuários</h2>
            <p className="text-xs text-neutral-500 font-medium uppercase tracking-wider">{cliente.nome}</p>
          </div>
          <button onClick={onClose} className="p-2 hover:bg-neutral-200 dark:hover:bg-neutral-700 rounded-full transition-colors">
            <X size={20} />
          </button>
        </div>

        <div className="p-8 space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 bg-neutral-50 dark:bg-neutral-800/50 p-6 rounded-2xl border border-neutral-100 dark:border-neutral-800">
            <div className="space-y-1.5">
              <label className="text-[10px] font-black uppercase tracking-widest text-neutral-400 ml-1">Usuário</label>
              <select 
                value={selectedUser} onChange={e => setSelectedUser(Number(e.target.value))}
                className="w-full px-4 py-2.5 bg-white dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold text-sm"
              >
                <option value={0}>Selecionar Usuário...</option>
                {allUsers?.map(u => <option key={u.id} value={u.id}>{u.nome}</option>)}
              </select>
            </div>
            <div className="space-y-1.5">
              <label className="text-[10px] font-black uppercase tracking-widest text-neutral-400 ml-1">Área / Unidade</label>
              <div className="flex gap-2">
                <select 
                    value={selectedArea} onChange={e => setSelectedArea(Number(e.target.value))}
                    className="flex-1 px-4 py-2.5 bg-white dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold text-sm"
                >
                    <option value={0}>Selecionar Área...</option>
                    {allAreas?.map(a => <option key={a.id} value={a.id}>{a.nome}</option>)}
                </select>
                <button 
                  type="button"
                  onClick={() => setQuickAreaOpen(true)}
                  className="p-3 bg-blue-50 dark:bg-blue-900/30 text-blue-600 rounded-xl hover:bg-blue-100 transition-colors"
                  title="Nova Área"
                >
                  <Plus size={18} />
                </button>
              </div>
            </div>
            <button 
              onClick={() => associateMutation.mutate({ idUsuario: selectedUser, idArea: selectedArea })}
              disabled={!selectedUser || !selectedArea || associateMutation.isPending}
              className="md:col-span-2 bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 rounded-xl transition-all shadow-lg shadow-blue-500/25 disabled:opacity-50 flex items-center justify-center gap-2"
            >
              {associateMutation.isPending ? <Loader2 className="animate-spin" size={20} /> : <Plus size={20} />}
              <span>Vincular Usuário</span>
            </button>
          </div>

          <AnimatePresence>
            {isQuickAreaOpen && (
              <div className="fixed inset-0 z-[60] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
                <QuickAreaOverlay onClose={() => setQuickAreaOpen(false)} />
              </div>
            )}
          </AnimatePresence>

          <div className="space-y-4">
            <h3 className="font-bold text-sm text-neutral-500 uppercase tracking-widest ml-1">Usuários Ativos</h3>
            <div className="max-h-[300px] overflow-y-auto space-y-2 pr-2 custom-scrollbar">
              {loadingAssoc ? (
                <div className="py-10 flex justify-center"><Loader2 className="animate-spin text-blue-600" /></div>
              ) : associations?.length === 0 ? (
                <div className="py-10 text-center text-sm font-bold text-neutral-400 bg-neutral-50 dark:bg-neutral-800/20 rounded-2xl border-2 border-dashed border-neutral-100 dark:border-neutral-800">
                  Nenhum usuário vinculado
                </div>
              ) : associations?.map(assoc => (
                <div key={assoc.idUsuario} className="flex items-center justify-between p-4 bg-white dark:bg-neutral-800 border border-neutral-100 dark:border-neutral-800 rounded-2xl group hover:border-blue-500/30 transition-all">
                  <div className="flex items-center gap-4">
                    <div className="w-10 h-10 rounded-xl bg-neutral-100 dark:bg-neutral-700 flex items-center justify-center text-neutral-500 font-black">
                      {assoc.nome.charAt(0)}
                    </div>
                    <div>
                      <p className="font-bold text-sm">{assoc.nome}</p>
                      <p className="text-[10px] font-black uppercase tracking-tighter text-blue-600">{assoc.area}</p>
                    </div>
                  </div>
                  <button 
                    onClick={() => removeMutation.mutate(assoc.idUsuario)}
                    className="p-2 hover:bg-red-50 dark:hover:bg-red-900/30 text-neutral-300 hover:text-red-600 rounded-lg transition-colors"
                  >
                    <Trash2 size={16} />
                  </button>
                </div>
              ))}
            </div>
          </div>
        </div>
      </motion.div>
    </div>
  );
}

function ClienteModal({ onClose, cliente }: { onClose: () => void; cliente: Cliente | null }) {
  const [formData, setFormData] = useState({
    nome: cliente?.nome || '',
    cnpj: cliente?.cnpj || ''
  });
  
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: typeof formData) => {
      const payload = {
        ...data,
        cnpj: unmask(data.cnpj)
      };
      return cliente ? api.put(`/cliente/${cliente.id}`, { ...payload, id: cliente.id }) : api.post('/cliente', payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientes'] });
      onClose();
    },
  });

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
      <motion.div 
        initial={{ opacity: 0, scale: 0.95, y: 20 }}
        animate={{ opacity: 1, scale: 1, y: 0 }}
        exit={{ opacity: 0, scale: 0.95, y: 20 }}
        className="bg-white dark:bg-neutral-900 w-full max-w-md rounded-3xl shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800"
      >
        <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 flex items-center justify-between bg-neutral-50/50 dark:bg-neutral-800/50">
          <h2 className="text-xl font-bold">{cliente ? 'Editar Cliente' : 'Novo Cliente'}</h2>
          <button onClick={onClose} className="p-2 hover:bg-neutral-200 dark:hover:bg-neutral-700 rounded-full transition-colors">
            <X size={20} />
          </button>
        </div>

        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(formData); }} className="p-8 space-y-6">
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Nome do Cliente / Empresa</label>
              <div className="relative">
                <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text" required
                  value={formData.nome}
                  onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                  placeholder="Ex: Analytica Solutions"
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">CNPJ</label>
              <div className="relative">
                <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text" required
                  value={maskCNPJ(formData.cnpj)}
                  onChange={(e) => setFormData({ ...formData, cnpj: unmask(e.target.value) })}
                  placeholder="00.000.000/0000-00"
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold"
                />
              </div>
            </div>
          </div>

          <div className="flex gap-3 pt-4">
            <button 
              type="button" 
              onClick={onClose}
              className="flex-1 px-4 py-2.5 border border-neutral-200 dark:border-neutral-700 rounded-xl font-medium hover:bg-neutral-50 dark:hover:bg-neutral-800 transition-colors"
            >
              Cancelar
            </button>
            <button 
              type="submit"
              disabled={mutation.isPending}
              className="flex-1 bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 disabled:opacity-50 flex items-center justify-center gap-2"
            >
              {mutation.isPending ? <Loader2 className="animate-spin" size={20} /> : (cliente ? 'Salvar' : 'Criar')}
            </button>
          </div>
        </form>
      </motion.div>
    </div>
  );
}

function QuickAreaOverlay({ onClose }: { onClose: () => void }) {
  const [nome, setNome] = useState('');
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: (data: any) => api.post('/area', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['areas'] });
      onClose();
    }
  });

  return (
    <motion.div 
      initial={{ scale: 0.9, opacity: 0 }} 
      animate={{ scale: 1, opacity: 1 }} 
      className="bg-white dark:bg-neutral-900 p-8 rounded-[2rem] shadow-2xl w-full max-w-sm border border-neutral-200 dark:border-neutral-800"
    >
      <h4 className="text-xl font-bold mb-6">Nova Área</h4>
      <div className="space-y-4">
        <input 
          type="text" placeholder="Nome da área/unidade" autoFocus value={nome} onChange={e => setNome(e.target.value)}
          className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
        />
        <div className="flex gap-2 pt-2">
          <button onClick={onClose} className="flex-1 py-3 bg-neutral-100 dark:bg-neutral-800 rounded-xl font-bold">Cancelar</button>
          <button onClick={() => mutation.mutate({ nome })} className="flex-1 py-3 bg-blue-600 text-white rounded-xl font-bold">Criar</button>
        </div>
      </div>
    </motion.div>
  );
}
