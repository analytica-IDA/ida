import { Plus, Trash2, Edit2, TrendingUp, Loader2, X, AlertCircle } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useState, useEffect } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';

// Interfaces
interface Cliente {
    id: number;
    nome: string;
    cnpj: string;
}

interface ModeloControle {
    id: number;
    nome: string;
}

interface ClienteModeloControle {
    id: number;
    idCliente: number;
    idModeloControle: number;
    modeloControle?: ModeloControle;
}

const MODELO_CADASTRO = 1;
const MODELO_VAREJO = 2;
const MODELO_SAUDE = 3;

export default function LancamentosPage() {
    const [selectedClienteId, setSelectedClienteId] = useState<number>(0);
    const [selectedModeloId, setSelectedModeloId] = useState<number>(0);
    const [dataInicial, setDataInicial] = useState<string>('');
    const [dataFinal, setDataFinal] = useState<string>('');
    const [isModalOpen, setModalOpen] = useState(false);
    const [editingItem, setEditingItem] = useState<any>(null);
    const queryClient = useQueryClient();

    const { data: userProfile } = useQuery<any>({
        queryKey: ['user-me'],
        queryFn: async () => {
            const { data } = await api.get('/user/me');
            return data;
        },
    });

    const isAdmin = userProfile?.role?.toLowerCase() === 'admin' || userProfile?.role?.toLowerCase() === 'administrador';

    // Load clients
    const { data: clientes } = useQuery<Cliente[]>({
        queryKey: ['clientes'],
        queryFn: async () => {
            const { data } = await api.get('/cliente');
            return data;
        },
    });

    // Automatically select the client if there is only one
    useEffect(() => {
        if (clientes && clientes.length === 1 && !selectedClienteId) {
            setSelectedClienteId(clientes[0].id);
        }
    }, [clientes, selectedClienteId]);

    // Load selected client models
    const { data: clientModels, isLoading: isLoadingModels } = useQuery<ClienteModeloControle[]>({
        queryKey: ['cliente-modelos', selectedClienteId],
        queryFn: async () => {
            if (!selectedClienteId) return [];
            const { data } = await api.get('/ClienteModeloControle');
            return data.filter((m: ClienteModeloControle) => m.idCliente === selectedClienteId);
        },
        enabled: !!selectedClienteId,
    });

    // Automatically select the first model if available and none is selected
    useEffect(() => {
        if (clientModels && clientModels.length > 0) {
            if (!clientModels.find(m => m.idModeloControle === selectedModeloId)) {
                setSelectedModeloId(clientModels[0].idModeloControle);
            }
        } else {
            setSelectedModeloId(0);
        }
    }, [clientModels, selectedModeloId]);

    // Load lancamentos for the selected client and model
    const { data: lancamentos, isLoading: isLoadingLancamentos } = useQuery<any[]>({
        queryKey: ['lancamentos', selectedClienteId, selectedModeloId, dataInicial, dataFinal],
        queryFn: async () => {
            if (!selectedClienteId || !selectedModeloId) return [];
            try {
                let url = `/lancamento/cliente/${selectedClienteId}?idModeloControle=${selectedModeloId}`;
                if (dataInicial) url += `&dataInicial=${dataInicial}`;
                if (dataFinal) url += `&dataFinal=${dataFinal}`;

                const { data } = await api.get(url);
                return data;
            } catch (error) {
                return [];
            }
        },
        enabled: !!selectedClienteId && !!selectedModeloId,
    });

    const deleteMutation = useMutation({
        mutationFn: (id: number) => api.delete(`/lancamento/${id}`),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['lancamentos', selectedClienteId, selectedModeloId] }),
    });

    const renderTableHeaders = () => {
        if (selectedModeloId === MODELO_CADASTRO) {
            return (
                <>
                    <th className="px-6 py-4">Data</th>
                    <th className="px-6 py-4">Usuário</th>
                    <th className="px-6 py-4">Clicks Link</th>
                    <th className="px-6 py-4">Cadastros</th>
                    <th className="px-6 py-4">Ticket Médio</th>
                    {!isAdmin && <th className="px-6 py-4 text-right">Ações</th>}
                </>
            );
        }
        if (selectedModeloId === MODELO_VAREJO) {
            return (
                <>
                    <th className="px-6 py-4">Data</th>
                    <th className="px-6 py-4">Usuário</th>
                    <th className="px-6 py-4">Atendimentos</th>
                    <th className="px-6 py-4">Fechamentos</th>
                    <th className="px-6 py-4">Faturamento</th>
                    {!isAdmin && <th className="px-6 py-4 text-right">Ações</th>}
                </>
            );
        }
        if (selectedModeloId === MODELO_SAUDE) {
            return (
                <>
                    <th className="px-6 py-4">Data</th>
                    <th className="px-6 py-4">Usuário</th>
                    <th className="px-6 py-4">Contatos Reais</th>
                    <th className="px-6 py-4">Conv. Consultas</th>
                    <th className="px-6 py-4">Entradas Redes</th>
                    <th className="px-6 py-4">Entrada Google</th>
                    {!isAdmin && <th className="px-6 py-4 text-right">Ações</th>}
                </>
            );
        }
        return null;
    };

    const renderTableRow = (item: any) => {
        const data = new Date(item.dataLancamento).toLocaleDateString('pt-BR');
        const usuario = item.usuario?.pessoa?.nome || `User ${item.idUsuario}`;

        if (selectedModeloId === MODELO_CADASTRO) {
            return (
                <tr key={item.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors">
                    <td className="px-6 py-4">{data}</td>
                    <td className="px-6 py-4 font-medium">{usuario}</td>
                    <td className="px-6 py-4">{item.qtdClickLink}</td>
                    <td className="px-6 py-4">{item.qtdCadastros}</td>
                    <td className="px-6 py-4 text-emerald-600 font-medium">R$ {item.vlrTicketMedio?.toLocaleString('pt-BR')}</td>
                    {!isAdmin && (
                        <td className="px-6 py-4 text-right">
                            <button onClick={() => { setEditingItem(item); setModalOpen(true); }} className="p-2 text-blue-500 hover:bg-blue-50 dark:hover:bg-blue-900/30 rounded-lg transition-colors mr-2"><Edit2 size={18} /></button>
                            <button onClick={() => { if (confirm('Excluir lançamento?')) deleteMutation.mutate(item.id); }} className="p-2 text-red-500 hover:bg-red-50 dark:hover:bg-red-900/30 rounded-lg transition-colors"><Trash2 size={18} /></button>
                        </td>
                    )}
                </tr>
            );
        }
        if (selectedModeloId === MODELO_VAREJO) {
            return (
                <tr key={item.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors">
                    <td className="px-6 py-4">{data}</td>
                    <td className="px-6 py-4 font-medium">{usuario}</td>
                    <td className="px-6 py-4">{item.qtdAtendimento}</td>
                    <td className="px-6 py-4">{item.qtdFechamento}</td>
                    <td className="px-6 py-4 text-emerald-600 font-medium">R$ {item.faturamento?.toLocaleString('pt-BR')}</td>
                    {!isAdmin && (
                        <td className="px-6 py-4 text-right">
                            <button onClick={() => { setEditingItem(item); setModalOpen(true); }} className="p-2 text-blue-500 hover:bg-blue-50 dark:hover:bg-blue-900/30 rounded-lg transition-colors mr-2"><Edit2 size={18} /></button>
                            <button onClick={() => { if (confirm('Excluir lançamento?')) deleteMutation.mutate(item.id); }} className="p-2 text-red-500 hover:bg-red-50 dark:hover:bg-red-900/30 rounded-lg transition-colors"><Trash2 size={18} /></button>
                        </td>
                    )}
                </tr>
            );
        }
        if (selectedModeloId === MODELO_SAUDE) {
            return (
                <tr key={item.id} className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors">
                    <td className="px-6 py-4">{data}</td>
                    <td className="px-6 py-4 font-medium">{usuario}</td>
                    <td className="px-6 py-4">{item.qtdContatosReais}</td>
                    <td className="px-6 py-4">{item.qtdConversaoConsultas}</td>
                    <td className="px-6 py-4">{item.qtdEntradaRedesSociais}</td>
                    <td className="px-6 py-4">{item.qtdEntradaGoogle}</td>
                    {!isAdmin && (
                        <td className="px-6 py-4 text-right">
                            <button onClick={() => { setEditingItem(item); setModalOpen(true); }} className="p-2 text-blue-500 hover:bg-blue-50 dark:hover:bg-blue-900/30 rounded-lg transition-colors mr-2"><Edit2 size={18} /></button>
                            <button onClick={() => { if (confirm('Excluir lançamento?')) deleteMutation.mutate(item.id); }} className="p-2 text-red-500 hover:bg-red-50 dark:hover:bg-red-900/30 rounded-lg transition-colors"><Trash2 size={18} /></button>
                        </td>
                    )}
                </tr>
            );
        }
        return null;
    };

    return (
        <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Lançamentos de Resultados</h1>
                    <p className="text-neutral-500 dark:text-neutral-400 mt-1">Registre e acompanhe as métricas de performance dos clientes.</p>
                </div>
                {!isAdmin && (
                    <button
                        onClick={() => { setEditingItem(null); setModalOpen(true); }}
                        disabled={!selectedClienteId || !selectedModeloId}
                        className="flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 disabled:bg-neutral-400 text-white px-5 py-2.5 rounded-xl transition-all shadow-lg shadow-blue-500/25 disabled:shadow-none font-semibold"
                    >
                        <Plus size={20} />
                        Novo Lançamento
                    </button>
                )}
            </div>

            <div className="space-y-6">
                {/* Header section with Select and New Launch Button */}
                <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 bg-white dark:bg-neutral-900 p-6 rounded-3xl border border-neutral-200 dark:border-neutral-800 shadow-sm">
                    <div className="flex flex-wrap items-center gap-4 w-full sm:w-auto">
                        <div className="flex flex-col">
                            <label className="text-[10px] font-black uppercase tracking-widest text-neutral-500 mb-1 ml-1">Cliente</label>
                            <select
                                className="bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 text-neutral-900 dark:text-neutral-100 text-sm rounded-xl focus:ring-blue-500 focus:border-blue-500 block w-full p-3 min-w-[200px] font-bold outline-none"
                                value={selectedClienteId}
                                onChange={(e) => setSelectedClienteId(Number(e.target.value))}
                            >
                                <option value={0}>Selecione um cliente</option>
                                {clientes?.map(c => (
                                    <option key={c.id} value={c.id}>{c.nome}</option>
                                ))}
                            </select>
                        </div>

                        <div className="flex flex-col">
                            <label className="text-[10px] font-black uppercase tracking-widest text-neutral-500 mb-1 ml-1">Data Inicial</label>
                            <input
                                type="date"
                                className="bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 text-neutral-900 dark:text-neutral-100 text-sm rounded-xl focus:ring-blue-500 focus:border-blue-500 block w-full p-3 font-bold outline-none"
                                value={dataInicial}
                                onChange={(e) => setDataInicial(e.target.value)}
                            />
                        </div>

                        <div className="flex flex-col">
                            <label className="text-[10px] font-black uppercase tracking-widest text-neutral-500 mb-1 ml-1">Data Final</label>
                            <input
                                type="date"
                                className="bg-neutral-50 dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 text-neutral-900 dark:text-neutral-100 text-sm rounded-xl focus:ring-blue-500 focus:border-blue-500 block w-full p-3 font-bold outline-none"
                                value={dataFinal}
                                onChange={(e) => setDataFinal(e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="flex items-center gap-3 w-full sm:w-auto">
                        {!isAdmin && selectedClienteId > 0 && selectedModeloId > 0 && (
                            <button
                                onClick={() => { setEditingItem(null); setModalOpen(true); }}
                                className="w-full sm:w-auto bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-2xl flex items-center justify-center gap-2 transition-all shadow-lg shadow-blue-500/20"
                            >
                                <Plus size={20} />
                                Novo Lançamento
                            </button>
                        )}
                    </div>
                </div>
                {selectedClienteId > 0 && (
                    <div className="flex-1 space-y-2">
                        <label className="text-xs font-bold text-neutral-500 uppercase tracking-wider">Modelo de Controle</label>
                        {isLoadingModels ? (
                            <div className="px-4 py-3 flex items-center text-neutral-400"><Loader2 className="animate-spin mr-2" size={16} /> Carregando modelos...</div>
                        ) : clientModels && clientModels.length > 0 ? (
                            <div className="flex gap-2">
                                {clientModels.map(cm => (
                                    <button
                                        key={cm.idModeloControle}
                                        onClick={() => setSelectedModeloId(cm.idModeloControle)}
                                        className={`flex-1 px-4 py-3 rounded-xl font-bold transition-all border ${selectedModeloId === cm.idModeloControle
                                            ? 'bg-blue-50 border-blue-200 text-blue-700 dark:bg-blue-900/40 dark:border-blue-800/60 dark:text-blue-300'
                                            : 'bg-white border-neutral-200 text-neutral-600 hover:bg-neutral-50 dark:bg-neutral-800 dark:border-neutral-700 dark:text-neutral-400'
                                            }`}
                                    >
                                        {cm.modeloControle?.nome}
                                    </button>
                                ))}
                            </div>
                        ) : (
                            <div className="px-4 py-3 text-red-500 text-sm font-medium flex items-center bg-red-50 rounded-xl dark:bg-red-900/20">
                                <AlertCircle size={16} className="mr-2" /> Cliente não possui modelo de controle configurado.
                            </div>
                        )}
                    </div>
                )}
            </div>

            {selectedClienteId > 0 && selectedModeloId > 0 && (
                <div className="bg-white dark:bg-neutral-900 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden">
                    <div className="px-6 py-4 border-b border-neutral-100 dark:border-neutral-800 bg-neutral-50/50 dark:bg-neutral-800/30">
                        <h3 className="font-bold flex items-center text-neutral-700 dark:text-neutral-200">
                            <TrendingUp size={18} className="mr-2 text-blue-600" />
                            Histórico de Lançamentos
                        </h3>
                    </div>
                    <div className="overflow-x-auto">
                        <table className="w-full text-left border-collapse whitespace-nowrap">
                            <thead>
                                <tr className="bg-neutral-50 dark:bg-neutral-800/80 border-b border-neutral-200 dark:border-neutral-800 text-sm font-semibold text-neutral-600 dark:text-neutral-400">
                                    {renderTableHeaders()}
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800 text-sm">
                                {isLoadingLancamentos ? (
                                    <tr>
                                        <td colSpan={8} className="px-6 py-20 text-center">
                                            <div className="flex flex-col items-center gap-3">
                                                <Loader2 className="animate-spin text-blue-600" size={32} />
                                                <span className="text-neutral-500">Carregando dados...</span>
                                            </div>
                                        </td>
                                    </tr>
                                ) : lancamentos?.length === 0 ? (
                                    <tr>
                                        <td colSpan={8} className="px-6 py-12 text-center text-neutral-500 font-medium">
                                            Nenhum lançamento encontrado para os filtros selecionados.
                                        </td>
                                    </tr>
                                ) : (
                                    lancamentos?.map(renderTableRow)
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}

            <AnimatePresence>
                {isModalOpen && (
                    <LancamentoModal
                        onClose={() => { setModalOpen(false); setEditingItem(null); }}
                        idCliente={selectedClienteId}
                        idModeloControle={selectedModeloId}
                        item={editingItem}
                        onSuccess={() => {
                            queryClient.invalidateQueries({ queryKey: ['lancamentos', selectedClienteId, selectedModeloId] });
                            setModalOpen(false);
                            setEditingItem(null);
                        }}
                    />
                )}
            </AnimatePresence>
        </div>
    );
}

// Sub-Componentes para Modal de Criação

const InputField = ({ label, name, type = "number", required = true, value, onChange, placeholder, disabled = false }: any) => (
    <div className="space-y-1.5 flex-1 min-w-[200px]">
        <label className="text-[11px] font-black uppercase tracking-widest text-neutral-500 ml-1">{label}</label>
        <input
            type={type} required={required} name={name} step={type === "number" ? "any" : undefined}
            value={value || ''} onChange={onChange} placeholder={placeholder}
            disabled={disabled}
            className={`w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800/80 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none focus:ring-2 focus:ring-blue-500/30 font-bold transition-all placeholder:text-neutral-400 dark:placeholder:text-neutral-600 ${disabled ? 'opacity-60 cursor-not-allowed' : ''}`}
        />
    </div>
);

function LancamentoModal({ onClose, idCliente, idModeloControle, onSuccess, item }: { onClose: () => void, idCliente: number, idModeloControle: number, onSuccess: () => void, item?: any }) {
    const isVarejo = idModeloControle === 2;
    const isCadastro = idModeloControle === 1;
    const isSaude = idModeloControle === 3;

    // Default to today
    const [formData, setFormData] = useState<any>({ dataLancamento: new Date().toISOString().split('T')[0] });

    useEffect(() => {
        const fetchInvestments = async () => {
            if (item) {
                const dateStr = item.dataLancamento ? item.dataLancamento.split('T')[0] : '';
                setFormData({
                    ...item,
                    dataLancamento: dateStr,
                    vlrInvestimentoMetaReadOnly: item.clienteInvestimentoMeta?.vlrInvestimentoMeta || 0,
                    vlrInvestimentoGoogleReadOnly: item.clienteInvestimentoGoogle?.vlrInvestimentoGoogle || 0
                });
            } else {
                try {
                    const [metaRes, googleRes] = await Promise.all([
                        api.get(`/ClienteInvestimentoMeta/cliente/${idCliente}`),
                        api.get(`/ClienteInvestimentoGoogle/cliente/${idCliente}`)
                    ]);
                    setFormData({
                        dataLancamento: new Date().toISOString().split('T')[0],
                        idClienteInvestimentoMeta: metaRes.data?.id || null,
                        idClienteInvestimentoGoogle: googleRes.data?.id || null,
                        vlrInvestimentoMetaReadOnly: metaRes.data?.vlrInvestimentoMeta || 0,
                        vlrInvestimentoGoogleReadOnly: googleRes.data?.vlrInvestimentoGoogle || 0
                    });
                } catch (error) {
                    setFormData({ dataLancamento: new Date().toISOString().split('T')[0] });
                }
            }
        };
        fetchInvestments();
    }, [item, idCliente]);

    const mutation = useMutation({
        mutationFn: (data: any) => {
            let endpoint = '';
            if (isVarejo) endpoint = '/lancamento/varejo';
            if (isCadastro) endpoint = '/lancamento/cadastro';
            if (isSaude) endpoint = '/lancamento/saude';

            const payload = { ...data, idCliente };

            // Numbers formatting
            Object.keys(payload).forEach(key => {
                if (typeof payload[key] === 'string' && payload[key].trim() !== '' && key !== 'dataLancamento') {
                    payload[key] = Number(payload[key].replace(',', '.'));
                }
            });

            if (item && item.id) {
                return api.put(`${endpoint}/${item.id}`, payload);
            }
            return api.post(endpoint, payload);
        },
        onSuccess: () => {
            onSuccess();
        },
        onError: (err: any) => {
            alert("Erro ao salvar o lançamento: " + (err.response?.data?.message || err.message));
        }
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm overflow-y-auto">
            <motion.div
                initial={{ scale: 0.95, opacity: 0, y: 20 }} animate={{ scale: 1, opacity: 1, y: 0 }}
                className="bg-white dark:bg-neutral-900 w-full max-w-3xl rounded-[2rem] shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800 my-8"
            >
                <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 flex items-center justify-between bg-neutral-50/50 dark:bg-neutral-800/50 sticky top-0 z-10">
                    <div className="flex items-center gap-4">
                        <div className="w-12 h-12 rounded-2xl bg-blue-600 flex items-center justify-center text-white">
                            <TrendingUp size={24} />
                        </div>
                        <div>
                            <h3 className="text-xl font-bold">{item ? "Editar Lançamento" : "Novo Lançamento"}</h3>
                            <p className="text-xs text-neutral-500 font-medium">Preencha os dados do ciclo</p>
                        </div>
                    </div>
                    <button onClick={onClose} className="p-2 hover:bg-neutral-200 dark:hover:bg-neutral-700 rounded-full transition-colors">
                        <X size={20} />
                    </button>
                </div>

                <form onSubmit={e => { e.preventDefault(); mutation.mutate(formData); }} className="p-8 space-y-8">
                    <div className="flex flex-wrap gap-4">
                        {isVarejo && (
                            <>
                                <InputField label="Data do Lançamento" name="dataLancamento" type="date" value={formData.dataLancamento} onChange={handleChange} />
                                <InputField label="Qtd Atendimentos" name="qtdAtendimento" value={formData.qtdAtendimento} onChange={handleChange} />
                                <InputField label="Qtd Fechamentos" name="qtdFechamento" value={formData.qtdFechamento} onChange={handleChange} />
                                <InputField label="Faturamento" name="faturamento" value={formData.faturamento} onChange={handleChange} placeholder="R$ 0,00" />
                                <InputField label="Qtd Instagram" name="qtdInstagram" value={formData.qtdInstagram} onChange={handleChange} />
                                <InputField label="Qtd Facebook" name="qtdFacebook" value={formData.qtdFacebook} onChange={handleChange} />
                                <InputField label="Qtd Google" name="qtdGoogle" value={formData.qtdGoogle} onChange={handleChange} />
                                <InputField label="Qtd Indicação" name="qtdIndicacao" value={formData.qtdIndicacao} onChange={handleChange} />
                                <InputField label="Investimento Meta (R$)" name="vlrInvestimentoMetaReadOnly" required={false} value={formData.vlrInvestimentoMetaReadOnly} disabled={true} placeholder="R$ 0,00" />
                                <InputField label="Investimento Google (R$)" name="vlrInvestimentoGoogleReadOnly" required={false} value={formData.vlrInvestimentoGoogleReadOnly} disabled={true} placeholder="R$ 0,00" />
                            </>
                        )}
                        {isCadastro && (
                            <>
                                <InputField label="Data do Lançamento" name="dataLancamento" type="date" value={formData.dataLancamento} onChange={handleChange} />
                                <InputField label="Cliques no Link" name="qtdClickLink" value={formData.qtdClickLink} onChange={handleChange} />
                                <InputField label="Qtd Cadastros" name="qtdCadastros" value={formData.qtdCadastros} onChange={handleChange} />
                                <InputField label="Ticket Médio (R$)" name="vlrTicketMedio" value={formData.vlrTicketMedio} onChange={handleChange} placeholder="R$ 0,00" />
                                <InputField label="Investimento Meta (R$)" name="vlrInvestimentoMetaReadOnly" required={false} value={formData.vlrInvestimentoMetaReadOnly} disabled={true} placeholder="R$ 0,00" />
                                <InputField label="Investimento Google (R$)" name="vlrInvestimentoGoogleReadOnly" required={false} value={formData.vlrInvestimentoGoogleReadOnly} disabled={true} placeholder="R$ 0,00" />
                            </>
                        )}
                        {isSaude && (
                            <>
                                <InputField label="Data do Lançamento" name="dataLancamento" type="date" value={formData.dataLancamento} onChange={handleChange} />
                                <InputField label="Cliques (Meta)" name="qtdClickMeta" value={formData.qtdClickMeta} onChange={handleChange} />
                                <InputField label="Cliques (Google)" name="qtdClickGoogle" value={formData.qtdClickGoogle} onChange={handleChange} />
                                <InputField label="Contatos Reais" name="qtdContatosReais" value={formData.qtdContatosReais} onChange={handleChange} />
                                <InputField label="Conversões (Consultas)" name="qtdConversaoConsultas" value={formData.qtdConversaoConsultas} onChange={handleChange} />
                                <InputField label="Ticket Médio Consulta" name="vlrTicketMedioConsultas" required={false} value={formData.vlrTicketMedioConsultas} onChange={handleChange} placeholder="R$ 0,00" />
                                <InputField label="Entrada Redes Sociais" name="qtdEntradaRedesSociais" value={formData.qtdEntradaRedesSociais} onChange={handleChange} />
                                <InputField label="Entrada Google" name="qtdEntradaGoogle" value={formData.qtdEntradaGoogle} onChange={handleChange} />
                                <InputField label="Investimento Meta (R$)" name="vlrInvestimentoMetaReadOnly" required={false} value={formData.vlrInvestimentoMetaReadOnly} disabled={true} placeholder="R$ 0,00" />
                                <InputField label="Investimento Google (R$)" name="vlrInvestimentoGoogleReadOnly" required={false} value={formData.vlrInvestimentoGoogleReadOnly} disabled={true} placeholder="R$ 0,00" />
                            </>
                        )}
                    </div>

                    <div className="flex gap-4 pt-4 border-t border-neutral-100 dark:border-neutral-800">
                        <button type="button" onClick={onClose} className="flex-1 py-4 bg-neutral-100 dark:bg-neutral-800 text-neutral-700 dark:text-neutral-300 rounded-2xl font-bold transition-all hover:bg-neutral-200 dark:hover:bg-neutral-700">Cancelar</button>
                        <button type="submit" disabled={mutation.isPending} className="flex-1 py-4 bg-blue-600 text-white rounded-2xl font-bold shadow-lg shadow-blue-500/20 hover:bg-blue-700 disabled:opacity-50 transition-all">
                            {mutation.isPending ? <Loader2 className="animate-spin inline mr-2" /> : null}
                            Confirmar Lançamento
                        </button>
                    </div>
                </form>
            </motion.div>
        </div>
    );
}
