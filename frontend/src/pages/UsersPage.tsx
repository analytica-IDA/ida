import { Plus, Search, Filter, Edit2, Trash2 } from 'lucide-react';
import { motion } from 'framer-motion';

export default function UsersPage() {
  return (
    <motion.div 
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      className="space-y-8"
    >
      <div className="flex flex-col lg:flex-row lg:items-end justify-between gap-6">
        <div>
          <h2 className="text-3xl font-black tracking-tight text-neutral-900 dark:text-white">Gerenciamento de Usuários</h2>
          <p className="text-neutral-500 dark:text-neutral-400 font-medium mt-1">Total de 12 usuários ativos no sistema</p>
        </div>
        <div className="flex items-center gap-3">
          <button className="flex items-center justify-center gap-2 px-6 py-3 bg-white dark:bg-neutral-900 border border-neutral-200 dark:border-neutral-800 text-neutral-700 dark:text-neutral-300 rounded-2xl font-bold hover:bg-neutral-50 transition-all shadow-sm">
            <Filter size={18} />
            <span>Filtros</span>
          </button>
          <button className="flex items-center justify-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-2xl font-black hover:bg-blue-700 transition-all shadow-lg shadow-blue-500/20 active:scale-95">
            <Plus size={18} strokeWidth={3} />
            <span>Criar Novo Usuário</span>
          </button>
        </div>
      </div>

      <div className="bg-white dark:bg-neutral-900 rounded-[2rem] shadow-xl shadow-neutral-200/50 dark:shadow-none border border-neutral-100 dark:border-neutral-800 overflow-hidden">
        <div className="p-6 border-b border-neutral-100 dark:border-neutral-800/50 flex flex-col md:flex-row md:items-center justify-between gap-4 bg-neutral-50/30 dark:bg-neutral-800/10">
          <div className="relative flex-1 max-w-md group">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400 group-focus-within:text-blue-500 transition-colors" size={18} />
            <input 
              type="text" 
              placeholder="Buscar por nome, login ou email..." 
              className="w-full pl-12 pr-4 py-3 bg-white dark:bg-neutral-800 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none focus:ring-4 focus:ring-blue-500/10 focus:border-blue-500/50 transition-all font-medium text-sm"
            />
          </div>
          <div className="flex items-center gap-2 text-xs font-bold text-neutral-400 uppercase tracking-widest px-2">
            Ordernar por: <span className="text-blue-600 dark:text-blue-400 cursor-pointer hover:underline ml-1">Mais recentes</span>
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-neutral-50/50 dark:bg-neutral-900/50 text-[10px] font-black text-neutral-400 uppercase tracking-[0.15em]">
              <tr>
                <th className="px-8 py-5 text-left">Identificação</th>
                <th className="px-8 py-5 text-left">Atribuição</th>
                <th className="px-8 py-5 text-left">Situação</th>
                <th className="px-8 py-5 text-left">Atividade</th>
                <th className="px-8 py-5 text-center">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-neutral-100 dark:divide-neutral-800">
              <UserRow 
                name="Paulo Takeda" 
                login="admin" 
                email="paulo.takeda@gmail.com" 
                role="Administrador" 
                status="Ativo" 
                lastSeen="Hoje às 14:00"
                avatarColor="bg-blue-100 text-blue-600"
              />
              <UserRow 
                name="Beatriz Silva" 
                login="beatriz" 
                email="beatriz@analytica.com" 
                role="Proprietário" 
                status="Ativo" 
                lastSeen="Ontem às 18:30"
                avatarColor="bg-purple-100 text-purple-600"
              />
              <UserRow 
                name="Lucas Oliveira" 
                login="lucas_v" 
                email="lucas@analytica.com" 
                role="Vendedor" 
                status="Pendente" 
                lastSeen="Há 2 dias"
                avatarColor="bg-amber-100 text-amber-600"
              />
            </tbody>
          </table>
        </div>
        
        <div className="p-6 bg-neutral-50/30 dark:bg-neutral-800/10 border-t border-neutral-100 dark:border-neutral-800 flex items-center justify-between">
            <span className="text-sm font-semibold text-neutral-500">Mostrando 3 de 12 registros</span>
            <div className="flex gap-2">
                <button className="px-4 py-2 rounded-lg border border-neutral-200 dark:border-neutral-700 text-sm font-bold opacity-50 cursor-not-allowed">Anterior</button>
                <button className="px-4 py-2 rounded-lg border border-neutral-200 dark:border-neutral-700 text-sm font-bold hover:bg-neutral-100 dark:hover:bg-neutral-800 transition-colors">Próximo</button>
            </div>
        </div>
      </div>
    </motion.div>
  );
}

function UserRow({ name, login, email, role, status, lastSeen, avatarColor }: any) {
  return (
    <tr className="group hover:bg-neutral-50/50 dark:hover:bg-neutral-800/30 transition-colors">
      <td className="px-8 py-6">
        <div className="flex items-center gap-4">
          <div className={cn("w-12 h-12 rounded-2xl flex items-center justify-center font-black text-lg transition-transform group-hover:scale-110", avatarColor)}>
            {name.charAt(0)}
          </div>
          <div className="flex flex-col">
            <span className="font-bold text-neutral-900 dark:text-neutral-100 leading-tight">{name}</span>
            <div className="flex items-center gap-2 mt-1">
                <span className="text-xs font-semibold text-neutral-400">@{login}</span>
                <span className="w-1 h-1 bg-neutral-300 rounded-full"></span>
                <span className="text-xs font-semibold text-neutral-400 truncate max-w-[150px]">{email}</span>
            </div>
          </div>
        </div>
      </td>
      <td className="px-8 py-6">
        <div className="flex flex-col">
          <span className="text-sm font-bold text-neutral-700 dark:text-neutral-300">{role}</span>
          <span className="text-[10px] uppercase tracking-wider text-neutral-400 font-bold mt-1">Acesso Full</span>
        </div>
      </td>
      <td className="px-8 py-6">
        <span className={cn(
          "px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-wider",
          status === 'Ativo' ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400" : "bg-amber-100 text-amber-700 dark:bg-amber-900/30 dark:text-amber-400"
        )}>
          {status}
        </span>
      </td>
      <td className="px-8 py-6">
        <span className="text-sm font-semibold text-neutral-500">{lastSeen}</span>
      </td>
      <td className="px-8 py-6">
        <div className="flex justify-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
          <button className="p-2 hover:bg-blue-50 dark:hover:bg-blue-900/30 text-blue-600 rounded-xl transition-colors">
            <Edit2 size={16} />
          </button>
          <button className="p-2 hover:bg-red-50 dark:hover:bg-red-900/30 text-red-600 rounded-xl transition-colors">
            <Trash2 size={16} />
          </button>
        </div>
      </td>
    </tr>
  );
}

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

