'use client';

import { Eye, FileText } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderRequestIntroProps {
 introExpand: string;
 introText: string;
 introTitle: string;
 isSelected: boolean;
 onSelectRequest: () => void;
}

export function ReaderRequestIntro({
 introExpand,
 introText,
 introTitle,
 isSelected,
 onSelectRequest,
}: ReaderRequestIntroProps) {
 const text = isSelected || introText.length <= 150 ? introText : `${introText.substring(0, 150)}...`;
 return (
  <div className={cn('p-5 rounded-2xl tn-panel-soft shadow-inner')}>
   <div className={cn('flex items-center justify-between mb-3')}>
    <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary flex items-center gap-2')}>
     <FileText className={cn('w-3.5 h-3.5')} />
     {introTitle}
    </div>
    {introText.length > 150 && !isSelected ? (
     <button type="button" onClick={onSelectRequest} className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent tn-hover-text-primary flex items-center gap-1.5 transition-colors')}>
      <Eye className={cn('w-3.5 h-3.5')} />
      {introExpand}
     </button>
    ) : null}
   </div>
   <p className={cn('text-xs tn-text-secondary leading-relaxed italic border-l-2 tn-border-l-accent-soft pl-4 py-1')}>{text}</p>
  </div>
 );
}
