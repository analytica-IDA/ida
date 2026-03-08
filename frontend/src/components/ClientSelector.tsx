import { Filter, ChevronDown } from 'lucide-react';

export interface Cliente {
    id: number;
    nome: string;
}

interface ClientSelectorProps {
    clientes: Cliente[] | undefined;
    selectedValue: number | null;
    onChange: (value: number | null) => void;
    disabled?: boolean;
    isLoading?: boolean;
}

export default function ClientSelector({ clientes, selectedValue, onChange, disabled, isLoading }: ClientSelectorProps) {
    return (
        <div className="relative group min-w-[240px]">
            <div className={`absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none transition-colors duration-300 ${disabled ? 'text-neutral-400' : 'text-blue-500 group-focus-within:text-blue-600'}`}>
                <Filter size={18} />
            </div>

            <select
                disabled={disabled || isLoading}
                value={selectedValue || ''}
                onChange={(e) => onChange(e.target.value ? Number(e.target.value) : null)}
                className={`w-full appearance-none bg-white dark:bg-neutral-900 px-12 py-3 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm text-sm font-bold text-neutral-700 dark:text-neutral-200 transition-all duration-300
                    ${(disabled || isLoading) ? 'opacity-70 cursor-not-allowed bg-neutral-50 dark:bg-neutral-800/50' : 'hover:border-blue-500/50 hover:shadow-lg cursor-pointer focus:outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500'}
                `}
            >
                <option value="" className="font-semibold text-neutral-900 bg-white dark:text-neutral-100 dark:bg-neutral-900">
                    {isLoading ? 'Carregando...' : 'Todos os Clientes'}
                </option>
                {clientes?.map(c => (
                    <option key={c.id} value={c.id} className="font-medium text-neutral-800 bg-white dark:text-neutral-200 dark:bg-neutral-900">
                        {c.nome}
                    </option>
                ))}
            </select>

            <div className={`absolute inset-y-0 right-0 pr-4 flex items-center pointer-events-none transition-transform duration-300 ${disabled ? 'text-neutral-400' : 'text-neutral-500 group-focus-within:text-blue-500 group-focus-within:rotate-180'}`}>
                <ChevronDown size={18} />
            </div>
        </div>
    );
}
