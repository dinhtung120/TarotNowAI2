import { cache } from 'react';
import { getServerSessionSnapshot, type ServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';

/**
 * Request-scoped snapshot cache for Server Components/layouts.
 * Prevents duplicated /profile round-trips when multiple auth gates run in the same render tree.
 */
export const getCachedServerSessionSnapshot = cache(async (): Promise<ServerSessionSnapshot> =>
 getServerSessionSnapshot({ allowRefresh: false }),
);
