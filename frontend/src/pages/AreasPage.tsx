import { Plus, Search, Edit2, Trash2, MapPin, Loader2, X, Building } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';

interface Area {
  id: number;
  nome: string;
}

export default function AreasPage() {
  const [isModalOpen, setModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [editingArea, setEditingArea] = useState<Area | null>(null);
  const queryClient = useQueryClient();

  const { data: areas, isLoading } = useQuery<Area[]>({
    queryKey: ['areas'],
    queryFn: async () => {
      const { data } = await api.get('/area');
      return data;
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/area/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['areas'] }),
  });

  const filteredAreas = areas?.filter(a => 
    a.nome.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Áreas</h1>
          <p className="text-neutral-500 dark:text-neutral-400 mt-1">Gerencie os departamentos e áreas da organização.</p>
        </div>
        <button 
          onClick={() => { setEditingArea(null); setModalOpen(true); }}
          className="flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-5 py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 font-semibold"
        >
          <Plus size={20} />
          Nova Área
        </button>
      </div>

      <div className="flex items-center gap-4 bg-white dark:bg-neutral-900 p-4 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={20} />
          <input 
            type="text" 
            placeholder="Buscar por nome da área..." 
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border-none rounded-xl focus:ring-2 focus:ring-blue-500/50 transition-all outline-none"
          />
        </div>
      </div>

      <div className="bg-white dark:bg-neutral-900 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden text-sm md:text-base">
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-neutral-50 dark:bg-neutral-800/50 border-b border-neutral-200 dark:border-neutral-800">
              <th className="px-6 py-4 font-semibold text-neutral-600 dark:text-neutral-300">ID</th>
              <th className="px-6 py-4 font-semibold text-neutral-600 dark:text-neutral-300">Nome da Área</th>
              <th className="px-6 py-4 font-semibold text-neutral-600 dark:text-neutral-300 text-right">Ações</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800">
            {isLoading ? (
              <tr>
                <td colSpan={3} className="px-6 py-20 text-center">
                  <div className="flex flex-col items-center gap-3">
                    <Loader2 className="animate-spin text-blue-600" size={32} />
                    <span className="text-neutral-500">Carregando áreas...</span>
                  </div>
                </td>
              </tr>
            ) : filteredAreas?.map((area) => (
              <tr key={area.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors group">
                <td className="px-6 py-4 text-neutral-500">#{area.id}</td>
                <td className="px-6 py-4">
                  <div className="flex items-center gap-3">
                    <div className="w-8 h-8 rounded-lg bg-emerald-100 dark:bg-emerald-900/30 flex items-center justify-center text-emerald-600">
                      <MapPin size={16} />
                    </div>
                    <span className="font-medium">{area.nome}</span>
                  </div>
                </td>
                <td className="px-6 py-4 text-right">
                  <div className="flex items-center justify-end gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                    <button 
                      onClick={() => { setEditingArea(area); setModalOpen(true); }}
                      className="p-2 hover:bg-white dark:hover:bg-neutral-700 rounded-lg text-neutral-500 hover:text-blue-600 transition-colors shadow-sm"
                    >
                      <Edit2 size={18} />
                    </button>
                    <button 
                      onClick={() => { if(confirm('Excluir área?')) deleteMutation.mutate(area.id); }}
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
          <AreaModal 
            onClose={() => setModalOpen(false)} 
            area={editingArea}
          />
        )}
      </AnimatePresence>
    </div>
  );
}

function AreaModal({ onClose, area }: { onClose: () => void; area: Area | null }) {
  const [formData, setFormData] = useState({
    nome: area?.nome || ''
  });
  
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: (data: typeof formData) => 
      area ? api.put(`/area/${area.id}`, { ...data, id: area.id }) : api.post('/area', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['areas'] });
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
          <h2 className="text-xl font-bold">{area ? 'Editar Área' : 'Nova Área'}</h2>
          <button onClick={onClose} className="p-2 hover:bg-neutral-200 dark:hover:bg-neutral-700 rounded-full transition-colors">
            <X size={20} />
          </button>
        </div>

        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(formData); }} className="p-8 space-y-6">
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-1.5 text-neutral-700 dark:text-neutral-300">Nome da Área</label>
              <div className="relative">
                <Building className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={18} />
                <input 
                  type="text" 
                  required
                  autoFocus
                  value={formData.nome}
                  onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
                  placeholder="Ex: Comercial, RH, TI"
                  className="w-full pl-10 pr-4 py-2.5 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl focus:ring-2 focus:ring-blue-500/50 outline-none"
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
              {mutation.isPending ? <Loader2 className="animate-spin" size={20} /> : (area ? 'Salvar' : 'Criar')}
            </button>
          </div>
        </form>
      </motion.div>
    </div>
  );
}
