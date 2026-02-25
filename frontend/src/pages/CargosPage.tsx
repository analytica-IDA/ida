import { Plus, Search, Edit2, Trash2, Briefcase, Loader2, X, Shield, Building2 } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';


interface Role {
  id: number;
  nome: string;
}

interface Cargo {
  id: number;
  nome: string;
  idRole: number;
  role?: Role;
  idCliente?: number;
  nomeCliente?: string;
}

export default function CargosPage() {
  const [isModalOpen, setModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCliente, setSelectedCliente] = useState<string>('');
  const [editingCargo, setEditingCargo] = useState<Cargo | null>(null);
  const [isQuickRoleOpen, setQuickRoleOpen] = useState(false);
  const [isQuickClienteOpen, setQuickClienteOpen] = useState(false);
  const queryClient = useQueryClient();

  const { data: userProfile } = useQuery<any>({
    queryKey: ['user-me'],
    queryFn: async () => {
      const { data } = await api.get('/user/me');
      return data;
    },
  });

  const { data: clientes } = useQuery<any[]>({
    queryKey: ['clientes'],
    queryFn: async () => {
      const { data } = await api.get('/cliente');
      return data;
    },
  });

  const { data: cargos, isLoading } = useQuery<Cargo[]>({
    queryKey: ['cargos'],
    queryFn: async () => {
      const { data } = await api.get('/cargo');
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

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/cargo/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['cargos'] }),
  });

  const filteredCargos = cargos?.filter(c => {
    const matchesSearch = c.nome.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCliente = !selectedCliente || c.nomeCliente === selectedCliente;
    return matchesSearch && matchesCliente;
  });

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Cargos</h1>
          <p className="text-neutral-500 dark:text-neutral-400 mt-1">Configure os cargos e suas associações de acesso.</p>
        </div>
        <button 
          onClick={() => { setEditingCargo(null); setModalOpen(true); }}
          className="flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-5 py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 font-semibold"
        >
          <Plus size={20} />
          Criar Novo Cargo
        </button>
      </div>

      <div className="flex items-center gap-4 bg-white dark:bg-neutral-900 p-4 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={20} />
          <input 
            type="text" 
            placeholder="Buscar por nome do cargo..." 
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border-none rounded-xl focus:ring-2 focus:ring-blue-500/50 transition-all outline-none"
          />
        </div>
        
        {userProfile?.role === 'admin' && (
          <div className="relative min-w-[200px]">
            <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
            <select 
              value={selectedCliente}
              onChange={(e) => setSelectedCliente(e.target.value)}
              className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border-none rounded-xl focus:ring-2 focus:ring-blue-500/50 transition-all outline-none appearance-none font-medium"
            >
              <option value="">Todos os Clientes</option>
              {clientes?.map(c => (
                <option key={c.id} value={c.nome}>{c.nome}</option>
              ))}
            </select>
          </div>
        )}
      </div>

      <div className="bg-white dark:bg-neutral-900 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-neutral-50 dark:bg-neutral-800/50 border-b border-neutral-200 dark:border-neutral-800">
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Código</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Cargo</th>
              {userProfile?.role === 'admin' && (
                <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Cliente</th>
              )}
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Nível de Acesso (Role)</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300 text-right">Ações</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800">
            {isLoading ? (
              <tr>
                <td colSpan={userProfile?.role === 'admin' ? 5 : 4} className="px-6 py-20 text-center">
                  <div className="flex flex-col items-center gap-3">
                    <Loader2 className="animate-spin text-blue-600" size={32} />
                    <span className="text-neutral-500">Carregando cargos...</span>
                  </div>
                </td>
              </tr>
            ) : filteredCargos?.map((cargo) => (
              <tr key={cargo.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors group">
                <td className="px-6 py-4 text-sm text-neutral-500">#{cargo.id}</td>
                <td className="px-6 py-4">
                  <div className="flex items-center gap-3">
                    <div className="w-8 h-8 rounded-lg bg-blue-100 dark:bg-blue-900/30 flex items-center justify-center text-blue-600">
                      <Briefcase size={16} />
                    </div>
                    <span className="font-medium">{cargo.nome}</span>
                  </div>
                </td>
                {userProfile?.role === 'admin' && (
                  <td className="px-6 py-4">
                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-neutral-100 dark:bg-neutral-800 text-neutral-600 dark:text-neutral-400 border border-neutral-200 dark:border-neutral-700">
                      {cargo.nomeCliente || 'Sem cliente'}
                    </span>
                  </td>
                )}
                <td className="px-6 py-4 font-medium text-neutral-600 dark:text-neutral-400">
                  {cargo.role?.nome || "Sem nível atribuído"}
                </td>
                <td className="px-6 py-4 text-right">
                  <div className="flex items-center justify-end gap-2 transition-opacity">
                    <button 
                      onClick={() => { setEditingCargo(cargo); setModalOpen(true); }}
                      className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-lg text-neutral-500 hover:text-blue-600 transition-colors shadow-sm"
                    >
                      <Edit2 size={18} />
                    </button>
                    <button 
                      onClick={() => deleteMutation.mutate(cargo.id)}
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
          <CargoModal 
            onClose={() => setModalOpen(false)} 
            roles={roles || []}
            cargo={editingCargo}
            userProfile={userProfile}
            clientes={clientes || []}
            onQuickRole={() => setQuickRoleOpen(true)}
            onQuickCliente={() => setQuickClienteOpen(true)}
          />
        )}
        {isQuickRoleOpen && (
          <QuickRoleModal onClose={() => setQuickRoleOpen(false)} />
        )}
        {isQuickClienteOpen && (
          <QuickClienteModal onClose={() => setQuickClienteOpen(false)} />
        )}
      </AnimatePresence>
    </div>
  );
}

