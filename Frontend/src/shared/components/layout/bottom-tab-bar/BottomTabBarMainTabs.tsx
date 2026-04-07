import { Link } from '@/i18n/routing';
import { cn } from '@/lib/utils';
import type { BottomTabGroup, BottomTabLinkHref } from './config';

interface BottomTabBarMainTabsProps {
 tabs: BottomTabGroup[];
 activeTabId: string | null;
 activeMenu: string | null;
 tNav: (key: string) => string;
 onToggleMenu: (tab: BottomTabGroup) => void;
}

export default function BottomTabBarMainTabs({
 tabs,
 activeTabId,
 activeMenu,
 tNav,
 onToggleMenu,
}: BottomTabBarMainTabsProps) {
 return (
  <div className={cn('flex items-stretch justify-around gap-1 px-2 py-2')}>
   {tabs.map((tab) => {
   const isMainActive = activeTabId === tab.id;
   const isOpen = activeMenu === tab.id;
   const Icon = tab.icon;
    const content = (
     <>
      <Icon className={cn('w-[22px] h-[22px] transition-transform duration-300', isMainActive || isOpen ? 'scale-110' : 'scale-100')} />
      <span className={cn('text-[9px] uppercase tracking-wider truncate mt-1', isMainActive || isOpen ? 'font-black' : 'font-bold')}>
       {tNav(tab.labelKey)}
      </span>
      {isMainActive ? (
       <div className={cn('absolute top-1.5 right-3 w-1.5 h-1.5 rounded-full bg-[var(--purple-accent)] shadow-[0_0_6px_var(--purple-accent)]')} />
      ) : null}
     </>
    );

    const itemClassName = cn(
      'relative flex-1 min-w-0 flex flex-col items-center justify-center py-2.5 rounded-2xl transition-all duration-300 min-h-[52px]',
      isMainActive || isOpen
       ? 'text-[var(--purple-muted)] bg-[var(--purple-50)] border border-[var(--purple-100)] shadow-sm'
       : 'text-[var(--text-muted)] active:text-[var(--text-secondary)] border border-transparent',
    );

    if (tab.href) return <Link key={tab.id} href={tab.href as BottomTabLinkHref} onClick={() => onToggleMenu(tab)} className={itemClassName}>{content}</Link>;
    return <button key={tab.id} type="button" onClick={() => onToggleMenu(tab)} className={itemClassName}>{content}</button>;
   })}
  </div>
 );
}
