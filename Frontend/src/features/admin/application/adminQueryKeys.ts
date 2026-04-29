export interface AdminReadingsFilterKey {
 username: string;
 spreadType: string;
 startDate: string;
 endDate: string;
}

function normalizeFilterValue(value: string | null | undefined): string {
 return (value ?? '').trim();
}

function normalizeReadingsFilterKey(filters: Partial<AdminReadingsFilterKey>): AdminReadingsFilterKey {
 return {
  username: normalizeFilterValue(filters.username),
  spreadType: normalizeFilterValue(filters.spreadType),
  startDate: normalizeFilterValue(filters.startDate),
  endDate: normalizeFilterValue(filters.endDate),
 };
}

export const adminQueryKeys = {
 all: ['admin'] as const,
 dashboardStats: () => [...adminQueryKeys.all, 'dashboard-stats'] as const,
 users: (page: number, searchTerm: string) => [...adminQueryKeys.all, 'users', page, normalizeFilterValue(searchTerm)] as const,
 usersRoot: () => [...adminQueryKeys.all, 'users'] as const,
 deposits: (page: number, statusFilter: string) =>
  [...adminQueryKeys.all, 'deposits', page, normalizeFilterValue(statusFilter)] as const,
 depositsRoot: () => [...adminQueryKeys.all, 'deposits'] as const,
 readerRequests: (page: number, statusFilter: string) =>
  [...adminQueryKeys.all, 'reader-requests', page, normalizeFilterValue(statusFilter)] as const,
 readerRequestsRoot: () => [...adminQueryKeys.all, 'reader-requests'] as const,
 readings: (page: number, filters: Partial<AdminReadingsFilterKey>) =>
  [...adminQueryKeys.all, 'readings', page, normalizeReadingsFilterKey(filters)] as const,
 withdrawalsQueue: () => [...adminQueryKeys.all, 'withdrawals', 'queue'] as const,
 withdrawalDetail: (withdrawalId: string | null) =>
  [...adminQueryKeys.all, 'withdrawals', 'detail', withdrawalId ?? 'none'] as const,
 disputesList: () => [...adminQueryKeys.all, 'disputes', 'list'] as const,
} as const;
