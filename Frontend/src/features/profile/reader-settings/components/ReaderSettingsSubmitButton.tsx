'use client';

import { Loader2, Save } from 'lucide-react';
import { Button } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface ReaderSettingsSubmitButtonProps {
 disabled: boolean;
 loadingLabel: string;
 saveLabel: string;
}

export function ReaderSettingsSubmitButton({
 disabled,
 loadingLabel,
 saveLabel,
}: ReaderSettingsSubmitButtonProps) {
 return (
  <Button variant="primary" type="submit" disabled={disabled} className={cn('w-full h-12')}>
   {disabled ? (
    <>
     <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} />
     {loadingLabel}
    </>
   ) : (
    <>
     <Save className={cn('w-4 h-4 mr-2')} />
     {saveLabel}
    </>
   )}
  </Button>
 );
}
