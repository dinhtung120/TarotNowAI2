'use client';

import { useCallback, useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 listReaderRequests,
 processReaderRequest,
 type AdminReaderRequest,
} from '@/actions/adminActions';

export function useAdminReaderRequests() {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [requests, setRequests] = useState<AdminReaderRequest[]>([]);
 const [totalCount, setTotalCount] = useState(0);
 const [page, setPage] = useState(1);
 const [statusFilter, setStatusFilter] = useState('pending');
 const [loading, setLoading] = useState(true);
 const [processing, setProcessing] = useState<string | null>(null);
 const [adminNote, setAdminNote] = useState('');
 const [selectedRequest, setSelectedRequest] = useState<AdminReaderRequest | null>(null);

 const pageSize = 10;

 const fetchRequests = useCallback(async () => {
  setLoading(true);
  const result = await listReaderRequests(page, pageSize, statusFilter);
  if (result) {
   setRequests(result.requests);
   setTotalCount(result.totalCount);
  }
  setLoading(false);
 }, [page, pageSize, statusFilter]);

 useEffect(() => {
  const timer = window.setTimeout(() => {
   void fetchRequests();
  }, 0);

  return () => {
   window.clearTimeout(timer);
  };
 }, [fetchRequests]);

 const handleProcess = async (
  requestId: string,
  action: 'approve' | 'reject'
 ) => {
  setProcessing(requestId);
  const success = await processReaderRequest(requestId, action, adminNote);
  if (success) {
   toast.success(
    action === 'approve'
     ? t('reader_requests.toast.approve_success')
     : t('reader_requests.toast.reject_success')
   );
   setAdminNote('');
   setSelectedRequest(null);
   await fetchRequests();
  } else {
   toast.error(t('reader_requests.toast.process_failed'));
  }
  setProcessing(null);
 };

 return {
  t,
  locale,
  requests,
  totalCount,
  page,
  setPage,
  statusFilter,
  setStatusFilter,
  loading,
  processing,
  adminNote,
  setAdminNote,
  selectedRequest,
  setSelectedRequest,
  totalPages: Math.ceil(totalCount / pageSize),
  handleProcess,
 };
}
