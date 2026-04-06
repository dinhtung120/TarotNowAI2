'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { QuestDefinition } from './gamification.types';

interface AdminQuestTableProps {
  quests: QuestDefinition[];
  onEdit: (code: string) => void;
  onDelete: (code: string) => void;
}

export function AdminQuestTable({ quests, onEdit, onDelete }: AdminQuestTableProps) {
  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left">
        <thead>
          <tr className="bg-slate-800/40 text-slate-400 text-xs font-bold uppercase tracking-wider">
            <th className="px-6 py-4">Mã</th>
            <th className="px-6 py-4">Nhiệm Vụ (VI)</th>
            <th className="px-6 py-4">Loại</th>
            <th className="px-6 py-4">Mục Tiêu</th>
            <th className="px-6 py-4 text-right">Thao Tác</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-slate-800/40 text-slate-300">
          {quests.map((quest) => (
            <tr key={quest.code} className="group hover:bg-slate-800/30 transition-colors">
              <td className="px-6 py-4">
                <code className="text-pink-400 font-mono text-xs">{quest.code}</code>
              </td>
              <td className="px-6 py-4 font-bold">{quest.titleVi}</td>
              <td className="px-6 py-4">
                <span className="px-2 py-1 bg-slate-800 rounded-lg text-xs border border-slate-700">{quest.questType}</span>
              </td>
              <td className="px-6 py-4">{quest.target}</td>
              <td className="px-6 py-4 text-right flex justify-end gap-2">
                <button onClick={() => onEdit(quest.code)} className="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
                  <Edit className="w-4 h-4" />
                </button>
                <button onClick={() => onDelete(quest.code)} className="p-2 hover:bg-red-500/10 rounded-lg text-slate-400 hover:text-red-400 transition-colors">
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
