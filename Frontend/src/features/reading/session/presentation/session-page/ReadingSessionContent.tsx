import { cn } from "@/lib/utils";
import AiInterpretationPanel from "@/features/reading/session/presentation/session-page/AiInterpretationPanel";
import DrawPhaseSection from "@/features/reading/session/presentation/session-page/DrawPhaseSection";
import ReadingSessionHeader from "@/features/reading/session/presentation/session-page/ReadingSessionHeader";
import RevealedCardsGrid from "@/features/reading/session/presentation/session-page/RevealedCardsGrid";
import SessionQuestionCard from "@/features/reading/session/presentation/session-page/SessionQuestionCard";
import type { ReadingSessionContentProps } from "@/features/reading/session/presentation/session-page/ReadingSessionContent.types";

export default function ReadingSessionContent({ activeDeckRows, allCardsFlipped, cards, cardsToDraw, deckCardWidth, error, flippedIndex, flyingCards, getCardImageUrl, getCardMeaning, getCardName, handleChangeCard, handleRandomSelect, handleReveal, horizontalOverlapFactor, isRevealing, isShuffling, onBack, onPickCard, onRemovePickedCard, pickedCardSet, pickedCards, question, rowOverlapMargin, sessionId, setDeckCardRef, shufflePaths, stackAnchorRef, texts, AiInterpretationStream }: ReadingSessionContentProps) {
 return (
  <div className={cn("relative z-10 mx-auto h-full tn-maxw-1600")}>
   <ReadingSessionHeader title={texts.title} backLabel={texts.backLabel} sessionLabel={texts.sessionLabel} onBack={onBack} />
   <div className={cn(cards.length > 0 ? "tn-grid-cols-1-2-md" : "grid grid-cols-1", "items-start gap-8")}>
    <div className={cn("space-y-8")}>
     {question ? <SessionQuestionCard label={texts.questionLabel} question={question} /> : null}
     {cards.length === 0 ? <DrawPhaseSection activeDeckRows={activeDeckRows} cardsToDraw={cardsToDraw} deckCardWidth={deckCardWidth} error={error} flyingCards={flyingCards} horizontalOverlapFactor={horizontalOverlapFactor} isRevealing={isRevealing} isShuffling={isShuffling} pickedCardCountText={texts.pickCountText} pickedCardSet={pickedCardSet} pickedCards={pickedCards} pickedDoneText={texts.pickDoneText} pickedPromptText={texts.pickPromptText} randomSelectText={texts.pickRandomText} rowOverlapMargin={rowOverlapMargin} shufflePaths={shufflePaths} shuffleSubtitle={texts.shuffleSubtitle} shuffleTitle={texts.shuffleTitle} stackAnchorRef={stackAnchorRef} changeCardText={texts.changeCardText} modalDescription={texts.modalDescription} modalRevealText={texts.modalRevealText} modalRevealingText={texts.modalRevealingText} modalTitle={texts.modalTitle} onChangeCard={handleChangeCard} onPickCard={onPickCard} onRandomSelect={handleRandomSelect} onRemovePickedCard={onRemovePickedCard} onReveal={handleReveal} setDeckCardRef={setDeckCardRef} /> : <RevealedCardsGrid cards={cards} flippedIndex={flippedIndex} meaningLabel={texts.meaningLabel} getCardImageUrl={getCardImageUrl} getCardMeaning={getCardMeaning} getCardName={getCardName} />}
    </div>
    <AiInterpretationPanel allCardsFlipped={allCardsFlipped} cards={cards} footerNote={texts.aiFooterNote} liveLabel={texts.aiLive} sessionId={sessionId} subtitle={texts.aiSubtitle} title={texts.aiTitle} AiInterpretationStream={AiInterpretationStream} />
   </div>
  </div>
 );
}
