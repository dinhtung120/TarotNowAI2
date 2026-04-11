import { Bot } from 'lucide-react';
import type { HistoryDetailResponse } from '@/features/reading/application/actions/history';
import { LazyMarkdown } from '@/shared/components/LazyMarkdown';
import { cn } from '@/lib/utils';

interface HistoryDetailAiSummaryProps {
 detail: HistoryDetailResponse;
 labels: {
  title: string;
  fallbackDescription: (count: number) => string;
 };
}

export function HistoryDetailAiSummary({
 detail,
 labels,
}: HistoryDetailAiSummaryProps) {
 if (!detail.isCompleted) {
  return null;
 }

 return (
  <div className={cn('mt-32 max-w-4xl mx-auto')}>
   <div className={cn('flex flex-col items-center gap-8 text-center tn-bg-glass border tn-border-soft tn-p-12-16-sm tn-rounded-4xl relative overflow-hidden group')}>
    <div className={cn('absolute -top-24 -left-24 w-64 h-64 tn-bg-accent-10 tn-blur-100 rounded-full')} />
    <div className={cn('absolute -bottom-24 -right-24 w-64 h-64 tn-bg-warning-10 tn-blur-100 rounded-full')} />
    <div className={cn('w-16 h-16 tn-panel rounded-full flex items-center justify-center animate-bounce-slow relative z-10 shadow-xl')}>
     <Bot className={cn('w-8 h-8 tn-text-accent')} />
    </div>
    <div className={cn('relative z-10 w-full text-left')}>
     <h3 className={cn('text-2xl font-black italic tracking-tight tn-text-primary mb-6 uppercase text-center')}>
      {labels.title}
     </h3>
     {detail.aiSummary ? (
      <div className={cn('prose prose-purple max-w-none prose-p:leading-relaxed prose-p:tn-text-secondary prose-headings:font-serif prose-headings:tn-text-warning prose-strong:tn-text-accent prose-strong:font-bold prose-em:tn-text-secondary prose-em:italic prose-li:tn-text-secondary text-left')}>
       <LazyMarkdown>{detail.aiSummary}</LazyMarkdown>
       {detail.followups?.map((followup) => (
        <div key={`${followup.question}-${followup.answer.slice(0, 24)}`} className={cn('mt-6 space-y-4')}>
         <div className={cn('flex justify-end')}>
          <div className={cn('tn-bg-warning-10 border tn-border-warning-20 px-5 py-4 rounded-3xl rounded-tr-none tn-maxw-85p tn-text-warning')}>
           {followup.question}
          </div>
         </div>
         <div className={cn('flex justify-start')}>
          <div className={cn('tn-bg-accent-10 border tn-border-accent-20 px-5 py-4 rounded-3xl rounded-tl-none tn-maxw-85p')}>
           <LazyMarkdown>{followup.answer}</LazyMarkdown>
          </div>
         </div>
        </div>
       ))}
      </div>
     ) : (
      <p className={cn('tn-text-secondary font-medium tn-leading-18 max-w-2xl mx-auto text-center')}>
       {labels.fallbackDescription(detail.aiInteractions.length)}
      </p>
     )}
    </div>
    <div className={cn('h-px w-32 bg-gradient-to-r from-transparent via-purple-500 to-transparent opacity-50 relative z-10')} />
    <div className={cn('flex items-center gap-2 opacity-40 relative z-10')}>
     <div className={cn('w-1.5 h-1.5 tn-bg-accent rounded-full animate-pulse')} />
     <div className={cn('w-1.5 h-1.5 tn-bg-warning rounded-full animate-pulse delay-75')} />
     <div className={cn('w-1.5 h-1.5 bg-white rounded-full animate-pulse delay-150')} />
    </div>
   </div>
  </div>
 );
}
