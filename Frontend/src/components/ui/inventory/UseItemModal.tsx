'use client';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';
import InlineErrorAlert from '@/shared/components/ui/InlineErrorAlert';
import UseItemActionButton from '@/components/ui/inventory/UseItemActionButton';
import UseItemCardSelector from '@/components/ui/inventory/UseItemCardSelector';
import UseItemCardPreview from '@/components/ui/inventory/UseItemCardPreview';
import UseItemResultPanel from '@/components/ui/inventory/UseItemResultPanel';
import UseItemQuantitySelector from '@/components/ui/inventory/UseItemQuantitySelector';
import { useUseItemModalController } from '@/components/ui/inventory/useUseItemModalController';
import type { UseItemModalProps } from '@/components/ui/inventory/UseItemModal.types';

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
  const {
   canSubmit,
   closeModal,
   handleUseItem,
   maxQuantity,
   needCard,
   result,
   safeQuantity,
   selectedCard,
   selectedCardId,
   setQuantity,
   setSelectedCardId,
   submitError,
   text,
   useAgain,
  } = useUseItemModalController({
   item,
   locale,
   cardOptions,
   labels,
   onClose,
   onUse,
  });
  if (!item || !text) return null;
  return (
    <Modal isOpen={isOpen} onClose={closeModal} title={text.name} description={text.description} size="md">
      <div className={cn('space-y-6 py-2')}>
        <InlineErrorAlert message={submitError} className={cn('px-4 py-3')} />
        {!result && item.isConsumable ? (
          <div className={cn('animate-in fade-in slide-in-from-top-2 duration-500')}>
            <UseItemQuantitySelector
              label={labels.quantity}
              value={safeQuantity}
              max={item.quantity}
              totalQuantity={item.quantity}
              onChange={(nextQuantity) => setQuantity(Math.max(1, Math.min(nextQuantity, maxQuantity)))}
              disabled={isPending}
            />
          </div>
        ) : null}
        {needCard ? (
          <>
            <UseItemCardSelector label={labels.selectCard} value={selectedCardId} onChange={setSelectedCardId} cardOptions={cardOptions} />
            <UseItemCardPreview card={selectedCard} label={labels.selectedCard} />
          </>
        ) : null}
        {result ? (
          <UseItemResultPanel
            effectSummaries={result.effectSummaries ?? []}
            labels={{
              effectType: labels.effectType,
              rolledValue: labels.rolledValue,
              beforeAfter: labels.beforeAfter,
              done: labels.done,
              useAgain: labels.useAgain,
            }}
            onDone={closeModal}
            onUseAgain={useAgain}
          />
        ) : (
          <UseItemActionButton
            itemCode={item.itemCode}
            quantity={safeQuantity}
            selectedCardId={selectedCardId}
            canSubmit={canSubmit}
            isPending={isPending}
            useNowLabel={labels.useNow}
            onUse={(payload) => void handleUseItem(payload)}
          />
        )}
      </div>
    </Modal>
  );
}
