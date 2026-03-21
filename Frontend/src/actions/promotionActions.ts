/*
 * ===================================================================
 * FILE: promotionActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao tiếp với API Quản trị Khuyến Mãi (Promotions).
 *   Hỗ trợ Admin CRUD các gói khuyến mãi nạp Kim Cương.
 * ===================================================================
 */
'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

// ======================================================================
// Kiểu dữ liệu cho Promotions
// ======================================================================

/** Chương trình khuyến mãi nạp tiền */
export interface DepositPromotion {
 id: string;
 minAmountVnd: number;
 bonusDiamond: number;
 isActive: boolean;
 createdAt: string;
 updatedAt?: string;
}

// ======================================================================
// Actions — Admin Promotions APIs
// CRUD đầy đủ cho quản lý chương trình khuyến mãi nạp tiền.
// Backend route: /api/v1/admin/promotions
// ======================================================================

/**
 * Lấy danh sách tất cả chương trình khuyến mãi.
 * Endpoint AllowAnonymous nên có thể gọi từ phía client lẫn admin.
 * * @param onlyActive - true = chỉ lấy promo đang hoạt động
 */
export async function listPromotions(onlyActive = false): Promise<DepositPromotion[] | null> {
 const accessToken = await getServerAccessToken();

 try {
 const result = await serverHttpRequest<DepositPromotion[]>(`/admin/promotions?onlyActive=${onlyActive}`, {
 method: 'GET',
 token: accessToken,
 fallbackErrorMessage: 'Failed to list promotions',
 });

 if (!result.ok) {
 logger.error('PromotionAction.listPromotions', result.error, {
 status: result.status,
 onlyActive,
 });
 return null;
 }

 return result.data;
 } catch (error) {
 logger.error('PromotionAction.listPromotions', error, { onlyActive });
 return null;
 }
}

/**
 * Tạo chương trình khuyến mãi mới (Admin only).
 * * @param minAmountVnd - Số tiền tối thiểu để kích hoạt khuyến mãi
 * @param bonusDiamond - Số Diamond thưởng thêm
 */
export async function createPromotion(minAmountVnd: number, bonusDiamond: number): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>('/admin/promotions', {
 method: 'POST',
 token: accessToken,
 json: { minAmountVnd, bonusDiamond },
 fallbackErrorMessage: 'Failed to create promotion',
 });

 if (!result.ok) {
 logger.error('PromotionAction.createPromotion', result.error, {
 status: result.status,
 minAmountVnd,
 bonusDiamond,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('PromotionAction.createPromotion', error, { minAmountVnd, bonusDiamond });
 return false;
 }
}

/**
 * Cập nhật chương trình khuyến mãi (Admin only).
 * * @param id - ID promotion cần cập nhật
 * @param data - Dữ liệu mới (minAmountVnd, bonusDiamond, isActive)
 */
export async function updatePromotion(
 id: string, data: { minAmountVnd: number; bonusDiamond: number; isActive: boolean }
): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>(`/admin/promotions/${id}`, {
 method: 'PUT',
 token: accessToken,
 json: data,
 fallbackErrorMessage: 'Failed to update promotion',
 });

 if (!result.ok) {
 logger.error('PromotionAction.updatePromotion', result.error, {
 status: result.status,
 promotionId: id,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('PromotionAction.updatePromotion', error, { promotionId: id });
 return false;
 }
}

/**
 * Xóa chương trình khuyến mãi (Admin only).
 * * @param id - ID promotion cần xóa
 */
export async function deletePromotion(id: string): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

 try {
 const result = await serverHttpRequest<unknown>(`/admin/promotions/${id}`, {
 method: 'DELETE',
 token: accessToken,
 fallbackErrorMessage: 'Failed to delete promotion',
 });

 if (!result.ok) {
 logger.error('PromotionAction.deletePromotion', result.error, {
 status: result.status,
 promotionId: id,
 });
 return false;
 }
 return true;
 } catch (error) {
 logger.error('PromotionAction.deletePromotion', error, { promotionId: id });
 return false;
 }
}
