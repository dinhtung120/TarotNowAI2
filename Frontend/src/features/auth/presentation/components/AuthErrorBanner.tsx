'use client';

import { cn } from '@/lib/utils';

interface AuthErrorBannerProps {
 message: string;
}

export function AuthErrorBanner({ message }: AuthErrorBannerProps) {
 if (!message) {
  return null;
 }

 return (
  <div className={cn("mb-6 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3")}>
   <div className={cn("w-1.5 h-1.5 rounded-full bg-[var(--danger)] animate-pulse")} />
   {message}
  </div>
 );
}
