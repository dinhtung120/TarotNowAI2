import type { QueryClient } from '@tanstack/react-query';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

export const READERS_DIRECTORY_PAGE_SIZE = RUNTIME_POLICY_FALLBACKS.ui.readers.directoryPageSize;
export const CHAT_INBOX_PREFETCH_STALE_MS = RUNTIME_POLICY_FALLBACKS.ui.prefetch.chatInboxStaleMs;
export const readersDirectoryQueryKey = ['readers', 1, READERS_DIRECTORY_PAGE_SIZE, '', '', ''] as const;

export async function swallowPrefetch(run: () => Promise<void>): Promise<void> {
 try {
  await run();
 } catch {
  /* Prefetch server-side là best-effort nếu thiếu session hoặc API lỗi. */
 }
}

export type PrefetchQueryClient = QueryClient;
