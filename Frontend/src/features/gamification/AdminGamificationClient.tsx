'use client';

import { useState } from 'react';
import { Plus, Edit, Trash2, Trophy, Medal, Crown } from 'lucide-react';
import { useAdminGamification } from './useAdminGamification';
import type { QuestDefinition, AchievementDefinition, TitleDefinition } from './gamification.types';

interface Props {
  initialQuests: QuestDefinition[];
  initialAchievements: AchievementDefinition[];
  initialTitles: TitleDefinition[];
}

export default function AdminGamificationClient({ 
  initialQuests, 
  initialAchievements, 
  initialTitles 
}: Props) {
  const { deleteQuest, deleteAchievement, deleteTitle } = useAdminGamification();
  const [activeTab, setActiveTab] = useState<'quests' | 'achievements' | 'titles'>('quests');

  // Placeholder for Edit/Create Modals (simulated for now by prompt)
  const handleCreate = (type: string) => {
    alert(`Mở Modal tạo ${type}. Tính năng CRUD đầy đủ đang được triển khai...`);
  };

  const handleEdit = (type: string, code: string) => {
    alert(`Mở Modal sửa ${type}: ${code}`);
  };

  const handleDelete = (type: string, code: string) => {
    if (confirm(`Bạn có chắc chắn muốn xóa ${type} này không? (${code})`)) {
      if (type === 'Quest') deleteQuest.mutate(code);
      if (type === 'Achievement') deleteAchievement.mutate(code);
      if (type === 'Title') deleteTitle.mutate(code);
    }
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <header className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-3xl font-black text-white tracking-tight">Quản Trị Gamification</h1>
          <p className="text-slate-400 mt-1">Cấu hình nhiệm vụ, thành tựu và danh hiệu hệ thống.</p>
        </div>
        <button 
          onClick={() => handleCreate(activeTab === 'quests' ? 'Nhiệm Vụ' : activeTab === 'achievements' ? 'Thành Tựu' : 'Danh Hiệu')}
          className="flex items-center gap-2 px-5 py-2.5 bg-indigo-600 hover:bg-indigo-500 text-white rounded-xl shadow-lg shadow-indigo-500/20 transition-all font-bold"
        >
          <Plus className="w-5 h-5" />
          Tạo Mới
        </button>
      </header>

      {/* Tabs */}
      <nav className="flex gap-2 p-1 bg-slate-900/50 rounded-2xl border border-slate-800 w-fit">
        <button
          onClick={() => setActiveTab('quests')}
          className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
            activeTab === 'quests' ? 'bg-indigo-600 text-white shadow-lg' : 'text-slate-400 hover:text-slate-200'
          }`}
        >
          <Trophy className="w-4 h-4" />
          Nhiệm Vụ
        </button>
        <button
          onClick={() => setActiveTab('achievements')}
          className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
            activeTab === 'achievements' ? 'bg-indigo-600 text-white shadow-lg' : 'text-slate-400 hover:text-slate-200'
          }`}
        >
          <Medal className="w-4 h-4" />
          Thành Tựu
        </button>
        <button
          onClick={() => setActiveTab('titles')}
          className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all ${
            activeTab === 'titles' ? 'bg-indigo-600 text-white shadow-lg' : 'text-slate-400 hover:text-slate-200'
          }`}
        >
          <Crown className="w-4 h-4" />
          Danh Hiệu
        </button>
      </nav>

      {/* Tables container */}
      <div className="bg-slate-900/40 border border-slate-800/60 rounded-3xl backdrop-blur-md overflow-hidden shadow-2xl">
        {activeTab === 'quests' && (
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
                {initialQuests.map(q => (
                  <tr key={q.code} className="group hover:bg-slate-800/30 transition-colors">
                    <td className="px-6 py-4"><code className="text-pink-400 font-mono text-xs">{q.code}</code></td>
                    <td className="px-6 py-4 font-bold">{q.titleVi}</td>
                    <td className="px-6 py-4"><span className="px-2 py-1 bg-slate-800 rounded-lg text-xs border border-slate-700">{q.questType}</span></td>
                    <td className="px-6 py-4">{q.target}</td>
                    <td className="px-6 py-4 text-right flex justify-end gap-2">
                       <button onClick={() => handleEdit('Quest', q.code)} className="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
                         <Edit className="w-4 h-4" />
                       </button>
                       <button onClick={() => handleDelete('Quest', q.code)} className="p-2 hover:bg-red-500/10 rounded-lg text-slate-400 hover:text-red-400 transition-colors">
                         <Trash2 className="w-4 h-4" />
                       </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {activeTab === 'achievements' && (
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
                {initialAchievements.map(a => (
                  <tr key={a.code} className="group hover:bg-slate-800/30 transition-colors">
                    <td className="px-6 py-4"><code className="text-yellow-400 font-mono text-xs">{a.code}</code></td>
                    <td className="px-6 py-4 font-bold">{a.titleVi}</td>
                    <td className="px-6 py-4">
                      {a.isHidden ? 
                        <span className="text-slate-500 text-xs">Có</span> : 
                        <span className="text-green-500 text-xs">Không</span>
                      }
                    </td>
                    <td className="px-6 py-4 text-indigo-400 text-sm font-medium">{a.grantsTitleCode || '-'}</td>
                    <td className="px-6 py-4 text-right flex justify-end gap-2">
                       <button onClick={() => handleEdit('Achievement', a.code)} className="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
                         <Edit className="w-4 h-4" />
                       </button>
                       <button onClick={() => handleDelete('Achievement', a.code)} className="p-2 hover:bg-red-500/10 rounded-lg text-slate-400 hover:text-red-400 transition-colors">
                         <Trash2 className="w-4 h-4" />
                       </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {activeTab === 'titles' && (
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
                {initialTitles.map(t => (
                  <tr key={t.code} className="group hover:bg-slate-800/30 transition-colors">
                    <td className="px-6 py-4"><code className="text-cyan-400 font-mono text-xs">{t.code}</code></td>
                    <td className="px-6 py-4 font-bold">{t.nameVi}</td>
                    <td className="px-6 py-4">
                      <span className={`px-2 py-0.5 rounded text-[10px] font-black uppercase ${
                        t.rarity === 'Legendary' ? 'bg-orange-500/20 text-orange-400 border border-orange-500/50' :
                        t.rarity === 'Epic' ? 'bg-purple-500/20 text-purple-400 border border-purple-500/50' :
                        'bg-slate-700 text-slate-300'
                      }`}>
                        {t.rarity}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-right flex justify-end gap-2">
                       <button onClick={() => handleEdit('Title', t.code)} className="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
                         <Edit className="w-4 h-4" />
                       </button>
                       <button onClick={() => handleDelete('Title', t.code)} className="p-2 hover:bg-red-500/10 rounded-lg text-slate-400 hover:text-red-400 transition-colors">
                         <Trash2 className="w-4 h-4" />
                       </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
