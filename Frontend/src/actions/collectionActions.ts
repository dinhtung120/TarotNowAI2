/*
 * ===================================================================
 * FILE: collectionActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Truy xuất Bộ Sưu Tập Lá Bài (Tarot Collection) của người dùng hiện tại
 *   thông qua kênh Server-side an toàn (Next.js Actions).
 *
 * LƯU Ý CACHE:
 *   Sử dụng cấu hình `next: { revalidate: 0 }` để "Xé Áo" bộ nhớ Cache mặc định của Next.js,
 *   giúp người dùng bốc lá bài xong vào lại tủ đồ là thấy thẻ bài hiện lên ngay.
 * ===================================================================
 */
"use server";

import { getTranslations } from "next-intl/server";
import { getServerAccessToken } from "@/shared/infrastructure/auth/serverAuth";
import { serverHttpRequest } from "@/shared/infrastructure/http/serverHttpClient";
import { logger } from "@/shared/infrastructure/logging/logger";

export interface UserCollectionDto {
 cardId: number;
 level: number;
 copies: number;
 expGained: number;
 lastDrawnAt: string;
}

export async function getUserCollection() {
 const t = await getTranslations("ApiErrors");

 try {
 const token = await getServerAccessToken();

 if (!token) {
 return { success: false, error: t("unauthorized") };
 }

 const result = await serverHttpRequest<UserCollectionDto[]>("/reading/collection", {
 method: "GET",
 token,
 // Thêm cache revalidate tránh stale data
 next: { revalidate: 0 },
 fallbackErrorMessage: t("unknown_error"),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { success: false, error: t("unauthorized") };
 }
 logger.error("CollectionAction.getUserCollection", result.error, { status: result.status });
 return { success: false, error: result.error || t("unknown_error") };
 }

 return { success: true, data: result.data };
 } catch (error) {
 logger.error("CollectionAction.getUserCollection", error);
 return { success: false, error: t("network_error") };
 }
}
