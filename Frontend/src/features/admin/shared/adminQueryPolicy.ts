export const ADMIN_QUERY_POLICY = {
 dashboard: {
  staleTime: 120_000,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  retry: 1,
 },
 list: {
  staleTime: 90_000,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  retry: 1,
 },
 heavyList: {
  staleTime: 120_000,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  retry: 0,
 },
 detail: {
  staleTime: 60_000,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  retry: 1,
 },
} as const;
