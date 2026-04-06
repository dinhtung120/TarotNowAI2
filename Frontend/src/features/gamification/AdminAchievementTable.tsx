'use client';

import { Edit, Trash2 } from 'lucide-react';
import type { AchievementDefinition } from './gamification.types';

interface AdminAchievementTableProps {
  achievements: AchievementDefinition[];
  onEdit: (code: string) => void;
  onDelete: (code: string) => void;
}

export function AdminAchievementTable({ achievements, onEdit, onDelete }: AdminAchievementTableProps) {
  return (
    <div className="overflow-x-auto">
      <table className="w-full text-left">
        <thead>
          <tr className="bg-slate-800/40 text-slate-400 text-xs font-bold uppercase tracking-wider">
            <th className="px-6 py-4">Mã</th>
            <th className="px-6 py-4">Thành Tựu (VI)</th>
            <th className="px-6 py-4">Ẩn</th>
            <th className="px-6 py-4">Danh Hiệu Tặng Kèm</th>
            <th className="px-6 py-4 text-right">Thao Tác</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-slate-800/40 text-slate-300">
          {achievements.map((achievement) => (
            <tr key={achievement.code} className="group hover:bg-slate-800/30 transition-colors">
              <td className="px-6 py-4">
                <code className="text-yellow-400 font-mono text-xs">{achievement.code}</code>
              </td>
              <td className="px-6 py-4 font-bold">{achievement.titleVi}</td>
              <td className="px-6 py-4">
                {achievement.isHidden ? (
                  <span className="text-slate-500 text-xs">Có</span>
                ) : (
                  <span className="text-green-500 text-xs">Không</span>
                )}
              </td>
              <td className="px-6 py-4 text-indigo-400 text-sm font-medium">{achievement.grantsTitleCode || '-'}</td>
              <td className="px-6 py-4 text-right flex justify-end gap-2">
                <button onClick={() => onEdit(achievement.code)} className="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
                  <Edit className="w-4 h-4" />
                </button>
                <button onClick={() => onDelete(achievement.code)} className="p-2 hover:bg-red-500/10 rounded-lg text-slate-400 hover:text-red-400 transition-colors">
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
