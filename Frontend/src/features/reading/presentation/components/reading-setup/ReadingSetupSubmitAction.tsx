import { Zap } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReadingSetupSubmitActionProps {
 isInitializing: boolean;
 preparingLabel: string;
 submitLabel: string;
}

export function ReadingSetupSubmitAction({
 isInitializing,
 preparingLabel,
 submitLabel,
}: ReadingSetupSubmitActionProps) {
 return (
  <div className={cn('flex justify-center pt-4 animate-in fade-in zoom-in-95 duration-700 delay-500')}>
   <Button
    type="submit"
    variant="brand"
    size="lg"
    isLoading={isInitializing}
    rightIcon={!isInitializing ? <Zap className={cn('w-5 h-5 ml-2')} /> : undefined}
    className={cn('tn-w-full-auto-sm rounded-full font-black tracking-widest uppercase tn-shadow-reading-submit tn-hover-shadow-reading-submit')}
   >
    {isInitializing ? preparingLabel : submitLabel}
   </Button>
  </div>
 );
}
