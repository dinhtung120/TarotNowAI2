'use client';

import { Eye, FileText } from 'lucide-react';
import type { AdminReaderRequest } from '@/features/admin/reader-requests/actions/reader-requests';
import { cn } from '@/lib/utils';
import ReaderSocialLinksInline from '@/features/reader/shared/ReaderSocialLinksInline';

interface ReaderRequestIntroProps {
 introExpand: string;
 introTitle: string;
 specialtiesTitle: string;
 yearsOfExperienceTitle: string;
 socialLinksTitle: string;
 priceTitle: string;
 request: AdminReaderRequest;
 isSelected: boolean;
 onSelectRequest: () => void;
}

export function ReaderRequestIntro(props: ReaderRequestIntroProps) {
 const text = props.isSelected || props.request.bio.length <= 150
  ? props.request.bio
  : `${props.request.bio.substring(0, 150)}...`;

 return (
  <div className={cn('space-y-4 rounded-2xl tn-panel-soft p-5 shadow-inner')}>
   <div className={cn('flex items-center justify-between')}>
    <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary flex items-center gap-2')}>
     <FileText className={cn('w-3.5 h-3.5')} />
     {props.introTitle}
    </div>
    {props.request.bio.length > 150 && !props.isSelected ? (
     <button type="button" onClick={props.onSelectRequest} className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent tn-hover-text-primary flex items-center gap-1.5 transition-colors')}>
      <Eye className={cn('w-3.5 h-3.5')} />
      {props.introExpand}
     </button>
    ) : null}
   </div>

   <p className={cn('text-xs tn-text-secondary leading-relaxed italic border-l-2 tn-border-l-accent-soft pl-4 py-1')}>{text}</p>

   <div className={cn('grid grid-cols-1 gap-3 md:grid-cols-2')}>
    <div className={cn('text-xs tn-text-secondary')}><span className={cn('font-black uppercase tn-text-tertiary mr-2')}>{props.yearsOfExperienceTitle}:</span>{props.request.yearsOfExperience}+</div>
    <div className={cn('text-xs tn-text-secondary')}><span className={cn('font-black uppercase tn-text-tertiary mr-2')}>{props.priceTitle}:</span>{props.request.diamondPerQuestion}</div>
   </div>

   <div className={cn('text-xs tn-text-secondary')}>
    <span className={cn('font-black uppercase tn-text-tertiary mr-2')}>{props.specialtiesTitle}:</span>
    {props.request.specialties.join(', ')}
   </div>

   <div className={cn('flex items-center gap-2')}>
    <span className={cn('font-black uppercase text-[10px] tn-text-tertiary')}>{props.socialLinksTitle}:</span>
    <ReaderSocialLinksInline
     facebookUrl={props.request.facebookUrl}
     instagramUrl={props.request.instagramUrl}
     tikTokUrl={props.request.tikTokUrl}
    />
   </div>
  </div>
 );
}
