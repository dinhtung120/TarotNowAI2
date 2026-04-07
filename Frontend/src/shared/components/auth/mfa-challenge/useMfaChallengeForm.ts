import { useState } from 'react';
import type { MfaChallengeResult } from '@/shared/components/auth/mfa-challenge/types';

interface UseMfaChallengeFormArgs {
  genericErrorText: string;
  onClose: () => void;
  onSuccess: (code: string) => void;
  skipApiCall: boolean;
  onChallenge?: (code: string) => Promise<MfaChallengeResult>;
}

export function useMfaChallengeForm({ genericErrorText, onClose, onSuccess, skipApiCall, onChallenge }: UseMfaChallengeFormArgs) {
  const [code, setCode] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const resetFormState = () => {
    setCode('');
    setError('');
    setLoading(false);
  };

  const handleClose = () => {
    resetFormState();
    onClose();
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    if (code.length < 6) return;

    setLoading(true);
    setError('');

    if (skipApiCall) {
      const successCode = code;
      resetFormState();
      onSuccess(successCode);
      return;
    }

    if (!onChallenge) {
      setError(genericErrorText);
      setLoading(false);
      return;
    }

    const result = await onChallenge(code);
    if (result.success) {
      const successCode = code;
      resetFormState();
      onSuccess(successCode);
      return;
    }

    setError(result.error || genericErrorText);
    setLoading(false);
  };

  return {
    code,
    error,
    loading,
    handleClose,
    handleSubmit,
    setCode,
  };
}
