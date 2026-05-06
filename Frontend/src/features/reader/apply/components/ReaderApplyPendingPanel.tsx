import { Clock } from 'lucide-react';
import { isReaderSpecialtyValue } from '@/features/reader/shared/readerSpecialties';
import type { MyReaderRequest } from '@/features/reader/shared';
import { ReaderApplyStatusPanel } from '@/features/reader/apply/components/ReaderApplyStatusPanel';

type TranslateFn = (key: string, values?: Record<string, string | number | Date>) => string;

interface ReaderApplyPendingPanelProps {
 existingRequest: MyReaderRequest;
 locale: string;
 t: TranslateFn;
}

export function ReaderApplyPendingPanel({
 existingRequest,
 locale,
 t,
}: ReaderApplyPendingPanelProps) {
 const specialtyLabel = (existingRequest.specialties ?? [])
  .map((item) => (isReaderSpecialtyValue(item) ? t(`form.specialties.${item}`) : item))
  .join(', ');
 const socialLabel = [
  existingRequest.facebookUrl,
  existingRequest.instagramUrl,
  existingRequest.tikTokUrl,
 ].filter(Boolean).join(' • ');

 return (
  <ReaderApplyStatusPanel
   accent="warning"
   icon={Clock}
   title={t('pending.title')}
   description={t('pending.desc')}
   introLabel={t('pending.intro_label')}
   introText={existingRequest.bio}
   footerLabel={t('pending.sent_at', {
    date: new Date(existingRequest.createdAt || '').toLocaleString(locale),
   })}
   details={[
    { label: t('form.specialties_label'), value: specialtyLabel || t('form.not_provided') },
    { label: t('form.years_experience_label'), value: `${existingRequest.yearsOfExperience ?? '--'}` },
    { label: t('form.diamond_per_question_label'), value: `${existingRequest.diamondPerQuestion ?? '--'}` },
    { label: t('form.social_links_label'), value: socialLabel || t('form.not_provided') },
   ]}
  />
 );
}
