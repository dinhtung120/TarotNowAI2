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
    className={cn('w-full sm:w-auto rounded-full font-black tracking-widest uppercase shadow-[0_10px_40px_var(--c-255-255-255-15)] hover:shadow-[0_15px_50px_var(--c-255-255-255-25)]')}
   >
    {isInitializing ? preparingLabel : submitLabel}
   </Button>
  </div>
 );
}
