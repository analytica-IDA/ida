import { Edit2, Wallet, Loader2, X, AlertCircle, Save } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';
import AreaSelector from '../components/AreaSelector';

interface Cliente {
    id: number;
    nome: string;
}

interface Investimento {
    id: number;
    idCliente: number;
    idArea: number | null;
    vlrInvestimentoMeta: number;
    vlrInvestimentoGoogle: number;
    dtUltimaAtualizacao: string;
}

export default function InvestimentosPage() {
    const [selectedClienteId, setSelectedClienteId] = useState<number>(0);
    const [selectedAreaId, setSelectedAreaId] = useState<number | null>(null);
    const [isMetaModalOpen, setMetaModalOpen] = useState(false);
    const [isGoogleModalOpen, setGoogleModalOpen] = useState(false);
    const [editingMeta, setEditingMeta] = useState<Investimento | null>(null);
    const [editingGoogle, setEditingGoogle] = useState<Investimento | null>(null);
    const queryClient = useQueryClient();

    const { data: clientes } = useQuery<Cliente[]>({
        queryKey: ['clientes'],
        queryFn: async () => {
            const { data } = await api.get('/cliente');
            return data;
        },
    });

    const { data: investimentoMeta, isLoading: isLoadingMeta } = useQuery<Investimento | null>({
        queryKey: ['investimento-meta', selectedClienteId, selectedAreaId],
        queryFn: async () => {
            if (!selectedClienteId) return null;
            const { data } = await api.get(`/ClienteInvestimentoMeta/cliente/${selectedClienteId}`, {
                params: { idArea: selectedAreaId }
            });
            return data || null;
        },
        enabled: !!selectedClienteId,
    });

    const { data: investimentoGoogle, isLoading: isLoadingGoogle } = useQuery<Investimento | null>({
        queryKey: ['investimento-google', selectedClienteId, selectedAreaId],
        queryFn: async () => {
            if (!selectedClienteId) return null;
            const { data } = await api.get(`/ClienteInvestimentoGoogle/cliente/${selectedClienteId}`, {
                params: { idArea: selectedAreaId }
            });
            return data || null;
        },
        enabled: !!selectedClienteId,
    });

    const saveMetaMutation = useMutation({
        mutationFn: (data: Partial<Investimento>) => {
            if (data.id) return api.put(`/ClienteInvestimentoMeta/${data.id}`, data);
            return api.post('/ClienteInvestimentoMeta', data);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['investimento-meta', selectedClienteId, selectedAreaId] });
            setMetaModalOpen(false);
            setEditingMeta(null);
        }
    });

    const saveGoogleMutation = useMutation({
        mutationFn: (data: Partial<Investimento>) => {
            if (data.id) return api.put(`/ClienteInvestimentoGoogle/${data.id}`, data);
            return api.post('/ClienteInvestimentoGoogle', data);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['investimento-google', selectedClienteId, selectedAreaId] });
            setGoogleModalOpen(false);
            setEditingGoogle(null);
        }
    });

    return (
        <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Investimentos</h1>
                    <p className="text-neutral-500 dark:text-neutral-400 mt-1">Defina as metas de investimento real por cliente.</p>
                </div>
            </div>

            <div className="bg-white dark:bg-neutral-900 p-6 rounded-3xl border border-neutral-200 dark:border-neutral-800 shadow-sm flex flex-col md:flex-row gap-6">
                <div className="flex flex-col w-full max-w-sm">
                    <label className="text-[10px] font-black uppercase tracking-widest text-neutral-500 mb-1 ml-1">Cliente</label>
                    <select
                        className="bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 text-neutral-900 dark:text-neutral-100 text-sm rounded-xl focus:ring-blue-500 focus:border-blue-500 block w-full p-3 font-bold outline-none"
                        value={selectedClienteId}
                        onChange={(e) => {
                            setSelectedClienteId(Number(e.target.value));
                            setSelectedAreaId(null);
                        }}
                    >
                        <option value={0}>Selecione um cliente</option>
                        {clientes?.map(c => (
                            <option key={c.id} value={c.id}>{c.nome}</option>
                        ))}
                    </select>
                </div>

                <div className="flex flex-col w-full max-w-sm">
                    <label className="text-[10px] font-black uppercase tracking-widest text-neutral-500 mb-1 ml-1">Área</label>
                    <AreaSelector
                        idCliente={selectedClienteId > 0 ? selectedClienteId : null}
                        selectedValue={selectedAreaId}
                        onChange={setSelectedAreaId}
                        onlyVendedores={true}
                    />
                </div>
            </div>

            {selectedClienteId > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                    {/* Meta Card */}
                    <div className="bg-white dark:bg-neutral-900 rounded-3xl border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden flex flex-col">
                        <div className="px-6 py-4 border-b border-neutral-100 dark:border-neutral-800 bg-neutral-50/50 dark:bg-neutral-800/30 flex justify-between items-center">
                            <h3 className="font-bold flex items-center text-neutral-700 dark:text-neutral-200">
                                <Wallet size={18} className="mr-2 text-blue-600" />
                                Investimento Meta
                            </h3>
                            <button
                                onClick={() => { setEditingMeta(investimentoMeta || null); setMetaModalOpen(true); }}
                                className="p-2 text-blue-600 hover:bg-blue-50 dark:hover:bg-blue-900/30 rounded-lg transition-colors"
                            >
                                <Edit2 size={18} />
                            </button>
                        </div>
                        <div className="p-8 flex-1 flex flex-col items-center justify-center text-center space-y-4">
                            {isLoadingMeta ? (
                                <Loader2 className="animate-spin text-blue-600" size={32} />
                            ) : investimentoMeta ? (
                                <>
                                    <span className="text-4xl font-black text-neutral-900 dark:text-white">
                                        R$ {investimentoMeta.vlrInvestimentoMeta?.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                                    </span>
                                    <p className="text-sm text-neutral-500">Última atualização: {new Date(investimentoMeta.dtUltimaAtualizacao).toLocaleDateString('pt-BR')}</p>
                                </>
                            ) : (
                                <>
                                    <div className="w-16 h-16 bg-neutral-100 dark:bg-neutral-800 rounded-full flex items-center justify-center text-neutral-400 mb-2">
                                        <AlertCircle size={32} />
                                    </div>
                                    <p className="text-neutral-500 font-medium">Nenhum valor definido</p>
                                    <button
                                        onClick={() => { setEditingMeta(null); setMetaModalOpen(true); }}
                                        className="text-blue-600 font-bold text-sm hover:underline"
                                    >
                                        Configurar agora
                                    </button>
                                </>
                            )}
                        </div>
                    </div>

                    {/* Google Card */}
                    <div className="bg-white dark:bg-neutral-900 rounded-3xl border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden flex flex-col">
                        <div className="px-6 py-4 border-b border-neutral-100 dark:border-neutral-800 bg-neutral-50/50 dark:bg-neutral-800/30 flex justify-between items-center">
                            <h3 className="font-bold flex items-center text-neutral-700 dark:text-neutral-200">
                                <Wallet size={18} className="mr-2 text-emerald-600" />
                                Investimento Google
                            </h3>
                            <button
                                onClick={() => { setEditingGoogle(investimentoGoogle || null); setGoogleModalOpen(true); }}
                                className="p-2 text-emerald-600 hover:bg-emerald-50 dark:hover:bg-emerald-900/30 rounded-lg transition-colors"
                            >
                                <Edit2 size={18} />
                            </button>
                        </div>
                        <div className="p-8 flex-1 flex flex-col items-center justify-center text-center space-y-4">
                            {isLoadingGoogle ? (
                                <Loader2 className="animate-spin text-emerald-600" size={32} />
                            ) : investimentoGoogle ? (
                                <>
                                    <span className="text-4xl font-black text-neutral-900 dark:text-white">
                                        R$ {investimentoGoogle.vlrInvestimentoGoogle?.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                                    </span>
                                    <p className="text-sm text-neutral-500">Última atualização: {new Date(investimentoGoogle.dtUltimaAtualizacao).toLocaleDateString('pt-BR')}</p>
                                </>
                            ) : (
                                <>
                                    <div className="w-16 h-16 bg-neutral-100 dark:bg-neutral-800 rounded-full flex items-center justify-center text-neutral-400 mb-2">
                                        <AlertCircle size={32} />
                                    </div>
                                    <p className="text-neutral-500 font-medium">Nenhum valor definido</p>
                                    <button
                                        onClick={() => { setEditingGoogle(null); setGoogleModalOpen(true); }}
                                        className="text-emerald-600 font-bold text-sm hover:underline"
                                    >
                                        Configurar agora
                                    </button>
                                </>
                            )}
                        </div>
                    </div>
                </div>
            )}

            <AnimatePresence>
                {isMetaModalOpen && (
                    <InvestmentModal
                        title="Investimento Meta"
                        field="vlrInvestimentoMeta"
                        item={editingMeta}
                        idCliente={selectedClienteId}
                        idArea={selectedAreaId}
                        onClose={() => setMetaModalOpen(false)}
                        onSave={(data) => saveMetaMutation.mutate(data)}
                        isPending={saveMetaMutation.isPending}
                    />
                )}
                {isGoogleModalOpen && (
                    <InvestmentModal
                        title="Investimento Google"
                        field="vlrInvestimentoGoogle"
                        item={editingGoogle}
                        idCliente={selectedClienteId}
                        idArea={selectedAreaId}
                        onClose={() => setGoogleModalOpen(false)}
                        onSave={(data) => saveGoogleMutation.mutate(data)}
                        isPending={saveGoogleMutation.isPending}
                    />
                )}
            </AnimatePresence>
        </div>
    );
}

