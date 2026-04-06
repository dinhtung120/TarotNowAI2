'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { TitleDefinition } from './gamification.types';

interface AdminTitleTableProps {
  titles: TitleDefinition[];
  onEdit: (code: string) => void;
  onDelete: (code: string) => void;
}

export function AdminTitleTable({ titles, onEdit, onDelete }: AdminTitleTableProps) {
  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left">
        <thead>
          <tr className="bg-slate-800/40 text-slate-400 text-xs font-bold uppercase tracking-wider">
            <th className="px-6 py-4">Mã</th>
            <th className="px-6 py-4">Danh Hiệu (VI)</th>
            <th className="px-6 py-4">Độ Hiếm</th>
            <th className="px-6 py-4 text-right">Thao Tác</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-slate-800/40 text-slate-300">
          {titles.map((title) => (
            <tr key={title.code} className="group hover:bg-slate-800/30 transition-colors">
              <td className="px-6 py-4">
                <code className="text-cyan-400 font-mono text-xs">{title.code}</code>
              </td>
              <td className="px-6 py-4 font-bold">{title.nameVi}</td>
              <td className="px-6 py-4">
                <span
                  className={`px-2 py-0.5 rounded text-[10px] font-black uppercase ${
                    title.rarity === 'Legendary'
                      ? 'bg-orange-500/20 text-orange-400 border border-orange-500/50'
                      : title.rarity === 'Epic'
                        ? 'bg-purple-500/20 text-purple-400 border border-purple-500/50'
                        : 'bg-slate-700 text-slate-300'
                  }`}
                >
                  {title.rarity}
                </span>
              </td>
              <td className="px-6 py-4 text-right flex justify-end gap-2">
                <button onClick={() => onEdit(title.code)} className="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
                  <Edit className="w-4 h-4" />
                </button>
                <button onClick={() => onDelete(title.code)} className="p-2 hover:bg-red-500/10 rounded-lg text-slate-400 hover:text-red-400 transition-colors">
                  <Trash2 className="w-4 h-4" />
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
