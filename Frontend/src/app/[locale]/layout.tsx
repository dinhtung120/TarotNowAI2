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
import type { Metadata } from "next";
import { Geist, Geist_Mono, Playfair_Display } from "next/font/google";
import { NextIntlClientProvider } from 'next-intl';
import { getMessages, getTranslations } from 'next-intl/server';
import Navbar from '@/components/common/Navbar';
import AuthSessionManager from "@/components/auth/AuthSessionManager";
import { Toaster } from 'react-hot-toast';
import "./globals.css";

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
});

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
});

const playfair = Playfair_Display({
    variable: "--font-playfair",
    subsets: ["latin"],
});

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

    return (
        <html lang={locale} data-theme="prismatic-royal">
            <body
                className={`${geistSans.variable} ${geistMono.variable} ${playfair.variable} antialiased`}
            >
                <NextIntlClientProvider messages={messages}>
                    <AuthSessionManager />
                    <Navbar />
                    {children}
                    <Toaster position="top-right" />
                </NextIntlClientProvider>
            </body>
        </html>
    );
}
