"use client";

import { Plus } from "lucide-react";
import type { AchievementDefinition, QuestDefinition, TitleDefinition } from "@/features/gamification/gamification.types";
import { AdminAchievementTable } from "@/features/gamification/AdminAchievementTable";
import { AdminGamificationTabs } from "@/features/gamification/AdminGamificationTabs";
import { AdminQuestTable } from "@/features/gamification/AdminQuestTable";
import { AdminTitleTable } from "@/features/gamification/AdminTitleTable";
import { useAdminGamificationClientState } from "@/features/gamification/useAdminGamificationClientState";

interface Props {
 initialQuests: QuestDefinition[];
 initialAchievements: AchievementDefinition[];
 initialTitles: TitleDefinition[];
}

export default function AdminGamificationClient({ initialQuests, initialAchievements, initialTitles }: Props) {
 const state = useAdminGamificationClientState();
 const createType = state.activeTab === "quests" ? "Nhiệm Vụ" : state.activeTab === "achievements" ? "Thành Tựu" : "Danh Hiệu";

 return (
  <div className="p-6 max-w-7xl mx-auto space-y-6">
   <header className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4"><div><h1 className="text-3xl font-black text-white tracking-tight">Quản Trị Gamification</h1><p className="text-slate-400 mt-1">Cấu hình nhiệm vụ, thành tựu và danh hiệu hệ thống.</p></div><button type="button" onClick={() => state.handleCreate(createType)} className="flex items-center gap-2 px-5 py-2.5 bg-indigo-600 hover:bg-indigo-500 text-white rounded-xl shadow-lg shadow-indigo-500/20 transition-all font-bold"><Plus className="w-5 h-5" />Tạo Mới</button></header>
   <AdminGamificationTabs activeTab={state.activeTab} onChange={state.setActiveTab} />
   <div className="bg-slate-900/40 border border-slate-800/60 rounded-3xl backdrop-blur-md overflow-hidden shadow-2xl">
    {state.activeTab === "quests" ? <AdminQuestTable quests={initialQuests} onEdit={(code) => state.handleEdit("Quest", code)} onDelete={(code) => state.handleDelete("Quest", code)} /> : null}
    {state.activeTab === "achievements" ? <AdminAchievementTable achievements={initialAchievements} onEdit={(code) => state.handleEdit("Achievement", code)} onDelete={(code) => state.handleDelete("Achievement", code)} /> : null}
    {state.activeTab === "titles" ? <AdminTitleTable titles={initialTitles} onEdit={(code) => state.handleEdit("Title", code)} onDelete={(code) => state.handleDelete("Title", code)} /> : null}
   </div>
  </div>
 );
}
