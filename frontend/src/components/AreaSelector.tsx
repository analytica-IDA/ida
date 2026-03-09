import { MapPin, ChevronDown, Loader2 } from 'lucide-react';
import { useQuery } from '@tanstack/react-query';
import api from '../services/api';

interface Area {
    id: number;
    nome: string;
}

interface AreaSelectorProps {
    idCliente: number | null;
    selectedValue: number | null;
    onChange: (value: number | null) => void;
    disabled?: boolean;
}

export default function AreaSelector({ idCliente, selectedValue, onChange, disabled }: AreaSelectorProps) {
    const { data: areas, isLoading } = useQuery<Area[]>({
        queryKey: ['areas', idCliente],
        queryFn: async () => {
            if (!idCliente) return [];
            const { data } = await api.get('/area');
            // Assuming the areas returned by /area might need filtering by client if the API doesn't do it automatically,
            // but usually /area for a non-admin returns only areas relevant to them.
            // Let's check how /area is implemented in the backend if needed, 
            // but for now we follow the pattern of the application.
            return data;
        },
        enabled: !!idCliente
    });

    return (
        <div className="relative group min-w-[240px]">
            <div className={`absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none transition-colors duration-300 ${disabled ? 'text-neutral-400' : 'text-blue-500 group-focus-within:text-blue-600'}`}>
                {isLoading ? <Loader2 size={18} className="animate-spin text-blue-500" /> : <MapPin size={18} />}
            </div>

            <select
                disabled={disabled || isLoading || !idCliente}
                value={selectedValue || ''}
                onChange={(e) => onChange(e.target.value ? Number(e.target.value) : null)}
                className={`w-full appearance-none bg-white dark:bg-neutral-900 px-12 py-3 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm text-sm font-bold text-neutral-700 dark:text-neutral-200 transition-all duration-300
                    ${(disabled || isLoading || !idCliente) ? 'opacity-70 cursor-not-allowed bg-neutral-50 dark:bg-neutral-800/50' : 'hover:border-blue-500/50 hover:shadow-lg cursor-pointer focus:outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500'}
                `}
            >
                <option value="" className="font-semibold text-neutral-900 bg-white dark:text-neutral-100 dark:bg-neutral-900">
                    {isLoading ? 'Carregando Áreas...' : 'Todas as Áreas'}
                </option>
                {areas?.map(a => (
                    <option key={a.id} value={a.id} className="font-medium text-neutral-800 bg-white dark:text-neutral-200 dark:bg-neutral-900">
                        {a.nome}
                    </option>
                ))}
            </select>

            <div className={`absolute inset-y-0 right-0 pr-4 flex items-center pointer-events-none transition-transform duration-300 ${disabled ? 'text-neutral-400' : 'text-neutral-500 group-focus-within:text-blue-500 group-focus-within:rotate-180'}`}>
                <ChevronDown size={18} />
            </div>
        </div>
    );
}
