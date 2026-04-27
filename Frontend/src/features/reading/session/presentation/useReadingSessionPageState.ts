import { useCallback, useMemo } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import { getProfileAction } from '@/features/profile/public';
import { generateShufflePaths } from '@/features/reading/session/presentation/session-page/utils';
import type { ReadingSessionContentProps } from '@/features/reading/session/presentation/session-page/ReadingSessionContent.types';
import { useDeckSelection } from '@/features/reading/session/presentation/session-page/useDeckSelection';
import { useRevealReading } from '@/features/reading/session/presentation/session-page/useRevealReading';
import { useSessionSetup } from '@/features/reading/session/presentation/session-page/useSessionSetup';
import { useCardsCatalog } from '@/shared/application/hooks/useCardsCatalog';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import { invalidateUserStateQueries } from '@/shared/infrastructure/query/invalidateUserStateQueries';
import { useAuthStore } from '@/store/authStore';

interface UseReadingSessionPageStateResult {
  contentProps: Omit<ReadingSessionContentProps, 'AiInterpretationStream'>;
}

export function useReadingSessionPageState(sessionId: string): UseReadingSessionPageStateResult {
  const queryClient = useQueryClient();
  const navigation = useOptimizedNavigation();
  const sessionShort = sessionId.split('-')[0] ?? '';
  const t = useTranslations('ReadingSession');
  const tAi = useTranslations('AiInterpretation');
  const updateUser = useAuthStore((state) => state.updateUser);
  const { cardsToDraw, question } = useSessionSetup(sessionId);
  const { getCardImageUrl, getCardMeaning, getCardName } = useCardsCatalog();
  const shufflePaths = useMemo(() => generateShufflePaths(), []);

  const refreshProfile = useCallback(async () => {
    const profileResponse = await getProfileAction();
    if (profileResponse.success && profileResponse.data) {
      updateUser(profileResponse.data);
    }

    await invalidateUserStateQueries(queryClient, [
      'wallet',
      'inventory',
      'collection',
      'readingSetup',
      'readingHistory',
      'profile',
      'gamification',
      'notifications',
    ]);
  }, [queryClient, updateUser]);

  const { allCardsFlipped, cards, error, flippedIndex, isRevealing, revealCards } = useRevealReading({
    sessionId,
    revealFailedMessage: t('errors.reveal_failed'),
    onProfileRefresh: refreshProfile,
  });

  const deckSelection = useDeckSelection({ cardsToDraw, isRevealing });
  const { clearTransientAnimations, pickedCards, removePickedCard } = deckSelection;

  const handleReveal = useCallback(async () => {
    clearTransientAnimations();
    await revealCards();
  }, [clearTransientAnimations, revealCards]);

  const handleChangeCard = useCallback(() => {
    const lastPickedCard = pickedCards[pickedCards.length - 1];
    if (typeof lastPickedCard === 'number') removePickedCard(lastPickedCard);
  }, [pickedCards, removePickedCard]);

  const texts = useMemo(
    () => ({
      aiFooterNote: t('ai.footer_note'),
      aiLive: t('ai.live'),
      aiSubtitle: t('ai.subtitle', { id: sessionId.substring(0, 8) }),
      aiTitle: tAi('title'),
      backLabel: t('header.back_to_setup'),
      changeCardText: t('modal.change_card'),
      meaningLabel: t('cards.meaning_label'),
      reversedLabel: t('cards.orientation_reversed'),
      modalDescription: t('modal.desc'),
      modalRevealText: t('modal.reveal'),
      modalRevealingText: t('modal.revealing'),
      modalTitle: t('modal.title'),
      pickCountText: t('pick.count', { picked: pickedCards.length, total: cardsToDraw }),
      pickDoneText: t('pick.done'),
      pickPromptText: t('pick.prompt', { remaining: cardsToDraw - pickedCards.length }),
      pickRandomText: t('pick.random'),
      questionLabel: t('question.label'),
      sessionLabel: t('header.session', { id: sessionShort }),
      shuffleSubtitle: t('shuffle.cleansing'),
      shuffleTitle: t('shuffle.connecting_title'),
      title: t('header.title'),
      uprightLabel: t('cards.orientation_upright'),
    }),
    [cardsToDraw, pickedCards.length, sessionId, sessionShort, t, tAi],
  );

  return {
    contentProps: {
      activeDeckRows: deckSelection.activeDeckRows,
      allCardsFlipped,
      cards,
      cardsToDraw,
      error,
      flippedIndex,
      flyingCards: deckSelection.flyingCards,
      getCardImageUrl,
      getCardMeaning,
      getCardName,
      handleChangeCard,
      handleRandomSelect: deckSelection.handleRandomSelect,
      handleReveal,
      isRevealing,
      isShuffling: deckSelection.isShuffling,
      onBack: () => navigation.push('/reading'),
      onPickCard: deckSelection.addPickedCard,
      onRemovePickedCard: deckSelection.removePickedCard,
      pickedCardSet: deckSelection.pickedCardSet,
      pickedCards: deckSelection.pickedCards,
      question,
      sessionId,
      setDeckCardRef: deckSelection.setDeckCardRef,
      shufflePaths,
      stackAnchorRef: deckSelection.stackAnchorRef,
      texts,
    },
  };
}
