"use client";

import { Plus } from "lucide-react";
import type { AchievementDefinition, QuestDefinition, TitleDefinition } from "@/features/gamification/gamification.types";
import { AdminAchievementTable } from "@/features/gamification/AdminAchievementTable";
import { AdminGamificationTabs } from "@/features/gamification/AdminGamificationTabs";
import { AdminQuestTable } from "@/features/gamification/AdminQuestTable";
import { AdminTitleTable } from "@/features/gamification/AdminTitleTable";
import { useAdminGamificationClientState } from "@/features/gamification/useAdminGamificationClientState";
import { cn } from "@/lib/utils";
import ActionConfirmModal from "@/shared/components/ui/ActionConfirmModal";

interface Props {
 initialQuests: QuestDefinition[];
 initialAchievements: AchievementDefinition[];
 initialTitles: TitleDefinition[];
}

export default function AdminGamificationClient({ initialQuests, initialAchievements, initialTitles }: Props) {
 const state = useAdminGamificationClientState();
 const createType = state.activeTab === "quests" ? "Nhiệm Vụ" : state.activeTab === "achievements" ? "Thành Tựu" : "Danh Hiệu";
 const dialogTitle = state.dialog?.mode === "delete"
  ? `Xóa ${state.dialog.type}`
  : state.dialog?.mode === "edit"
   ? `Sửa ${state.dialog.type}`
   : state.dialog?.mode === "create"
    ? `Tạo ${state.dialog.type}`
    : "";
 const dialogDescription = state.dialog?.mode === "delete"
  ? `Bạn có chắc muốn xóa ${state.dialog.type} (${state.dialog.code})? Hành động này không thể hoàn tác.`
  : state.dialog?.mode === "edit"
   ? `Mở flow chỉnh sửa cho ${state.dialog.type} (${state.dialog.code})?`
   : state.dialog?.mode === "create"
    ? `Mở flow tạo mới cho ${state.dialog.type}?`
    : "";
 const dialogConfirmLabel = state.dialog?.mode === "delete" ? "Xóa" : "Xác nhận";

 return (
  <div className={cn("mx-auto", "max-w-7xl", "space-y-6", "p-6")}>
   <header className={cn("flex", "flex-row", "items-center", "justify-between", "gap-4")}>
    <div>
     <h1 className={cn("text-3xl", "font-black", "tracking-tight", "text-white")}>Quản Trị Gamification</h1>
     <p className={cn("mt-1", "text-slate-400")}>Cấu hình nhiệm vụ, thành tựu và danh hiệu hệ thống.</p>
    </div>
    <button
     type="button"
     onClick={() => state.handleCreate(createType)}
     className={cn("flex", "items-center", "gap-2", "rounded-xl", "bg-indigo-600", "px-5", "py-2.5", "font-bold", "text-white", "shadow-lg", "shadow-indigo-500/20", "transition-all")}
    >
     <Plus className={cn("h-5", "w-5")} />
     Tạo Mới
    </button>
   </header>
   <AdminGamificationTabs activeTab={state.activeTab} onChange={state.setActiveTab} />
   <div className={cn("overflow-hidden", "rounded-3xl", "border", "border-slate-800/60", "bg-slate-900/40", "shadow-2xl", "backdrop-blur-md")}>
    {state.activeTab === "quests" ? <AdminQuestTable quests={initialQuests} onEdit={(code) => state.handleEdit("Quest", code)} onDelete={(code) => state.handleDelete("Quest", code)} /> : null}
    {state.activeTab === "achievements" ? <AdminAchievementTable achievements={initialAchievements} onEdit={(code) => state.handleEdit("Achievement", code)} onDelete={(code) => state.handleDelete("Achievement", code)} /> : null}
    {state.activeTab === "titles" ? <AdminTitleTable titles={initialTitles} onEdit={(code) => state.handleEdit("Title", code)} onDelete={(code) => state.handleDelete("Title", code)} /> : null}
   </div>
   <ActionConfirmModal
    open={Boolean(state.dialog)}
    icon={<Plus className={cn("h-6 w-6 text-indigo-300")} />}
    title={dialogTitle}
    description={dialogDescription}
    confirmLabel={dialogConfirmLabel}
    cancelLabel="Hủy"
    confirmVariant={state.dialog?.mode === "delete" ? "danger" : "primary"}
    confirmLoading={state.isDeletePending}
    onCancel={state.closeDialog}
    onConfirm={state.confirmDialog}
   />
  </div>
 );
}
