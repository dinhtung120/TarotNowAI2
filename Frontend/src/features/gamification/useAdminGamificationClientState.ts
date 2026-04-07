"use client";

import { useState } from "react";
import { useAdminGamification } from "@/features/gamification/useAdminGamification";

export function useAdminGamificationClientState() {
 const { deleteQuest, deleteAchievement, deleteTitle } = useAdminGamification();
 const [activeTab, setActiveTab] = useState<"quests" | "achievements" | "titles">("quests");

 const handleCreate = (type: string) => {
  alert(`Mở Modal tạo ${type}. Tính năng CRUD đầy đủ đang được triển khai...`);
 };

 const handleEdit = (type: string, code: string) => {
  alert(`Mở Modal sửa ${type}: ${code}`);
 };

 const handleDelete = (type: string, code: string) => {
  if (!confirm(`Bạn có chắc chắn muốn xóa ${type} này không? (${code})`)) return;
  if (type === "Quest") deleteQuest.mutate(code);
  if (type === "Achievement") deleteAchievement.mutate(code);
  if (type === "Title") deleteTitle.mutate(code);
 };

 return { activeTab, setActiveTab, handleCreate, handleEdit, handleDelete };
}
