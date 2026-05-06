'use client';

import type { CommunityReportReasonCode } from '@/features/community/shared/types';
import { cn } from '@/lib/utils';

type Translator = (key: string, values?: Record<string, string | number>) => string;

interface PostReportFieldsProps {
 t: Translator;
 reasonCode: CommunityReportReasonCode | '';
 description: string;
 reasonError: string | null;
 descriptionError: string | null;
 isSubmitting: boolean;
 minDescriptionLength: number;
 reasonCodes: readonly CommunityReportReasonCode[];
 onReasonCodeChange: (value: CommunityReportReasonCode) => void;
 onDescriptionChange: (value: string) => void;
}

export function PostReportFields(props: PostReportFieldsProps) {
 const {
  t, reasonCode, description, reasonError, descriptionError, isSubmitting,
  minDescriptionLength, reasonCodes, onReasonCodeChange, onDescriptionChange,
 } = props;

 return (
  <>
   <div className={cn('space-y-2')}>
    <label htmlFor="community-report-reason" className={cn('text-xs font-black uppercase tracking-wider tn-text-muted')}>{t('report_modal.reason_label')}</label>
    <select id="community-report-reason" value={reasonCode} disabled={isSubmitting} onChange={(event) => onReasonCodeChange(event.target.value as CommunityReportReasonCode)} className={cn('tn-input tn-field-accent w-full rounded-xl px-3 py-2 text-sm')}>
     <option value="">{t('report_modal.reason_placeholder')}</option>
     {reasonCodes.map((code) => (
      <option key={code} value={code}>{t(`report_modal.reasons.${code}`)}</option>
     ))}
    </select>
    {reasonError ? <p className={cn('text-xs font-medium tn-text-danger')}>{reasonError}</p> : null}
   </div>

   <div className={cn('space-y-2')}>
    <label htmlFor="community-report-description" className={cn('text-xs font-black uppercase tracking-wider tn-text-muted')}>{t('report_modal.description_label')}</label>
    <textarea id="community-report-description" value={description} disabled={isSubmitting} onChange={(event) => onDescriptionChange(event.target.value)} rows={4} maxLength={500} placeholder={t('report_modal.description_placeholder', { min: minDescriptionLength })} className={cn('tn-input tn-field-accent custom-scrollbar min-h-28 w-full resize-y rounded-xl px-3 py-2 text-sm')} />
    <div className={cn('flex items-center justify-between gap-2')}>
     {descriptionError ? <p className={cn('text-xs font-medium tn-text-danger')}>{descriptionError}</p> : <span />}
     <span className={cn('text-xs tn-text-muted')}>{description.length}/500</span>
    </div>
   </div>
  </>
 );
}
