'use client';

import { cn } from '@/lib/utils';
import type { AdminRoute } from './types';
import { AdminDashboardNoticePanel } from './AdminDashboardNoticePanel';
import { AdminDashboardShortcuts } from './AdminDashboardShortcuts';

interface AdminDashboardMidSectionProps {
 onNavigate: (href: AdminRoute) => void;
}

export function AdminDashboardMidSection({ onNavigate }: AdminDashboardMidSectionProps) {
 return (
  <div className={cn('grid grid-cols-1 lg:grid-cols-3 gap-8')}>
   <AdminDashboardNoticePanel />
   <AdminDashboardShortcuts onNavigate={onNavigate} />
  </div>
 );
}
