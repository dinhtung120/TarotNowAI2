'use client';

import { useEffect, useState, type FormEvent } from 'react';
import {
  submitReaderApplication,
  getMyReaderRequest,
  type MyReaderRequest,
} from '@/actions/readerActions';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useReaderApplyPage(t: TranslateFn) {
  const [introText, setIntroText] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState<'success' | 'error'>('success');
  const [existingRequest, setExistingRequest] = useState<MyReaderRequest | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStatus = async () => {
      const result = await getMyReaderRequest();
      setExistingRequest(result);
      setLoading(false);
    };

    void fetchStatus();
  }, []);

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    if (introText.length < 20) {
      setMessage(t('validation.min_intro'));
      setMessageType('error');
      return;
    }

    setSubmitting(true);
    setMessage('');

    const result = await submitReaderApplication(introText);
    setMessage(result.message);
    setMessageType(result.success ? 'success' : 'error');
    setSubmitting(false);

    if (result.success) {
      const updatedRequest = await getMyReaderRequest();
      setExistingRequest(updatedRequest);
    }
  };

  return {
    introText,
    setIntroText,
    submitting,
    message,
    messageType,
    existingRequest,
    loading,
    handleSubmit,
  };
}
