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
    <Link href={item.href} onClick={onClick} className={cn('group relative flex min-h-10 items-center justify-between overflow-hidden rounded-xl px-4 py-2.5 transition-all duration-300', active ? 'border tn-border-accent-30 tn-bg-accent-20 tn-text-ink tn-shadow-glow-purple-sm' : 'border border-transparent tn-text-secondary tn-sidebar-item-hover')}>
      <div className={cn('relative z-10 flex items-center gap-3')}>
        <Icon className={cn('h-4 w-4 transition-all duration-300', active ? 'scale-110 tn-text-accent' : 'tn-group-text-ink')} />
        <span className={cn('tn-text-11 uppercase tracking-widest', active ? 'font-black tn-text-accent' : 'font-bold')}>{label}</span>
      </div>
      {item.badge && item.badge > 0 ? <span className={cn('relative z-10 flex h-5 tn-minw-20 items-center justify-center rounded-full tn-bg-danger-soft px-1.5 tn-text-9 font-black tn-text-danger')}>{item.badge > 99 ? '99+' : item.badge}</span> : null}
      {active ? <div className={cn('absolute bottom-1/4 left-0 top-1/4 w-1 rounded-r-full tn-bg-accent tn-shadow-glow-accent-soft')} /> : null}
    </Link>
  );
}
