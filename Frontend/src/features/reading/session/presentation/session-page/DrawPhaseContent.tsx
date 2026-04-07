import FlyingCardsLayer from '@/features/reading/session/presentation/session-page/FlyingCardsLayer';
import PickedCardsStack from '@/features/reading/session/presentation/session-page/PickedCardsStack';
import PickProgress from '@/features/reading/session/presentation/session-page/PickProgress';
import RevealConfirmModal from '@/features/reading/session/presentation/session-page/RevealConfirmModal';
import TarotDeckGrid from '@/features/reading/session/presentation/session-page/TarotDeckGrid';
import type { DrawPhaseContentProps } from '@/features/reading/session/presentation/session-page/DrawPhaseSection.types';
import { cn } from '@/lib/utils';

export default function DrawPhaseContent({ activeDeckRows, cardsToDraw, changeCardText, deckCardWidth, flyingCards, horizontalOverlapFactor, isRevealing, modalDescription, modalRevealText, modalRevealingText, modalTitle, pickedCardCountText, pickedCardSet, pickedCards, pickedDoneText, pickedPromptText, randomSelectText, rowOverlapMargin, stackAnchorRef, onChangeCard, onPickCard, onRandomSelect, onRemovePickedCard, onReveal, setDeckCardRef }: DrawPhaseContentProps) {
  return (
    <div className={cn('w-full animate-in zoom-in-95 duration-700 flex flex-col items-center')}>
      <PickProgress cardsToDraw={cardsToDraw} pickedCount={pickedCards.length} doneText={pickedDoneText} promptText={pickedPromptText} countText={pickedCardCountText} randomText={randomSelectText} onRandomSelect={onRandomSelect} />
      <PickedCardsStack pickedCards={pickedCards} isRevealing={isRevealing} stackAnchorRef={stackAnchorRef} onRemove={onRemovePickedCard} />
      <FlyingCardsLayer flyingCards={flyingCards} />
      <TarotDeckGrid
        activeDeckRows={activeDeckRows}
        cardsToDraw={cardsToDraw}
        deckCardWidth={deckCardWidth}
        horizontalOverlapFactor={horizontalOverlapFactor}
        isRevealing={isRevealing}
        pickedCards={pickedCards}
        pickedCardSet={pickedCardSet}
        rowOverlapMargin={rowOverlapMargin}
        onPickCard={onPickCard}
        setDeckCardRef={setDeckCardRef}
      />
      <RevealConfirmModal
        isOpen={pickedCards.length === cardsToDraw}
        isRevealing={isRevealing}
        title={modalTitle}
        description={modalDescription}
        revealText={modalRevealText}
        revealingText={modalRevealingText}
        changeCardText={changeCardText}
        onReveal={onReveal}
        onChangeCard={onChangeCard}
      />
    </div>
  );
}
