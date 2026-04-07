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
  <div className={cn("mb-6 p-4 rounded-xl tn-auth-error-banner border text-sm animate-in fade-in slide-in-from-top-2 flex items-center gap-3")}>
   <div className={cn("w-1.5 h-1.5 rounded-full tn-auth-error-dot animate-pulse")} />
   {message}
  </div>
 );
}
