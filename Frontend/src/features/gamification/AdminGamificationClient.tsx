'use client';

import { useState } from 'react';
import { Plus } from 'lucide-react';
import { useAdminGamification } from './useAdminGamification';
import type { QuestDefinition, AchievementDefinition, TitleDefinition } from './gamification.types';
import { AdminGamificationTabs } from './AdminGamificationTabs';
import { AdminQuestTable } from './AdminQuestTable';
import { AdminAchievementTable } from './AdminAchievementTable';
import { AdminTitleTable } from './AdminTitleTable';

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

      <AdminGamificationTabs activeTab={activeTab} onChange={setActiveTab} />

      <div className="bg-slate-900/40 border border-slate-800/60 rounded-3xl backdrop-blur-md overflow-hidden shadow-2xl">
        {activeTab === 'quests' && (
          <AdminQuestTable
            quests={initialQuests}
            onEdit={(code) => handleEdit('Quest', code)}
            onDelete={(code) => handleDelete('Quest', code)}
          />
        )}

        {activeTab === 'achievements' && (
          <AdminAchievementTable
            achievements={initialAchievements}
            onEdit={(code) => handleEdit('Achievement', code)}
            onDelete={(code) => handleDelete('Achievement', code)}
          />
        )}

        {activeTab === 'titles' && (
          <AdminTitleTable
            titles={initialTitles}
            onEdit={(code) => handleEdit('Title', code)}
            onDelete={(code) => handleDelete('Title', code)}
          />
        )}
      </div>
    </div>
  );
}
