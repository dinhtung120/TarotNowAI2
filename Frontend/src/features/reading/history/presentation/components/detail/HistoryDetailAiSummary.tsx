import { Bot } from 'lucide-react';
import ReactMarkdown from 'react-markdown';
import type { HistoryDetailResponse } from '@/features/reading/application/actions/history';
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
   <div className={cn('flex flex-col items-center gap-8 text-center bg-[var(--bg-glass)] border tn-border-soft p-12 sm:p-16 rounded-[4rem] relative overflow-hidden group')}>
    <div className={cn('absolute -top-24 -left-24 w-64 h-64 bg-[var(--purple-accent)]/10 blur-[100px] rounded-full group-hover:bg-[var(--purple-accent)]/20 transition-all')} />
    <div className={cn('absolute -bottom-24 -right-24 w-64 h-64 bg-[var(--warning)]/10 blur-[100px] rounded-full group-hover:bg-[var(--warning)]/20 transition-all')} />
    <div className={cn('w-16 h-16 tn-panel rounded-full flex items-center justify-center animate-bounce-slow relative z-10 shadow-xl')}>
     <Bot className={cn('w-8 h-8 text-[var(--purple-accent)]')} />
    </div>
    <div className={cn('relative z-10 w-full text-left')}>
     <h3 className={cn('text-2xl font-black italic tracking-tight tn-text-primary mb-6 uppercase text-center')}>
      {labels.title}
     </h3>
     {detail.aiSummary ? (
      <div className={cn('prose prose-purple max-w-none prose-p:leading-relaxed prose-p:tn-text-secondary prose-headings:font-serif prose-headings:text-[var(--warning)] prose-strong:text-[var(--purple-accent)] prose-strong:font-bold prose-em:tn-text-secondary prose-em:italic prose-li:tn-text-secondary text-left')}>
       <ReactMarkdown>{detail.aiSummary}</ReactMarkdown>
       {detail.followups?.map((followup) => (
        <div key={`${followup.question}-${followup.answer.slice(0, 24)}`} className={cn('mt-6 space-y-4')}>
         <div className={cn('flex justify-end')}>
          <div className={cn('bg-[var(--warning)]/10 border border-[var(--warning)]/20 px-5 py-4 rounded-3xl rounded-tr-none max-w-[85%] text-[var(--warning)]')}>
           {followup.question}
          </div>
         </div>
         <div className={cn('flex justify-start')}>
          <div className={cn('bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 px-5 py-4 rounded-3xl rounded-tl-none max-w-[85%]')}>
           <ReactMarkdown>{followup.answer}</ReactMarkdown>
          </div>
         </div>
        </div>
       ))}
      </div>
     ) : (
      <p className={cn('text-[var(--text-secondary)] font-medium leading-[1.8] max-w-2xl mx-auto text-center')}>
       {labels.fallbackDescription(detail.aiInteractions.length)}
      </p>
     )}
    </div>
    <div className={cn('h-px w-32 bg-gradient-to-r from-transparent via-[var(--purple-accent)] to-transparent opacity-50 relative z-10')} />
    <div className={cn('flex items-center gap-2 opacity-40 relative z-10')}>
     <div className={cn('w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-pulse')} />
     <div className={cn('w-1.5 h-1.5 bg-[var(--warning)] rounded-full animate-pulse delay-75')} />
     <div className={cn('w-1.5 h-1.5 bg-[var(--text-inverse)] rounded-full animate-pulse delay-150')} />
    </div>
   </div>
  </div>
 );
}
