import { beforeEach, describe, expect, it, vi } from 'vitest';
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
} from '@/features/gamification/admin/application/adminGamificationClient';
import {
 fetchJsonOrThrow,
 fetchWithTimeout,
} from '@/shared/application/gateways/clientFetch';

vi.mock('@/shared/application/gateways/clientFetch', () => ({
 fetchJsonOrThrow: vi.fn(),
 fetchWithTimeout: vi.fn(),
}));

const mockedFetchJsonOrThrow = vi.mocked(fetchJsonOrThrow);
const mockedFetchWithTimeout = vi.mocked(fetchWithTimeout);

describe('adminGamificationClient', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('uses the local admin BFF for list and upsert operations', async () => {
  mockedFetchJsonOrThrow.mockResolvedValue([] as never);

  await listAdminQuests();
  await listAdminAchievements();
  await listAdminTitles();
  await upsertAdminQuest({ code: 'Q1' } as never);
  await upsertAdminAchievement({ code: 'A1' } as never);
  await upsertAdminTitle({ code: 'T1' } as never);

  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith('/api/admin/gamification/quests', expect.any(Object), 'Failed to load admin quests.', 8_000);
  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith('/api/admin/gamification/achievements', expect.any(Object), 'Failed to load admin achievements.', 8_000);
  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith('/api/admin/gamification/titles', expect.any(Object), 'Failed to load admin titles.', 8_000);
  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith('/api/admin/gamification/quests', expect.objectContaining({ method: 'POST' }), 'Failed to save admin quest.', 8_000);
  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith('/api/admin/gamification/achievements', expect.objectContaining({ method: 'POST' }), 'Failed to save admin achievement.', 8_000);
  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith('/api/admin/gamification/titles', expect.objectContaining({ method: 'POST' }), 'Failed to save admin title.', 8_000);
 });

 it('throws when a delete request is rejected by the BFF', async () => {
  mockedFetchWithTimeout.mockResolvedValueOnce(new Response(null, { status: 500 }));
  mockedFetchWithTimeout.mockResolvedValueOnce(new Response(null, { status: 204 }));
  mockedFetchWithTimeout.mockResolvedValueOnce(new Response(null, { status: 204 }));

  await expect(deleteAdminQuest('quest/1')).rejects.toThrow('Failed to delete admin gamification entry.');
  await expect(deleteAdminAchievement('achievement-1')).resolves.toBeUndefined();
  await expect(deleteAdminTitle('title-1')).resolves.toBeUndefined();

  expect(mockedFetchWithTimeout).toHaveBeenCalledWith('/api/admin/gamification/quests/quest%2F1', expect.objectContaining({ method: 'DELETE' }), 8_000);
 });
});