function CargoModal({ onClose, roles, cargo, userProfile, clientes, onQuickRole, onQuickCliente }: { onClose: () => void; roles: Role[]; cargo: Cargo | null; userProfile: any; clientes: any[]; onQuickRole: () => void; onQuickCliente: () => void }) {
  const filteredRoles = roles.filter(role => {
    const userRole = userProfile?.role?.toLowerCase();
    const roleName = role.nome.toLowerCase();
    
    if (userRole === 'admin') return true;
    if (userRole === 'proprietário') return roleName === 'supervisor' || roleName === 'vendedor';
    if (userRole === 'supervisor') return roleName === 'vendedor';
    return false;
  });

  const [formData, setFormData] = useState<any>({
    nome: cargo?.nome || '',
    idRole: cargo?.idRole || (filteredRoles.length > 0 ? filteredRoles[0].id : 0),
    idCliente: cargo?.idCliente || ''
  });

  const [isQuickRoleOpen, setQuickRoleOpen] = useState(false);
  
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: typeof formData) => 
      cargo ? api.put(`/cargo/${cargo.id}`, { ...data, id: cargo.id }) : api.post('/cargo', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['cargos'] });
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
          <h2 className="text-xl font-bold">{cargo ? 'Editar Cargo' : 'Novo Cargo'}</h2>
          <button onClick={onClose} className="p-2 hover:bg-neutral-200 dark:hover:bg-neutral-700 rounded-full transition-colors">
            <X size={20} />
          </button>
        </div>

        <form onSubmit={(e) => { 
          e.preventDefault(); 
          const dataToSubmit = { ...formData };
          if (userProfile?.role === 'proprietário') {
            dataToSubmit.idCliente = userProfile.idCliente;
          }
          mutation.mutate(dataToSubmit); 
        }} className="p-8 space-y-6">
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Nome do Cargo</label>
              <div className="relative">
                <Briefcase className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text" 
                  required
                  value={formData.nome}
                  onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                  placeholder="Ex: Vendedor Sênior"
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Nível de Acesso (Role)</label>
              <div className="flex gap-2">
                <div className="relative flex-1">
                  <Shield className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                  <select 
                    required
                    value={formData.idRole}
                    onChange={(e) => setFormData({ ...formData, idRole: Number(e.target.value) })}
                    className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none appearance-none"
                  >
                    <option value="">Selecione um nível...</option>
                    {filteredRoles.map(role => (
                      <option key={role.id} value={role.id}>{role.nome}</option>
                    ))}
                  </select>
                </div>
                <button 
                  type="button"
                  onClick={onQuickRole}
                  className="p-3 bg-blue-50 dark:bg-blue-900/30 text-blue-600 rounded-xl hover:bg-blue-100 transition-colors"
                  title="Criar Novo Nível"
                >
                  <Plus size={20} />
                </button>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Cliente / Empresa</label>
              <div className="flex gap-2">
                <div className="relative flex-1">
                  <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                  {userProfile?.role === 'proprietário' ? (
                    <div className="w-full pl-10 pr-4 py-2.5 bg-neutral-100 dark:bg-neutral-800/50 border border-neutral-200 dark:border-neutral-700 rounded-xl text-neutral-600 dark:text-neutral-400 cursor-not-allowed flex justify-between items-center">
                      <span>{clientes.find(c => c.id === userProfile.idCliente)?.nome || 'Cliente Atual'}</span>
                      <span className="text-xs font-bold px-2 py-1 bg-blue-100 dark:bg-blue-900/30 text-blue-600 rounded-md">Vínculo Automático</span>
                    </div>
                  ) : (
                    <select 
                      required
                      value={formData.idCliente}
                      onChange={(e) => setFormData({ ...formData, idCliente: Number(e.target.value) })}
                      className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none appearance-none"
                    >
                      <option value="">Selecione um cliente...</option>
                      {clientes.map(cliente => (
                        <option key={cliente.id} value={cliente.id}>{cliente.nome}</option>
                      ))}
                    </select>
                  )}
                </div>
                {userProfile?.role === 'admin' && (
                  <button 
                    type="button"
                    onClick={onQuickCliente}
                    className="p-3 bg-blue-50 dark:bg-blue-900/30 text-blue-600 rounded-xl hover:bg-blue-100 transition-colors"
                    title="Criar Novo Cliente"
                  >
                    <Plus size={20} />
                  </button>
                )}
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
              {mutation.isPending ? <Loader2 className="animate-spin" size={20} /> : (cargo ? 'Salvar Alterações' : 'Criar Cargo')}
            </button>
          </div>
        </form>

        <AnimatePresence>
          {isQuickRoleOpen && (
            <QuickRoleModal onClose={() => setQuickRoleOpen(false)} />
          )}
        </AnimatePresence>
      </motion.div>
    </div>
  );
}

