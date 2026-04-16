'use client';

import { useMemo, useState } from 'react';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';
import { useUseItemModalState } from '@/shared/infrastructure/inventory/useUseItemModalState';
import UseItemActionButton from '@/components/ui/inventory/UseItemActionButton';
import UseItemCardSelector from '@/components/ui/inventory/UseItemCardSelector';
import UseItemStats from '@/components/ui/inventory/UseItemStats';
import UseItemCardPreview from '@/components/ui/inventory/UseItemCardPreview';
import UseItemResultPanel from '@/components/ui/inventory/UseItemResultPanel';
import UseItemQuantitySelector from '@/components/ui/inventory/UseItemQuantitySelector';
import type { UseItemModalProps } from '@/components/ui/inventory/UseItemModal.types';
import type { UseInventoryItemResponse } from '@/shared/infrastructure/inventory/inventoryTypes';

export default function UseItemModal({
  isOpen,
  item: initialItem,
  locale,
  cardOptions,
  labels,
  isPending,
  onClose,
  onUse,
}: UseItemModalProps) {
  // Giữ một bản copy của item để tránh bị mất dữ liệu khi inventory refresh
  const [item] = useState(initialItem);
  const [quantity, setQuantity] = useState(1);
  const [result, setResult] = useState<UseInventoryItemResponse | null>(null);
  const { selectedCardId, setSelectedCardId, text, needCard, canSubmit } = useUseItemModalState({
    item,
    locale,
  });

  const selectedCard = useMemo(
    () => cardOptions.find((card) => card.id === selectedCardId) ?? null,
    [cardOptions, selectedCardId],
  );

  if (!item || !text) {
    return null;
  }

  const handleUseItem = async (payload: { itemCode: string; quantity: number; targetCardId?: number }) => {
    try {
      const response = await onUse(payload);
      setResult(response);
    } catch (error) {
      console.error('Failed to use inventory item', error);
    }
  };

  const handleDone = () => {
    setResult(null);
    onClose();
  };

  const handleUseAgain = () => {
    setResult(null);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={handleDone}
      title={text.name}
      description={text.description}
      size="md"
    >
      <div className={cn('space-y-6 py-2')}>
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
          <UseItemStats
            quantityLabel={labels.quantity}
            quantity={item.quantity}
          />
          {item.isConsumable && !result ? (
            <UseItemQuantitySelector
              label={labels.quantity}
              value={quantity}
              max={item.quantity}
              onChange={setQuantity}
              disabled={isPending}
            />
          ) : null}
        </div>

        {needCard ? (
          <>
            <UseItemCardSelector
              label={labels.selectCard}
              value={selectedCardId}
              onChange={setSelectedCardId}
              cardOptions={cardOptions}
            />
            <UseItemCardPreview card={selectedCard} label={labels.selectedCard} />
          </>
        ) : null}

        {result?.effectSummaries && result.effectSummaries.length > 0 ? (
          <UseItemResultPanel
            effectSummaries={result.effectSummaries}
            labels={{
              effectType: labels.effectType,
              rolledValue: labels.rolledValue,
              beforeAfter: labels.beforeAfter,
              done: labels.done,
              useAgain: labels.useAgain,
            }}
            onDone={handleDone}
            onUseAgain={handleUseAgain}
          />
        ) : (
          <UseItemActionButton
            itemCode={item.itemCode}
            quantity={quantity}
            selectedCardId={selectedCardId}
            canSubmit={canSubmit}
            isPending={isPending}
            useNowLabel={labels.useNow}
            onUse={(payload) => {
              void handleUseItem(payload);
            }}
          />
        )}
      </div>
    </Modal>
  );
}
