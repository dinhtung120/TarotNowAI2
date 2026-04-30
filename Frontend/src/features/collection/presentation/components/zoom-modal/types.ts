import type { UserCollectionDto } from "@/features/collection/application/actions";
import type { TarotCardMeta } from "@/shared/domain/tarotData";

export interface CollectionZoomLabels {
  closeLabel: string;
  copiesLabel: string;
  levelLabel: string;
  lockedMeaning: string;
  unknownCard: string;
}

export interface CollectionZoomModalProps {
  cardData: TarotCardMeta | null;
  cardImageUrl?: string;
  cardPreviewImageUrl?: string;
  cardMeaning: string;
  cardName: string;
  labels: CollectionZoomLabels;
  suitLabel: string;
  userCard: UserCollectionDto | null;
  onClose: () => void;
}
