"use server";

import { cookies } from "next/headers";
import { getTranslations } from "next-intl/server";

export interface UserCollectionDto {
 cardId: number;
 level: number;
 copies: number;
 expGained: number;
 lastDrawnAt: string;
}

const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5037/api/v1";

export async function getUserCollection() {
 const t = await getTranslations("ApiErrors");

 try {
 const cookieStore = await cookies();
 const token = cookieStore.get("accessToken")?.value;

 if (!token) {
 return { success: false, error: t("unauthorized") || "Please login" };
 }

 const res = await fetch(`${API_URL}/reading/collection`, {
 method: "GET",
 headers: {
 Authorization: `Bearer ${token}`,
 },
 // Thêm cache revalidate tránh stale data
 next: { revalidate: 0 },
 });

 if (!res.ok) {
 if (res.status === 401) {
 return { success: false, error: t("unauthorized") };
 }
 const errorData = await res.json().catch(() => ({}));
 return { success: false, error: errorData.message || errorData.detail || t("unknown_error") };
 }

 const responseData: UserCollectionDto[] = await res.json();
 return { success: true, data: responseData };
 } catch (error: unknown) {
 const errorMessage = error instanceof Error ? error.message : t("network_error");
 return { success: false, error: errorMessage };
 }
}
