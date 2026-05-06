import { ArrowDownToLine, Plus, Sparkles } from 'lucide-react';
import { useOptimizedNavigation } from '@/shared/navigation/useOptimizedNavigation';
import { Button, SectionHeader } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface OverviewHeaderProps {
 tag: string;
 title: string;
 subtitle: string;
 depositCta: string;
 withdrawCta: string;
 canWithdraw: boolean;
}

export function OverviewHeader({ tag, title, subtitle, depositCta, withdrawCta, canWithdraw }: OverviewHeaderProps) {
 const navigation = useOptimizedNavigation();

 return (
  <SectionHeader
   tag={tag}
   tagIcon={<Sparkles className={cn('w-3 h-3')} />}
   title={title}
   subtitle={subtitle}
   action={
    <div className={cn('flex w-full gap-3 sm:w-auto')}>
     <Button
      variant="primary"
      onClick={() => navigation.push('/wallet/deposit')}
      className={cn('tn-w-full-auto-sm shadow-2xl')}
     >
      <Plus className={cn('w-4 h-4 mr-2')} />
      {depositCta}
     </Button>
     {canWithdraw ? (
      <Button
       variant="secondary"
       onClick={() => navigation.push('/wallet/withdraw')}
       className={cn('tn-w-full-auto-sm shadow-2xl')}
      >
       <ArrowDownToLine className={cn('w-4 h-4 mr-2')} />
       {withdrawCta}
      </Button>
     ) : null}
    </div>
   }
   className={cn('mb-12 animate-in fade-in slide-in-from-bottom-4 duration-1000')}
  />
 );
}
