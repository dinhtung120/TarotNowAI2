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

  const handleUseItem = async (payload: { itemCode: string; targetCardId?: number }) => {
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

  return (
    <Modal
      isOpen={isOpen}
      onClose={handleDone}
      title={text.name}
      description={text.description}
      size="md"
    >
      <div className={cn('space-y-6 py-2')}>
        <UseItemStats
          quantityLabel={labels.quantity}
          effectValueLabel={labels.effectValue}
          quantity={item.quantity}
          effectValue={item.effectValue}
        />
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
        {result?.effectSummary ? (
          <UseItemResultPanel
            effectSummary={result.effectSummary}
            labels={{
              effectType: labels.effectType,
              rolledValue: labels.rolledValue,
              beforeAfter: labels.beforeAfter,
              done: labels.done,
            }}
            onDone={handleDone}
          />
        ) : (
          <UseItemActionButton
            itemCode={item.itemCode}
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
