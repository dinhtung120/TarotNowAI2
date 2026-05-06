'use client';

import { cn } from '@/lib/utils';

export function ProfileAvatarSpinner() {
 return (
  <svg className={cn('w-6 h-6 text-white animate-spin')} viewBox="0 0 24 24" fill="none">
   <circle cx="12" cy="12" r="10" className={cn('opacity-25')} stroke="currentColor" strokeWidth="4" />
   <path className={cn('opacity-75')} fill="currentColor" d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z" />
  </svg>
 );
}
