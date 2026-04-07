import Image from 'next/image';
import { ChevronDown } from 'lucide-react';
import type { UserProfile } from '@/features/auth/domain/types';
import { cn } from '@/lib/utils';

interface NavbarAvatarTriggerProps {
  open: boolean;
  tNav: (key: string) => string;
  user: UserProfile | null;
  onToggle: () => void;
}

export default function NavbarAvatarTrigger({ open, tNav, user, onToggle }: NavbarAvatarTriggerProps) {
  return (
    <button type="button" onClick={onToggle} className={cn('tn-avatar-trigger-root flex min-h-11 cursor-pointer items-center gap-2 rounded-xl transition-all', open ? 'border tn-border tn-bg-elevated tn-text-ink tn-shadow-card' : 'border tn-border-soft tn-bg-surface-hover tn-text-secondary tn-avatar-trigger-hover')}>
      <div className={cn('relative flex h-7 w-7 items-center justify-center overflow-hidden rounded-full border tn-border tn-bg-purple-100 tn-text-10 font-black tn-text-ink')}>
        {user?.avatarUrl ? <Image src={user.avatarUrl} alt="avatar" fill sizes="28px" unoptimized className={cn('h-full w-full object-cover')} /> : user?.displayName?.charAt(0)?.toUpperCase() || 'U'}
      </div>
      <div className={cn('tn-avatar-trigger-meta -space-y-0.5 leading-tight')}>
        <span className={cn('tn-maxw-100 truncate tn-text-11 font-black tracking-wide tn-text-ink')}>{user?.displayName || tNav('profile')}</span>
        <div className={cn('flex tn-translate-y-0_5 items-center gap-1.5')}><div className={cn('flex h-4 items-center justify-center rounded-md border tn-border-accent-20 tn-bg-accent-10 px-1.5 tn-shadow-glow-accent-15')}><span className={cn('animate-pulse tn-text-9 font-black uppercase tracking-tighter tn-text-accent')}>Lv.{user?.level ?? 1}</span></div><span className={cn('tn-text-2xs font-bold uppercase tracking-widest tn-text-muted opacity-80')}>{user?.exp ?? 0} EXP</span></div>
      </div>
      <ChevronDown className={cn('tn-chevron-lg h-3 w-3 transition-transform duration-200', open ? 'rotate-180' : '')} />
    </button>
  );
}
