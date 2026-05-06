'use client';

import { useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
  getMfaStatus,
  setupMfa,
  verifyMfa,
  type MfaSetupResult,
} from '@/features/profile/mfa/actions/actions';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { createTypedSubmitHandler } from '@/shared/utils/typedSubmit';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

export function useProfileMfaPage(t: TranslateFn) {
 const [mfaEnabledOverride, setMfaEnabledOverride] = useState<boolean | null>(null);
  const [setupData, setSetupData] = useState<MfaSetupResult | null>(null);
  const [setupLoading, setSetupLoading] = useState(false);
  const [setupError, setSetupError] = useState('');
  const [code, setCode] = useState('');
  const [verifyLoading, setVerifyLoading] = useState(false);
  const [verifyError, setVerifyError] = useState('');

  const qrColors = useMemo(() => {
    if (typeof window === 'undefined') {
      return { dark: '', light: '' };
    }

    const root = getComputedStyle(document.documentElement);
    const dark =
      root.getPropertyValue('--qr-code-dark').trim() ||
      root.getPropertyValue('--text-ink').trim();
    const light =
      root.getPropertyValue('--qr-code-light').trim() ||
      root.getPropertyValue('--bg-elevated').trim();

    return { dark, light };
  }, []);

  const qrColorOptions =
    qrColors.dark && qrColors.light ? { color: qrColors } : {};

 const statusQuery = useQuery({
 queryKey: userStateQueryKeys.profile.mfaStatus(),
  queryFn: async () => {
   const result = await getMfaStatus();
   return result.success ? result.data ?? false : false;
  },
 });

 const mfaEnabled =
  mfaEnabledOverride ?? (typeof statusQuery.data === 'boolean' ? statusQuery.data : null);

  const handleStartSetup = async () => {
    setSetupLoading(true);
    setSetupError('');
    const res = await setupMfa();
    if (res.success && res.data) {
      setSetupData(res.data);
    } else {
      setSetupError(res.success ? t('mfa.setup_error_generic') : res.error);
    }
    setSetupLoading(false);
  };

  const handleVerify = createTypedSubmitHandler(async (payload: { code: string }) => {
    const normalizedCode = payload.code.trim();
    if (normalizedCode.length < 6) return;

    setVerifyLoading(true);
    setVerifyError('');
    const res = await verifyMfa(normalizedCode);
    if (res.success) {
   setMfaEnabledOverride(true);
      setSetupData(null);
    } else {
      setVerifyError(res.error);
    }
    setVerifyLoading(false);
  });

  const copyToClipboard = (value: string) => {
    navigator.clipboard.writeText(value);
  };

  return {
    mfaEnabled,
    setupData,
    setupLoading,
    setupError,
    code,
    setCode,
    verifyLoading,
    verifyError,
    qrColorOptions,
    handleStartSetup,
    handleVerify,
    copyToClipboard,
  };
}
