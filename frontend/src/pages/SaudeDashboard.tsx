import { Activity, TrendingUp, Users, Wallet, Loader2, MousePointer2, UserPlus, FileCheck } from 'lucide-react';
import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';
import { motion, AnimatePresence } from 'framer-motion';
import type { Cliente } from '../components/ClientSelector';
import ClientSelector from '../components/ClientSelector';
import AreaSelector from '../components/AreaSelector';

interface SaudeStats {
    totalClickMeta: number;
    totalClickGoogle: number;
    totalContatosReais: number;
    totalConversaoConsultas: number;
    totalFaturamento: number;
    roas: number;
    conversionRate: number;
    totalInvestimento: number;
    totalEntradaRedesSociais: number;
    totalEntradaGoogle: number;
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

export default function SaudeDashboard() {
    const [selectedClienteId, setSelectedClienteId] = useState<number | null>(() => {
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        return user.role !== 'admin' ? user.idCliente : null;
    });
    const [selectedAreaId, setSelectedAreaId] = useState<number | null>(null);

    const { data: stats, isLoading } = useQuery<SaudeStats>({
        queryKey: ['saude-stats', selectedClienteId, selectedAreaId],
        queryFn: async () => (await api.get('/dashboard/saude', { params: { idCliente: selectedClienteId, idArea: selectedAreaId } })).data
    });

    const { data: clientes, isLoading: isLoadingClientes } = useQuery<Cliente[]>({
        queryKey: ['clientes'],
        queryFn: async () => (await api.get('/cliente')).data
    });

    if (isLoading) {
        return (
            <div className="h-[60vh] flex items-center justify-center">
                <Loader2 className="animate-spin text-emerald-600" size={48} />
            </div>
        );
    }

    const funnelSteps = [
        { label: 'Cliques Totais', value: (stats?.totalClickMeta || 0) + (stats?.totalClickGoogle || 0), icon: MousePointer2, color: 'blue' },
        { label: 'Contatos Reais', value: stats?.totalContatosReais || 0, icon: Users, color: 'indigo' },
        { label: 'Consultas', value: stats?.totalConversaoConsultas || 0, icon: UserPlus, color: 'emerald' },
        { label: 'Faturamento', value: `R$ ${stats?.totalFaturamento?.toLocaleString() || '0'}`, icon: Wallet, color: 'emerald' }
    ];

    return (
        <div className="space-y-8 animate-in fade-in duration-700">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div className="flex items-center gap-4">
                    <div className="p-4 bg-emerald-600 rounded-3xl text-white shadow-lg shadow-emerald-500/20">
                        <Activity size={28} />
                    </div>
                    <div>
                        <h1 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white uppercase">Saúde</h1>
                        <p className="text-neutral-500 dark:text-neutral-400 font-medium">Funil de agendamentos e performance clínica.</p>
                    </div>
                </div>
                <div className="flex flex-col md:flex-row gap-4">
                    <AreaSelector
                        idCliente={selectedClienteId}
                        selectedValue={selectedAreaId}
                        onChange={setSelectedAreaId}
                    />
                    <ClientSelector
                        clientes={clientes}
                        selectedValue={selectedClienteId}
                        onChange={(val) => {
                            setSelectedClienteId(val);
                            setSelectedAreaId(null);
                        }}
                        isLoading={isLoadingClientes}
                        disabled={JSON.parse(localStorage.getItem('user') || '{}').role !== 'admin'}
                    />
                </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard title="Faturamento Clínico" value={`R$ ${stats?.totalFaturamento?.toLocaleString() || '0'}`} icon={<Wallet size={24} />} color="emerald" hint="Faturamento estimado baseado em consultas e ticket médio." />
                <StatCard title="ROAS" value={`${stats?.roas?.toFixed(2) || '0.00'}x`} icon={<TrendingUp size={24} />} color="emerald" hint="Retorno sobre investimento (Faturamento / Investimento Total)." />
                <StatCard title="Taxa de Conversão" value={`${((stats?.conversionRate || 0) * 100).toFixed(1)}%`} icon={<FileCheck size={24} />} color="blue" hint="Taxa de Cliques que se tornaram Contatos Reais." />
                <StatCard title="Investimento Total" value={`R$ ${stats?.totalInvestimento?.toLocaleString() || '0'}`} icon={<TrendingUp size={24} />} color="blue" hint="Soma de investimentos em Meta e Google." />
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                <div className="bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm">
                    <h3 className="font-bold text-lg uppercase tracking-tight mb-8">Funil de Conversão</h3>
                    <div className="space-y-4">
                        {funnelSteps.map((step, idx) => (
                            <div key={idx} className="relative">
                                <div className={`flex items-center justify-between p-6 rounded-3xl bg-neutral-50 dark:bg-neutral-800/50 border border-neutral-100 dark:border-neutral-800 relative z-10 overflow-hidden`}>
                                    <div className="flex items-center gap-4">
                                        <div className={`p-3 rounded-2xl bg-${step.color}-500/10 text-${step.color}-500`}>
                                            <step.icon size={24} />
                                        </div>
                                        <span className="font-bold text-neutral-700 dark:text-neutral-300 uppercase tracking-tight text-sm">{step.label}</span>
                                    </div>
                                    <span className="text-2xl font-black text-neutral-900 dark:text-white">{step.value}</span>

                                    {/* Progress bar backend */}
                                    <motion.div
                                        initial={{ width: 0 }}
                                        animate={{ width: `${100 - (idx * 15)}%` }}
                                        className={`absolute bottom-0 left-0 h-1 bg-${step.color}-600/30`}
                                    />
                                </div>
                                {idx < funnelSteps.length - 1 && (
                                    <div className="flex justify-center -my-2 relative z-0">
                                        <div className="w-8 h-8 rounded-full bg-neutral-100 dark:bg-neutral-800 border-4 border-white dark:border-neutral-900 flex items-center justify-center text-[10px] font-black text-neutral-400">
                                            ▼
                                        </div>
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>
                </div>

                <div className="space-y-8">
                    <div className="bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm">
                        <h3 className="font-bold text-lg uppercase tracking-tight mb-8">Origem do Tráfego</h3>
                        <div className="grid grid-cols-2 gap-6">
                            <div className="p-6 rounded-3xl bg-blue-50 dark:bg-blue-900/10 border border-blue-100 dark:border-blue-800/50">
                                <p className="text-[10px] font-black uppercase text-blue-500 mb-2">Redes Sociais</p>
                                <p className="text-3xl font-black">{stats?.totalEntradaRedesSociais || 0}</p>
                                <div className="h-1 w-full bg-blue-200 dark:bg-blue-900/30 rounded-full mt-4 overflow-hidden">
                                    <motion.div initial={{ width: 0 }} animate={{ width: `${((stats?.totalEntradaRedesSociais || 0) / ((stats?.totalEntradaRedesSociais || 0) + (stats?.totalEntradaGoogle || 0) || 1)) * 100}%` }} className="h-full bg-blue-600" />
                                </div>
                            </div>
                            <div className="p-6 rounded-3xl bg-red-50 dark:bg-red-900/10 border border-red-100 dark:border-red-800/50">
                                <p className="text-[10px] font-black uppercase text-red-500 mb-2">Google Ads</p>
                                <p className="text-3xl font-black">{stats?.totalEntradaGoogle || 0}</p>
                                <div className="h-1 w-full bg-red-200 dark:bg-red-900/30 rounded-full mt-4 overflow-hidden">
                                    <motion.div initial={{ width: 0 }} animate={{ width: `${((stats?.totalEntradaGoogle || 0) / ((stats?.totalEntradaRedesSociais || 0) + (stats?.totalEntradaGoogle || 0) || 1)) * 100}%` }} className="h-full bg-red-600" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="bg-white dark:bg-neutral-900 p-8 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 shadow-sm">
                        <h3 className="font-bold text-lg uppercase tracking-tight mb-8">Eficiência do Investimento</h3>
                        <div className="space-y-6">
                            <div className="flex justify-between items-end">
                                <div>
                                    <p className="text-[10px] font-black uppercase text-neutral-400 mb-1">CPA (Custo por Agendamento)</p>
                                    <p className="text-2xl font-black text-neutral-900 dark:text-white">R$ {((stats?.totalInvestimento || 0) / (stats?.totalConversaoConsultas || 1)).toFixed(2)}</p>
                                </div>
                                <div className="text-right">
                                    <p className="text-[10px] font-black uppercase text-neutral-400 mb-1">Custo por Clique</p>
                                    <p className="text-2xl font-black text-neutral-900 dark:text-white">R$ {((stats?.totalInvestimento || 0) / (((stats?.totalClickMeta || 0) + (stats?.totalClickGoogle || 0)) || 1)).toFixed(2)}</p>
                                </div>
                            </div>
                        </div>
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
