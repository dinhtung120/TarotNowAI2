import { userSidebarMenuGroups } from '@/shared/components/layout/user-sidebar/config';
import UserSidebarMenuGroup from '@/shared/components/layout/user-sidebar/UserSidebarMenuGroup';
import { cn } from '@/lib/utils';

interface UserSidebarPanelProps {
  activeHref: string | null;
  isOpen: boolean;
  tNav: (key: string) => string;
  tUserNav: (key: string) => string;
  onClose: () => void;
}

export default function UserSidebarPanel({ activeHref, isOpen, tNav, tUserNav, onClose }: UserSidebarPanelProps) {
  return (
    <div className={cn('absolute left-0 top-12 w-64 origin-top-left flex-col rounded-2xl border border-[var(--border-subtle)] bg-[var(--bg-overlay)]/95 backdrop-blur-2xl shadow-[var(--glow-purple-sm)] transition-all duration-300', isOpen ? 'visible flex scale-100 opacity-100' : 'invisible scale-95 opacity-0 pointer-events-none')}>
      <nav className={cn('custom-scrollbar max-h-[75vh] flex-1 space-y-5 overflow-y-auto rounded-2xl px-3 py-4')}>
        {userSidebarMenuGroups.map((group) => (
          <UserSidebarMenuGroup key={group.id} group={group} activeHref={activeHref} tNav={tNav} tUserNav={tUserNav} onClose={onClose} />
        ))}
      </nav>
    </div>
  );
}
