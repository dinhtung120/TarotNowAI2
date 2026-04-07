'use client';

import { Shield } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { SectionHeader } from '@/shared/components/ui';
import { useProfileMfaPage } from '@/features/profile/mfa/application/useProfileMfaPage';
import { cn } from '@/lib/utils';
import { MfaCheckingState } from '@/features/profile/mfa/presentation/components/MfaCheckingState';
import { MfaEnabledCard } from '@/features/profile/mfa/presentation/components/MfaEnabledCard';
import { MfaSetupFlow } from '@/features/profile/mfa/presentation/components/MfaSetupFlow';
import { MfaSetupStartCard } from '@/features/profile/mfa/presentation/components/MfaSetupStartCard';

export default function MfaSetupPage() {
 const t = useTranslations('Profile');
 const {
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
 } = useProfileMfaPage(t);

 if (mfaEnabled === null) {
  return <MfaCheckingState label={t('mfa.status_checking')} />;
 }

 return (
  <div className={cn('max-w-2xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000')}>
   <SectionHeader
    tag={t('mfa.tag')}
    tagIcon={<Shield className={cn('w-3 h-3 text-[var(--success)]')} />}
    title={t('mfa.title')}
    subtitle={t('mfa.subtitle')}
   />
   {mfaEnabled ? <MfaEnabledCard title={t('mfa.enabled_title')} description={t('mfa.enabled_desc')} ctaLabel={t('mfa.enabled_cta')} /> : null}
   {!mfaEnabled && !setupData ? <MfaSetupStartCard title={t('mfa.disabled_title')} description={t('mfa.disabled_desc')} errorMessage={setupError} loading={setupLoading} onStartSetup={handleStartSetup} actionLabel={t('mfa.start_setup')} /> : null}
   {!mfaEnabled && setupData ? (
    <MfaSetupFlow
     code={code}
     codePlaceholder={t('mfa.code_placeholder')}
     copyLabel={t('mfa.copy')}
     manualCodeLabel={t('mfa.manual_code_label')}
     onChangeCode={setCode}
     onCopy={copyToClipboard}
     onVerify={handleVerify}
     qrColorOptions={qrColorOptions}
     setupData={setupData}
     step1Title={t('mfa.step1_title')}
     step1Desc={t('mfa.step1_desc')}
     step2Title={t('mfa.step2_title')}
     step2Desc={t('mfa.step2_desc')}
     step3Title={t('mfa.step3_title')}
     step3Desc={t('mfa.step3_desc')}
     verifyError={verifyError}
     verifyLabel={t('mfa.verify_cta')}
     verifyLoading={verifyLoading}
    />
   ) : null}
  </div>
 );
}
