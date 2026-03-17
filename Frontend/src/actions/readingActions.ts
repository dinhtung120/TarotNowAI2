"use server";

import { cookies } from "next/headers";
import { getTranslations } from "next-intl/server";
import { API_BASE_URL } from "@/lib/api";

export interface InitReadingRequest {
 spreadType: string;
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
 const cookieStore = await cookies();
 const token = cookieStore.get("accessToken")?.value;

 if (!token) {
 return { success: false, error: t("unauthorized") };
 }

 const res = await fetch(`${API_BASE_URL}/reading/init`, {
 method: "POST",
 headers: {
 "Content-Type": "application/json",
 Authorization: `Bearer ${token}`,
 },
 body: JSON.stringify(data),
 });

 if (!res.ok) {
 const errorData = await res.json().catch(() => ({}));
 return { success: false, error: errorData.message || errorData.detail || t("unknown_error") };
 }

 const responseData: InitReadingResponse = await res.json();
 return { success: true, data: responseData };
 } catch {
 return { success: false, error: t("network_error") };
 }
}

/**
 * Hành động Lật Bài Tarot (Reveal)
 */
export async function revealReadingSession(data: RevealReadingRequest) {
 const t = await getTranslations("ApiErrors");

 try {
 const cookieStore = await cookies();
 const token = cookieStore.get("accessToken")?.value;

 if (!token) {
 return { success: false, error: t("unauthorized") };
 }

 const res = await fetch(`${API_BASE_URL}/reading/reveal`, {
 method: "POST",
 headers: {
 "Content-Type": "application/json",
 Authorization: `Bearer ${token}`,
 },
 body: JSON.stringify(data),
 });

 if (!res.ok) {
 const errorData = await res.json().catch(() => ({}));
 return { success: false, error: errorData.message || errorData.detail || t("unknown_error") };
 }

 const responseData: RevealReadingResponse = await res.json();
 return { success: true, data: responseData };
 } catch {
 return { success: false, error: t("network_error") };
 }
}
