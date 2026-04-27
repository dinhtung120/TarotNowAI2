"use client";

import { useState } from "react";
import toast from "react-hot-toast";
import { useAdminGamification } from "@/features/gamification/useAdminGamification";

type AdminGamificationDialogState =
 | { mode: "create"; type: string }
 | { mode: "edit"; type: string; code: string }
 | { mode: "delete"; type: "Quest" | "Achievement" | "Title"; code: string }
 | null;

export function useAdminGamificationClientState() {
 const { deleteQuest, deleteAchievement, deleteTitle } = useAdminGamification();
 const [activeTab, setActiveTab] = useState<"quests" | "achievements" | "titles">("quests");
 const [dialog, setDialog] = useState<AdminGamificationDialogState>(null);

 const handleCreate = (type: string) => {
  setDialog({ mode: "create", type });
 };

 const handleEdit = (type: string, code: string) => {
  setDialog({ mode: "edit", type, code });
 };

 const handleDelete = (type: "Quest" | "Achievement" | "Title", code: string) => {
  setDialog({ mode: "delete", type, code });
 };

 const closeDialog = () => {
  setDialog(null);
 };

 const confirmDialog = () => {
  if (!dialog) {
   return;
  }

  if (dialog.mode === "delete") {
   if (dialog.type === "Quest") {
    deleteQuest.mutate(dialog.code);
   }
   if (dialog.type === "Achievement") {
    deleteAchievement.mutate(dialog.code);
   }
   if (dialog.type === "Title") {
    deleteTitle.mutate(dialog.code);
   }
  } else if (dialog.mode === "create") {
   toast(`Flow tạo ${dialog.type} sẽ được bật ở wave tiếp theo.`);
  } else {
   toast(`Flow sửa ${dialog.type}: ${dialog.code} sẽ được bật ở wave tiếp theo.`);
  }

  setDialog(null);
 };

 const isDeletePending = dialog?.mode === "delete"
  && (
   (dialog.type === "Quest" && deleteQuest.isPending)
   || (dialog.type === "Achievement" && deleteAchievement.isPending)
   || (dialog.type === "Title" && deleteTitle.isPending)
  );

 return {
  activeTab,
  setActiveTab,
  handleCreate,
  handleEdit,
  handleDelete,
  dialog,
  closeDialog,
  confirmDialog,
  isDeletePending,
 };
}
