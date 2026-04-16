'use client';

import { useState, type FormEvent } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  submitReaderApplication,
  getMyReaderRequest,
  type MyReaderRequest,
} from '@/features/reader/application/actions';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useReaderApplyPage(t: TranslateFn) {
 const queryClient = useQueryClient();
  const [introText, setIntroText] = useState('');
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState<'success' | 'error'>('success');

 const statusQueryKey = userStateQueryKeys.reader.myRequest();
 const { data: existingRequest, isLoading, isFetching } = useQuery<MyReaderRequest | null>({
  queryKey: statusQueryKey,
  queryFn: async () => {
   const result = await getMyReaderRequest();
   return result.success ? result.data ?? null : null;
  },
 });

 const submitMutation = useMutation({
  mutationFn: async (text: string) => submitReaderApplication(text),
 });

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    if (introText.length < 20) {
      setMessage(t('validation.min_intro'));
      setMessageType('error');
      return;
    }

    setMessage('');

    const result = await submitMutation.mutateAsync(introText);
    setMessage(result.success ? result.data?.message || t('success.submitted') : result.error);
    setMessageType(result.success ? 'success' : 'error');

    if (result.success) {
   await queryClient.invalidateQueries({ queryKey: statusQueryKey });
    }
  };

  return {
    introText,
    setIntroText,
  submitting: submitMutation.isPending,
    message,
    messageType,
    existingRequest,
  loading: isLoading || isFetching,
    handleSubmit,
  };
}
