'use client';

import { Fragment } from 'react';
import type {
 AdminLayoutLabels,
 AdminSidebarMenuItem
} from './AdminLayout.types';
import { AdminSidebarBrand } from './admin-sidebar/AdminSidebarBrand';
import { AdminSidebarExitLink } from './admin-sidebar/AdminSidebarExitLink';
import { AdminSidebarMenu } from './admin-sidebar/AdminSidebarMenu';

interface AdminSidebarContentProps {
 labels: AdminLayoutLabels;
 menuItems: AdminSidebarMenuItem[];
 pathname: string;
 isDropdown?: boolean;
 onClose: () => void;
}

export function AdminSidebarContent({
 labels,
 menuItems,
 pathname,
 isDropdown = false,
 onClose,
}: AdminSidebarContentProps) {
 return (
  <Fragment>
   <AdminSidebarBrand isDropdown={isDropdown} subtitle={labels.subtitle} title={labels.title} />
   <AdminSidebarMenu
    isDropdown={isDropdown}
    menuItems={menuItems}
    onClose={onClose}
    pathname={pathname}
    sectionMain={labels.sectionMain}
   />
   <AdminSidebarExitLink isDropdown={isDropdown} label={labels.exitPortal} onClose={onClose} />
  </Fragment>
 );
}
