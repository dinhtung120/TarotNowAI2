import type { InventoryItem, UseInventoryItemResponse } from '@/shared/infrastructure/inventory/inventoryTypes';
import type { CardOption } from '@/components/ui/inventory/UseItemCardSelector';

export interface UseItemModalLabels {
  title: string;
  useNow: string;
  selectCard: string;
  quantity: string;
  effectValue: string;
  selectedCard: string;
  effectType: string;
  rolledValue: string;
  beforeAfter: string;
  done: string;
  useAgain: string;
  error: string;
}

export interface UseItemModalProps {
  isOpen: boolean;
  item: InventoryItem | null;
  locale: string;
  cardOptions: CardOption[];
  labels: UseItemModalLabels;
  isPending: boolean;
  onClose: () => void;
  onUse: (payload: { itemCode: string; quantity: number; targetCardId?: number }) => Promise<UseInventoryItemResponse>;
}
