import { Link } from '@/i18n/routing';
import type { NavbarNavItem } from '@/shared/components/common/navbar/config';
import { cn } from '@/lib/utils';

interface UserSidebarMenuItemProps {
  active: boolean;
  item: NavbarNavItem & { badge?: number };
  label: string;
  onClick: () => void;
}

export default function UserSidebarMenuItem({ active, item, label, onClick }: UserSidebarMenuItemProps) {
  const Icon = item.icon;

  return (
    <Link href={item.href} onClick={onClick} className={cn('group relative flex min-h-10 items-center justify-between overflow-hidden rounded-xl px-4 py-2.5 transition-all duration-300', active ? 'border border-[var(--purple-accent)]/30 bg-[var(--purple-accent)]/15 text-[var(--text-ink)] shadow-[var(--glow-purple-sm)]' : 'border border-transparent text-[var(--text-secondary)] hover:bg-[var(--bg-surface-hover)] hover:text-[var(--text-ink)]')}>
      <div className={cn('relative z-10 flex items-center gap-3')}>
        <Icon className={cn('h-4 w-4 transition-all duration-300', active ? 'scale-110 text-[var(--purple-accent)]' : 'group-hover:text-[var(--text-ink)]')} />
        <span className={cn('text-[11px] uppercase tracking-widest', active ? 'font-black text-[var(--purple-accent)]' : 'font-bold')}>{label}</span>
      </div>
      {item.badge && item.badge > 0 ? <span className={cn('relative z-10 flex h-5 min-w-[20px] items-center justify-center rounded-full bg-[var(--danger)]/20 px-1.5 text-[9px] font-black text-[var(--danger)]')}>{item.badge > 99 ? '99+' : item.badge}</span> : null}
      {active ? <div className={cn('absolute bottom-1/4 left-0 top-1/4 w-1 rounded-r-full bg-[var(--purple-accent)] shadow-[0_0_10px_var(--purple-accent)]')} /> : null}
    </Link>
  );
}
