/**
 * /src/features/gacha/hooks/useGacha.ts
 * React Query hooks for Gacha feature
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import type { SpinGachaRequestDto } from '../gacha.types'
import { 
  getGachaBanners, 
  getGachaOdds, 
  getGachaHistory, 
  spinGacha 
} from '../application/actions'

export const GACHA_QUERY_KEYS = {
  banners: ['gacha', 'banners'] as const,
  odds: (bannerCode: string) => ['gacha', 'odds', bannerCode] as const,
  history: (limit: number) => ['gacha', 'history', limit] as const,
}

export function useGachaBanners() {
  return useQuery({
    queryKey: GACHA_QUERY_KEYS.banners,
    queryFn: async () => {
      const result = await getGachaBanners();
      if (!result.success) throw new Error(result.error);
      return result.data;
    },
  })
}

export function useGachaOdds(bannerCode: string, enabled: boolean = true) {
  return useQuery({
    queryKey: GACHA_QUERY_KEYS.odds(bannerCode),
    queryFn: async () => {
      const result = await getGachaOdds(bannerCode);
      if (!result.success) throw new Error(result.error);
      return result.data;
    },
    enabled: enabled && !!bannerCode,
  })
}

export function useGachaHistory(limit: number = 50, enabled: boolean = true) {
  return useQuery({
    queryKey: GACHA_QUERY_KEYS.history(limit),
    queryFn: async () => {
      const result = await getGachaHistory(limit);
      if (!result.success) throw new Error(result.error);
      return result.data;
    },
    enabled: enabled,
  })
}

import { useWalletStore } from '@/store/walletStore'

export function useSpinGacha() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: async (data: SpinGachaRequestDto) => {
      const result = await spinGacha(data);
      if (!result.success) throw new Error(result.error);
      return result.data;
    },
    onSuccess: () => {
      // Invalidate history to fetch new pulls
      queryClient.invalidateQueries({ queryKey: ['gacha', 'history'] })
      // Invalidate wallet balance for Query cache
      queryClient.invalidateQueries({ queryKey: ['wallet'] })
      // Directly trigger store fetch for immediate feedback across the app
      useWalletStore.getState().fetchBalance();
    },
  })
}

