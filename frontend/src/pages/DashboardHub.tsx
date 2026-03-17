import { motion } from 'framer-motion';
import { Store, Activity, FileText, ArrowRight, TrendingUp, Target } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';

const models = [
    {
        id: 'varejo',
        title: 'Varejo',
        description: 'Análise de performance de vendas, canais de aquisição e resultados de vendedores.',
        icon: Store,
        color: 'blue',
        stats: ['Vendedores', 'Conversão', 'ROAS'],
        to: '/dashboard/varejo'
    },
    {
        id: 'saude',
        title: 'Saúde',
        description: 'Monitoramento do funil de agendamentos, cliques em anúncios e faturamento clínico.',
        icon: Activity,
        color: 'emerald',
        stats: ['Funil Clínico', 'Custo/Contato', 'Ticket Médio'],
        to: '/dashboard/saude'
    },
    {
        id: 'cadastro',
        title: 'Cadastros',
        description: 'Gestão de leads, volume de inscrições e eficiência de campanhas de captação.',
        icon: FileText,
        color: 'amber',
        stats: ['Volume Leads', 'Taxa Inscritos', 'CPA'],
        to: '/dashboard/cadastro'
    }
];

export default function DashboardHub() {
    const navigate = useNavigate();
    const { data: accessibleModels, isLoading } = useQuery<string[]>({
        queryKey: ['accessible-models'],
        queryFn: async () => (await api.get('/dashboard/accessible-models')).data
    });

    const filteredModels = models.filter(m => accessibleModels?.some((am: string) => am.toLowerCase() === m.title.toLowerCase()));

    if (isLoading) {
        return (
            <div className="h-[60vh] flex items-center justify-center">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
            </div>
        );
    }

    return (
        <div className="space-y-10 animate-in fade-in duration-1000">
            <div className="flex flex-col gap-2">
                <h1 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white uppercase">
                    Hub de Modelos de Controle
                </h1>
                <p className="text-neutral-500 dark:text-neutral-400 font-medium">
                    Selecione um modelo para visualizar métricas detalhadas e KPIs de performance.
                </p>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                {filteredModels.map((model, idx) => (
                    <motion.div
                        key={model.id}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: idx * 0.1 }}
                        onClick={() => navigate(model.to)}
                        className="group cursor-pointer bg-white dark:bg-neutral-900 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800 p-8 hover:border-blue-500/50 hover:shadow-2xl transition-all duration-500 relative overflow-hidden flex flex-col h-full"
                    >
                        <div className={`w-16 h-16 rounded-2xl bg-${model.color}-500/10 flex items-center justify-center text-${model.color}-500 mb-8 group-hover:scale-110 transition-transform duration-500 shadow-lg shadow-${model.color}-500/5`}>
                            <model.icon size={32} />
                        </div>

                        <div className="flex-1 space-y-4">
                            <h2 className="text-2xl font-black text-neutral-900 dark:text-white uppercase tracking-tight">
                                {model.title}
                            </h2>
                            <p className="text-neutral-500 dark:text-neutral-400 font-medium leading-relaxed">
                                {model.description}
                            </p>

                            <div className="flex flex-wrap gap-2 pt-4">
                                {model.stats.map(stat => (
                                    <span
                                        key={stat}
                                        className="px-3 py-1 rounded-full bg-neutral-100 dark:bg-neutral-800 text-[10px] font-black uppercase tracking-tighter text-neutral-500 dark:text-neutral-400"
                                    >
                                        {stat}
                                    </span>
                                ))}
                            </div>
                        </div>

                        <div className="mt-8 pt-6 border-t border-neutral-100 dark:border-neutral-800 flex items-center justify-between">
                            <span className="text-sm font-bold text-blue-600 dark:text-blue-400 opacity-0 group-hover:opacity-100 transition-opacity">
                                Ver Detalhes
                            </span>
                            <div className="w-10 h-10 rounded-full bg-neutral-50 dark:bg-neutral-800 flex items-center justify-center text-neutral-400 group-hover:bg-blue-600 group-hover:text-white transition-all">
                                <ArrowRight size={18} className="group-hover:translate-x-1 transition-transform" />
                            </div>
                        </div>

                        {/* Accent background decoration */}
                        <div className={`absolute -bottom-12 -right-12 w-32 h-32 bg-${model.color}-500/5 rounded-full blur-3xl opacity-0 group-hover:opacity-100 transition-opacity`} />
                    </motion.div>
                ))}
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                <div className="p-8 bg-gradient-to-br from-blue-600 to-blue-800 rounded-[2.5rem] text-white shadow-xl shadow-blue-500/20">
                    <div className="flex items-center gap-4 mb-6">
                        <div className="p-3 bg-white/10 rounded-2xl backdrop-blur-md">
                            <TrendingUp size={24} />
                        </div>
                        <h3 className="text-xl font-bold uppercase">Performance Global</h3>
                    </div>
                    <p className="text-blue-100 font-medium mb-6">
                        Acompanhe o crescimento consolidado de todos os modelos de negócio em tempo real.
                    </p>
                    <div className="grid grid-cols-2 gap-4">
                        <div className="p-4 bg-white/10 rounded-2xl">
                            <p className="text-[10px] font-black uppercase text-blue-200 mb-1">Taxa de Crescimento</p>
                            <p className="text-2xl font-black">+12.4%</p>
                        </div>
                        <div className="p-4 bg-white/10 rounded-2xl">
                            <p className="text-[10px] font-black uppercase text-blue-200 mb-1">ROAS Médio</p>
                            <p className="text-2xl font-black">4.2x</p>
                        </div>
                    </div>
                </div>

                <div className="p-8 bg-white dark:bg-neutral-900 rounded-[2.5rem] border border-neutral-200 dark:border-neutral-800">
                    <div className="flex items-center gap-4 mb-6">
                        <div className="p-3 bg-neutral-100 dark:bg-neutral-800 rounded-2xl text-blue-600">
                            <Target size={24} />
                        </div>
                        <h3 className="text-xl font-bold uppercase text-neutral-900 dark:text-white">Metas Mensais</h3>
                    </div>
                    <p className="text-neutral-500 dark:text-neutral-400 font-medium mb-6">
                        Distribuição de objetivos por modelo e cliente ativo no sistema.
                    </p>
                    <div className="space-y-4">
                        {['Varejo', 'Saúde', 'Cadastros'].map(m => (
                            <div key={m} className="space-y-1">
                                <div className="flex justify-between text-[10px] font-bold uppercase">
                                    <span>{m}</span>
                                    <span>75%</span>
                                </div>
                                <div className="h-2 bg-neutral-100 dark:bg-neutral-800 rounded-full overflow-hidden">
                                    <motion.div
                                        initial={{ width: 0 }}
                                        animate={{ width: '75%' }}
                                        transition={{ duration: 2, delay: 0.5 }}
                                        className="h-full bg-blue-600 rounded-full"
                                    />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}
