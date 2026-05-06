'use client';

import type { MfaSetupResult } from '@/features/profile/mfa/actions/actions';
import { cn } from '@/lib/utils';
import { MfaBackupCodesCard } from './MfaBackupCodesCard';
import { MfaQrCodeCard } from './MfaQrCodeCard';
import { MfaVerifyCodeCard } from './MfaVerifyCodeCard';
import type { TypedSubmitHandler } from '@/shared/utils/typedSubmit';

interface MfaSetupFlowProps {
 code: string;
 codePlaceholder: string;
 copyLabel: string;
 manualCodeLabel: string;
 onChangeCode: (value: string) => void;
 onCopy: (value: string) => void;
 onVerify: TypedSubmitHandler<{ code: string }>;
 qrColorOptions: Record<string, unknown>;
 setupData: MfaSetupResult;
 step1Desc: string;
 step1Title: string;
 step2Desc: string;
 step2Title: string;
 step3Desc: string;
 step3Title: string;
 verifyError: string;
 verifyLabel: string;
 verifyLoading: boolean;
}

export function MfaSetupFlow(props: MfaSetupFlowProps) {
 return (
  <div className={cn('space-y-8 animate-in slide-in-from-bottom-4 duration-700')}>
   <MfaQrCodeCard copyLabel={props.copyLabel} manualCodeLabel={props.manualCodeLabel} onCopy={props.onCopy} qrCodeUri={props.setupData.qrCodeUri} qrColorOptions={props.qrColorOptions} secretDisplay={props.setupData.secretDisplay} title={props.step1Title} subtitle={props.step1Desc} />
   <MfaBackupCodesCard backupCodes={props.setupData.backupCodes} title={props.step2Title} subtitle={props.step2Desc} />
   <MfaVerifyCodeCard code={props.code} codePlaceholder={props.codePlaceholder} errorMessage={props.verifyError} onChangeCode={props.onChangeCode} onSubmit={props.onVerify} subtitle={props.step3Desc} title={props.step3Title} verifyLabel={props.verifyLabel} verifyLoading={props.verifyLoading} />
  </div>
 );
}
