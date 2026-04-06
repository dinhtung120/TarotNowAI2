import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { getTranslations } from 'next-intl/server';
import AdminGamificationClient from '@/features/gamification/AdminGamificationClient';
import type { QuestDefinition, AchievementDefinition, TitleDefinition } from '@/features/gamification/gamification.types';

export async function generateMetadata() {
  return {
    title: 'Quản Trị Gamification - TarotNow Admin',
  };
}

export default async function AdminGamificationPage() {
  const token = await getServerAccessToken();
  
  // Batch fetch data on server
  const [questsRes, achievementsRes, titlesRes] = await Promise.all([
    serverHttpRequest<QuestDefinition[]>('/admin/gamification/quests', { token, next: { revalidate: 0 } }),
    serverHttpRequest<AchievementDefinition[]>('/admin/gamification/achievements', { token, next: { revalidate: 0 } }),
    serverHttpRequest<TitleDefinition[]>('/admin/gamification/titles', { token, next: { revalidate: 0 } }),
  ]);

  const quests = questsRes.ok ? questsRes.data || [] : [];
  const achievements = achievementsRes.ok ? achievementsRes.data || [] : [];
  const titles = titlesRes.ok ? titlesRes.data || [] : [];

  return (
    <AdminGamificationClient 
      initialQuests={quests}
      initialAchievements={achievements}
      initialTitles={titles}
    />
  );
}
