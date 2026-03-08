import { useState, useRef, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { ChevronDown, Search, Check, Filter } from 'lucide-react';

interface Cliente {
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
    const [isOpen, setIsOpen] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const containerRef = useRef<HTMLDivElement>(null);

    const selectedCliente = clientes?.find(c => c.id === selectedValue);
    const displayName = isLoading ? 'Carregando...' : (selectedCliente ? selectedCliente.nome : (selectedValue ? `Cliente #${selectedValue}` : 'Todos os Clientes'));

    const filteredClientes = clientes?.filter(c =>
        c.nome.toLowerCase().includes(searchTerm.toLowerCase())
    );

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
                setIsOpen(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    return (
        <div className="relative" ref={containerRef}>
            <button
                type="button"
                onClick={() => !disabled && setIsOpen(!isOpen)}
                disabled={disabled}
                className={`flex items-center gap-3 bg-white dark:bg-neutral-900 px-5 py-2.5 rounded-2xl border border-neutral-200 dark:border-neutral-800 shadow-sm transition-all duration-300 min-w-[240px] justify-between group
                    ${disabled ? 'opacity-50 cursor-not-allowed' : 'hover:border-blue-500/50 hover:shadow-lg active:scale-95 cursor-pointer'}
                    ${isOpen ? 'border-blue-500 ring-4 ring-blue-500/10' : ''}`}
            >
                <div className="flex items-center gap-2 overflow-hidden">
                    <Filter size={18} className={`${isOpen ? 'text-blue-500' : 'text-neutral-400'} transition-colors`} />
                    <span className="text-sm font-bold text-neutral-700 dark:text-neutral-200 truncate">
                        {displayName}
                    </span>
                </div>
                <ChevronDown
                    size={18}
                    className={`text-neutral-400 group-hover:text-neutral-600 dark:group-hover:text-neutral-200 transition-transform duration-300 ${isOpen ? 'rotate-180 text-blue-500' : ''}`}
                />
            </button>

            <AnimatePresence>
                {isOpen && (
                    <motion.div
                        initial={{ opacity: 0, y: 10, scale: 0.95 }}
                        animate={{ opacity: 1, y: 5, scale: 1 }}
                        exit={{ opacity: 0, y: 10, scale: 0.95 }}
                        transition={{ duration: 0.2, ease: "easeOut" }}
                        className="absolute right-0 top-full z-[120] w-72 mt-2 bg-white/90 dark:bg-neutral-900/90 backdrop-blur-xl border border-neutral-200 dark:border-neutral-800 rounded-[2rem] shadow-2xl overflow-hidden origin-top"
                    >
                        <div className="p-4 border-b border-neutral-100 dark:border-neutral-800">
                            <div className="relative">
                                <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-neutral-400" size={16} />
                                <input
                                    autoFocus
                                    type="text"
                                    placeholder="Pesquisar cliente..."
                                    className="w-full bg-neutral-50 dark:bg-neutral-800 border-none rounded-xl py-2 pl-10 pr-4 text-sm focus:ring-2 focus:ring-blue-500/20 text-neutral-700 dark:text-neutral-200"
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                />
                            </div>
                        </div>

                        <div className="max-h-64 overflow-y-auto p-2">
                            <button
                                onClick={() => {
                                    onChange(null);
                                    setIsOpen(false);
                                }}
                                className={`w-full flex items-center justify-between px-4 py-3 rounded-xl text-sm font-bold transition-all duration-200 mb-1
                                    ${!selectedValue ? 'bg-blue-500 text-white shadow-lg shadow-blue-500/20' : 'text-neutral-600 dark:text-neutral-400 hover:bg-neutral-50 dark:hover:bg-neutral-800'}`}
                            >
                                <span>Todos os Clientes</span>
                                {!selectedValue && <Check size={16} />}
                            </button>

                            {filteredClientes?.map((c) => (
                                <button
                                    key={c.id}
                                    onClick={() => {
                                        onChange(c.id);
                                        setIsOpen(false);
                                    }}
                                    className={`w-full flex items-center justify-between px-4 py-3 rounded-xl text-sm font-bold transition-all duration-200 mb-1
                                        ${selectedValue === c.id ? 'bg-blue-500 text-white shadow-lg shadow-blue-500/20' : 'text-neutral-600 dark:text-neutral-400 hover:bg-neutral-50 dark:hover:bg-neutral-800'}`}
                                >
                                    <span className="truncate">{c.nome}</span>
                                    {selectedValue === c.id && <Check size={16} />}
                                </button>
                            ))}

                            {filteredClientes?.length === 0 && (
                                <div className="py-8 text-center">
                                    <p className="text-sm text-neutral-400">Nenhum cliente encontrado</p>
                                </div>
                            )}
                        </div>
                    </motion.div>
                )}
            </AnimatePresence>
        </div>
    );
}
