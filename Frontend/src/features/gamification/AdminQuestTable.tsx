'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { AdminQuestDefinition } from '@/features/gamification/admin/adminGamification.types';
import { cn } from '@/lib/utils';

interface AdminQuestTableProps {
  quests: AdminQuestDefinition[];
  onEdit: (code: string) => void;
  onDelete: (code: string) => void;
}

export function AdminQuestTable({ quests, onEdit, onDelete }: AdminQuestTableProps) {
  return (
    <div className={cn("overflow-x-auto")}>
      <table className={cn("w-full text-left")}>
        <thead>
          <tr className={cn("bg-slate-800/40 text-slate-400 text-xs font-bold uppercase tracking-wider")}>
            <th className={cn("px-6 py-4")}>Mã</th>
            <th className={cn("px-6 py-4")}>Nhiệm Vụ (VI)</th>
            <th className={cn("px-6 py-4")}>Loại</th>
            <th className={cn("px-6 py-4")}>Mục Tiêu</th>
            <th className={cn("px-6 py-4")}>Trang Thai</th>
            <th className={cn("px-6 py-4 text-right")}>Thao Tác</th>
          </tr>
        </thead>
        <tbody className={cn("divide-y divide-slate-800/40 text-slate-300")}>
          {quests.map((quest) => (
            <tr key={quest.code} className={cn("group transition-colors")}>
              <td className={cn("px-6 py-4")}>
                <code className={cn("text-pink-400 font-mono text-xs")}>{quest.code}</code>
              </td>
              <td className={cn("px-6 py-4 font-bold")}>{quest.titleVi}</td>
              <td className={cn("px-6 py-4")}>
                <span className={cn("px-2 py-1 bg-slate-800 rounded-lg text-xs border border-slate-700")}>{quest.questType}</span>
              </td>
              <td className={cn("px-6 py-4")}>{quest.target}</td>
              <td className={cn("px-6 py-4")}>{quest.isActive ? 'Active' : 'Inactive'}</td>
              <td className={cn("px-6 py-4 text-right flex justify-end gap-2")}>
                <button type="button" onClick={() => onEdit(quest.code)} className={cn("p-2 rounded-lg text-slate-400 transition-colors")}>
                  <Edit className={cn("w-4 h-4")} />
                </button>
                <button type="button" onClick={() => onDelete(quest.code)} className={cn("p-2 rounded-lg text-slate-400 transition-colors")}>
                  <Trash2 className={cn("w-4 h-4")} />
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
