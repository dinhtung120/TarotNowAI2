

import { type ReactNode } from "react";
import UserSidebar from "./UserSidebar";
import BottomTabBar from "./BottomTabBar";
import AstralBackground from "./AstralBackground";
import RoutePrefetcher from "./RoutePrefetcher";
import { cn } from '@/lib/utils';

interface UserLayoutProps {
  children: ReactNode;
}

export default function UserLayout({ children }: UserLayoutProps) {
  return (
    <div className={cn("flex h-dvh flex-col bg-[var(--bg-void)] text-[var(--text-primary)] overflow-hidden relative")}>
      <RoutePrefetcher />
      <AstralBackground variant="subtle" particleCount={6} />
      {}
      <div className={cn("flex flex-1 min-h-0 pt-14 md:pt-16 overflow-hidden")}>
        {}
        <UserSidebar />

        {}
        <main className={cn("flex-1 min-h-0 relative z-10 custom-scrollbar overflow-y-auto pb-[calc(5.5rem+env(safe-area-inset-bottom))] md:pb-0")}>
          {children}
        </main>
      </div>

      {}
      <BottomTabBar />
    </div>
  );
}
