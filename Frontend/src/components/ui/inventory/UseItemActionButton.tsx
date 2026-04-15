'use client';

import Button from '@/shared/components/ui/Button';

interface UseItemActionButtonProps {
 itemCode: string;
 selectedCardId: number | '';
 canSubmit: boolean;
 isPending: boolean;
 useNowLabel: string;
 onUse: (payload: { itemCode: string; targetCardId?: number }) => void;
}

export default function UseItemActionButton({
 itemCode,
 selectedCardId,
 canSubmit,
 isPending,
 useNowLabel,
 onUse,
}: UseItemActionButtonProps) {
 return (
  <Button
   type="button"
   variant="primary"
   fullWidth
   isLoading={isPending}
   disabled={!canSubmit}
   onClick={() => onUse({ itemCode, targetCardId: selectedCardId === '' ? undefined : selectedCardId })}
  >
   {useNowLabel}
  </Button>
 );
}
