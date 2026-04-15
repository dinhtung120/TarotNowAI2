'use client';

import { useCallback, useMemo, useState } from 'react';
import { inventoryItemTypes } from '@/shared/infrastructure/inventory/inventoryConstants';
import type { InventoryItem } from '@/shared/infrastructure/inventory/inventoryTypes';

interface LocalizedInventoryText {
 name: string;
 description: string;
}

function resolveLocalizedText(item: InventoryItem, locale: string): LocalizedInventoryText {
 if (locale === 'en') return { name: item.nameEn, description: item.descriptionEn };
 if (locale === 'zh') return { name: item.nameZh, description: item.descriptionZh };
 return { name: item.nameVi, description: item.descriptionVi };
}

interface UseUseItemModalStateParams {
 item: InventoryItem | null;
 locale: string;
}

interface UseUseItemModalStateResult {
 selectedCardId: number | '';
 setSelectedCardId: (value: number | '') => void;
 text: LocalizedInventoryText | null;
 needCard: boolean;
 canSubmit: boolean;
}

export function useUseItemModalState({
 item,
 locale,
}: UseUseItemModalStateParams): UseUseItemModalStateResult {
 const itemKey = item?.itemDefinitionId ?? '';
 const [selectionState, setSelectionState] = useState<{
  itemKey: string;
  selectedCardId: number | '';
 }>({
  itemKey: '',
  selectedCardId: '',
 });

 const selectedCardId = selectionState.itemKey === itemKey ? selectionState.selectedCardId : '';
 const setSelectedCardId = useCallback((value: number | '') => {
  setSelectionState({
   itemKey,
   selectedCardId: value,
  });
 }, [itemKey]);

 const text = useMemo(() => (item ? resolveLocalizedText(item, locale) : null), [item, locale]);
 const needCard = item?.itemType === inventoryItemTypes.cardEnhancer;
 const canSubmit = !!item && item.canUse && (!needCard || selectedCardId !== '');

 return {
  selectedCardId,
  setSelectedCardId,
  text,
  needCard,
  canSubmit,
 };
}
