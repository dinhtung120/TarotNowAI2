'use client';

/* 
 * Import thành phần Button dùng chung.
 */
import Button from '@/shared/components/ui/Button';

/**
 * Props cho thành phần UseItemActionButton.
 */
interface UseItemActionButtonProps {
  itemCode: string;
  selectedCardId: number | '';
  canSubmit: boolean;
  isPending: boolean;
  useNowLabel: string;
  onUse: (payload: { itemCode: string; targetCardId?: number }) => void;
}

/**
 * UseItemActionButton - Nút bấm thực hiện hành động sử dụng vật phẩm cuối cùng.
 * Sử dụng variant 'brand' để nhấn mạnh đây là hành động chính của modal.
 */
export default function UseItemActionButton({
  itemCode,
  selectedCardId,
  canSubmit,
  isPending,
  useNowLabel,
  onUse,
}: UseItemActionButtonProps) {
  return (
    <div className="pt-4">
      <Button
        type="button"
        variant="brand"
        size="lg"
        fullWidth
        isLoading={isPending}
        disabled={!canSubmit}
        onClick={() => onUse({ itemCode, targetCardId: selectedCardId === '' ? undefined : selectedCardId })}
      >
        {useNowLabel}
      </Button>
    </div>
  );
}
