import type { UserProfile } from '@/features/auth/domain/types';
import { cn } from '@/lib/utils';

interface NavbarAvatarDropdownHeaderProps {
  tNav: (key: string) => string;
  user: UserProfile | null;
}

export default function NavbarAvatarDropdownHeader({ tNav, user }: NavbarAvatarDropdownHeaderProps) {
  return (
    <div className={cn('border-b border-[var(--border-subtle)] bg-gradient-to-br from-[var(--purple-accent)]/[0.03] to-transparent px-4 py-4')}>
      <div className={cn('mb-0.5 flex items-baseline justify-between')}>
        <p className={cn('truncate text-xs font-black tracking-tight text-[var(--text-ink)]')}>{user?.displayName || tNav('profile')}</p>
        <span className={cn('text-[10px] font-black text-[var(--purple-accent)]')}>Lv.{user?.level ?? 1}</span>
      </div>
      <p className={cn('mb-2 truncate text-[10px] text-[var(--text-muted)]')}>{user?.email || ''}</p>
      <div className={cn('h-1 w-full overflow-hidden rounded-full bg-[var(--bg-surface-hover)]')}><div className={cn('h-full bg-[var(--purple-accent)] shadow-[0_0_8px_var(--purple-accent)] transition-all duration-1000')} style={{ width: `${(user?.exp ?? 0) % 100}%` }} /></div>
      <div className={cn('mt-1 flex justify-between')}><span className={cn('text-[8px] font-bold uppercase tracking-tighter text-[var(--text-muted)]')}>Current EXP</span><span className={cn('text-[8px] font-black text-[var(--purple-muted)]')}>{user?.exp ?? 0}</span></div>
    </div>
  );
}
