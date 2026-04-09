

import { useAuthStore } from '@/store/authStore';
import { API_BASE_URL } from '@/shared/infrastructure/http/apiUrl';
import type { 
  GachaBannerDto, 
  GachaBannerOddsDto, 
  GachaHistoryItemDto, 
  SpinGachaRequestDto, 
  SpinGachaResult 
} from './gacha.types'
import { v4 as uuidv4 } from 'uuid'

async function authFetch<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = useAuthStore.getState().token;
  const url = `${API_BASE_URL}${path}`;

  const res = await fetch(url, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers || {}),
    },
  });

  if (!res.ok) {
    const errorBody = await res.text().catch(() => '');
    let message = `API error ${res.status}`;
    try {
      const parsed = JSON.parse(errorBody);
      message = parsed.detail || parsed.title || parsed.message || message;
    } catch {}
    throw new Error(message);
  }

  if (res.status === 204) {
      return {} as T;
  }
  
  return res.json();
}

export const gachaApi = {
  
  async getBanners(): Promise<GachaBannerDto[]> {
    return authFetch('/gacha/banners')
  },

  
  async getBannerOdds(bannerCode: string): Promise<GachaBannerOddsDto> {
    return authFetch(`/gacha/banners/${bannerCode}/odds`)
  },

  
  async getHistory(limit: number = 50): Promise<GachaHistoryItemDto[]> {
    return authFetch(`/gacha/history?limit=${limit}`)
  },

  
  async spin(data: SpinGachaRequestDto): Promise<SpinGachaResult> {
    const idempotencyKey = uuidv4()
    return authFetch('/gacha/spin', {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'X-Idempotency-Key': idempotencyKey
      }
    })
  }
}
