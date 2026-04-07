import { ArrowLeft, Zap } from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface DepositHeaderProps {
 backLabel: string;
 tag: string;
 title: string;
 subtitle: string;
}

export function DepositHeader({ backLabel, tag, title, subtitle }: DepositHeaderProps) {
 const router = useRouter();

 return (
  <>
   <button
    type="button"
    onClick={() => router.push('/wallet')}
    className={cn('group flex items-center gap-2 tn-wallet-back-link tn-text-10 font-black uppercase tn-tracking-02 w-fit min-h-11 px-2 rounded-xl')}
   >
    <ArrowLeft className={cn('w-3.5 h-3.5 tn-wallet-back-icon')} />
    {backLabel}
   </button>

   <SectionHeader
    tag={tag}
    tagIcon={<Zap className={cn('w-3 h-3 tn-text-warning')} />}
    title={title}
    subtitle={subtitle}
    className={cn('animate-in fade-in slide-in-from-bottom-4 duration-1000')}
   />
  </>
 );
}
