/*
 * ===================================================================
 * FILE: layout.tsx (Root Layout)
 * BỐI CẢNH (CONTEXT):
 *   Layout gốc (Root) bọc toàn bộ ứng dụng Next.js.
 *   Xử lý cấu hình font, multi-language (i18n), Auth Session, và Toast Notifications.
 * 
 * RENDERING (SERVER COMPONENT):
 *   Phục vụ HTML khung xương và Metadata chung ban đầu trên Server.
 *   Font chữ được tải bằng next/font để tối ưu Core Web Vitals (CLS/LCP).
 * ===================================================================
 */
import type { Metadata, Viewport } from "next";
import { Geist, Geist_Mono, Playfair_Display } from "next/font/google";
import { cookies } from "next/headers";
import { NextIntlClientProvider } from 'next-intl';
import { getMessages, getTranslations } from 'next-intl/server';
import ThemeStylesheetManager from "@/shared/components/common/ThemeStylesheetManager";
import {
    DEFAULT_THEME,
    getThemeStylesheetHref,
    isValidTheme,
    THEME_COOKIE_KEY,
    type ThemeId,
} from "@/shared/domain/theme";
import { Toaster } from 'react-hot-toast';
import "../globals.css";

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
    display: "swap",
});

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
    display: "swap",
});

const playfair = Playfair_Display({
    variable: "--font-playfair",
    subsets: ["latin"],
    display: "swap",
});

export const viewport: Viewport = {
  width: "device-width",
  initialScale: 1,
  viewportFit: "cover",
};

export async function generateMetadata(
    { params }: { params: Promise<{ locale: string }> },
): Promise<Metadata> {
    const { locale } = await params;
    const t = await getTranslations({ locale, namespace: "Common" });
    return {
        title: t("app_title"),
        description: t("app_description"),
    };
}

export default async function RootLayout({
    children,
    params,
}: Readonly<{
    children: React.ReactNode;
    params: Promise<{ locale: string }>;
}>) {
    const { locale } = await params;
    const messages = await getMessages();
    const cookieStore = await cookies();
    const cookieTheme = cookieStore.get(THEME_COOKIE_KEY)?.value ?? null;
    const initialTheme: ThemeId = isValidTheme(cookieTheme) ? cookieTheme : DEFAULT_THEME;

    return (
        <html lang={locale} data-theme={initialTheme}>
            <head>
                <link id="tn-theme-stylesheet" rel="stylesheet" href={getThemeStylesheetHref(initialTheme)} />
            </head>
            <body
                className={`${geistSans.variable} ${geistMono.variable} ${playfair.variable} antialiased`}
            >
                <ThemeStylesheetManager initialTheme={initialTheme} />
                <NextIntlClientProvider messages={messages}>
                    {children}
                    <Toaster position="top-right" />
                </NextIntlClientProvider>
            </body>
        </html>
    );
}
