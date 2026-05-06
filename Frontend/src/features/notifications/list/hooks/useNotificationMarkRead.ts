'use client';

import { useCallback } from 'react';
import { useTranslations } from 'next-intl';
import toast from 'react-hot-toast';

interface MarkReadResult {
 success: boolean;
}

interface UseNotificationMarkReadParams {
 markAsRead: (id: string) => Promise<MarkReadResult>;
}

export function useNotificationMarkRead({ markAsRead }: UseNotificationMarkReadParams) {
 const t = useTranslations('Notifications');

 const handleMarkRead = useCallback(
  async (id: string) => {
   const result = await markAsRead(id);
   if (result.success) {
    toast.success(t('mark_read_success'));
    return;
   }

   toast.error(t('mark_read_fail'));
  },
  [markAsRead, t],
 );

 return { handleMarkRead };
}
