/*
 * ===================================================================
 * FILE: readingActions.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Điều phối logic cốt lõi của việc Rút Bài Tarot (Tarot Reading).
 *   - initReadingSession: Trừ tiền (Gold/Diamond) và tạo Cột mốc.
 *   - revealReadingSession: Kích hoạt thuật toán random thẻ bài.
 * ===================================================================
 */
"use server";

import { getTranslations } from "next-intl/server";
import { getServerAccessToken } from "@/shared/infrastructure/auth/serverAuth";
import { serverHttpRequest } from "@/shared/infrastructure/http/serverHttpClient";
import { logger } from "@/shared/infrastructure/logging/logger";

export interface InitReadingRequest {
 spreadType: string;
 question?: string;
 currency?: string;
}

export interface InitReadingResponse {
 sessionId: string;
 costGold: number;
 costDiamond: number;
}

export interface RevealReadingRequest {
 sessionId: string;
}

export interface RevealReadingResponse {
 cards: number[];
}

/**
 * Hành động khởi tạo Phiên Rút Bài Tarot
 * (Gắn liền với logic Trừ Gold/Diamond từ Wallet qua Re-rendering)
 */
export async function initReadingSession(data: InitReadingRequest) {
 const t = await getTranslations("ApiErrors");

 try {
 const token = await getServerAccessToken();

 if (!token) {
 return { success: false, error: t("unauthorized") };
 }

 const result = await serverHttpRequest<InitReadingResponse>("/reading/init", {
 method: "POST",
 token,
 json: data,
 fallbackErrorMessage: t("unknown_error"),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { success: false, error: t("unauthorized") };
 }
 logger.error("ReadingAction.initReadingSession", result.error, { status: result.status });
 return { success: false, error: result.error || t("unknown_error") };
 }

 return { success: true, data: result.data };
 } catch (error) {
 logger.error("ReadingAction.initReadingSession", error);
 return { success: false, error: t("network_error") };
 }
}

/**
 * Hành động Lật Bài Tarot (Reveal)
 */
export async function revealReadingSession(data: RevealReadingRequest) {
 const t = await getTranslations("ApiErrors");

 try {
 const token = await getServerAccessToken();

 if (!token) {
 return { success: false, error: t("unauthorized") };
 }

 const result = await serverHttpRequest<RevealReadingResponse>("/reading/reveal", {
 method: "POST",
 token,
 json: data,
 fallbackErrorMessage: t("unknown_error"),
 });

 if (!result.ok) {
 if (result.status === 401) {
 return { success: false, error: t("unauthorized") };
 }
 logger.error("ReadingAction.revealReadingSession", result.error, { status: result.status });
 return { success: false, error: result.error || t("unknown_error") };
 }

 return { success: true, data: result.data };
 } catch (error) {
 logger.error("ReadingAction.revealReadingSession", error);
 return { success: false, error: t("network_error") };
 }
}
