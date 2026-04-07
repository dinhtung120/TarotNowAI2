'use client';

import { X } from 'lucide-react';
import { useTranslations } from 'next-intl';
import MfaChallengeModalContent from '@/shared/components/auth/mfa-challenge/MfaChallengeModalContent';
import type { MfaChallengeModalProps } from '@/shared/components/auth/mfa-challenge/types';
import { useMfaChallengeForm } from '@/shared/components/auth/mfa-challenge/useMfaChallengeForm';
import { cn } from '@/lib/utils';

export default function MfaChallengeModal({ isOpen, onClose, onSuccess, actionTitle, skipApiCall = false, onChallenge }: MfaChallengeModalProps) {
  const t = useTranslations('Auth');
  const tCommon = useTranslations('Common');
  const { code, error, loading, handleClose, handleSubmit, setCode } = useMfaChallengeForm({
    genericErrorText: t('mfa.error_generic'),
    onClose,
    onSuccess,
    skipApiCall,
    onChallenge,
  });

  if (!isOpen) return null;

  return (
    <div className={cn('fixed inset-0 z-50 flex items-center justify-center p-4 tn-overlay-strong animate-in fade-in duration-200')}>
      <div className={cn('relative w-full max-w-sm rounded-3xl border p-6 shadow-2xl tn-surface-strong tn-border animate-in zoom-in-95 duration-200')}>
        <button type="button" onClick={handleClose} aria-label={tCommon('close')} className={cn('absolute right-4 top-4 rounded-full p-2 tn-text-muted transition-colors hover:tn-surface-strong hover:tn-text-primary')}><X className={cn('h-5 w-5')} /></button>
        <MfaChallengeModalContent
          actionTitleText={actionTitle ?? t('mfa.action_default')}
          code={code}
          description={t('mfa.desc', { actionTitle: '{actionTitle}' })}
          error={error}
          loading={loading}
          placeholder={t('mfa.placeholder')}
          submitLabel={t('mfa.submit')}
          title={t('mfa.title')}
          onCodeChange={setCode}
          onSubmit={handleSubmit}
        />
      </div>
    </div>
  );
}
