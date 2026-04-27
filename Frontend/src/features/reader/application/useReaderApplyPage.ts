'use client';

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  submitReaderApplication,
  getMyReaderRequest,
  type MyReaderRequest,
  type SubmitReaderApplicationPayload,
} from '@/features/reader/application/actions';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useReaderApplyPage(t: TranslateFn) {
 const queryClient = useQueryClient();
 const statusQueryKey = userStateQueryKeys.reader.myRequest();

 const { data: existingRequest, isLoading, isFetching } = useQuery<MyReaderRequest | null>({
  queryKey: statusQueryKey,
  queryFn: async () => {
   const result = await getMyReaderRequest();
   return result.success ? result.data ?? null : null;
  },
 });

 const submitMutation = useMutation({
  mutationFn: async (payload: SubmitReaderApplicationPayload) => submitReaderApplication(payload),
  onSuccess: async (result) => {
   if (result.success) {
    await queryClient.invalidateQueries({ queryKey: statusQueryKey });
   }
  },
 });

 return {
  submitApplication: async (payload: SubmitReaderApplicationPayload) => {
   return submitMutation.mutateAsync(payload);
  },
  submitting: submitMutation.isPending,
  existingRequest,
  loading: isLoading || isFetching,
  defaultErrorMessage: t('errors.submit_failed'),
 };
}
