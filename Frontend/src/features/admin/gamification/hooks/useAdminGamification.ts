'use client';

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { ADMIN_QUERY_POLICY } from '@/features/admin/shared/adminQueryPolicy';
import {
 deleteAdminAchievement,
 deleteAdminQuest,
 deleteAdminTitle,
 listAdminAchievements,
 listAdminQuests,
 listAdminTitles,
 upsertAdminAchievement,
 upsertAdminQuest,
 upsertAdminTitle,
} from '@/features/admin/gamification/actions/adminGamificationClient';
import { adminGamificationKeys } from '@/features/admin/gamification/adminGamificationKeys';

export function useAdminGamification() {
 const queryClient = useQueryClient();

 const questsQuery = useQuery({
  queryKey: adminGamificationKeys.quests(),
  queryFn: listAdminQuests,
  ...ADMIN_QUERY_POLICY.list,
 });

 const achievementsQuery = useQuery({
  queryKey: adminGamificationKeys.achievements(),
  queryFn: listAdminAchievements,
  ...ADMIN_QUERY_POLICY.list,
 });

 const titlesQuery = useQuery({
  queryKey: adminGamificationKeys.titles(),
  queryFn: listAdminTitles,
  ...ADMIN_QUERY_POLICY.list,
 });

 const upsertQuest = useMutation({
  mutationFn: upsertAdminQuest,
  onSuccess: async () => {
   toast.success('Luu nhiem vu thanh cong.');
   await queryClient.invalidateQueries({ queryKey: adminGamificationKeys.quests() });
  },
  onError: (error: Error) => toast.error(error.message),
 });

 const deleteQuest = useMutation({
  mutationFn: deleteAdminQuest,
  onSuccess: async () => {
   toast.success('Xoa nhiem vu thanh cong.');
   await queryClient.invalidateQueries({ queryKey: adminGamificationKeys.quests() });
  },
  onError: (error: Error) => toast.error(error.message),
 });

 const upsertAchievement = useMutation({
  mutationFn: upsertAdminAchievement,
  onSuccess: async () => {
   toast.success('Luu thanh tuu thanh cong.');
   await queryClient.invalidateQueries({ queryKey: adminGamificationKeys.achievements() });
  },
  onError: (error: Error) => toast.error(error.message),
 });

 const deleteAchievement = useMutation({
  mutationFn: deleteAdminAchievement,
  onSuccess: async () => {
   toast.success('Xoa thanh tuu thanh cong.');
   await queryClient.invalidateQueries({ queryKey: adminGamificationKeys.achievements() });
  },
  onError: (error: Error) => toast.error(error.message),
 });

 const upsertTitle = useMutation({
  mutationFn: upsertAdminTitle,
  onSuccess: async () => {
   toast.success('Luu danh hieu thanh cong.');
   await queryClient.invalidateQueries({ queryKey: adminGamificationKeys.titles() });
  },
  onError: (error: Error) => toast.error(error.message),
 });

 const deleteTitle = useMutation({
  mutationFn: deleteAdminTitle,
  onSuccess: async () => {
   toast.success('Xoa danh hieu thanh cong.');
   await queryClient.invalidateQueries({ queryKey: adminGamificationKeys.titles() });
  },
  onError: (error: Error) => toast.error(error.message),
 });

 return {
  questsQuery,
  achievementsQuery,
  titlesQuery,
  upsertQuest,
  deleteQuest,
  upsertAchievement,
  deleteAchievement,
  upsertTitle,
  deleteTitle,
 };
}
