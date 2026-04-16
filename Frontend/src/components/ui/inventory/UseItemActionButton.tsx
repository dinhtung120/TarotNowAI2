'use client';

/* 
 * Import thành phần Button dùng chung.
 */
import Button from '@/shared/components/ui/Button';
import { cn } from '@/lib/utils';

/**
 * Props cho thành phần UseItemActionButton.
 */
interface UseItemActionButtonProps {
  itemCode: string;
  quantity: number;
  selectedCardId: number | '';
  canSubmit: boolean;
  isPending: boolean;
  useNowLabel: string;
  onUse: (payload: { itemCode: string; quantity: number; targetCardId?: number }) => void;
}

/**
 * UseItemActionButton - Nút bấm thực hiện hành động sử dụng vật phẩm cuối cùng.
 * Sử dụng variant 'brand' để nhấn mạnh đây là hành động chính của modal.
 */
export default function UseItemActionButton({
  itemCode,
  quantity,
  selectedCardId,
  canSubmit,
  isPending,
  useNowLabel,
  onUse,
}: UseItemActionButtonProps) {
  return (
    <div className={cn('pt-4')}>
      <Button
        type="button"
        variant="brand"
        size="xl"
        fullWidth
        isLoading={isPending}
        disabled={!canSubmit}
        className={cn('rounded-2xl font-black uppercase tracking-widest shadow-lg shadow-violet-500/20')}
        onClick={() => onUse({ 
          itemCode, 
          quantity,
          targetCardId: selectedCardId === '' ? undefined : selectedCardId 
        })}
      >
        {useNowLabel}
      </Button>
    </div>
  );
}
