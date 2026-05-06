export const HISTORY_PAGE_SIZE = 10;

export function historySessionsListQueryKey(
 page: number,
 filterType: string,
 filterDate: string
): readonly ['reading', 'history', 'sessions', number, number, string, string] {
 return ['reading', 'history', 'sessions', page, HISTORY_PAGE_SIZE, filterType, filterDate];
}

export function historyDetailQueryKey(sessionId: string): readonly ['reading', 'history', 'detail', string] {
 return ['reading', 'history', 'detail', sessionId];
}
