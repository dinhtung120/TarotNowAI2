"use client";

import { useMemo, useState } from 'react';
import type {
 AdminAchievementDefinition,
 AdminGamificationEntityType,
 AdminGamificationTab,
 AdminQuestDefinition,
 AdminTitleDefinition,
} from '@/features/gamification/admin/adminGamification.types';
import { useAdminGamification } from '@/features/gamification/admin/application/useAdminGamification';

type AdminGamificationEditorState =
 | { entityType: AdminGamificationEntityType; mode: 'create' }
 | { entityType: AdminGamificationEntityType; mode: 'edit'; code: string }
 | null;

type AdminGamificationDeleteState =
 | { entityType: AdminGamificationEntityType; code: string }
 | null;

const EMPTY_QUESTS: AdminQuestDefinition[] = [];
const EMPTY_ACHIEVEMENTS: AdminAchievementDefinition[] = [];
const EMPTY_TITLES: AdminTitleDefinition[] = [];

export function useAdminGamificationClientState() {
 const {
  questsQuery,
  achievementsQuery,
  titlesQuery,
  upsertQuest,
  deleteQuest,
  upsertAchievement,
  deleteAchievement,
  upsertTitle,
  deleteTitle,
 } = useAdminGamification();
 const [activeTab, setActiveTab] = useState<AdminGamificationTab>('quests');
 const [editor, setEditor] = useState<AdminGamificationEditorState>(null);
 const [deleteDialog, setDeleteDialog] = useState<AdminGamificationDeleteState>(null);

 const quests = questsQuery.data ?? EMPTY_QUESTS;
 const achievements = achievementsQuery.data ?? EMPTY_ACHIEVEMENTS;
 const titles = titlesQuery.data ?? EMPTY_TITLES;

 const selectedQuest = useMemo<AdminQuestDefinition | null>(() => {
  if (editor?.entityType !== 'quest' || editor.mode !== 'edit') {
   return null;
  }

  return quests.find((quest) => quest.code === editor.code) ?? null;
 }, [editor, quests]);

 const selectedAchievement = useMemo<AdminAchievementDefinition | null>(() => {
  if (editor?.entityType !== 'achievement' || editor.mode !== 'edit') {
   return null;
  }

  return achievements.find((achievement) => achievement.code === editor.code) ?? null;
 }, [achievements, editor]);

 const selectedTitle = useMemo<AdminTitleDefinition | null>(() => {
  if (editor?.entityType !== 'title' || editor.mode !== 'edit') {
   return null;
  }

  return titles.find((title) => title.code === editor.code) ?? null;
 }, [editor, titles]);

 const handleCreate = () => {
  const entityType = activeTab === 'quests' ? 'quest' : activeTab === 'achievements' ? 'achievement' : 'title';
  setEditor({ entityType, mode: 'create' });
 };

 const handleEdit = (entityType: AdminGamificationEntityType, code: string) => {
  setEditor({ entityType, mode: 'edit', code });
 };

 const handleDelete = (entityType: AdminGamificationEntityType, code: string) => {
  setDeleteDialog({ entityType, code });
 };

 const closeDeleteDialog = () => {
  setDeleteDialog(null);
 };

 const closeEditor = () => {
  setEditor(null);
 };

 const confirmDelete = async () => {
  if (!deleteDialog) {
   return;
  }

  try {
   if (deleteDialog.entityType === 'quest') {
    await deleteQuest.mutateAsync(deleteDialog.code);
   } else if (deleteDialog.entityType === 'achievement') {
    await deleteAchievement.mutateAsync(deleteDialog.code);
   } else {
    await deleteTitle.mutateAsync(deleteDialog.code);
   }

   setDeleteDialog(null);
  } catch {
   return;
  }
 };

 const submitQuest = async (value: AdminQuestDefinition) => {
  try {
   await upsertQuest.mutateAsync(value);
   setEditor(null);
  } catch {
   return;
  }
 };

 const submitAchievement = async (value: AdminAchievementDefinition) => {
  try {
   await upsertAchievement.mutateAsync(value);
   setEditor(null);
  } catch {
   return;
  }
 };

 const submitTitle = async (value: AdminTitleDefinition) => {
  try {
   await upsertTitle.mutateAsync(value);
   setEditor(null);
  } catch {
   return;
  }
 };

 const isLoading = questsQuery.isLoading || achievementsQuery.isLoading || titlesQuery.isLoading;
 const isEditorSubmitting = upsertQuest.isPending || upsertAchievement.isPending || upsertTitle.isPending;
 const isDeletePending = deleteQuest.isPending || deleteAchievement.isPending || deleteTitle.isPending;
 const error = questsQuery.error || achievementsQuery.error || titlesQuery.error;

 return {
  activeTab,
  setActiveTab,
  quests,
  achievements,
  titles,
  isLoading,
  error: error instanceof Error ? error.message : '',
  handleCreate,
  handleEdit,
  handleDelete,
  editor,
  selectedQuest,
  selectedAchievement,
  selectedTitle,
  closeEditor,
  submitQuest,
  submitAchievement,
  submitTitle,
  deleteDialog,
  closeDeleteDialog,
  confirmDelete,
  isEditorSubmitting,
  isDeletePending,
 };
}
