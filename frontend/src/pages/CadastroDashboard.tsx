import { FileText, TrendingUp, Wallet, Filter, Loader2, Info, MousePointer2, UserCheck, DollarSign } from 'lucide-react';
import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';
import { motion, AnimatePresence } from 'framer-motion';

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

export default function CadastroDashboard() {
    const [selectedClienteId, setSelectedClienteId] = useState<number | null>(() => {
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        return user.role !== 'admin' ? user.idCliente : null;
    });

    const { data: stats, isLoading } = useQuery<any>({
        queryKey: ['cadastro-stats', selectedClienteId],
        queryFn: async () => (await api.get('/dashboard/cadastro', { params: { idCliente: selectedClienteId } })).data
    });

    const { data: clientes } = useQuery<any[]>({
        queryKey: ['clientes'],
        queryFn: async () => (await api.get('/cliente')).data
    });

    if (isLoading) {
        return (
            <div className="h-[60vh] flex items-center justify-center">
                <Loader2 className="animate-spin text-amber-600" size={48} />
            </div>
        );
    }

    return (
        <div className="space-y-8 animate-in fade-in duration-700">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div className="flex items-center gap-4">
                    <div className="p-4 bg-amber-600 rounded-3xl text-white shadow-lg shadow-amber-500/20">
                        <FileText size={28} />
                    </div>
                    <div>
                        <h1 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white uppercase">Cadastros</h1>
                        <p className="text-neutral-500 dark:text-neutral-400 font-medium">Gestão de leads e conversão de formulários.</p>
                    </div>
                </div>
                <div className="flex items-center gap-2 bg-white dark:bg-neutral-900 px-4 py-2 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm">
                    <Filter size={18} className="text-neutral-400" />
                    <select
                        className="bg-transparent border-none text-sm font-bold text-neutral-700 dark:text-neutral-200 focus:ring-0 outline-none cursor-pointer pr-8 appearance-none disabled:opacity-50"
                        value={selectedClienteId || ""}
                        onChange={(e) => setSelectedClienteId(e.target.value ? Number(e.target.value) : null)}
                        disabled={JSON.parse(localStorage.getItem('user') || '{}').role !== 'admin'}
                    >
                        <option value="" className="dark:bg-neutral-900">Todos os Clientes</option>
                        {clientes?.map(c => <option key={c.id} value={c.id} className="dark:bg-neutral-900">{c.nome}</option>)}
                    </select>
                </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard title="Total de Cadastros" value={stats?.totalCadastros} icon={<UserCheck size={24} />} color="amber" hint="Número total de formulários de cadastro preenchidos." />
                <StatCard title="ROAS" value={`${stats?.roas?.toFixed(2)}x`} icon={<TrendingUp size={24} />} color="emerald" hint="Retorno sobre investimento (Faturamento Estimado / Investimento)." />
                <StatCard title="Taxa de Conversão" value={`${(stats?.conversionRate * 100).toFixed(1)}%`} icon={<MousePointer2 size={24} />} color="blue" hint="Taxa de Cliques no Link que se tornaram Cadastros." />
                <StatCard title="CPA Real" value={`R$ ${stats?.cpa?.toFixed(2)}`} icon={<DollarSign size={24} />} color="amber" hint="Custo por Cadastro (Investimento Total / Total de Cadastros)." />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                <div className="bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm">
                    <h3 className="font-bold text-lg uppercase tracking-tight mb-8">Cliques vs Cadastros</h3>
                    <div className="flex h-64 items-end gap-12 px-8 pb-4">
                        <div className="flex-1 flex flex-col items-center gap-4">
                            <motion.div initial={{ height: 0 }} animate={{ height: '100%' }} className="w-24 bg-blue-500/20 border-x border-t border-blue-500/50 rounded-t-3xl relative">
                                <span className="absolute -top-8 w-full text-center font-black text-blue-600">{stats?.totalClickLink}</span>
                            </motion.div>
                            <span className="text-[10px] font-black uppercase text-neutral-400">Cliques</span>
                        </div>
                        <div className="flex-1 flex flex-col items-center gap-4">
                            <motion.div initial={{ height: 0 }} animate={{ height: stats?.totalClickLink > 0 ? `${(stats.totalCadastros / stats.totalClickLink) * 100}%` : '0%' }} className="w-24 bg-amber-500 border-x border-t border-amber-600 rounded-t-3xl relative shadow-lg shadow-amber-500/20">
                                <span className="absolute -top-8 w-full text-center font-black text-amber-600">{stats?.totalCadastros}</span>
                            </motion.div>
                            <span className="text-[10px] font-black uppercase text-neutral-400">Cadastros</span>
                        </div>
                    </div>
                </div>

                <div className="bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm">
                    <h3 className="font-bold text-lg uppercase tracking-tight mb-8">Resumo Financeiro</h3>
                    <div className="space-y-8">
                        <div className="p-6 rounded-3xl bg-neutral-50 dark:bg-neutral-800/50 border border-neutral-100 dark:border-neutral-800 flex justify-between items-center">
                            <div>
                                <p className="text-[10px] font-black uppercase text-neutral-400 mb-1">Faturamento Estimado</p>
                                <p className="text-3xl font-black text-neutral-900 dark:text-white">R$ {stats?.totalFaturamento?.toLocaleString()}</p>
                            </div>
                            <div className="p-4 bg-emerald-500/10 text-emerald-500 rounded-2xl">
                                <TrendingUp size={24} />
                            </div>
                        </div>

                        <div className="p-6 rounded-3xl bg-neutral-50 dark:bg-neutral-800/50 border border-neutral-100 dark:border-neutral-800 flex justify-between items-center">
                            <div>
                                <p className="text-[10px] font-black uppercase text-neutral-400 mb-1">Investimento Total</p>
                                <p className="text-3xl font-black text-neutral-900 dark:text-white">R$ {stats?.totalInvestimento?.toLocaleString()}</p>
                            </div>
                            <div className="p-4 bg-blue-500/10 text-blue-500 rounded-2xl">
                                <Wallet size={24} />
                            </div>
                        </div>

                        <div className="flex items-center gap-2 p-4 bg-amber-50 dark:bg-amber-900/10 border border-amber-100 dark:border-amber-800/50 rounded-2xl">
                            <Info size={16} className="text-amber-500 shrink-0" />
                            <p className="text-[10px] text-amber-700 dark:text-amber-400 font-bold">
                                Faturamento calculado com base no ticket médio de R$ 30,00 por cadastro.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

function StatCard({ title, value, icon, color, hint }: any) {
    const colorClasses: any = {
        blue: "bg-blue-50 dark:bg-blue-900/20 text-blue-600 dark:text-blue-400",
        emerald: "bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600 dark:text-emerald-400",
        amber: "bg-amber-50 dark:bg-amber-900/20 text-amber-600 dark:text-amber-400"
    };

    return (
        <Tooltip text={hint}>
            <div className="bg-white dark:bg-neutral-900 p-6 rounded-[2rem] border border-neutral-200 dark:border-neutral-800 shadow-sm flex items-center gap-4 group hover:translate-y-[-4px] transition-all duration-300 w-full overflow-hidden">
                <div className={`p-4 rounded-2xl ${colorClasses[color]} group-hover:scale-110 transition-transform shadow-lg shadow-black/5 flex-shrink-0`}>
                    {icon}
                </div>
                <div className="flex-1 min-w-0">
                    <p className="text-[10px] font-black uppercase tracking-widest text-neutral-400 mb-1 truncate">{title}</p>
                    <div className="flex items-center gap-2">
                        <span className="text-xl lg:text-2xl font-black text-neutral-900 dark:text-white break-words line-clamp-1">{value}</span>
                    </div>
                </div>
            </div>
        </Tooltip>
    );
}
