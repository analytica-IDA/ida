import { BarChart3, TrendingUp, Users, Wallet, Calendar, Loader2, ArrowUpRight, ArrowDownRight, X, ChevronRight, Info } from 'lucide-react';
import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';
import { motion, AnimatePresence } from 'framer-motion';
import ClientSelector from '../components/ClientSelector';

function Tooltip({ text, children, className = "inline-block" }: { text: string, children: React.ReactNode, className?: string }) {
    const [show, setShow] = useState(false);

    return (
        <div className={`relative ${className}`} onMouseEnter={() => setShow(true)} onMouseLeave={() => setShow(false)}>
            {children}
            <AnimatePresence>
                {show && (
                    <motion.div
                        initial={{ opacity: 0, scale: 0.95, y: 10 }}
                        animate={{ opacity: 1, scale: 1, y: 0 }}
                        exit={{ opacity: 0, scale: 0.95, y: 10 }}
                        className="absolute z-[110] bottom-full left-1/2 -translate-x-1/2 mb-3 w-56 p-3 bg-neutral-900 border border-neutral-800 text-white text-[11px] font-medium rounded-2xl shadow-2xl backdrop-blur-md bg-neutral-900/90 pointer-events-none"
                    >
                        <div className="relative text-center">
                            {text}
                            <div className="absolute top-full left-1/2 -translate-x-1/2 mt-[12px] w-0 h-0 border-l-[6px] border-l-transparent border-r-[6px] border-r-transparent border-t-[6px] border-t-neutral-900" />
                        </div>
                    </motion.div>
                )}
            </AnimatePresence>
        </div>
    );
}

