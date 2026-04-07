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
    <button type="button" onClick={onToggle} className={cn('flex min-h-11 cursor-pointer items-center gap-2 rounded-xl px-2 py-1.5 transition-all sm:px-3 sm:py-2', open ? 'border border-[var(--border-default)] bg-[var(--bg-elevated)] text-[var(--text-ink)] shadow-[var(--shadow-card)]' : 'border border-[var(--border-subtle)] bg-[var(--bg-surface-hover)] text-[var(--text-secondary)] hover:bg-[var(--bg-elevated)] hover:text-[var(--text-ink)]')}>
      <div className={cn('relative flex h-7 w-7 items-center justify-center overflow-hidden rounded-full border border-[var(--border-default)] bg-[var(--purple-100)] text-[10px] font-black text-[var(--text-ink)]')}>
        {user?.avatarUrl ? <Image src={user.avatarUrl} alt="avatar" fill sizes="28px" unoptimized className={cn('h-full w-full object-cover')} /> : user?.displayName?.charAt(0)?.toUpperCase() || 'U'}
      </div>
      <div className={cn('hidden -space-y-0.5 leading-tight lg:flex lg:flex-col lg:items-start')}>
        <span className={cn('max-w-[100px] truncate text-[11px] font-black tracking-wide text-[var(--text-ink)]')}>{user?.displayName || tNav('profile')}</span>
        <div className={cn('flex translate-y-[2px] items-center gap-1.5')}><div className={cn('flex h-4 items-center justify-center rounded-md border border-[var(--purple-accent)]/20 bg-[var(--purple-accent)]/10 px-1.5 shadow-[0_0_10px_rgba(168,156,255,0.1)]')}><span className={cn('animate-pulse text-[9px] font-black uppercase tracking-tighter text-[var(--purple-accent)]')}>Lv.{user?.level ?? 1}</span></div><span className={cn('text-[8px] font-bold uppercase tracking-widest text-[var(--text-muted)] opacity-80')}>{user?.exp ?? 0} EXP</span></div>
      </div>
      <ChevronDown className={cn('hidden h-3 w-3 transition-transform duration-200 lg:block', open ? 'rotate-180' : '')} />
    </button>
  );
}
