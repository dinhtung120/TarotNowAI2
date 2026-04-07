import { Plus, Sparkles } from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { Button, SectionHeader } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface OverviewHeaderProps {
 tag: string;
 title: string;
 subtitle: string;
 depositCta: string;
}

export function OverviewHeader({ tag, title, subtitle, depositCta }: OverviewHeaderProps) {
 const router = useRouter();

 return (
  <SectionHeader
   tag={tag}
   tagIcon={<Sparkles className={cn('w-3 h-3')} />}
   title={title}
   subtitle={subtitle}
   action={
    <Button
     variant="primary"
     onClick={() => router.push('/wallet/deposit')}
     className={cn('tn-w-full-auto-sm shadow-2xl')}
    >
     <Plus className={cn('w-4 h-4 mr-2')} />
     {depositCta}
    </Button>
   }
   className={cn('mb-12 animate-in fade-in slide-in-from-bottom-4 duration-1000')}
  />
 );
}
