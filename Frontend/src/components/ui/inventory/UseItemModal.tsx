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
  item,
  locale,
  cardOptions,
  labels,
  isPending,
  onClose,
  onUse,
}: UseItemModalProps) {
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

  const maxQuantity = Math.max(1, Math.min(10, item.quantity));
  const safeQuantity = Math.max(1, Math.min(quantity, maxQuantity));

  const handleUseItem = async (payload: { itemCode: string; quantity: number; targetCardId?: number }) => {
    try {
      const response = await onUse({
        ...payload,
        quantity: Math.max(1, Math.min(payload.quantity, maxQuantity)),
      });
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
    if (!item.canUse || item.quantity <= 0) {
      return;
    }

    const nextQuantity = safeQuantity;
    setQuantity(nextQuantity);

    void handleUseItem({
      itemCode: item.itemCode,
      quantity: nextQuantity,
      targetCardId: selectedCardId === '' ? undefined : selectedCardId
    });
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
              value={safeQuantity}
              max={item.quantity}
              onChange={(nextQuantity) => {
                setQuantity(Math.max(1, Math.min(nextQuantity, maxQuantity)));
              }}
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
            quantity={safeQuantity}
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
