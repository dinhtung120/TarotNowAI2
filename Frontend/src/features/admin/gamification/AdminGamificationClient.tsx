"use client";

import { Plus } from "lucide-react";
import { useAdminGamificationClientState } from '@/features/admin/gamification/hooks/useAdminGamificationClientState';
import { AdminAchievementTable } from '@/features/admin/gamification/components/AdminAchievementTable';
import { AdminGamificationFormModal } from '@/features/admin/gamification/components/AdminGamificationFormModal';
import { AdminGamificationTabs } from '@/features/admin/gamification/components/AdminGamificationTabs';
import { AdminQuestTable } from '@/features/admin/gamification/components/AdminQuestTable';
import { AdminTitleTable } from '@/features/admin/gamification/components/AdminTitleTable';
import { cn } from "@/lib/utils";
import ActionConfirmModal from "@/shared/ui/ActionConfirmModal";

export default function AdminGamificationClient() {
 const state = useAdminGamificationClientState();
 const dialogTitle = state.deleteDialog ? `Delete ${state.deleteDialog.entityType}` : "";
 const dialogDescription = state.deleteDialog
  ? `Delete ${state.deleteDialog.entityType} (${state.deleteDialog.code})? This action cannot be undone.`
  : "";

 return (
  <div className={cn("mx-auto", "max-w-7xl", "space-y-6", "p-6")}>
   <header className={cn("flex", "flex-row", "items-center", "justify-between", "gap-4")}>
    <div>
     <h1 className={cn("text-3xl", "font-black", "tracking-tight", "text-white")}>Quản Trị Gamification</h1>
     <p className={cn("mt-1", "text-slate-400")}>Cấu hình nhiệm vụ, thành tựu và danh hiệu hệ thống.</p>
    </div>
    <button
     type="button"
     onClick={state.handleCreate}
     className={cn("flex", "items-center", "gap-2", "rounded-xl", "bg-indigo-600", "px-5", "py-2.5", "font-bold", "text-white", "shadow-lg", "shadow-indigo-500/20", "transition-all")}
    >
     <Plus className={cn("h-5", "w-5")} />
     Tạo Mới
    </button>
   </header>
   <AdminGamificationTabs activeTab={state.activeTab} onChange={state.setActiveTab} />
   {state.error ? <p className={cn("text-sm text-rose-400")}>{state.error}</p> : null}
   <div className={cn("overflow-hidden", "rounded-3xl", "border", "border-slate-800/60", "bg-slate-900/40", "shadow-2xl", "backdrop-blur-md")}>
    {state.activeTab === "quests" ? <AdminQuestTable quests={state.quests} onEdit={(code) => state.handleEdit("quest", code)} onDelete={(code) => state.handleDelete("quest", code)} /> : null}
    {state.activeTab === "achievements" ? <AdminAchievementTable achievements={state.achievements} onEdit={(code) => state.handleEdit("achievement", code)} onDelete={(code) => state.handleDelete("achievement", code)} /> : null}
    {state.activeTab === "titles" ? <AdminTitleTable titles={state.titles} onEdit={(code) => state.handleEdit("title", code)} onDelete={(code) => state.handleDelete("title", code)} /> : null}
   </div>
   <AdminGamificationFormModal
    entityType={state.editor?.entityType ?? null}
    initialQuest={state.selectedQuest}
    initialAchievement={state.selectedAchievement}
    initialTitle={state.selectedTitle}
    open={Boolean(state.editor)}
    onClose={state.closeEditor}
    onSubmitQuest={state.submitQuest}
    onSubmitAchievement={state.submitAchievement}
    onSubmitTitle={state.submitTitle}
    submitting={state.isEditorSubmitting}
   />
   <ActionConfirmModal
    open={Boolean(state.deleteDialog)}
    icon={<Plus className={cn("h-6 w-6 text-indigo-300")} />}
    title={dialogTitle}
    description={dialogDescription}
    confirmLabel="Delete"
    cancelLabel="Cancel"
    confirmVariant="danger"
    confirmLoading={state.isDeletePending}
    onCancel={state.closeDeleteDialog}
    onConfirm={state.confirmDelete}
   />
  </div>
 );
}
