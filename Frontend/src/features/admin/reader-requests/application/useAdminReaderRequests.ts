'use client';

import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 listReaderRequests,
 processReaderRequest,
 type AdminReaderRequest,
} from '@/features/admin/application/actions';

export function useAdminReaderRequests() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const queryClient = useQueryClient();

 const [page, setPage] = useState(1);
 const [statusFilter, setStatusFilter] = useState('pending');
 const [processing, setProcessing] = useState<string | null>(null);
 const [adminNote, setAdminNote] = useState('');
 const [selectedRequest, setSelectedRequest] = useState<AdminReaderRequest | null>(null);

 const pageSize = 10;

 const { data, isLoading, isFetching } = useQuery({
  queryKey: ['admin', 'reader-requests', page, statusFilter],
  queryFn: async () => {
   const result = await listReaderRequests(page, pageSize, statusFilter);
   if (!result.success || !result.data) {
    return {
     requests: [] as AdminReaderRequest[],
     totalCount: 0,
    };
   }
   return result.data;
  },
 });

 const processMutation = useMutation({
  mutationFn: async (payload: { requestId: string; action: 'approve' | 'reject'; note?: string }) =>
   processReaderRequest(payload.requestId, payload.action, payload.note),
 });

 const handleProcess = async (
  requestId: string,
  action: 'approve' | 'reject'
 ) => {
  setProcessing(requestId);
  const result = await processMutation.mutateAsync({
   requestId,
   action,
   note: adminNote,
  });
  if (result.success) {
   toast.success(
    action === 'approve'
     ? t('reader_requests.toast.approve_success')
     : t('reader_requests.toast.reject_success')
   );
   setAdminNote('');
   setSelectedRequest(null);
   await queryClient.invalidateQueries({ queryKey: ['admin', 'reader-requests'] });
  } else {
   toast.error(t('reader_requests.toast.process_failed'));
  }
  setProcessing(null);
 };

 return {
  t,
  locale,
  requests: data?.requests ?? [],
  totalCount: data?.totalCount ?? 0,
  page,
  setPage,
  statusFilter,
  setStatusFilter,
  loading: isLoading || isFetching,
  processing,
  adminNote,
  setAdminNote,
  selectedRequest,
  setSelectedRequest,
  totalPages: useMemo(() => Math.ceil((data?.totalCount ?? 0) / pageSize), [data, pageSize]),
  handleProcess,
 };
}
