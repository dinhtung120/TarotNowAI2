'use client';

/* 
 * Import các hooks tuần tự để quản lý trạng thái fetch hoạt họa Lottie.
 * - useMemo: Tính toán độ hiếm cao nhất từ danh sách phần thưởng.
 * - useQuery: Thực hiện tải file JSON hoạt họa một cách bất đồng bộ.
 */
import { useMemo } from 'react';
import { useQuery } from '@tanstack/react-query';
import type { PullGachaReward } from '@/shared/infrastructure/gacha/gachaTypes';

/**
 * Bản đồ ưu tiên và ánh xạ độ hiếm.
 * Hỗ trợ cả định dạng chuỗi (Legendary) và định dạng số (5 sao) thường thấy từ Backend.
 */
const rarityMap: Record<string, { priority: number; key: string }> = {
  'mythic':    { priority: 4, key: 'mythic' },
  'legendary': { priority: 3, key: 'legendary' },
  '5':         { priority: 3, key: 'legendary' }, // 5 sao tương đương Legendary
  'epic':      { priority: 2, key: 'epic' },
  '4':         { priority: 2, key: 'epic' },      // 4 sao tương đương Epic
  'rare':      { priority: 1, key: 'rare' },
  '3':         { priority: 1, key: 'rare' },
};

/**
 * Đường dẫn mẫu đến các tệp hoạt họa Lottie trong thư mục public.
 */
const rarityLottiePaths: Record<string, string> = {
  mythic: '/lottie/mythic-drop.json',
  legendary: '/lottie/legendary-drop.json',
  epic: '/lottie/epic-drop.json',
};

/**
 * Chuẩn hóa giá trị độ hiếm để đảm bảo so sánh chính xác.
 */
function normalizeValue(val: string | number | undefined | null): string {
  if (val === undefined || val === null) return '';
  return String(val).trim().toLowerCase();
}

/**
 * useRareDropLottie - Hook tùy chỉnh để xác định và tải hoạt họa mở thưởng 
 * dựa trên vật phẩm hiếm nhất trong kết quả quay Gacha.
 */
export function useRareDropLottie(rewards: PullGachaReward[]) {
  
  /* Xác định URL của hoạt họa phù hợp nhất */
  const animationUrl = useMemo(() => {
    if (!rewards.length) return '';

    /* Tìm kiếm độ hiếm cao nhất dựa trên bản đồ ưu tiên */
    let highestRarityKey = '';
    let highestPriority = -1;

    for (const reward of rewards) {
      const normalizedRarity = normalizeValue(reward.rarity);
      const config = rarityMap[normalizedRarity];
      
      if (config && config.priority > highestPriority) {
        highestPriority = config.priority;
        highestRarityKey = config.key;
      }
    }

    return highestRarityKey ? rarityLottiePaths[highestRarityKey] : '';
  }, [rewards]);

  /* Thực hiện tải dữ liệu JSON của hoạt họa Lottie */
  const animationQuery = useQuery({
    queryKey: ['gacha', 'lottie', animationUrl],
    queryFn: async () => {
      if (!animationUrl) return null;
      
      try {
        const response = await fetch(animationUrl, { cache: 'force-cache' });
        if (!response.ok) return null;
        return (await response.json()) as object;
      } catch (err) {
        console.error('Lỗi khi tải Lottie animation:', err);
        return null;
      }
    },
    enabled: Boolean(animationUrl),
    staleTime: 1000 * 60 * 30, // Lưu trữ trong cache 30 phút để tái sử dụng
  });

  return {
    animationData: animationQuery.data ?? null,
    hasRareDrop: Boolean(animationUrl),
    isLoading: animationQuery.isLoading
  };
}
