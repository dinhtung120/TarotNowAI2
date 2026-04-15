import type { Metadata, Viewport } from "next";
import { Geist, Geist_Mono, Playfair_Display } from "next/font/google";
import { cookies } from "next/headers";
import { NextIntlClientProvider } from "next-intl";
import { getMessages, getTranslations } from "next-intl/server";
import { Toaster } from "react-hot-toast";
import { cn } from "@/lib/utils";
import ThemeStylesheetManager from "@/shared/components/common/ThemeStylesheetManager";
import { DEFAULT_THEME, getThemeStylesheetHref, isValidTheme, THEME_COOKIE_KEY, type ThemeId } from "@/shared/domain/theme";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppQueryProvider from "@/shared/components/common/AppQueryProvider";
import AuthBootstrap from "@/shared/components/auth/AuthBootstrap";
import { AUTH_COOKIE } from "@/shared/infrastructure/auth/authConstants";
import { getServerSessionSnapshot } from "@/shared/infrastructure/auth/serverAuth";
import "../globals.css";

const geistSans = Geist({
 variable: "--font-geist-sans",
 subsets: ["latin", "latin-ext"],
 display: "swap",
 preload: true,
 adjustFontFallback: true,
});
const geistMono = Geist_Mono({
 variable: "--font-geist-mono",
 subsets: ["latin", "latin-ext"],
 display: "swap",
 preload: false,
 adjustFontFallback: true,
});
const playfair = Playfair_Display({
 variable: "--font-playfair",
 subsets: ["latin", "latin-ext", "vietnamese"],
 display: "swap",
 preload: false,
 adjustFontFallback: true,
});

export const viewport: Viewport = { width: "device-width", initialScale: 1, viewportFit: "cover" };

export async function generateMetadata({ params }: { params: Promise<{ locale: string }> }): Promise<Metadata> {
 const { locale } = await params;
 const t = await getTranslations({ locale, namespace: "Common" });
 return { title: t("app_title"), description: t("app_description") };
}

export default async function RootLayout({ children, params }: Readonly<{ children: React.ReactNode; params: Promise<{ locale: string }> }>) {
 const { locale } = await params;
 const messages = await getMessages();
 const cookieStore = await cookies();
 const cookieTheme = cookieStore.get(THEME_COOKIE_KEY)?.value ?? null;
 const initialTheme: ThemeId = isValidTheme(cookieTheme) ? cookieTheme : DEFAULT_THEME;
 const hasAccessCookie = Boolean(cookieStore.get(AUTH_COOKIE.ACCESS)?.value);
 const sessionSnapshot = hasAccessCookie
  ? await getServerSessionSnapshot()
  : { authenticated: false, user: null };

 return (
  <html lang={locale} data-theme={initialTheme}>
   <head><link id="tn-theme-stylesheet" rel="stylesheet" href={getThemeStylesheetHref(initialTheme)} /></head>
   <body className={cn(geistSans.variable, geistMono.variable, playfair.variable, "antialiased")}>
    <ThemeStylesheetManager initialTheme={initialTheme} />
	    <NextIntlClientProvider messages={messages}>
	     <AppQueryProvider>
	      <AuthBootstrap initialUser={sessionSnapshot.user} />
	      <AppAuthSessionManager />
      {children}
      <Toaster position="top-right" />
     </AppQueryProvider>
    </NextIntlClientProvider>
   </body>
  </html>
 );
}
