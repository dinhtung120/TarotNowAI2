'use client';

import { AtSign } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ProfileIdentityProps {
 displayName: string;
 username: string;
}

export function ProfileIdentity({
 displayName,
 username,
}: ProfileIdentityProps) {
 return (
  <div>
   <h2 className={cn('text-3xl font-black tn-text-primary italic tracking-tighter mb-1')}>{displayName}</h2>
   <div className={cn('flex items-center tn-justify-center-start-md gap-1.5 tn-text-secondary font-medium')}>
    <AtSign className={cn('w-3 h-3')} />
    <span className={cn('text-sm')}>@{username}</span>
   </div>
  </div>
 );
}
