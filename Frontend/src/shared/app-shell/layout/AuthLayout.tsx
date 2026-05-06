"use client";

import { type ReactNode } from "react";
import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
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
  <div className={cn("min-h-dvh flex items-center justify-center tn-bg-void relative overflow-hidden font-sans px-4 py-10")}>
   <div className={cn("tn-auth-layout-blob tn-auth-layout-blob-a absolute rounded-full opacity-70 animate-slow-pulse")} />
   <div className={cn("tn-auth-layout-blob tn-auth-layout-blob-b absolute rounded-full opacity-70 animate-slow-pulse")} />

   <div className={cn("relative z-10 w-full max-w-md tn-auth-layout-card")}>
    <div className={cn("text-center mb-3")}>
     <Link href="/" className={cn("inline-flex items-center justify-center min-h-11 px-1 text-2xl font-black italic tracking-tighter lunar-metallic-text mb-1")}>
      TarotNow AI
     </Link>
    </div>

    <div className={cn("text-center mb-8")}>
     <h1 className={cn("text-3xl font-extrabold tn-text-ink tracking-tight mb-2")}>
      {title}
     </h1>
     {subtitle && (
      <p className={cn("tn-text-secondary text-sm font-medium")}>
       {subtitle}
      </p>
     )}
    </div>

    {children}
   </div>
  </div>
 );
}
