import { memo } from 'react';
import { ArrowLeft, ReceiptText } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';

interface DepositHistoryHeaderProps {
 locale: string;
 tag: string;
 title: string;
 subtitle: string;
 backLabel: string;
}

function DepositHistoryHeaderComponent({
 locale,
 tag,
 title,
 subtitle,
 backLabel,
}: DepositHistoryHeaderProps) {
 return (
  <header className={cn('rounded-3xl border tn-border-soft tn-panel-soft p-6 md:p-8')}>
   <Link
    href={{ pathname: '/wallet/deposit' }}
    locale={locale}
    className={cn('inline-flex min-h-11 items-center gap-2 rounded-xl px-3 py-2 text-xs font-black uppercase tracking-widest tn-text-secondary tn-hover-text-primary tn-hover-surface-soft')}
   >
    <ArrowLeft className={cn('h-4 w-4')} />
    {backLabel}
   </Link>
   <div className={cn('mt-4 flex items-center gap-2 text-xs font-black uppercase tracking-[0.24em] tn-text-secondary')}>
    <ReceiptText className={cn('h-4 w-4')} />
    {tag}
   </div>
   <h1 className={cn('mt-3 text-2xl font-black tn-text-primary md:text-4xl')}>{title}</h1>
   <p className={cn('mt-2 max-w-3xl text-sm tn-text-secondary')}>{subtitle}</p>
  </header>
 );
}

export const DepositHistoryHeader = memo(DepositHistoryHeaderComponent);
DepositHistoryHeader.displayName = 'DepositHistoryHeader';
