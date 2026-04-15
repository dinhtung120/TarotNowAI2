'use client';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';
import { useUseItemModalState } from '@/shared/infrastructure/inventory/useUseItemModalState';
import UseItemActionButton from '@/components/ui/inventory/UseItemActionButton';
import UseItemCardSelector, { type CardOption } from '@/components/ui/inventory/UseItemCardSelector';
import UseItemStats from '@/components/ui/inventory/UseItemStats';
interface UseItemModalLabels {
 useNow: string;
 selectCard: string;
 quantity: string;
 effectValue: string;
}
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
 const { selectedCardId, setSelectedCardId, text, needCard, canSubmit } = useUseItemModalState({
  item,
  locale,
 });
 if (!item || !text) return null;
 return (
  <Modal isOpen={isOpen} onClose={onClose} title={text.name} description={text.description} size="md">
   <div className={cn('space-y-4')}>
    <UseItemStats
     quantityLabel={labels.quantity}
     effectValueLabel={labels.effectValue}
     quantity={item.quantity}
     effectValue={item.effectValue}
    />
    {needCard ? (
     <UseItemCardSelector
      label={labels.selectCard}
      value={selectedCardId}
      onChange={setSelectedCardId}
      cardOptions={cardOptions}
     />
    ) : null}
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
