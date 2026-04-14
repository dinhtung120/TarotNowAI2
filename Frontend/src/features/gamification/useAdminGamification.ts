'use client';

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { toast } from 'react-hot-toast';
import { getApiBaseUrl } from '@/shared/infrastructure/http/apiUrl';
import type { QuestDefinition, AchievementDefinition, TitleDefinition } from './gamification.types';



async function adminFetch(path: string, options: RequestInit = {}) {
  const adminApiBase = `${getApiBaseUrl()}/admin/gamification`;
  const res = await fetch(`${adminApiBase}${path}`, {
    ...options,
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
      ...options.headers,
    },
  });

  if (!res.ok) {
    const error = await res.json().catch(() => ({ message: 'Admin API error' }));
    throw new Error(error.message || 'Error occurred');
  }

  return res.json().catch(() => null);
}

export function useAdminGamification() {
  const queryClient = useQueryClient();

  const upsertQuest = useMutation({
    mutationFn: (quest: QuestDefinition) => 
      adminFetch('/quests', { method: 'POST', body: JSON.stringify(quest) }),
    onSuccess: () => {
      toast.success('Lưu Nhiệm vụ thành công!');
      queryClient.invalidateQueries({ queryKey: ['admin', 'quests'] });
    },
    onError: (err: Error) => toast.error(`Lỗi: ${err.message}`),
  });

  const deleteQuest = useMutation({
    mutationFn: (code: string) => 
      adminFetch(`/quests/${code}`, { method: 'DELETE' }),
    onSuccess: () => {
      toast.success('Xóa Nhiệm vụ thành công!');
      queryClient.invalidateQueries({ queryKey: ['admin', 'quests'] });
    },
    onError: (err: Error) => toast.error(`Lỗi: ${err.message}`),
  });

  const upsertAchievement = useMutation({
    mutationFn: (ach: AchievementDefinition) =>
      adminFetch('/achievements', { method: 'POST', body: JSON.stringify(ach) }),
    onSuccess: () => {
      toast.success('Lưu Thành tựu thành công!');
      queryClient.invalidateQueries({ queryKey: ['admin', 'achievements'] });
    },
    onError: (err: Error) => toast.error(`Lỗi: ${err.message}`),
  });

  const deleteAchievement = useMutation({
    mutationFn: (code: string) =>
      adminFetch(`/achievements/${code}`, { method: 'DELETE' }),
    onSuccess: () => {
      toast.success('Xóa Thành tựu thành công!');
      queryClient.invalidateQueries({ queryKey: ['admin', 'achievements'] });
    },
    onError: (err: Error) => toast.error(`Lỗi: ${err.message}`),
  });

  const upsertTitle = useMutation({
    mutationFn: (title: TitleDefinition) =>
      adminFetch('/titles', { method: 'POST', body: JSON.stringify(title) }),
    onSuccess: () => {
      toast.success('Lưu Danh hiệu thành công!');
      queryClient.invalidateQueries({ queryKey: ['admin', 'titles'] });
    },
    onError: (err: Error) => toast.error(`Lỗi: ${err.message}`),
  });

  const deleteTitle = useMutation({
    mutationFn: (code: string) =>
      adminFetch(`/titles/${code}`, { method: 'DELETE' }),
    onSuccess: () => {
      toast.success('Xóa Danh hiệu thành công!');
      queryClient.invalidateQueries({ queryKey: ['admin', 'titles'] });
    },
    onError: (err: Error) => toast.error(`Lỗi: ${err.message}`),
  });

  return {
    upsertQuest, deleteQuest,
    upsertAchievement, deleteAchievement,
    upsertTitle, deleteTitle,
  };
}