function QuickRoleModal({ onClose }: { onClose: () => void }) {
  const [nome, setNome] = useState('');
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: (data: any) => api.post('/role', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['roles'] });
      onClose();
    }
  });

  return (
    <div className="fixed inset-0 z-[60] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <motion.div initial={{ scale: 0.9 }} animate={{ scale: 1 }} className="bg-white dark:bg-neutral-900 p-8 rounded-[2rem] shadow-2xl w-full max-w-sm border border-neutral-200 dark:border-neutral-800">
        <h4 className="text-xl font-bold mb-6">Novo Nível de Acesso</h4>
        <div className="space-y-4">
          <input 
            type="text" placeholder="Nome do nível (ex: gerente)" value={nome} onChange={e => setNome(e.target.value)}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
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

function QuickClienteModal({ onClose }: { onClose: () => void }) {
  const [nome, setNome] = useState('');
  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: (data: any) => api.post('/cliente', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientes'] });
      onClose();
    }
  });

  return (
    <div className="fixed inset-0 z-[60] flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
      <motion.div initial={{ scale: 0.9 }} animate={{ scale: 1 }} className="bg-white dark:bg-neutral-900 p-8 rounded-[2rem] shadow-2xl w-full max-w-sm border border-neutral-200 dark:border-neutral-800">
        <h4 className="text-xl font-bold mb-6">Novo Cliente</h4>
        <div className="space-y-4">
          <input 
            type="text" placeholder="Razão social / Nome" value={nome} onChange={e => setNome(e.target.value)}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none font-bold placeholder:text-neutral-400"
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
