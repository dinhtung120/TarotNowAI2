import type { Metadata, Viewport } from "next";
import { Geist, Geist_Mono, Playfair_Display } from "next/font/google";
import { cookies } from "next/headers";
import { NextIntlClientProvider } from "next-intl";
import { getMessages, getTranslations } from "next-intl/server";
import { Toaster } from "react-hot-toast";
import { cn } from "@/lib/utils";
import { pickClientMessages, ROOT_CLIENT_NAMESPACES } from "@/i18n/clientMessages";
import ThemeStylesheetManager from "@/shared/components/common/ThemeStylesheetManager";
import { DEFAULT_THEME, getThemeStylesheetHref, isValidTheme, THEME_COOKIE_KEY, type ThemeId } from "@/shared/domain/theme";
import AppAuthSessionManager from "@/features/auth/presentation/components/AppAuthSessionManager";
import AppQueryProvider from "@/shared/components/common/AppQueryProvider";
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
 const rootClientMessages = pickClientMessages(messages, ROOT_CLIENT_NAMESPACES);
 const cookieStore = await cookies();
 const cookieTheme = cookieStore.get(THEME_COOKIE_KEY)?.value ?? null;
 const initialTheme: ThemeId = isValidTheme(cookieTheme) ? cookieTheme : DEFAULT_THEME;

 return (
  <html lang={locale} data-theme={initialTheme}>
   <head><link id="tn-theme-stylesheet" rel="stylesheet" href={getThemeStylesheetHref(initialTheme)} /></head>
   <body className={cn(geistSans.variable, geistMono.variable, playfair.variable, "antialiased")}>
    <ThemeStylesheetManager initialTheme={initialTheme} />
		    <NextIntlClientProvider messages={rootClientMessages}>
	     <AppQueryProvider>
	      <AppAuthSessionManager />
      {children}
      <Toaster position="top-right" />
     </AppQueryProvider>
    </NextIntlClientProvider>
   </body>
  </html>
 );
}
