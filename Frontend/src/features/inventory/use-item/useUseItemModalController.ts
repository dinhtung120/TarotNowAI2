'use client';

import { useCallback, useMemo, useState } from 'react';
import { resolveErrorMessage } from '@/shared/application/utils/resolveErrorMessage';
import { useUseItemModalState } from '@/features/inventory/use-item/useUseItemModalState';
import type { UseItemModalLabels } from '@/features/inventory/use-item/UseItemModal.types';
import type { CardOption } from '@/features/inventory/shared/cardOption';
import type { InventoryItem, UseInventoryItemResponse } from '@/features/inventory/shared/inventoryTypes';

interface UseUseItemModalControllerOptions {
 item: InventoryItem | null;
 locale: string;
 cardOptions: CardOption[];
 labels: UseItemModalLabels;
 onClose: () => void;
 onUse: (payload: { itemCode: string; quantity: number; targetCardId?: number }) => Promise<UseInventoryItemResponse>;
}

export function useUseItemModalController({
 item,
 locale,
 cardOptions,
 labels,
 onClose,
 onUse,
}: UseUseItemModalControllerOptions) {
 const [quantity, setQuantity] = useState(1);
 const [result, setResult] = useState<UseInventoryItemResponse | null>(null);
 const [submitError, setSubmitError] = useState<string | null>(null);
 const { selectedCardId, setSelectedCardId, text, needCard, canSubmit } = useUseItemModalState({ item, locale });
 const selectedCard = useMemo(
  () => cardOptions.find((card) => card.id === selectedCardId) ?? null,
  [cardOptions, selectedCardId],
 );

 const maxQuantity = Math.max(1, Math.min(10, item?.quantity ?? 1));
 const safeQuantity = Math.max(1, Math.min(quantity, maxQuantity));

 const handleUseItem = useCallback(async (payload: { itemCode: string; quantity: number; targetCardId?: number }) => {
  setSubmitError(null);
  try {
   const response = await onUse({ ...payload, quantity: Math.max(1, Math.min(payload.quantity, maxQuantity)) });
   setResult(response);
  } catch (error) {
   setSubmitError(resolveErrorMessage(error, labels.error));
  }
 }, [labels.error, maxQuantity, onUse]);

 const closeModal = useCallback(() => {
  setResult(null);
  setSubmitError(null);
  onClose();
 }, [onClose]);

 const useAgain = useCallback(() => {
  if (!item?.canUse || item.quantity <= 0) {
   setResult(null);
   setSubmitError(null);
   return;
  }

  setResult(null);
  setSubmitError(null);
  setQuantity(safeQuantity);
  void handleUseItem({
   itemCode: item.itemCode,
   quantity: safeQuantity,
   targetCardId: selectedCardId === '' ? undefined : selectedCardId,
  });
 }, [handleUseItem, item, safeQuantity, selectedCardId]);

 return {
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
 };
}
