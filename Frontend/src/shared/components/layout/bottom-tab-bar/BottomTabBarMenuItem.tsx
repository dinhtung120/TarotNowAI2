import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';
import type { BottomTabLinkHref, BottomTabSubItem } from './config';

interface BottomTabBarMenuItemProps {
 isActive: boolean;
 onSelect: () => void;
 sub: BottomTabSubItem;
 tNav: (key: string) => string;
}

export function BottomTabBarMenuItem({ isActive, onSelect, sub, tNav }: BottomTabBarMenuItemProps) {
 const SubIcon = sub.icon;
 return (
  <Link
   href={sub.href as BottomTabLinkHref}
   onClick={onSelect}
   className={cn(
    'flex items-center gap-3 rounded-2xl p-3 transition-all',
    isActive
     ? 'border border-[var(--purple-100)] bg-[var(--purple-50)] text-[var(--purple-accent)]'
     : 'border border-transparent bg-transparent text-[var(--text-secondary)] hover:bg-[var(--bg-surface-hover)]',
   )}
  >
   <SubIcon className={cn('h-5 w-5', isActive ? 'text-[var(--purple-accent)]' : 'text-[var(--text-muted)]')} />
   <span className={cn('tn-text-13 tracking-wide', isActive ? 'font-black' : 'font-semibold')}>
    {tNav(sub.labelKey)}
   </span>
  </Link>
 );
}
