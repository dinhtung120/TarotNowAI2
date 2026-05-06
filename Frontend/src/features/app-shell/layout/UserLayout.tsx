

import { type ReactNode } from "react";
import { BottomTabBar } from '@/features/app-shell/public';
import UserSidebar from '@/shared/app-shell/navigation/user-sidebar/UserSidebar';
import AstralBackground from "./AstralBackground";
import { cn } from '@/lib/utils';

interface UserLayoutProps {
  children: ReactNode;
}

export default function UserLayout({ children }: UserLayoutProps) {
  return (
    <div className={cn("flex h-dvh flex-col tn-user-layout-shell overflow-hidden relative")}>
      <AstralBackground variant="subtle" particleCount={6} />
      {}
      <div className={cn("flex flex-1 min-h-0 tn-user-layout-content overflow-hidden")}>
        {}
        <UserSidebar />

        {}
        <main className={cn("flex-1 min-h-0 relative z-10 custom-scrollbar overflow-y-auto tn-user-layout-main")}>
          {children}
        </main>
      </div>

      {}
      <BottomTabBar />
    </div>
  );
}
