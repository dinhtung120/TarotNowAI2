'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { AdminAchievementDefinition } from '@/features/gamification/admin/application/adminGamification.types';
import { cn } from '@/lib/utils';

interface AdminAchievementTableProps {
  achievements: AdminAchievementDefinition[];
  onEdit: (code: string) => void;
  onDelete: (code: string) => void;
}

export function AdminAchievementTable({ achievements, onEdit, onDelete }: AdminAchievementTableProps) {
  return (
    <div className={cn("overflow-x-auto")}>
      <table className={cn("w-full text-left")}>
        <thead>
          <tr className={cn("bg-slate-800/40 text-slate-400 text-xs font-bold uppercase tracking-wider")}>
            <th className={cn("px-6 py-4")}>Mã</th>
            <th className={cn("px-6 py-4")}>Thành Tựu (VI)</th>
            <th className={cn("px-6 py-4")}>Trang Thai</th>
            <th className={cn("px-6 py-4")}>Danh Hiệu Tặng Kèm</th>
            <th className={cn("px-6 py-4 text-right")}>Thao Tác</th>
          </tr>
        </thead>
        <tbody className={cn("divide-y divide-slate-800/40 text-slate-300")}>
          {achievements.map((achievement) => (
            <tr key={achievement.code} className={cn("group transition-colors")}>
              <td className={cn("px-6 py-4")}>
                <code className={cn("text-yellow-400 font-mono text-xs")}>{achievement.code}</code>
              </td>
              <td className={cn("px-6 py-4 font-bold")}>{achievement.titleVi}</td>
              <td className={cn("px-6 py-4")}>
                {achievement.isActive ? (
                  <span className={cn("text-green-500 text-xs")}>Active</span>
                ) : (
                  <span className={cn("text-slate-500 text-xs")}>Inactive</span>
                )}
              </td>
              <td className={cn("px-6 py-4 text-indigo-400 text-sm font-medium")}>{achievement.grantsTitleCode || '-'}</td>
              <td className={cn("px-6 py-4 text-right flex justify-end gap-2")}>
                <button type="button" onClick={() => onEdit(achievement.code)} className={cn("p-2 rounded-lg text-slate-400 transition-colors")}>
                  <Edit className={cn("w-4 h-4")} />
                </button>
                <button type="button" onClick={() => onDelete(achievement.code)} className={cn("p-2 rounded-lg text-slate-400 transition-colors")}>
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
