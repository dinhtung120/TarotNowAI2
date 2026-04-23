import type { LucideIcon } from 'lucide-react';

export interface AdminLayoutLabels {
 title: string;
 subtitle: string;
 sectionMain: string;
 exitPortal: string;
 menu: {
  overview: string;
  users: string;
  deposits: string;
  promotions: string;
  readings: string;
  readerRequests: string;
  withdrawals: string;
  disputes: string;
  systemConfigs: string;
 };
}

export interface MenuConfigItem {
 key: keyof AdminLayoutLabels['menu'];
 href: string;
 icon: LucideIcon;
}

export interface AdminSidebarMenuItem {
 href: string;
 icon: LucideIcon;
 name: string;
}
