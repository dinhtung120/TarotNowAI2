"use server";

import { cookies } from "next/headers";
import { getTranslations } from "next-intl/server";

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

const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";

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
 return { success: false, error: t("unauthorized") || "Please login" };
 }

 const res = await fetch(`${API_URL}/reading/init`, {
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
 } catch (error: unknown) {
 return { success: false, error: (error as Error).message || t("network_error") };
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
 return { success: false, error: t("unauthorized") || "Please login" };
 }

 const res = await fetch(`${API_URL}/reading/reveal`, {
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
 } catch (error: unknown) {
 return { success: false, error: (error as Error).message || t("network_error") };
 }
}