export default function ReportsPage() {
    const [selectedClienteId, setSelectedClienteId] = useState<number | null>(() => {
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        return user.role !== 'admin' ? user.idCliente : null;
    });
    const [selectedUserId, setSelectedUserId] = useState<number | null>(null);
    const [isDetailsModalOpen, setDetailsModalOpen] = useState(false);

    const { data: distribution, isLoading: isLoadingDist } = useQuery<Array<{ label: string; value: number }>>({
        queryKey: ['report-dist', selectedClienteId],
        queryFn: async () => (await api.get('/report/launch-distribution', { params: { idCliente: selectedClienteId } })).data
    });

    const { data: productivity, isLoading: isLoadingProd } = useQuery<Array<{ id: number; userName: string; faturamento: number; launchCount: number; clients: string[] }>>({
        queryKey: ['report-prod', selectedClienteId],
        queryFn: async () => (await api.get('/report/user-productivity', { params: { idCliente: selectedClienteId } })).data
    });

    const { data: clientes, isLoading: isLoadingClientes } = useQuery<Array<{ id: number; nome: string }>>({
        queryKey: ['clientes'],
        queryFn: async () => (await api.get('/cliente')).data
    });

    const { data: userDetails, isLoading: isLoadingDetails } = useQuery<Array<{ type: string; date: string; summary: string; valor: number }>>({
        queryKey: ['user-details', selectedUserId],
        queryFn: async () => (await api.get(`/report/user-details/${selectedUserId}`)).data,
        enabled: !!selectedUserId
    });

    return (
        <div className="p-8 max-w-7xl mx-auto space-y-8 animate-in fade-in duration-700">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight text-neutral-900 dark:text-white">Relatórios Estratégicos</h1>
                    <p className="text-neutral-500 dark:text-neutral-400 mt-1">Visão analítica de performance e investimentos.</p>
                </div>
                <ClientSelector
                    clientes={clientes}
                    selectedValue={selectedClienteId}
                    onChange={setSelectedClienteId}
                    isLoading={isLoadingClientes}
                    disabled={JSON.parse(localStorage.getItem('user') || '{}').role !== 'admin'}
                />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                {/* Distribution Chart */}
                <motion.div
                    layout
                    className="lg:col-span-2 bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm flex flex-col"
                >
                    <div className="flex items-center justify-between mb-8">
                        <h3 className="font-bold text-lg flex items-center">
                            <BarChart3 size={20} className="mr-2 text-blue-600" />
                            Distribuição de Lançamentos
                            <Tooltip text="Proporção de lançamentos realizados em cada modelo de negócio (Varejo, Cadastro ou Saúde).">
                                <Info size={14} className="ml-2 text-neutral-400 cursor-help" />
                            </Tooltip>
                        </h3>
                        <span className="text-[10px] font-black uppercase tracking-widest text-neutral-400">Total acumulado</span>
                    </div>

                    <div className="flex-1 flex flex-col justify-end space-y-6 min-h-[300px]">
                        <AnimatePresence mode="wait">
                            {isLoadingDist ? (
                                <motion.div key="loading" initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }} className="h-64 flex items-center justify-center">
                                    <Loader2 className="animate-spin text-blue-600" />
                                </motion.div>
                            ) : (
                                <motion.div key="content" initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }} className="space-y-6">
                                    {distribution?.map((item, idx) => (
                                        <Tooltip key={item.label} text={`Representa ${item.value} lançamentos realizados no modelo ${item.label}.`} className="block w-full">
                                            <div className="space-y-2 w-full">
                                                <div className="flex justify-between items-end">
                                                    <span className="text-sm font-bold text-neutral-600 dark:text-neutral-400">{item.label}</span>
                                                    <span className="text-lg font-black">{item.value}</span>
                                                </div>
                                                <div className="h-3 bg-neutral-100 dark:bg-neutral-800 rounded-full overflow-hidden">
                                                    <motion.div
                                                        initial={{ width: 0 }}
                                                        animate={{ width: `${(item.value / Math.max(...distribution.map(d => d.value), 1)) * 100}%` }}
                                                        transition={{ duration: 1, delay: idx * 0.1 }}
                                                        className={`h-full rounded-full ${idx === 0 ? 'bg-blue-500' : idx === 1 ? 'bg-indigo-500' : 'bg-emerald-500 shadow-lg shadow-emerald-500/20'}`}
                                                    />
                                                </div>
                                            </div>
                                        </Tooltip>
                                    ))}
                                </motion.div>
                            )}
                        </AnimatePresence>
                    </div>
                </motion.div>

                {/* Performance Stats Cards with Layout Animation */}
                <motion.div layout className="grid grid-cols-1 gap-6">
                    <StatCard
                        title="ROI Estimado"
                        value="+12.4%"
                        icon={<TrendingUp size={24} />}
                        color="emerald"
                        trend="up"
                        hint="Retorno sobre investimento estimado baseado em conversões e ticket médio."
                    />
                    <StatCard
                        title="Investimento Total"
                        value="R$ 45.2k"
                        icon={<Wallet size={24} />}
                        color="blue"
                        trend="up"
                        hint="Soma total investida em campanhas Meta e Google para os clientes selecionados."
                    />
                    <StatCard
                        title="Conversão Média"
                        value="8.2%"
                        icon={<TrendingUp size={24} />}
                        color="indigo"
                        trend="down"
                        hint="Taxa de sucesso na conversão de leads em vendas finalizadas."
                    />
                </motion.div>
            </div>

            {/* Productivity Ranking */}
            <motion.div layout className="bg-white dark:bg-neutral-900 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm overflow-hidden">
                <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 bg-neutral-50/50 dark:bg-neutral-800/30 flex items-center justify-between">
                    <h3 className="font-bold flex items-center">
                        <Users size={20} className="mr-2 text-indigo-600" />
                        Produtividade da Equipe
                        <Tooltip text="Ranking dos usuários mais ativos baseado no volume total de lançamentos registrados.">
                            <Info size={14} className="ml-2 text-neutral-400 cursor-help" />
                        </Tooltip>
                    </h3>
                    <Calendar size={18} className="text-neutral-400" />
                </div>
                <div className="p-0">
                    <table className="w-full text-left">
                        <thead>
                            <tr className="text-[10px] font-black uppercase tracking-widest text-neutral-400 border-b border-neutral-50 dark:border-neutral-800">
                                <th className="px-8 py-4">Usuário / Clientes</th>
                                <th className="px-8 py-4 text-center">Lançamentos</th>
                                <th className="px-8 py-4 text-right">Ação</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-neutral-50 dark:divide-neutral-800 text-sm">
                            <AnimatePresence mode="popLayout">
                                {isLoadingProd ? (
                                    <tr key="loading"><td colSpan={3} className="px-8 py-8 text-center"><Loader2 className="animate-spin inline text-indigo-600" /></td></tr>
                                ) : (
                                    productivity?.map((p, idx) => (
                                        <motion.tr
                                            key={p.id}
                                            initial={{ opacity: 0, x: -20 }}
                                            animate={{ opacity: 1, x: 0 }}
                                            exit={{ opacity: 0, x: 20 }}
                                            transition={{ delay: idx * 0.05 }}
                                            className="hover:bg-neutral-50 dark:hover:bg-neutral-800/50 transition-colors group"
                                        >
                                            <td className="px-8 py-4 font-bold text-neutral-700 dark:text-neutral-200">
                                                <Tooltip text={`Este usuário atende: ${p.clients?.join(", ") || 'Nenhum cliente'}`}>
                                                    <div>{p.userName}</div>
                                                    <div className="text-[10px] text-neutral-400 font-medium truncate max-w-[200px]">
                                                        {p.clients?.join(", ")}
                                                    </div>
                                                </Tooltip>
                                            </td>
                                            <td className="px-8 py-4 text-center">
                                                <Tooltip text={`Total de ${p.launchCount} registros feitos por este usuário.`}>
                                                    <span className="px-3 py-1 bg-indigo-50 dark:bg-indigo-900/30 text-indigo-600 dark:text-indigo-400 rounded-lg font-black text-xs cursor-default">
                                                        {p.launchCount}
                                                    </span>
                                                </Tooltip>
                                            </td>
                                            <td className="px-8 py-4 text-right">
                                                <button
                                                    onClick={() => { setSelectedUserId(p.id); setDetailsModalOpen(true); }}
                                                    className="text-xs font-bold text-neutral-400 hover:text-indigo-600 transition-colors opacity-0 group-hover:opacity-100 uppercase tracking-wider flex items-center justify-end w-full gap-1"
                                                >
                                                    Ver detalhes <ChevronRight size={14} />
                                                </button>
                                            </td>
                                        </motion.tr>
                                    ))
                                )}
                            </AnimatePresence>
                        </tbody>
                    </table>
                </div>
            </motion.div>

            {/* Details Modal */}
            <AnimatePresence>
                {isDetailsModalOpen && (
                    <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
                        <motion.div
                            initial={{ scale: 0.9, opacity: 0, y: 20 }}
                            animate={{ scale: 1, opacity: 1, y: 0 }}
                            exit={{ scale: 0.9, opacity: 0, y: 20 }}
                            className="bg-white dark:bg-neutral-900 w-full max-w-2xl rounded-[2.5rem] shadow-2xl overflow-hidden border border-neutral-200 dark:border-neutral-800"
                        >
                            <div className="px-8 py-6 border-b border-neutral-100 dark:border-neutral-800 flex items-center justify-between bg-neutral-50/50 dark:bg-neutral-800/30">
                                <h3 className="text-xl font-bold flex items-center gap-2">
                                    <Users size={20} className="text-blue-600" />
                                    Últimos Lançamentos
                                </h3>
                                <button onClick={() => setDetailsModalOpen(false)} className="p-2 hover:bg-neutral-100 dark:hover:bg-neutral-800 rounded-full transition-colors">
                                    <X size={20} />
                                </button>
                            </div>
                            <div className="p-8 space-y-4 max-h-[60vh] overflow-y-auto custom-scrollbar">
                                {isLoadingDetails ? (
                                    <div className="flex justify-center py-12"><Loader2 className="animate-spin text-blue-600" size={32} /></div>
                                ) : (
                                    userDetails?.map((detail, idx) => (
                                        <div key={idx} className="flex items-center justify-between p-4 rounded-2xl bg-neutral-50 dark:bg-neutral-800/50 border border-neutral-100 dark:border-neutral-800">
                                            <div className="flex items-center gap-4">
                                                <div className={`p-2 rounded-xl text-white ${detail.type === 'Varejo' ? 'bg-blue-500' : detail.type === 'Cadastro' ? 'bg-indigo-500' : 'bg-emerald-500'}`}>
                                                    <BarChart3 size={16} />
                                                </div>
                                                <div>
                                                    <div className="font-bold text-neutral-800 dark:text-neutral-100">{detail.type}</div>
                                                    <div className="text-xs text-neutral-500">{new Date(detail.date).toLocaleDateString('pt-BR')}</div>
                                                </div>
                                            </div>
                                            <div className="text-sm font-medium text-neutral-600 dark:text-neutral-400">{detail.summary}</div>
                                        </div>
                                    ))
                                )}
                            </div>
                            <div className="p-8 border-top border-neutral-100 dark:border-neutral-800 text-center">
                                <button onClick={() => setDetailsModalOpen(false)} className="px-8 py-3 bg-neutral-100 dark:bg-neutral-800 text-neutral-700 dark:text-neutral-300 rounded-2xl font-bold hover:bg-neutral-200 transition-all">Fechar</button>
                            </div>
                        </motion.div>
                    </div>
                )}
            </AnimatePresence>
        </div>
    );
}

