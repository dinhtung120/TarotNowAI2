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
  <div className={cn('tn-grid-cols-1-3-lg gap-8')}>
   <AdminDashboardNoticePanel />
   <AdminDashboardShortcuts onNavigate={onNavigate} />
  </div>
 );
}
