'use client';

import { Search, UserPlus, Users } from 'lucide-react';
import { Button, Input, SectionHeader } from '@/shared/ui';
import { cn } from '@/lib/utils';
import type { AdminUsersTranslateFn } from './types';

interface AdminUsersPageHeaderProps {
 onOpenAdd: () => void;
 onSearchTermChange: (value: string) => void;
 searchTerm: string;
 totalCount: number;
 t: AdminUsersTranslateFn;
}

export function AdminUsersPageHeader({
 onOpenAdd,
 onSearchTermChange,
 searchTerm,
 totalCount,
 t,
}: AdminUsersPageHeaderProps) {
 return (
  <div className={cn('flex flex-col tn-md-flex-row tn-md-items-end justify-between gap-6')}>
   <SectionHeader tag={t('users.header.tag')} tagIcon={<Users className={cn('w-3 h-3 tn-text-accent')} />} title={t('users.header.title')} subtitle={t('users.header.subtitle', { count: totalCount })} className={cn('mb-0 text-left items-start')} />
   <div className={cn('flex items-center gap-3 shrink-0')}>
    <Input leftIcon={<Search className={cn('w-4 h-4')} />} placeholder={t('users.search.placeholder')} value={searchTerm} onChange={(event) => onSearchTermChange(event.target.value)} className={cn('tn-w-full-80-md')} />
    <Button variant="primary" onClick={onOpenAdd} className={cn('px-5 py-3 whitespace-nowrap tn-shadow-accent-20')}>
     <span className={cn('inline-flex items-center gap-2')}><UserPlus className={cn('w-4 h-4')} />{t('users.add_user.button')}</span>
    </Button>
   </div>
  </div>
 );
}
