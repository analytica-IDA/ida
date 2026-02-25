import { Plus, Search, Edit2, Trash2, UserCircle, Loader2, X, Mail, Phone, CreditCard, Building2 } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState, useEffect } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';
import { maskCPF, maskPhone, unmask } from '../utils/maskUtils';

interface Pessoa {
  id: number;
  nome: string;
  email: string;
  cpf: string;
  telefone: string;
  nomeCliente?: string;
}

export default function PessoasPage() {
  const [isModalOpen, setModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCliente, setSelectedCliente] = useState<string>('');
  const [editingPessoa, setEditingPessoa] = useState<Pessoa | null>(null);
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

  const { data: pessoas, isLoading } = useQuery<Pessoa[]>({
    queryKey: ['pessoas'],
    queryFn: async () => {
      const { data } = await api.get('/pessoa');
      return data;
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/pessoa/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['pessoas'] }),
  });

  const filteredPessoas = pessoas?.filter(p => {
    const matchesSearch = p.nome.toLowerCase().includes(searchTerm.toLowerCase()) ||
      p.email?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      p.cpf?.includes(searchTerm);
    
    const matchesCliente = !selectedCliente || p.nomeCliente === selectedCliente;
    
    return matchesSearch && matchesCliente;
  });

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Pessoas</h1>
          <p className="text-neutral-500 dark:text-neutral-400 mt-1">Cadastro centralizado de dados pessoais.</p>
        </div>
        <button 
          onClick={() => { setEditingPessoa(null); setModalOpen(true); }}
          className="flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-5 py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 font-semibold"
        >
          <Plus size={20} />
          Nova Pessoa
        </button>
      </div>

      <div className="flex items-center gap-4 bg-white dark:bg-neutral-900 p-4 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={20} />
          <input 
            type="text" 
            placeholder="Buscar por nome, email ou CPF..." 
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
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Pessoa</th>
              {userProfile?.role === 'admin' && (
                <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Cliente</th>
              )}
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">CPF</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300">Contato</th>
              <th className="px-6 py-4 text-sm font-semibold text-neutral-600 dark:text-neutral-300 text-right">Ações</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800">
            {isLoading ? (
              <tr>
                <td colSpan={userProfile?.role === 'admin' ? 6 : 5} className="px-6 py-20 text-center">
                  <div className="flex flex-col items-center gap-3">
                    <Loader2 className="animate-spin text-blue-600" size={32} />
                    <span className="text-neutral-500">Carregando pessoas...</span>
                  </div>
                </td>
              </tr>
            ) : filteredPessoas?.map((pessoa) => (
              <tr key={pessoa.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors group">
                <td className="px-6 py-4 text-sm text-neutral-500">#{pessoa.id}</td>
                <td className="px-6 py-4">
                  <div className="flex items-center gap-3">
                    <div className="w-10 h-10 rounded-2xl bg-blue-100 dark:bg-blue-900/30 flex items-center justify-center text-blue-600 border border-neutral-200 dark:border-neutral-800/50">
                      <UserCircle size={24} />
                    </div>
                    <div>
                      <span className="block font-semibold">{pessoa.nome}</span>
                      <span className="block text-xs text-neutral-500">{pessoa.email}</span>
                    </div>
                  </div>
                </td>
                {userProfile?.role === 'admin' && (
                  <td className="px-6 py-4">
                    <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-neutral-100 dark:bg-neutral-800 text-neutral-600 dark:text-neutral-400 border border-neutral-200 dark:border-neutral-700">
                      {pessoa.nomeCliente || 'Sem cliente'}
                    </span>
                  </td>
                )}
                <td className="px-6 py-4 text-sm font-medium text-neutral-700 dark:text-neutral-300">
                  {pessoa.cpf ? maskCPF(pessoa.cpf) : 'Não informado'}
                </td>
                <td className="px-6 py-4 text-sm text-neutral-600 dark:text-neutral-400">
                  {pessoa.telefone ? maskPhone(pessoa.telefone) : 'Sem telefone'}
                </td>
                <td className="px-6 py-4 text-right">
                  <div className="flex items-center justify-end gap-2 transition-opacity">
                    <button 
                      onClick={() => { setEditingPessoa(pessoa); setModalOpen(true); }}
                      className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-lg text-neutral-500 hover:text-blue-600 transition-colors shadow-sm"
                    >
                      <Edit2 size={18} />
                    </button>
                    <button 
                      onClick={() => { if(confirm('Excluir pessoa?')) deleteMutation.mutate(pessoa.id); }}
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
          <PessoaModal 
            onClose={() => setModalOpen(false)} 
            pessoa={editingPessoa}
            userProfile={userProfile}
            clientes={clientes || []}
          />
        )}
      </AnimatePresence>
    </div>
  );
}

