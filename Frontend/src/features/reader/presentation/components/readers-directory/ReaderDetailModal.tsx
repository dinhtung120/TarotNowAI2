import Modal from '@/shared/components/ui/Modal';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';
import ReaderSocialLinksInline from '@/features/reader/presentation/components/ReaderSocialLinksInline';
import { ReaderDetailActions } from '@/features/reader/presentation/components/readers-directory/ReaderDetailActions';
import { ReaderDetailHeader } from '@/features/reader/presentation/components/readers-directory/ReaderDetailHeader';
import { ReaderDetailStats } from '@/features/reader/presentation/components/readers-directory/ReaderDetailStats';

interface ReaderDetailModalProps {
 reader: ReaderProfile | null;
 bio: string;
 isStartingConversation: boolean;
 onClose: () => void;
 onStartConversation: () => Promise<void>;
 labels: {
  readerFallback: string;
  perQuestionSuffix: string;
  yearsExperienceLabel: string;
  startConversationCta: string;
  online: string;
  busy: string;
  offline: string;
 };
}

export function ReaderDetailModal({ reader, bio, isStartingConversation, onClose, onStartConversation, labels }: ReaderDetailModalProps) {
 if (!reader) return null;

 return (
  <Modal isOpen={Boolean(reader)} onClose={onClose} title={reader.displayName || labels.readerFallback} size="lg" className={cn('tn-reader-modal-panel')}>
   <div className={cn('custom-scrollbar max-h-[72vh] space-y-6 overflow-y-auto pr-1')}>
    <ReaderDetailHeader displayName={reader.displayName} avatarUrl={reader.avatarUrl} status={reader.status} fallbackLabel={labels.readerFallback} labels={{ online: labels.online, busy: labels.busy, offline: labels.offline }} />
    <p className={cn('whitespace-pre-wrap text-sm leading-relaxed tn-text-primary md:text-base')}>{bio}</p>
    <ReaderDetailStats avgRating={reader.avgRating} totalReviews={reader.totalReviews} diamondPerQuestion={reader.diamondPerQuestion} yearsOfExperience={reader.yearsOfExperience} perQuestionSuffix={labels.perQuestionSuffix} yearsExperienceLabel={labels.yearsExperienceLabel} />
    {reader.specialties.length > 0 ? (
     <div className={cn('flex flex-wrap gap-2')}>
      {reader.specialties.map((spec) => <span key={spec} className={cn('rounded-lg border tn-border px-3 py-1 tn-surface tn-text-10 font-black uppercase tracking-wider tn-text-secondary')}>{spec}</span>)}
     </div>
    ) : null}
    <ReaderSocialLinksInline facebookUrl={reader.facebookUrl} instagramUrl={reader.instagramUrl} tikTokUrl={reader.tikTokUrl} size="md" />
    <ReaderDetailActions isStartingConversation={isStartingConversation} ctaLabel={labels.startConversationCta} onStartConversation={onStartConversation} />
   </div>
  </Modal>
 );
}