interface InvestmentModalProps {
    title: string;
    field: keyof Investimento;
    item: Investimento | null;
    idCliente: number;
    idArea: number | null;
    onClose: () => void;
    onSave: (data: Partial<Investimento>) => void;
    isPending: boolean;
}

function InvestmentModal({ title, field, item, idCliente, idArea, onClose, onSave, isPending }: InvestmentModalProps) {
    const [value, setValue] = useState(item ? (item[field] as number).toString().replace('.', ',') : '0,00');

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
            <motion.div
                initial={{ scale: 0.95, opacity: 0, y: 20 }} animate={{ scale: 1, opacity: 1, y: 0 }}
                className="bg-white dark:bg-neutral-900 w-full max-w-md rounded-[2rem] shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800"
            >
                <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 flex items-center justify-between">
                    <h3 className="text-xl font-bold">{title}</h3>
                    <button onClick={onClose} className="p-2 hover:bg-neutral-100 dark:hover:bg-neutral-800 rounded-full">
                        <X size={20} />
                    </button>
                </div>
                <div className="p-8 space-y-6">
                    <div className="space-y-2">
                        <label className="text-xs font-black uppercase tracking-widest text-neutral-500 ml-1">Valor do Investimento</label>
                        <div className="relative">
                            <span className="absolute left-4 top-1/2 -translate-y-1/2 font-bold text-neutral-400">R$</span>
                            <input
                                type="text"
                                className="w-full pl-12 pr-4 py-4 bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-2xl outline-none focus:ring-2 focus:ring-blue-500/30 font-black text-2xl transition-all"
                                value={value}
                                onChange={(e) => setValue(e.target.value)}
                                autoFocus
                            />
                        </div>
                    </div>
                    <div className="flex gap-4">
                        <button onClick={onClose} className="flex-1 py-4 bg-neutral-100 dark:bg-neutral-800 text-neutral-700 dark:text-neutral-300 rounded-2xl font-bold transition-all hover:bg-neutral-200">Cancelar</button>
                        <button
                            disabled={isPending}
                            onClick={() => {
                                const numValue = Number(value.replace(',', '.'));
                                onSave({ ...item, idCliente, idArea, [field]: numValue });
                            }}
                            className="flex-1 py-4 bg-blue-600 text-white rounded-2xl font-bold shadow-lg shadow-blue-500/20 hover:bg-blue-700 disabled:opacity-50 transition-all flex items-center justify-center gap-2"
                        >
                            {isPending ? <Loader2 size={20} className="animate-spin" /> : <Save size={20} />}
                            Salvar
                        </button>
                    </div>
                </div>
            </motion.div>
        </div>
    );
}
