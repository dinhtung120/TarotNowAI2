import { Link } from '@/i18n/routing';
import { cn } from '@/lib/utils';
import type { RefObject } from 'react';
import type { BottomTabLinkHref, BottomTabSubItem } from './config';

interface BottomTabBarMenuPanelProps {
 activeMenu: string | null;
 activeSubItems: BottomTabSubItem[] | null;
 pathname: string;
 tNav: (key: string) => string;
 menuRef: RefObject<HTMLDivElement | null>;
 onSelect: () => void;
 matchesPath: (candidatePath: string, prefix: string) => boolean;
}

export default function BottomTabBarMenuPanel({
 activeMenu,
 activeSubItems,
 pathname,
 tNav,
 menuRef,
 onSelect,
 matchesPath,
}: BottomTabBarMenuPanelProps) {
 if (!activeSubItems) return null;

 return (
  <div
   ref={menuRef}
   className={cn(
    'absolute bottom-full mb-3 p-3 bg-[var(--bg-glass)] backdrop-blur-2xl border border-[var(--border-subtle)] rounded-3xl shadow-[0_12px_40px_rgba(0,0,0,0.18)] animate-in slide-in-from-bottom-2 duration-200 z-[60] min-w-[150px]',
    activeMenu === 'tarot' && 'left-[37.5%] -translate-x-1/2',
    activeMenu === 'social' && 'left-[62.5%] -translate-x-1/2',
    activeMenu === 'account' && 'right-3',
   )}
   style={{ WebkitBackdropFilter: 'blur(20px)' }}
  >
   <div className={cn('flex flex-col gap-1.5')}>
    {activeSubItems.map((sub) => {
     const SubIcon = sub.icon;
     const isSubActive = sub.matchPrefixes.some((prefix) => matchesPath(pathname, prefix));
     return (
      <Link
       key={sub.href}
       href={sub.href as BottomTabLinkHref}
       onClick={onSelect}
       className={cn(
        'flex items-center gap-3 p-3 rounded-2xl transition-all',
        isSubActive
         ? 'bg-[var(--purple-50)] text-[var(--purple-accent)] border border-[var(--purple-100)]'
         : 'bg-transparent hover:bg-[var(--bg-surface-hover)] text-[var(--text-secondary)] border border-transparent',
       )}
      >
       <SubIcon
        className={cn(
         'w-5 h-5',
         isSubActive ? 'text-[var(--purple-accent)]' : 'text-[var(--text-muted)]',
        )}
       />
       <span className={cn('text-[13px] tracking-wide', isSubActive ? 'font-black' : 'font-semibold')}>
        {tNav(sub.labelKey)}
       </span>
      </Link>
     );
    })}
   </div>
  </div>
 );
}
