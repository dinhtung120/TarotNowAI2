import type { UserProfile } from '@/features/auth/domain/types';
import { cn } from '@/lib/utils';

interface NavbarAvatarDropdownHeaderProps {
  tNav: (key: string) => string;
  user: UserProfile | null;
}

export default function NavbarAvatarDropdownHeader({ tNav, user }: NavbarAvatarDropdownHeaderProps) {
  return (
    <div className={cn('border-b tn-border-soft tn-avatar-dropdown-header px-4 py-4')}>
      <div className={cn('mb-0.5 flex items-baseline justify-between')}>
        <p className={cn('truncate text-xs font-black tracking-tight tn-text-ink')}>{user?.displayName || tNav('profile')}</p>
        <span className={cn('tn-text-10 font-black tn-text-accent')}>Lv.{user?.level ?? 1}</span>
      </div>
      <p className={cn('mb-2 truncate tn-text-10 tn-text-muted')}>{user?.email || ''}</p>
      <div className={cn('h-1 w-full overflow-hidden rounded-full tn-bg-surface-hover')}><div className={cn('h-full tn-bg-accent tn-shadow-glow-accent-sm transition-all duration-1000')} style={{ width: `${(user?.exp ?? 0) % 100}%` }} /></div>
      <div className={cn('mt-1 flex justify-between')}><span className={cn('tn-text-2xs font-bold uppercase tracking-tighter tn-text-muted')}>Current EXP</span><span className={cn('tn-text-2xs font-black tn-text-accent-soft')}>{user?.exp ?? 0}</span></div>
    </div>
  );
}
