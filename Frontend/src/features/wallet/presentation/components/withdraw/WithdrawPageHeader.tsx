import { ArrowLeft, Sparkles } from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface WithdrawPageHeaderProps {
 backLabel: string;
 tag: string;
 title: string;
 subtitle: string;
}

export function WithdrawPageHeader({
 backLabel,
 tag,
 title,
 subtitle,
}: WithdrawPageHeaderProps) {
 const router = useRouter();

 return (
  <>
   <button
    type="button"
    onClick={() => router.push('/wallet')}
    className={cn('group flex items-center gap-2 tn-wallet-back-link tn-text-10 font-black uppercase tn-tracking-02 mb-8 w-fit min-h-11 px-2 rounded-xl')}
   >
    <ArrowLeft className={cn('w-3.5 h-3.5 tn-wallet-back-icon')} />
    {backLabel}
   </button>
   <SectionHeader
    tag={tag}
    tagIcon={<Sparkles className={cn('w-3 h-3 tn-text-success')} />}
    title={title}
    subtitle={subtitle}
    className={cn('animate-in fade-in slide-in-from-bottom-4 duration-1000')}
   />
  </>
 );
}
