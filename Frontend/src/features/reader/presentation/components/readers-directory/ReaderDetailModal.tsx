import Image from 'next/image';
import { BriefcaseBusiness, Gem, Loader2, MessageCircle, Star, X } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';
import { ReaderStatusIndicator } from './ReaderStatusIndicator';
import { resolveAvatarUrl } from '@/shared/infrastructure/http/assetUrl';
import ReaderSocialLinksInline from '@/features/reader/presentation/components/ReaderSocialLinksInline';

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

export function ReaderDetailModal({
 reader,
 bio,
 isStartingConversation,
 onClose,
 onStartConversation,
 labels,
}: ReaderDetailModalProps) {
 if (!reader) return null;
 const avatarSrc = resolveAvatarUrl(reader.avatarUrl);

 return (
  <div className={cn('tn-reader-modal-root fixed inset-0 z-[100] animate-in fade-in duration-500 flex flex-col')}>
   <div className={cn('absolute inset-0 tn-overlay-strong')} onClick={onClose} />
   <div className={cn('relative z-10 flex-1 overflow-y-auto custom-scrollbar flex flex-col p-4 pt-24 pb-28 md:p-8')}>
    <div className={cn('tn-reader-modal-panel relative m-auto max-w-2xl w-full tn-panel animate-in zoom-in-95 slide-in-from-bottom-10 duration-500 flex flex-col')}>
     <div className={cn('absolute top-4 right-4 z-20')}>
      <button type="button" onClick={onClose} className={cn('w-9 h-9 rounded-xl tn-surface border tn-border-soft flex items-center justify-center tn-hover-surface-strong shadow-sm')}>
       <X className={cn('w-4 h-4')} />
      </button>
     </div>
     
     {/* Thêm container cuộn cho nội dung bên trong - Ensuring scrollability for long bios */}
     <div className={cn('flex-1 overflow-y-auto custom-scrollbar p-6 md:p-8')}>
      <div className={cn('space-y-6')}>
       <div className={cn('flex items-center gap-4')}>
        <div className={cn('w-16 h-16 rounded-full tn-surface-strong border-2 tn-border flex items-center justify-center text-xl font-black tn-text-primary overflow-hidden relative flex-shrink-0')}>
         {avatarSrc ? <Image src={avatarSrc} alt={reader.displayName} fill sizes="64px" unoptimized className={cn('w-full h-full object-cover bg-white')} /> : reader.displayName?.charAt(0)?.toUpperCase() || '?'}
        </div>
        <div className={cn('min-w-0')}>
         <h3 className={cn('text-2xl font-black italic tracking-tight truncate')}>{reader.displayName || labels.readerFallback}</h3>
         <div className={cn('mt-1')}><ReaderStatusIndicator status={reader.status} labels={{ online: labels.online, busy: labels.busy, offline: labels.offline }} /></div>
        </div>
       </div>

       {/* Phần Bio: Đảm bảo hiển thị đầy đủ và có thể cuộn nếu quá dài - Displaying full bio with proper line height */}
       <div className={cn('space-y-2')}>
        <p className={cn('text-sm md:text-base tn-text-primary leading-relaxed whitespace-pre-wrap')}>{bio}</p>
       </div>

       <div className={cn('flex flex-wrap items-center gap-3')}>
        <div className={cn('flex items-center gap-1.5 tn-surface px-3 py-1.5 rounded-lg border tn-border-soft')}><Star className={cn('w-3.5 h-3.5 tn-text-warning')} fill="currentColor" /><span className={cn('text-xs font-black tn-text-primary')}>{reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}</span><span className={cn('tn-text-10 tn-text-tertiary font-bold')}>({reader.totalReviews})</span></div>
        <div className={cn('flex items-center gap-1.5 tn-bg-accent-10 px-3 py-1.5 rounded-lg border tn-border-accent-20')}><Gem className={cn('w-3.5 h-3.5 tn-text-accent')} /><span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent')}>{reader.diamondPerQuestion} {labels.perQuestionSuffix}</span></div>
        <div className={cn('flex items-center gap-1.5 tn-surface px-3 py-1.5 rounded-lg border tn-border-soft')}><BriefcaseBusiness className={cn('w-3.5 h-3.5 tn-text-secondary')} /><span className={cn('tn-text-10 font-black uppercase tn-text-secondary')}>{reader.yearsOfExperience}+ {labels.yearsExperienceLabel}</span></div>
       </div>

       {reader.specialties.length > 0 ? (
        <div className={cn('flex flex-wrap gap-2')}>
         {reader.specialties.map((spec) => (
          <span key={spec} className={cn('px-3 py-1 rounded-lg tn-surface tn-text-secondary tn-text-10 font-black uppercase tracking-wider border tn-border')}>
           {spec}
          </span>
         ))}
        </div>
       ) : null}
       <ReaderSocialLinksInline
        facebookUrl={reader.facebookUrl}
        instagramUrl={reader.instagramUrl}
        tikTokUrl={reader.tikTokUrl}
        size="md"
       />

       {/* Footer: Cố định ở cuối phần cuộn hoặc có thể tách ra nếu muốn luôn hiển thị - CTA button at the bottom of the content */}
       <div className={cn('pt-6 border-t tn-border-soft flex justify-end')}>
        <Button onClick={onStartConversation} disabled={isStartingConversation} className={cn('w-full sm:w-auto px-6 py-3 font-bold')}>
         {isStartingConversation ? <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} /> : <MessageCircle className={cn('w-4 h-4 mr-2')} />}
         {labels.startConversationCta}
        </Button>
       </div>
      </div>
     </div>
    </div>
   </div>
  </div>
 );
}
