import type { LucideIcon } from 'lucide-react';

export type AdminRoute =
 | '/admin/users'
 | '/admin/deposits'
 | '/admin/promotions'
 | '/admin/readings'
 | '/admin/reader-requests';

export interface AdminStatCard {
 name: string;
 value: number;
 icon: LucideIcon;
 color: string;
 bg: string;
 hoverRing: string;
 href: AdminRoute;
}
