import { jsx as a, jsxs as n } from "react/jsx-runtime";
import z from "@/features/reading/session/presentation/session-page/ReadingSessionHeader";
import M from "@/features/reading/session/presentation/session-page/SessionQuestionCard";
import O from "@/features/reading/session/presentation/session-page/DrawPhaseSection";
import q from "@/features/reading/session/presentation/session-page/RevealedCardsGrid";
import B from "@/features/reading/session/presentation/session-page/AiInterpretationPanel";
import { cn as o } from "@/lib/utils";
function G({
  activeDeckRows: l,
  allCardsFlipped: d,
  cards: i,
  cardsToDraw: t,
  deckCardWidth: s,
  error: m,
  flippedIndex: p,
  flyingCards: c,
  getCardImageUrl: g,
  getCardMeaning: f,
  getCardName: C,
  handleChangeCard: v,
  handleRandomSelect: T,
  handleReveal: u,
  horizontalOverlapFactor: k,
  isRevealing: R,
  isShuffling: h,
  onBack: b,
  onPickCard: S,
  onRemovePickedCard: L,
  pickedCardSet: P,
  pickedCards: D,
  question: r,
  rowOverlapMargin: N,
  sessionId: w,
  setDeckCardRef: I,
  shufflePaths: y,
  stackAnchorRef: A,
  texts: e,
  AiInterpretationStream: F,
}) {
  return n("div", {
    className: o("relative z-10 mx-auto h-full max-w-[1600px]"),
    children: [
      a(z, {
        title: e.title,
        backLabel: e.backLabel,
        sessionLabel: e.sessionLabel,
        onBack: b,
      }),
      n("div", {
        className: o(
          "grid grid-cols-1 items-start gap-8",
          i.length > 0 && "md:grid-cols-2",
        ),
        children: [
          n("div", {
            className: o("space-y-8"),
            children: [
              r && a(M, { label: e.questionLabel, question: r }),
              i.length === 0
                ? a(O, {
                    activeDeckRows: l,
                    cardsToDraw: t,
                    deckCardWidth: s,
                    error: m,
                    flyingCards: c,
                    horizontalOverlapFactor: k,
                    isRevealing: R,
                    isShuffling: h,
                    pickedCardCountText: e.pickCountText,
                    pickedCardSet: P,
                    pickedCards: D,
                    pickedDoneText: e.pickDoneText,
                    pickedPromptText: e.pickPromptText,
                    randomSelectText: e.pickRandomText,
                    rowOverlapMargin: N,
                    shufflePaths: y,
                    shuffleSubtitle: e.shuffleSubtitle,
                    shuffleTitle: e.shuffleTitle,
                    stackAnchorRef: A,
                    changeCardText: e.changeCardText,
                    modalDescription: e.modalDescription,
                    modalRevealText: e.modalRevealText,
                    modalRevealingText: e.modalRevealingText,
                    modalTitle: e.modalTitle,
                    onChangeCard: v,
                    onPickCard: S,
                    onRandomSelect: T,
                    onRemovePickedCard: L,
                    onReveal: u,
                    setDeckCardRef: I,
                  })
                : a(q, {
                    cards: i,
                    flippedIndex: p,
                    meaningLabel: e.meaningLabel,
                    getCardImageUrl: g,
                    getCardMeaning: f,
                    getCardName: C,
                  }),
            ],
          }),
          a(B, {
            allCardsFlipped: d,
            cards: i,
            footerNote: e.aiFooterNote,
            liveLabel: e.aiLive,
            sessionId: w,
            subtitle: e.aiSubtitle,
            title: e.aiTitle,
            AiInterpretationStream: F,
          }),
        ],
      }),
    ],
  });
}
export { G as default };
