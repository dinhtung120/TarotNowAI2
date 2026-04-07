'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { 
  GachaBannerDto, 
  GachaBannerOddsDto, 
  GachaHistoryItemDto, 
  SpinGachaRequestDto, 
  SpinGachaResult 
} from '../gacha.types';
import { v4 as uuidv4 } from 'uuid';

export async function getGachaBanners(): Promise<ActionResult<GachaBannerDto[]>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<GachaBannerDto[]>('/gacha/banners', {
      method: 'GET',
      token: accessToken,
      fallbackErrorMessage: 'Không thể lấy danh sách vòng quay',
    });

    if (!result.ok) {
      logger.error('GachaAction.getGachaBanners', result.error, { status: result.status });
      return actionFail(result.error || 'Lỗi khi lấy danh sách vòng quay');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('GachaAction.getGachaBanners', error);
    return actionFail('Lỗi kết nối máy chủ gacha');
  }
}

export async function getGachaOdds(bannerCode: string): Promise<ActionResult<GachaBannerOddsDto>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<GachaBannerOddsDto>(`/gacha/banners/${bannerCode}/odds`, {
      method: 'GET',
      token: accessToken,
      fallbackErrorMessage: 'Không thể lấy tỷ lệ vòng quay',
    });

    if (!result.ok) {
      logger.error('GachaAction.getGachaOdds', result.error, { status: result.status });
      return actionFail(result.error || 'Lỗi khi lấy tỷ lệ vòng quay');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('GachaAction.getGachaOdds', error);
    return actionFail('Lỗi khi truy vấn tỷ lệ vật phẩm');
  }
}

/**
 * Lấy lịch sử quay gacha của người dùng hiện tại.
 */
export async function getGachaHistory(limit: number = 50): Promise<ActionResult<GachaHistoryItemDto[]>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<GachaHistoryItemDto[]>(`/gacha/history?limit=${limit}`, {
      method: 'GET',
      token: accessToken,
      fallbackErrorMessage: 'Không thể lấy lịch sử vòng quay',
    });

    if (!result.ok) {
      logger.error('GachaAction.getGachaHistory', result.error, { status: result.status });
      return actionFail(result.error || 'Lỗi khi lấy lịch sử vòng quay');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('GachaAction.getGachaHistory', error);
    return actionFail('Lỗi khi truy xuất lịch sử');
  }
}

/**
 * Thực hiện quay gacha. Sử dụng Idempotency Key để chống lặp yêu cầu khi mạng lỗi.
 */
export async function spinGacha(data: SpinGachaRequestDto): Promise<ActionResult<SpinGachaResult>> {
  const accessToken = await getServerAccessToken();
  if (!accessToken) return actionFail('Unauthorized');

  const idempotencyKey = uuidv4();

  try {
    const result = await serverHttpRequest<SpinGachaResult>('/gacha/spin', {
      method: 'POST',
      token: accessToken,
      json: data,
      headers: {
        'X-Idempotency-Key': idempotencyKey
      },
      fallbackErrorMessage: 'Không thể thực hiện vòng quay',
    });

    if (!result.ok) {
      logger.error('GachaAction.spinGacha', result.error, { status: result.status });
      return actionFail(result.error || 'Vòng quay thất bại');
    }

    return actionOk(result.data);
  } catch (error) {
    logger.error('GachaAction.spinGacha', error);
    return actionFail('Lỗi hệ thống khi quay gacha');
  }
}
