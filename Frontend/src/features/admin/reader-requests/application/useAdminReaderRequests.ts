'use client';

import { useCallback, useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 listReaderRequests,
 processReaderRequest,
} from '@/features/admin/application/actions';
import { ADMIN_QUERY_POLICY } from '@/features/admin/application/adminQueryPolicy';
import { adminQueryKeys } from '@/features/admin/application/adminQueryKeys';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

export function useAdminReaderRequests() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const queryClient = useQueryClient();

 const [page, setPage] = useState(1);
 const [statusFilter, setStatusFilter] = useState('pending');
 const [processing, setProcessing] = useState<string | null>(null);
 const [noteByRequestId, setNoteByRequestId] = useState<Record<string, string>>({});
 const [selectedRequestId, setSelectedRequestId] = useState<string | null>(null);

 const pageSize = 10;

 const { data, isLoading, isFetching, error } = useQuery({
  queryKey: adminQueryKeys.readerRequests(page, statusFilter),
 queryFn: async () => {
  const result = await listReaderRequests(page, pageSize, statusFilter);
  return queryFnOrThrow(result, 'Failed to load reader requests');
 },
  ...ADMIN_QUERY_POLICY.list,
 });

 const processMutation = useMutation({
  mutationFn: async (payload: { requestId: string; action: 'approve' | 'reject'; note?: string }) =>
   processReaderRequest(payload.requestId, payload.action, payload.note),
 });

 const updateAdminNote = useCallback((requestId: string, note: string) => {
  setNoteByRequestId((currentNotes) => ({
   ...currentNotes,
   [requestId]: note,
  }));
 }, []);

 const clearAdminNote = useCallback((requestId: string) => {
  setNoteByRequestId((currentNotes) => {
   if (!currentNotes[requestId]) {
    return currentNotes;
   }

   const nextNotes = { ...currentNotes };
   delete nextNotes[requestId];
   return nextNotes;
  });
 }, []);

 const getAdminNote = useCallback((requestId: string) => noteByRequestId[requestId] || '', [noteByRequestId]);
 const selectRequest = useCallback((requestId: string) => {
  setSelectedRequestId(requestId);
 }, []);

 const handleProcess = useCallback(async (
  requestId: string,
  action: 'approve' | 'reject'
 ) => {
  setProcessing(requestId);
  const result = await processMutation.mutateAsync({
   requestId,
   action,
   note: getAdminNote(requestId),
  });
  if (result.success) {
   toast.success(
    action === 'approve'
     ? t('reader_requests.toast.approve_success')
     : t('reader_requests.toast.reject_success')
   );
   clearAdminNote(requestId);
   setSelectedRequestId(null);
   await queryClient.invalidateQueries({
    queryKey: adminQueryKeys.readerRequests(page, statusFilter),
    exact: true,
   });
  } else {
   toast.error(t('reader_requests.toast.process_failed'));
  }
  setProcessing(null);
 }, [clearAdminNote, getAdminNote, page, processMutation, queryClient, statusFilter, t]);

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
  listError: error instanceof Error ? error.message : '',
  processing,
  selectedRequestId,
  selectRequest,
  getAdminNote,
  updateAdminNote,
  totalPages: useMemo(() => Math.ceil((data?.totalCount ?? 0) / pageSize), [data, pageSize]),
  handleProcess,
 };
}