function StatCard({ title, value, icon, color, trend, hint }: any) {
    const colorClasses: Record<string, string> = {
        blue: "bg-blue-50 dark:bg-blue-900/20 text-blue-600 dark:text-blue-400",
        emerald: "bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600 dark:text-emerald-400",
        indigo: "bg-indigo-50 dark:bg-indigo-900/20 text-indigo-600 dark:text-indigo-400"
    };

    return (
        <Tooltip text={hint}>
            <div className="bg-white dark:bg-neutral-900 p-6 rounded-3xl border border-neutral-200 dark:border-neutral-800 shadow-sm flex items-center gap-6 group hover:translate-y-[-4px] transition-all duration-300 w-full">
                <div className={`p-4 rounded-2xl ${colorClasses[color] || ""} group-hover:scale-110 transition-transform`}>
                    {icon}
                </div>
                <div>
                    <p className="text-[10px] font-black uppercase tracking-widest text-neutral-400">{title}</p>
                    <div className="flex items-center gap-2">
                        <span className="text-2xl font-black text-neutral-900 dark:text-white">{value}</span>
                        <span className={`flex items-center text-[10px] font-bold ${trend === 'up' ? 'text-emerald-500' : 'text-rose-500'}`}>
                            {trend === 'up' ? <ArrowUpRight size={12} /> : <ArrowDownRight size={12} />}
                            2.1%
                        </span>
                    </div>
                </div>
            </div>
        </Tooltip>
    );
}