function PessoaModal({ onClose, pessoa, userProfile, clientes }: { onClose: () => void; pessoa: any | null; userProfile: any; clientes: any[] }) {
  const [formData, setFormData] = useState({
    nome: pessoa?.nome || '',
    email: pessoa?.email || '',
    cpf: pessoa?.cpf || '',
    telefone: pessoa?.telefone || '',
    idCliente: pessoa?.idCliente?.toString() || userProfile?.idCliente?.toString() || '',
  });

  // Atualiza via Props caso o Contexto do usuário demore um microssegunto a mais pra ser validado
  useEffect(() => {
    if (!pessoa?.idCliente && userProfile?.idCliente && !formData.idCliente) {
      setFormData(prev => ({ ...prev, idCliente: userProfile.idCliente.toString() }));
    }
  }, [userProfile, pessoa]);

  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: typeof formData) => {
      const payload = {
        ...data,
        cpf: unmask(data.cpf),
        telefone: unmask(data.telefone),
        idCliente: data.idCliente ? Number(data.idCliente) : null
      };
      return pessoa ? api.put(`/pessoa/${pessoa.id}`, { ...payload, id: pessoa.id }) : api.post('/pessoa', payload);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pessoas'] });
      onClose();
    },
    onError: (error: any) => {
      console.error('Mutation error (Pessoa):', error);
      const msg = error.response?.data?.message || error.response?.data?.title || error.message || 'Erro desconhecido ao salvar pessoa.';
      alert(`Falha ao salvar: ${msg}`);
    }
  });

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
      <motion.div 
        initial={{ opacity: 0, scale: 0.95, y: 20 }}
        animate={{ opacity: 1, scale: 1, y: 0 }}
        exit={{ opacity: 0, scale: 0.95, y: 20 }}
        className="bg-white dark:bg-neutral-900 w-full max-w-lg rounded-3xl shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800"
      >
        <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 flex items-center justify-between bg-neutral-50/50 dark:bg-neutral-800/50">
          <h2 className="text-xl font-bold">{pessoa ? 'Editar Pessoa' : 'Nova Pessoa'}</h2>
          <button onClick={onClose} className="p-2 hover:bg-neutral-200 dark:hover:bg-neutral-700 rounded-full transition-colors">
            <X size={20} />
          </button>
        </div>

        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(formData); }} className="p-8 space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="md:col-span-2">
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Nome Completo</label>
              <div className="relative">
                <UserCircle className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text" required
                  value={formData.nome}
                  onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold"
                />
              </div>
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">E-mail</label>
              <div className="relative">
                <Mail className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="email" required
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">CPF</label>
              <div className="relative">
                <CreditCard className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text"
                  value={maskCPF(formData.cpf)}
                  onChange={(e) => setFormData({ ...formData, cpf: unmask(e.target.value).substring(0, 11) })}
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold"
                  placeholder="000.000.000-00"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Telefone</label>
              <div className="relative">
                <Phone className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text"
                  value={maskPhone(formData.telefone)}
                  onChange={(e) => setFormData({ ...formData, telefone: unmask(e.target.value).substring(0, 13) })}
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold"
                  placeholder="+55 (00) 00000-0000"
                />
              </div>
            </div>

            {userProfile?.role === 'admin' && (
              <div className="md:col-span-2">
                <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Cliente Vinculado</label>
                <div className="relative">
                  <Building2 className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                  <select 
                    required
                    value={formData.idCliente}
                    onChange={(e) => setFormData({ ...formData, idCliente: e.target.value })}
                    className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none font-bold appearance-none"
                  >
                    <option value="" disabled>Selecione um cliente (Obrigatório)</option>
                    {clientes.map(c => (
                      <option key={c.id} value={c.id}>{c.nome}</option>
                    ))}
                  </select>
                </div>
              </div>
            )}
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
              onClick={() => {
                if (!formData.nome || !formData.email) {
                   // Form contains HTML validation natively
                }
                if (userProfile?.role === 'admin' && !formData.idCliente) {
                   alert("Por favor, selecione um Cliente Vinculado.");
                }
              }}
              className="flex-1 bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 disabled:opacity-50 flex items-center justify-center gap-2"
            >
              {mutation.isPending ? <Loader2 className="animate-spin" size={20} /> : (pessoa ? 'Salvar' : 'Cadastrar')}
            </button>
          </div>
        </form>
      </motion.div>
    </div>
  );
}
