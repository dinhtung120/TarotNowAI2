import { useCardsCatalog } from '@/shared/application/hooks/useCardsCatalog';
import { cn } from '@/lib/utils';
import { HistoryDetailCardItem } from './HistoryDetailCardItem';

interface HistoryDetailCardsGridProps {
 parsedCards: number[];
 essenceLabel: string;
 fallbackCardName: (index: number) => string;
}

export function HistoryDetailCardsGrid({
 parsedCards,
 essenceLabel,
 fallbackCardName,
}: HistoryDetailCardsGridProps) {
 const { getCardImageUrl, getCardMeaning, getCardName } = useCardsCatalog();

 return (
  <div className={cn('grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-8 lg:gap-10')}>
   {parsedCards.map((cardId, index) => (
    <HistoryDetailCardItem
     key={`history-card-${cardId}`}
     index={index}
     cardName={getCardName(cardId) ?? fallbackCardName(cardId + 1)}
     cardMeaning={getCardMeaning(cardId) ?? ''}
     cardImageUrl={getCardImageUrl(cardId)}
     essenceLabel={essenceLabel}
    />
   ))}
  </div>
 );
}
