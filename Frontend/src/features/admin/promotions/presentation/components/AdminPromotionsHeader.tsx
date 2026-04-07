'use client';

import { PlusCircle, Ticket, X } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { Button, SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminPromotionsHeaderProps {
 isCreating: boolean;
 onToggleCreate: () => void;
}

export function AdminPromotionsHeader({ isCreating, onToggleCreate }: AdminPromotionsHeaderProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('flex flex-col tn-md-flex-row tn-md-items-end justify-between gap-6')}>
   <SectionHeader
    tag={t('promotions.header.tag')}
    tagIcon={<Ticket className={cn('w-3 h-3 tn-text-warning')} />}
    title={t('promotions.header.title')}
    subtitle={t('promotions.header.subtitle')}
    className={cn('mb-0 text-left items-start')}
   />
   <Button
    variant={isCreating ? 'secondary' : 'primary'}
    onClick={onToggleCreate}
    className={cn(
     'shrink-0',
     !isCreating &&
      'tn-btn-warning-solid'
    )}
   >
    {isCreating ? <X className={cn('w-4 h-4')} /> : <PlusCircle className={cn('w-4 h-4')} />}
    {isCreating ? t('promotions.actions.toggle_create_cancel') : t('promotions.actions.toggle_create_add')}
   </Button>
  </div>
 );
}
