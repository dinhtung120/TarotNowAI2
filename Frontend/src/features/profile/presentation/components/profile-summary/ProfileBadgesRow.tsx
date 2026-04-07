'use client';

import { cn } from '@/lib/utils';
import { ProfileInfoBadge } from './ProfileInfoBadge';

interface ProfileBadgesRowProps {
 levelLabel: string;
 levelValue: string;
 numerologyLabel: string;
 numerologyValue: string;
 zodiacLabel: string;
 zodiacValue: string;
}

export function ProfileBadgesRow({
 levelLabel,
 levelValue,
 numerologyLabel,
 numerologyValue,
 zodiacLabel,
 zodiacValue,
}: ProfileBadgesRowProps) {
 return (
  <div className={cn('flex flex-wrap gap-3 tn-justify-center-start-md')}>
   <ProfileInfoBadge label={levelLabel} value={levelValue} dotClass={cn('bg-[var(--purple-accent)] shadow-[0_0_8px_var(--c-168-85-247-60)]')} />
   <ProfileInfoBadge label={zodiacLabel} value={zodiacValue} dotClass={cn('bg-[var(--info)] shadow-[0_0_8px_var(--c-59-130-246-60)]')} />
   <ProfileInfoBadge label={numerologyLabel} value={numerologyValue} dotClass={cn('bg-[var(--warning)] shadow-[0_0_8px_var(--c-245-158-11-60)]')} />
  </div>
 );
}
