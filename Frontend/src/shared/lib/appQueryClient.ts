import { QueryClient } from '@tanstack/react-query';

export function createAppQueryClient(): QueryClient {
 return new QueryClient({
  defaultOptions: {
   queries: {
    staleTime: 15_000,
    gcTime: 10 * 60 * 1000,
    refetchOnWindowFocus: false,
    refetchOnReconnect: true,
    refetchOnMount: false,
    retry: 1,
   },
  },
 });
}
