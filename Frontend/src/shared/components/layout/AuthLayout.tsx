"use client";

import { type ReactNode } from "react";
import { Link } from "@/i18n/routing";
import { cn } from '@/lib/utils';

interface AuthLayoutProps {
 
 title: string;

 
 subtitle?: string;

 
 children: ReactNode;
}

export default function AuthLayout({
 title,
 subtitle,
 children,
}: AuthLayoutProps) {
return (
 <div className={cn("min-h-dvh flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans px-4 py-10")}>
 {}
 <div className={cn("absolute top-[-12%] left-[-12%] w-96 h-96 bg-[color:var(--c-215-189-226-62)] rounded-full filter blur-[120px] opacity-70 animate-slow-pulse")} />
 <div className={cn("absolute bottom-[-12%] right-[-12%] w-96 h-96 bg-[color:var(--c-178-232-214-58)] rounded-full filter blur-[120px] opacity-70 animate-slow-pulse")} />

 {}
 <div className={cn("relative z-10 w-full max-w-md p-6 sm:p-8 bg-[var(--bg-glass)] border border-[var(--border-default)] shadow-[var(--shadow-elevated)] rounded-3xl ")}>
 {}
 <div className={cn("text-center mb-3")}>
 <Link
 href="/"
 className={cn("inline-flex items-center justify-center min-h-11 px-1 text-2xl font-black italic tracking-tighter lunar-metallic-text mb-1")}
 >
 TarotNow AI
 </Link>
 </div>

 {}
 <div className={cn("text-center mb-8")}>
 <h1 className={cn("text-3xl font-extrabold text-[var(--text-ink)] tracking-tight mb-2")}>
 {title}
 </h1>
 {subtitle && (
 <p className={cn("text-[var(--text-secondary)] text-sm font-medium")}>
 {subtitle}
 </p>
 )}
 </div>

 {}
 {children}
 </div>
 </div>
 );
}
