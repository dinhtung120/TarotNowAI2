'use client';

/* 
 * Import các thành phần và logic xử lý Modal.
 * - Modal: Thành phần nền tảng cho hộp thoại.
 * - useUseItemModalState: Hook quản lý trạng thái logic của modal sử dụng vật phẩm.
 */
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';
import { useUseItemModalState } from '@/shared/infrastructure/inventory/useUseItemModalState';
import UseItemActionButton from '@/components/ui/inventory/UseItemActionButton';
import UseItemCardSelector, { type CardOption } from '@/components/ui/inventory/UseItemCardSelector';
import UseItemStats from '@/components/ui/inventory/UseItemStats';

/**
 * Interface cho các nhãn ngôn ngữ dùng trong Modal.
 */
interface UseItemModalLabels {
  useNow: string;
  selectCard: string;
  quantity: string;
  effectValue: string;
}

/**
 * Props của thành phần UseItemModal.
 */
interface UseItemModalProps {
  isOpen: boolean;
  item: InventoryItem | null;
  locale: string;
  cardOptions: CardOption[];
  labels: UseItemModalLabels;
  isPending: boolean;
  onClose: () => void;
  onUse: (payload: { itemCode: string; targetCardId?: number }) => void;
}

/**
 * UseItemModal - Thành phần hiển thị hộp thoại xác nhận và lựa chọn khi người dùng muốn sử dụng vật phẩm.
 * Thiết kế tinh tế, tích hợp sẵn các chỉ số và bộ chọn mục tiêu.
 */
export default function UseItemModal({
  isOpen,
  item,
  locale,
  cardOptions,
  labels,
  isPending,
  onClose,
  onUse,
}: UseItemModalProps) {
  
  /* Khởi tạo trạng thái logic cho Modal: Tên vật phẩm, nhu cầu chọn card, ... */
  const { selectedCardId, setSelectedCardId, text, needCard, canSubmit } = useUseItemModalState({
    item,
    locale,
  });

  /* Nếu không có vật phẩm hoặc tiêu đề, không hiển thị Modal để tránh lỗi giao diện */
  if (!item || !text) return null;

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={onClose} 
      title={text.name} 
      description={text.description} 
      size="md"
    >
      <div className={cn('space-y-8 py-2')}>
        
        {/* 1. Hiển thị các chỉ số hiện tại của vật phẩm (Số lượng & Hiệu ứng) */}
        <UseItemStats
          quantityLabel={labels.quantity}
          effectValueLabel={labels.effectValue}
          quantity={item.quantity}
          effectValue={item.effectValue}
        />

        {/* 2. Nếu vật phẩm cần tác động lên một lá bài cụ thể, hiển thị bộ chọn */}
        {needCard && (
          <div className="animate-in fade-in slide-in-from-top-2 duration-500">
            <UseItemCardSelector
              label={labels.selectCard}
              value={selectedCardId}
              onChange={setSelectedCardId}
              cardOptions={cardOptions}
            />
          </div>
        )}

        {/* 3. Nút bấm thực hiện hành động Sử dụng ngay */}
        <UseItemActionButton
          itemCode={item.itemCode}
          selectedCardId={selectedCardId}
          canSubmit={canSubmit}
          isPending={isPending}
          useNowLabel={labels.useNow}
          onUse={onUse}
        />
      </div>
    </Modal>
  );
}
