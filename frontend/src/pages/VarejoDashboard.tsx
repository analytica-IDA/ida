import { Store, TrendingUp, Users, Wallet, Loader2, Instagram, Facebook, Search, Award } from 'lucide-react';
import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';
import { motion, AnimatePresence } from 'framer-motion';
import ClientSelector from '../components/ClientSelector';
import type { Cliente } from '../components/ClientSelector';

interface VarejoStats {
    totalFaturamento: number;
    roas: number;
    conversionRate: number;
    totalInvestimento: number;
    totalInstagram: number;
    totalAtendimento: number;
    totalFacebook: number;
    totalGoogle: number;
    totalIndicacao: number;
    totalInvestimentoMeta: number;
    totalInvestimentoGoogle: number;
}

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

export default function VarejoDashboard() {
    const [selectedClienteId, setSelectedClienteId] = useState<number | null>(() => {
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        return user.role !== 'admin' ? user.idCliente : null;
    });

    const { data: stats, isLoading } = useQuery<VarejoStats>({
        queryKey: ['varejo-stats', selectedClienteId],
        queryFn: async () => (await api.get('/dashboard/varejo', { params: { idCliente: selectedClienteId } })).data
    });

    const { data: clientes, isLoading: isLoadingClientes } = useQuery<Cliente[]>({
        queryKey: ['clientes'],
        queryFn: async () => (await api.get('/cliente')).data
    });

    if (isLoading) {
        return (
            <div className="h-[60vh] flex items-center justify-center">
                <Loader2 className="animate-spin text-blue-600" size={48} />
            </div>
        );
    }

    return (
        <div className="space-y-8 animate-in fade-in duration-700">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div className="flex items-center gap-4">
                    <div className="p-4 bg-blue-600 rounded-3xl text-white shadow-lg shadow-blue-500/20">
                        <Store size={28} />
                    </div>
                    <div>
                        <h1 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white uppercase">Varejo</h1>
                        <p className="text-neutral-500 dark:text-neutral-400 font-medium">Análise de vendas e performance de canais.</p>
                    </div>
                </div>
                <ClientSelector
                    clientes={clientes}
                    selectedValue={selectedClienteId}
                    onChange={setSelectedClienteId}
                    isLoading={isLoadingClientes}
                    disabled={JSON.parse(localStorage.getItem('user') || '{}').role !== 'admin'}
                />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard title="Faturamento Total" value={`R$ ${stats?.totalFaturamento?.toLocaleString() || '0'}`} icon={<Wallet size={24} />} color="blue" hint="Soma de todos os faturamentos registrados no período." />
                <StatCard title="ROAS" value={`${stats?.roas?.toFixed(2) || '0.00'}x`} icon={<TrendingUp size={24} />} color="emerald" hint="Retorno sobre investimento (Faturamento / Investimento Total)." />
                <StatCard title="Conversão" value={`${((stats?.conversionRate || 0) * 100).toFixed(1)}%`} icon={<Award size={24} />} color="indigo" hint="Eficiência de fechamento (Fechamentos / Atendimentos)." />
                <StatCard title="Total Investido" value={`R$ ${stats?.totalInvestimento?.toLocaleString() || '0'}`} icon={<TrendingUp size={24} />} color="blue" hint="Soma de investimentos em Meta e Google." />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                <div className="lg:col-span-2 bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm">
                    <div className="flex items-center justify-between mb-8">
                        <h3 className="font-bold text-lg flex items-center uppercase tracking-tight">
                            <TrendingUp size={20} className="mr-2 text-blue-600" />
                            Distribuição por Canais
                        </h3>
                    </div>

                    <div className="space-y-6">
                        <ChannelRow label="Instagram" value={stats?.totalInstagram || 0} total={stats?.totalAtendimento || 1} icon={<Instagram size={18} />} color="bg-gradient-to-r from-purple-500 to-pink-500" />
                        <ChannelRow label="Facebook" value={stats?.totalFacebook || 0} total={stats?.totalAtendimento || 1} icon={<Facebook size={18} />} color="bg-blue-600" />
                        <ChannelRow label="Google" value={stats?.totalGoogle || 0} total={stats?.totalAtendimento || 1} icon={<Search size={18} />} color="bg-red-500" />
                        <ChannelRow label="Indicação" value={stats?.totalIndicacao || 0} total={stats?.totalAtendimento || 1} icon={<Users size={18} />} color="bg-emerald-500" />
                    </div>
                </div>

                <div className="bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm flex flex-col justify-between">
                    <h3 className="font-bold text-lg uppercase tracking-tight mb-6">Investimentos</h3>
                    <div className="space-y-8">
                        <div>
                            <p className="text-[10px] font-black uppercase text-neutral-400 mb-2">Meta Ads</p>
                            <p className="text-2xl font-black text-neutral-900 dark:text-white">R$ {stats?.totalInvestimentoMeta?.toLocaleString()}</p>
                            <div className="h-1.5 w-full bg-neutral-100 dark:bg-neutral-800 rounded-full mt-2">
                                <motion.div
                                    initial={{ width: 0 }}
                                    animate={{ width: (stats?.totalInvestimento ?? 0) > 0 ? `${((stats?.totalInvestimentoMeta ?? 0) / (stats?.totalInvestimento ?? 1)) * 100}%` : '0%' }}
                                    className="h-full bg-blue-500 rounded-full"
                                />
                            </div>
                        </div>
                        <div>
                            <p className="text-[10px] font-black uppercase text-neutral-400 mb-2">Google Ads</p>
                            <p className="text-2xl font-black text-neutral-900 dark:text-white">R$ {stats?.totalInvestimentoGoogle?.toLocaleString() || '0'}</p>
                            <div className="h-1.5 w-full bg-neutral-100 dark:bg-neutral-800 rounded-full mt-2">
                                <motion.div
                                    initial={{ width: 0 }}
                                    animate={{ width: (stats?.totalInvestimento ?? 0) > 0 ? `${((stats?.totalInvestimentoGoogle ?? 0) / (stats?.totalInvestimento ?? 1)) * 100}%` : '0%' }}
                                    className="h-full bg-red-500 rounded-full"
                                />
                            </div>
                        </div>
                    </div>
                    <div className="mt-8 p-4 bg-neutral-50 dark:bg-neutral-800/50 rounded-2xl border border-neutral-100 dark:border-neutral-800">
                        <p className="text-[10px] font-black uppercase text-neutral-400 mb-1">Total Consolidado</p>
                        <p className="text-xl font-bold">R$ {stats?.totalInvestimento?.toLocaleString()}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

interface StatCardProps {
    title: string;
    value: string | number;
    icon: React.ReactNode;
    color: 'blue' | 'emerald' | 'indigo';
    hint: string;
}

function StatCard({ title, value, icon, color, hint }: StatCardProps) {
    const colorClasses: Record<string, string> = {
        blue: "bg-blue-50 dark:bg-blue-900/20 text-blue-600 dark:text-blue-400",
        emerald: "bg-emerald-50 dark:bg-emerald-900/20 text-emerald-600 dark:text-emerald-400",
        indigo: "bg-indigo-50 dark:bg-indigo-900/20 text-indigo-600 dark:text-indigo-400"
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

interface ChannelRowProps {
    label: string;
    value: number;
    total: number;
    icon: React.ReactNode;
    color: string;
}

function ChannelRow({ label, value, total, icon, color }: ChannelRowProps) {
    const percentage = total > 0 ? (value / total) * 100 : 0;
    return (
        <div className="space-y-2">
            <div className="flex justify-between items-center">
                <div className="flex items-center gap-3">
                    <div className={`p-2 rounded-lg ${color} text-white shadow-sm`}>
                        {icon}
                    </div>
                    <span className="text-sm font-bold text-neutral-700 dark:text-neutral-300">{label}</span>
                </div>
                <div className="text-right">
                    <span className="text-sm font-black">{value}</span>
                    <span className="text-[10px] text-neutral-400 ml-2 font-bold">{percentage.toFixed(1)}%</span>
                </div>
            </div>
            <div className="h-2.5 bg-neutral-100 dark:bg-neutral-800 rounded-full overflow-hidden">
                <motion.div
                    initial={{ width: 0 }}
                    animate={{ width: `${percentage}%` }}
                    transition={{ duration: 1 }}
                    className={`h-full rounded-full ${color}`}
                />
            </div>
        </div>
    );
}
