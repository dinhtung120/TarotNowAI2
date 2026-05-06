'use client';

import Button from '@/shared/ui/Button';
import { cn } from '@/lib/utils';

type Translator = (key: string, values?: Record<string, string | number>) => string;

interface PostReportActionsProps {
 t: Translator;
 isSubmitting: boolean;
 onCancel: () => void;
}

export function PostReportActions({ t, isSubmitting, onCancel }: PostReportActionsProps) {
 return (
  <div className={cn('flex justify-end gap-3 pt-1')}>
   <Button type="button" variant="secondary" onClick={onCancel} disabled={isSubmitting}>
    {t('report_modal.cancel')}
   </Button>
   <Button type="submit" variant="danger" isLoading={isSubmitting}>
    {isSubmitting ? t('report_modal.submitting') : t('report_modal.submit')}
   </Button>
  </div>
 );
}
