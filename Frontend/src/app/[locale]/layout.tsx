import type { Metadata } from "next";
import { Geist, Geist_Mono, Playfair_Display } from "next/font/google";
import { NextIntlClientProvider } from 'next-intl';
import { getMessages } from 'next-intl/server';
import Navbar from '@/components/common/Navbar';
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

export const metadata: Metadata = {
 title: "TarotNow AI",
 description: "Trải nghiệm Tarot minh bạch và hiện đại.",
};

export default async function RootLayout({
 children,
}: Readonly<{
 children: React.ReactNode;
}>) {
 const messages = await getMessages();

 return (
 <html lang="en">
 <body
 className={`${geistSans.variable} ${geistMono.variable} ${playfair.variable} antialiased`}
 >
 <NextIntlClientProvider messages={messages}>
 <Navbar />
 {children}
 <Toaster position="top-right" />
 </NextIntlClientProvider>
 </body>
 </html>
 );
}
