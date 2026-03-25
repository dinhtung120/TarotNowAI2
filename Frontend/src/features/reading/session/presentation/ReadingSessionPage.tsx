/*
 * ===================================================================
 * FILE: reading/session/[id]/page.tsx (Phiên Trải Bài Tương Tác)
 * BỐI CẢNH (CONTEXT):
 *   Giao diện tương tác chính khi bốc bài Tarot và nhận giải nghĩa từ AI.
 * 
 * KIẾN TRÚC & HOẠT ẢNH (ANIMATION):
 *   - Hiệu ứng `shuffle` (xào bài) -> Dàn bài (`deckRowsDesktop/Mobile`).
 *   - Cho phép User tự chọn lá bài bằng tay, hoặc auto Random.
 *   - Lá bài bốc xong sẽ bay (FlyingCard) vào stack riêng.
 *   - Khi đã bốc đủ, gọi `revealReadingSession` để chốt kết quả, sau đó lật bài.
 *   - Sau khi bài lật hết, render `AiInterpretationStream` để AI bắt đầu streaming kết quả.
 * ===================================================================
 */
"use client";

import dynamic from "next/dynamic";
import { Suspense, useEffect, useMemo, useRef, useState } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { revealReadingSession } from "@/features/reading/application/actions";
import { TAROT_CARD_COUNT } from "@/shared/domain/tarotData";
import { Sparkles, ArrowLeft, RefreshCw, Dices } from "lucide-react";
import { useTranslations } from "next-intl";
import { useCardsCatalog } from "@/shared/application/hooks/useCardsCatalog";
import {
 getSessionStorageItem,
 getSessionStorageNumber,
} from "@/shared/infrastructure/storage/browserStorage";

// Import actions và store để đồng bộ Level/EXP sau khi bốc bài
import { getProfileAction } from "@/features/profile/public";
import { useAuthStore } from "@/store/authStore";

const AiInterpretationStream = dynamic(
 () => import("@/features/reading/presentation/components/AiInterpretationStream"),
 { loading: () => null },
);
const AstralBackground = dynamic(
 () => import("@/shared/components/layout/AstralBackground"),
 { ssr: false, loading: () => null },
);

const SHUFFLE_CARD_COUNT = 9;
const DECK_WAVE_Y_AMPLITUDE = 6;
const DECK_WAVE_ROTATION_AMPLITUDE = 7;
const DECK_CARDS_PER_ROW_DESKTOP = 26; // 78 / 3 rows
const DECK_CARDS_PER_ROW_MOBILE = 13; // 78 / 6 rows
const DECK_HORIZONTAL_OVERLAP_FACTOR = 0.5;
const DECK_VERTICAL_OVERLAP_FACTOR = 0.2;
const CARD_ASPECT_HEIGHT_RATIO = 1.5; // aspect 2/3 => height = width * 1.5
const PICKED_STACK_CARD_WIDTH = 72;
const PICKED_STACK_CARD_HEIGHT = 108;
const PICKED_STACK_TOP_OFFSET = 96;
const PICKED_STACK_RIGHT_OFFSET = 24;
const PICKED_STACK_X_STEP = 2;
const PICKED_STACK_Y_STEP = 6;

interface FlyingCard {
 key: string;
 startX: number;
 startY: number;
 deltaX: number;
 deltaY: number;
 rotate: number;
 stackIndex: number;
}

// Generate positions for an elegant fanning and splitting shuffle effect
const generateShufflePaths = () => {
 return Array.from({ length: SHUFFLE_CARD_COUNT }).map((_, i) => {
 // We'll divide cards into left hand (negative) and right hand (positive)
 const isLeft = i % 2 === 0;
 const directionMultiplier = isLeft ? -1 : 1;
 const fanAngle = (i / SHUFFLE_CARD_COUNT) * 40 - 20; // Fan from -20deg to 20deg
 const offset = 100 * directionMultiplier;

 return {
 tx: `${offset}px`,
 ty: `${Math.abs(fanAngle)}px`,
 r: `${fanAngle}deg`,
 tx2: `${offset * 0.5}px`,
 ty2: `-${Math.abs(fanAngle) * 2}px`,
 r2: `${fanAngle * 0.5}deg`,
 delay: `${i * 0.15}s`,
 duration: "2s",
 z: i
 };
 });
};

const pickRandomIndexes = (availableIndexes: number[], count: number) => {
 const shuffled = [...availableIndexes];
 for (let i = shuffled.length - 1; i > 0; i--) {
 const j = Math.floor(Math.random() * (i + 1));
 [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
 }
 return shuffled.slice(0, count);
};

const chunkDeckRows = (indexes: number[], cardsPerRow: number) => {
 const rows: number[][] = [];
 for (let i = 0; i < indexes.length; i += cardsPerRow) {
 rows.push(indexes.slice(i, i + cardsPerRow));
 }
 return rows;
};

export default function ReadingSessionPage() {
 const params = useParams();
 const router = useRouter();
 const sessionId = params.id as string;
 const t = useTranslations("ReadingSession");
 const tAi = useTranslations("AiInterpretation");
 const { getCardImageUrl, getCardName, getCardMeaning } = useCardsCatalog();
 const sessionShort = sessionId.split("-")[0];

 const updateUser = useAuthStore((s) => s.updateUser); // Hook cập nhật Profile Store

 const [cards, setCards] = useState<number[]>([]);

 const [isRevealing, setIsRevealing] = useState(false);
 const [error, setError] = useState("");
 const [flippedIndex, setFlippedIndex] = useState<number>(-1); // Index của lá bài đang được lật
 const [pickedCards, setPickedCards] = useState<number[]>([]);
 const [flyingCards, setFlyingCards] = useState<FlyingCard[]>([]);
 const [isShuffling, setIsShuffling] = useState(true);
 const [isMobileViewport, setIsMobileViewport] = useState(false);
 const [shufflePaths] = useState<Record<string, string | number>[]>(generateShufflePaths);
 const flipTimersRef = useRef<number[]>([]);
 const animationTimersRef = useRef<number[]>([]);
 const flightSequenceRef = useRef(0);
 const pickedCardsRef = useRef<number[]>([]);
 const deckCardRefs = useRef<Map<number, HTMLDivElement>>(new Map());
 const stackAnchorRef = useRef<HTMLDivElement | null>(null);

 useEffect(() => {
 const timer = window.setTimeout(() => setIsShuffling(false), 2200);
 return () => clearTimeout(timer);
 }, []);

 useEffect(() => {
 const mediaQuery = window.matchMedia("(max-width: 639px)");
 const updateViewport = () => setIsMobileViewport(mediaQuery.matches);
 updateViewport();
 mediaQuery.addEventListener("change", updateViewport);
 return () => mediaQuery.removeEventListener("change", updateViewport);
 }, []);

 useEffect(() => {
 pickedCardsRef.current = pickedCards;
 }, [pickedCards]);

 useEffect(() => {
 return () => {
 flipTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
 flipTimersRef.current = [];
 animationTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
 animationTimersRef.current = [];
 };
 }, []);

 // Restore data từ sessionStorage (SSR-safe)
 const question = getSessionStorageItem(`question_${sessionId}`, "");
 const cardsToDraw = getSessionStorageNumber(`cardsToDraw_${sessionId}`, 1);
 const deckIndexes = useMemo(
 () => Array.from({ length: TAROT_CARD_COUNT }, (_, index) => index),
 [],
 );
 const deckRowsDesktop = useMemo(
 () => chunkDeckRows(deckIndexes, DECK_CARDS_PER_ROW_DESKTOP),
 [deckIndexes],
 );
 const deckRowsMobile = useMemo(
 () => chunkDeckRows(deckIndexes, DECK_CARDS_PER_ROW_MOBILE),
 [deckIndexes],
 );
 const activeDeckRows = isMobileViewport ? deckRowsMobile : deckRowsDesktop;
 const deckCardWidth = isMobileViewport
 ? "clamp(30px, 8.4vw, 42px)"
 : "clamp(56px, 6.8vw, 98px)";
 const rowOverlapMargin = `calc(var(--deck-card-w) * -${DECK_VERTICAL_OVERLAP_FACTOR * CARD_ASPECT_HEIGHT_RATIO})`;
 const pickedCardSet = useMemo(() => new Set(pickedCards), [pickedCards]);

 const getStackPlacement = (stackIndex: number) => {
 const clampedIndex = Math.min(stackIndex, 7);
 return {
 xOffset: clampedIndex * PICKED_STACK_X_STEP,
 yOffset: clampedIndex * PICKED_STACK_Y_STEP,
 rotate: -8 + (clampedIndex % 5) * 2,
 };
 };

 const getStackTarget = (stackIndex: number) => {
 const { xOffset, yOffset, rotate } = getStackPlacement(stackIndex);
 const anchorRect = stackAnchorRef.current?.getBoundingClientRect();
 if (anchorRect) {
 return {
 x: anchorRect.left + PICKED_STACK_CARD_WIDTH / 2 + xOffset,
 y: anchorRect.top + PICKED_STACK_CARD_HEIGHT / 2 + yOffset,
 rotate,
 };
 }
 return {
 x: window.innerWidth - PICKED_STACK_RIGHT_OFFSET - PICKED_STACK_CARD_WIDTH / 2 + xOffset,
 y: PICKED_STACK_TOP_OFFSET + PICKED_STACK_CARD_HEIGHT / 2 + yOffset,
 rotate,
 };
 };

 const launchCardToStack = (cardId: number, stackIndex: number, sourceElement?: HTMLDivElement | null) => {
 const sourceRect = sourceElement?.getBoundingClientRect();
 const startX = sourceRect ? sourceRect.left + sourceRect.width / 2 : window.innerWidth / 2;
 const startY = sourceRect ? sourceRect.top + sourceRect.height / 2 : window.innerHeight * 0.72;
 const target = getStackTarget(stackIndex);

 flightSequenceRef.current += 1;
 const flight: FlyingCard = {
 key: `${cardId}-${flightSequenceRef.current}`,
 startX,
 startY,
 deltaX: target.x - startX,
 deltaY: target.y - startY,
 rotate: target.rotate,
 stackIndex,
 };

 setFlyingCards((prev) => [...prev, flight]);
 const timerId = window.setTimeout(() => {
 setFlyingCards((prev) => prev.filter((item) => item.key !== flight.key));
 }, 460);
 animationTimersRef.current.push(timerId);
 };

 const setDeckCardRef = (cardId: number) => (node: HTMLDivElement | null) => {
 if (node) {
 deckCardRefs.current.set(cardId, node);
 return;
 }
 deckCardRefs.current.delete(cardId);
 };

 const addPickedCard = (cardId: number, sourceElement?: HTMLDivElement | null) => {
 if (isRevealing) return;

 const currentPicks = pickedCardsRef.current;
 if (currentPicks.includes(cardId) || currentPicks.length >= cardsToDraw) return;

 launchCardToStack(cardId, currentPicks.length, sourceElement);
 const nextPicks = [...currentPicks, cardId];
 pickedCardsRef.current = nextPicks;
 setPickedCards(nextPicks);
 };

 const removePickedCard = (cardId: number) => {
 if (isRevealing) return;

 const currentPicks = pickedCardsRef.current;
 if (!currentPicks.includes(cardId)) return;

 const nextPicks = currentPicks.filter((pickedId) => pickedId !== cardId);
 pickedCardsRef.current = nextPicks;
 setPickedCards(nextPicks);
 };

 const handleReveal = async () => {
 setIsRevealing(true);
 setError("");
 flipTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
 flipTimersRef.current = [];
 animationTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
 animationTimersRef.current = [];
 setFlyingCards([]);
 setFlippedIndex(-1);

 const response = await revealReadingSession({ sessionId });

 if (response.success && response.data) {
 setCards(response.data.cards);


 // Auto-flip từng lá bài một với delay
 response.data.cards.forEach((_, idx) => {
 const timerId = window.setTimeout(() => {
 setFlippedIndex(idx);
 }, (idx + 1) * 800);
 flipTimersRef.current.push(timerId);
 });

 // ==========================================
 // ĐỒNG BỘ EXP & LEVEL (Gamification Sync)
 // ==========================================
 // Sau khi bốc bài thành công, gọi API lấy Profile mới nhất để cập nhật Navbar
 // Tránh việc EXP bị đứng im do Store chỉ lưu dữ liệu cũ lúc Login.
 const profileResponse = await getProfileAction();
 if (profileResponse.success && profileResponse.data) {
 updateUser(profileResponse.data);
 }

 } else {
 setError(response.error || t("errors.reveal_failed"));
 }
 setIsRevealing(false);
 };

 /**
 * Tự động chọn ngẫu nhiên các lá bài còn thiếu.
 * Logic: Lấy bộ index 0-77, loại trừ các lá đã chọn, * xáo trộn và lấy đủ số lượng còn thiếu.
 */
 const handleRandomSelect = () => {
 if (isRevealing || pickedCardsRef.current.length >= cardsToDraw) return;

 const currentPickedSet = new Set(pickedCardsRef.current);
 const remainingCount = cardsToDraw - pickedCardsRef.current.length;
 const availableIdxs = deckIndexes.filter((idx) => !currentPickedSet.has(idx));

 const newPicks = pickRandomIndexes(availableIdxs, remainingCount);

 // Hiệu ứng bốc từng lá một để tăng tính trải nghiệm "magic"
 newPicks.forEach((idx, i) => {
 const timerId = window.setTimeout(() => {
 const sourceElement = deckCardRefs.current.get(idx) ?? null;
 addPickedCard(idx, sourceElement);
 }, i * 180);
 animationTimersRef.current.push(timerId);
 });
 };

 // Calculate when all cards have finished flipping
 const allCardsFlipped = cards.length > 0 && flippedIndex >= cards.length - 1;

 return (
 <div className="min-h-dvh tn-text-primary px-4 pb-4 pt-24 md:px-6 md:pb-6 md:pt-24 overflow-x-hidden relative font-sans">
 <AstralBackground variant="subtle" particleCount={8} />
 <div className="max-w-[1600px] mx-auto relative z-10 h-full">
 {/* Header - Fixed at top of the layout */}
 <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 mb-8 pb-4 border-b tn-border">
 <button
 onClick={() => router.push("/reading")}
 className="flex items-center tn-text-secondary hover:tn-text-primary transition min-h-11 px-2 rounded-xl hover:tn-surface-soft"
 >
 <ArrowLeft className="w-5 h-5 mr-2" />
 {t("header.back_to_setup")}
 </button>
 <div className="text-right">
 <h1 className="text-xl sm:text-2xl font-bold bg-gradient-to-r from-[var(--purple-accent)] to-[var(--warning)] text-transparent bg-clip-text">
 {t("header.title")}
 </h1>
 <p className="text-xs tn-text-muted font-mono mt-1">{t("header.session", { id: sessionShort })}</p>
 </div>
 </div>

 <div className={`grid grid-cols-1 ${cards.length > 0 ? "md:grid-cols-2" : ""} gap-8 items-start`}>
 {/* LEFT COLUMN: Cards & Question */}
 <div className="space-y-8">
 {/* Question Display */}
 {question && (
 <div className="tn-overlay p-6 rounded-2xl border border-[var(--purple-accent)]/20 text-center ">
 <p className="text-sm text-[var(--purple-accent)] uppercase tracking-widest mb-2 font-semibold">{t("question.label")}</p>
 <p className="text-lg sm:text-xl font-serif tn-text-primary italic">&quot;{question}&quot;</p>
 </div>
 )}

 {/* Drawing Phase Area */}
 {cards.length === 0 && (
 <div className="flex flex-col items-center justify-center py-10 w-full min-h-[50vh]">
 {error && (
 <p className="text-[var(--danger)] mb-6 bg-[var(--danger)]/30 px-6 py-3 rounded-xl border border-[var(--danger)]/30">
 {error}
 </p>
 )}

 {isShuffling ? (
 <div className="flex flex-col items-center justify-center animate-in fade-in zoom-in duration-1000 mt-12">
 <div className="relative w-36 h-56 perspective-1000 mb-20 mt-10 will-change-transform">
 {shufflePaths.map((path) => (
 <div
 key={`shuffle-card-${path.z}`}
 className="absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)] via-[var(--purple-accent)] to-[color:var(--c-61-49-80-55)] rounded-xl border-2 border-[var(--purple-accent)]/30 shadow-[0_10px_30px_var(--c-168-85-247-20)] tarot-shuffling-card motion-reduce:animate-none"
 style={{
 '--tx': path.tx, '--ty': path.ty, '--r': path.r,
 '--tx2': path.tx2, '--ty2': path.ty2, '--r2': path.r2,
 '--anim-duration': path.duration,
 '--anim-delay': path.delay,
 '--z': path.z,
 } as React.CSSProperties}
 >
 <div className="absolute inset-0 w-full h-full opacity-50 flex items-center justify-center tn-starfield z-0 rounded-xl "></div>
 <div className="absolute inset-0 flex items-center justify-center z-10">
 <div className="w-16 h-24 border border-[var(--purple-accent)]/30 rounded flex items-center justify-center tn-overlay-soft ">
 <Sparkles className="w-6 h-6 text-[var(--purple-accent)]/60" />
 </div>
 </div>
 <div className="absolute inset-2 border border-[var(--purple-accent)]/20 rounded-lg pointer-events-none"></div>
 </div>
 ))}
 </div>
 <h2 className="text-2xl font-serif font-medium bg-gradient-to-r from-[var(--purple-accent)] via-[var(--danger)] to-[var(--warning)] text-transparent bg-clip-text animate-pulse drop-shadow-sm">
 {t("shuffle.connecting_title")}
 </h2>
 <p className="text-sm tn-text-secondary mt-3 flex items-center">
 <RefreshCw className="w-4 h-4 mr-2 animate-spin text-[var(--purple-accent)]" />
 {t("shuffle.cleansing")}
 </p>
 </div>
 ) : (
 <div className="w-full flex flex-col items-center animate-in zoom-in-95 duration-700">
 <div className="mb-4 text-center">
 <h2 className="text-xl sm:text-2xl text-[var(--purple-accent)] font-medium mb-2 drop-shadow-[0_0_10px_var(--c-168-85-247-50)]">
 {pickedCards.length < cardsToDraw
 ? t("pick.prompt", { remaining: cardsToDraw - pickedCards.length })
 : t("pick.done")}
 </h2>
 <p className="text-sm tn-text-secondary">
 {t("pick.count", { picked: pickedCards.length, total: cardsToDraw })}
 </p>
 {pickedCards.length < cardsToDraw && (
 <button
 onClick={handleRandomSelect}
 className="mt-4 relative z-50 flex items-center gap-2 px-6 py-2.5 min-h-11 rounded-full tn-surface-strong border border-[var(--purple-accent)]/50 text-xs font-bold text-[var(--purple-accent)] hover:bg-[var(--purple-accent)]/60 hover:border-[var(--purple-accent)] hover:tn-text-primary transition-all shadow-[0_0_20px_var(--c-168-85-247-20)] group active:scale-95"
 >
 <Dices className="w-4 h-4 group-hover:rotate-12 transition-transform" />
 {t("pick.random")}
 </button>
 )}
 </div>

 <div className="w-full flex justify-center sm:justify-end mb-6 px-2">
 <div className="relative w-[90px] h-[180px]">
 <div
 ref={stackAnchorRef}
 className="absolute left-0 top-0 w-[72px] aspect-[14/22] rounded-md border border-dashed border-[var(--purple-accent)]/30 bg-[var(--purple-accent)]/10"
 />
 {pickedCards.map((cardId, stackIndex) => {
 const placement = getStackPlacement(stackIndex);
 return (
 <button
 key={`${cardId}-${stackIndex}`}
 type="button"
 onClick={() => removePickedCard(cardId)}
 disabled={isRevealing}
 className="absolute left-0 top-0 w-[72px] aspect-[14/22] rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-md tarot-deck-card transition-transform duration-200 hover:-translate-y-1 disabled:pointer-events-none"
 style={{
 transform: `translate(${placement.xOffset}px, ${placement.yOffset}px) rotate(${placement.rotate}deg)`,
 zIndex: stackIndex + 1,
 }}
 >
 <div className="absolute inset-1 border border-[var(--purple-accent)]/30 rounded-sm opacity-60 pointer-events-none"></div>
 <div className="absolute inset-0 rounded-md bg-[radial-gradient(circle_at_24%_22%,var(--c-255-255-255-15)_0,transparent_34%),linear-gradient(160deg,var(--c-61-49-80-20),transparent_65%)] pointer-events-none"></div>
 </button>
 );
 })}
 </div>
 </div>

 {flyingCards.length > 0 && (
 <div className="pointer-events-none fixed inset-0 z-[60]">
 {flyingCards.map((card) => (
 <div
 key={card.key}
 className="tarot-flying-card rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-md"
 style={{
 top: `${card.startY}px`,
 left: `${card.startX}px`,
 zIndex: 120 + card.stackIndex,
 '--fly-x': `${card.deltaX}px`,
 '--fly-y': `${card.deltaY}px`,
 '--fly-rotate': `${card.rotate}deg`,
 } as React.CSSProperties}
 >
 <div className="absolute inset-1 border border-[var(--purple-accent)]/30 rounded-sm opacity-60 pointer-events-none"></div>
 <div className="absolute inset-0 rounded-md bg-[radial-gradient(circle_at_24%_22%,var(--c-255-255-255-15)_0,transparent_34%),linear-gradient(160deg,var(--c-61-49-80-20),transparent_65%)] pointer-events-none"></div>
 </div>
 ))}
 </div>
 )}

 <div
 className={`w-full max-w-[1520px] mx-auto mb-20 transition-opacity duration-300
 ${pickedCards.length === cardsToDraw ? 'opacity-35 pointer-events-none' : ''}`}
 style={{ '--deck-card-w': deckCardWidth } as React.CSSProperties}
 >
 <div className="relative mx-auto w-full max-w-[1420px] px-1 sm:px-2">
 {activeDeckRows.map((row, rowIndex) => (
 <div
 key={`row-${rowIndex}`}
 className="flex justify-center items-end"
 style={rowIndex === 0 ? undefined : { marginTop: rowOverlapMargin }}
 >
 {row.map((idx, columnIndex) => {
 const isPicked = pickedCardSet.has(idx);
 const waveOffset = Math.sin(idx * 0.55) * DECK_WAVE_Y_AMPLITUDE;
 const waveRotation = Math.sin(idx * 0.35) * DECK_WAVE_ROTATION_AMPLITUDE;

 return (
 <div
 key={idx}
 className="relative w-[var(--deck-card-w)] aspect-[14/22] group tarot-card-fan"
 style={{
 marginLeft: columnIndex === 0 ? 0 : `calc(var(--deck-card-w) * -${DECK_HORIZONTAL_OVERLAP_FACTOR})`,
 transform: `translateY(${waveOffset}px) rotate(${waveRotation}deg)`,
 zIndex: isPicked ? 0 : columnIndex + rowIndex * 100,
 }}
 >
 <div
 ref={setDeckCardRef(idx)}
 onClick={(event) => {
 if (isRevealing || isPicked || pickedCards.length >= cardsToDraw) return;
 addPickedCard(idx, event.currentTarget);
 }}
 className={`w-full h-full relative cursor-pointer transition-all duration-150 ease-out transform rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-sm tarot-deck-card
 ${isPicked
 ? 'opacity-0 scale-95 pointer-events-none'
 : pickedCards.length < cardsToDraw
 ? 'hover:-translate-y-3 md:hover:-translate-y-5 hover:shadow-[0_0_10px_var(--c-168-85-247-70)] hover:border-[var(--purple-accent)] hover:scale-105 opacity-90 hover:z-40'
 : 'opacity-45 cursor-default'
 }`}
 >
 <div className="absolute inset-1 border border-[var(--purple-accent)]/30 rounded-sm opacity-60 pointer-events-none"></div>
 <div className="absolute inset-0 rounded-md bg-[radial-gradient(circle_at_24%_22%,var(--c-255-255-255-15)_0,transparent_34%),linear-gradient(160deg,var(--c-61-49-80-20),transparent_65%)] pointer-events-none"></div>
 </div>
 </div>
 );
 })}
 </div>
 ))}
 </div>
 </div>

 {pickedCards.length === cardsToDraw && (
 <div className="fixed inset-0 z-50 flex items-center justify-center tn-overlay animate-in fade-in zoom-in duration-500">
 <div className="tn-surface-strong p-8 rounded-3xl border border-[var(--purple-accent)]/30 shadow-[0_0_50px_var(--c-168-85-247-20)] text-center flex flex-col items-center max-w-md mx-4">
 <div className="w-16 h-16 bg-[var(--purple-accent)]/20 rounded-full flex items-center justify-center mb-6">
 <Sparkles className="w-8 h-8 text-[var(--warning)] animate-pulse" />
 </div>
 <h3 className="text-2xl font-serif font-bold tn-text-primary mb-2">{t("modal.title")}</h3>
 <p className="tn-text-secondary mb-8 text-sm">{t("modal.desc")}</p>

 <button
 onClick={handleReveal}
 disabled={isRevealing}
 className="group relative inline-flex items-center justify-center w-full px-8 py-4 font-bold tn-text-primary transition-all duration-300 font-pj rounded-2xl overflow-hidden bg-gradient-to-r from-[var(--purple-accent)] to-[var(--danger)] hover:from-[var(--purple-accent)] hover:to-[var(--danger)] hover:scale-[1.02] shadow-[0_0_40px_var(--c-168-85-247-40)]"
 >
 {isRevealing ? (
 <RefreshCw className="w-5 h-5 animate-spin mr-3 tn-text-primary" />
 ) : (
 <Sparkles className="w-5 h-5 mr-3 text-[var(--warning)] group-hover:animate-spin" />
 )}
 {isRevealing ? t("modal.revealing") : t("modal.reveal")}
 </button>

 {!isRevealing && (
 <button
 onClick={() => {
 const lastPickedCard = pickedCards[pickedCards.length - 1];
 if (typeof lastPickedCard === "number") {
 removePickedCard(lastPickedCard);
 }
 }}
 className="mt-6 inline-flex items-center min-h-11 px-2 rounded-xl hover:tn-surface-soft text-sm font-medium tn-text-secondary hover:tn-text-primary transition-colors"
 >
 {t("modal.change_card")}
 </button>
 )}
                </div>
              </div>
            )}
          </div>
        )}
      </div>
    )}

  {/* Revealed Cards Area (3 cards per row) */}
  {cards.length > 0 && (
 <div className="grid grid-cols-2 sm:grid-cols-3 gap-3 md:gap-4 perspective-1000 items-start">
 {cards.map((cardId, index) => {
 const isFlipped = flippedIndex >= index;
 const cardImageUrl = getCardImageUrl(cardId);
 const cardName = getCardName(cardId) ?? `Card ${cardId + 1}`;
 const cardMeaning = getCardMeaning(cardId) ?? '';

 return (
 <div key={`revealed-card-${cardId}`} className="flex flex-col items-center gap-3 w-full max-w-[180px] mx-auto">
 {/* 3D Flip Card Container */}
 <div className="relative w-full aspect-[14/22] preserve-3d transition-transform duration-700 ease-out cursor-pointer group tarot-card-flip"
 style={{ transform: isFlipped ? 'rotateY(180deg)' : 'rotateY(0deg)' }}>

 {/* Card Back */}
 <div className="absolute inset-0 backface-hidden w-full h-full tn-grad-lunar rounded-xl border-2 tn-border flex flex-col items-center justify-center shadow-xl">
 <div className="w-[85%] h-[90%] border tn-border-soft rounded-lg flex items-center justify-center tn-starfield">
 <Sparkles className="w-8 h-8 tn-text-muted" />
 </div>
 </div>

 {/* Card Front */}
 <div className="absolute inset-0 backface-hidden w-full h-full bg-transparent flex items-center justify-center"
 style={{ transform: 'rotateY(180deg)' }}>
 {/*
  * [TINH CHỈNH UI]: Đã xóa viền vàng theo yêu cầu.
  * Chỉ giữ lại phần "hình ảnh" (box đại diện) và tên lá bài ở bên dưới.
  */}
 <div className="w-full h-full tn-surface-strong rounded-xl flex items-center justify-center shadow-xl overflow-hidden relative border tn-border-soft">
 <div className="absolute inset-0 bg-gradient-to-tr from-[var(--purple-accent)]/20 to-transparent pointer-events-none"></div>
 <div className="absolute inset-2 border border-[var(--purple-accent)]/10 rounded-lg pointer-events-none"></div>
 {cardImageUrl ? (
 <img
 src={cardImageUrl}
 alt={cardName}
 className="h-full w-full object-cover"
 loading="lazy"
 />
 ) : (
 <span className="text-5xl font-serif font-black tn-text-primary/10 drop-shadow-sm">{index + 1}</span>
 )}
 </div>
 </div>
 </div>

        {/* Main Card Name and Meaning */}
        <div className={`mt-4 text-center transition-opacity duration-1000 delay-500 ${isFlipped ? 'opacity-100' : 'opacity-0'}`}>
          <h3 className="text-sm font-bold tn-text-primary font-serif leading-tight px-2 mb-2">
            {cardName}
          </h3>
 <p className="text-[10px] font-semibold text-[var(--purple-accent)] mb-1 uppercase tracking-widest">{t("cards.meaning_label")}</p>
 <p className="text-xs tn-text-secondary leading-relaxed px-2 line-clamp-3">
 {cardMeaning}
 </p>
 </div>
 </div>
 );
 })}
 </div>
 )}
 </div>

 {/* RIGHT COLUMN: AI Interpretation */}
 {cards.length > 0 && (
 <div className="md:col-span-1 h-full md:sticky md:top-24">
 <div className="tn-surface-strong border tn-border shadow-2xl rounded-3xl overflow-hidden flex flex-col md:max-h-[calc(100dvh-160px)]">
 <div className="p-5 border-b tn-border flex items-center justify-between tn-overlay">
 <div className="flex items-center gap-3">
 <div className="w-10 h-10 rounded-full bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20">
 <Sparkles className="w-5 h-5 text-[var(--purple-accent)] animate-pulse" />
 </div>
 <div>
 <h2 className="text-lg font-bold tn-text-primary">{tAi("title")}</h2>
 <p className="text-[10px] tn-text-muted font-mono tracking-tighter uppercase">{t("ai.subtitle", { id: sessionId.substring(0, 8) })}</p>
 </div>
 </div>
 <div className="px-3 py-1 bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/20 rounded-full">
 <span className="text-[10px] text-[var(--purple-accent)] font-bold uppercase tracking-widest">{t("ai.live")}</span>
 </div>
 </div>
 <div className="flex-1 overflow-hidden flex flex-col bg-gradient-to-b from-transparent to-[var(--purple-accent)]/5">
 <Suspense
 fallback={
 <div className="flex flex-1 items-center justify-center gap-3 tn-text-muted">
 <RefreshCw className="h-4 w-4 animate-spin text-[var(--purple-accent)]" />
 <span className="text-[10px] font-black uppercase tracking-widest">{tAi("title")}</span>
 </div>
 }
 >
 <AiInterpretationStream
 sessionId={sessionId}
 cards={cards}
 isReadyToShow={allCardsFlipped}
 />
 </Suspense>
 </div>

 {/* Bottom decorative area */}
 <div className="px-6 py-4 tn-overlay border-t tn-border flex items-center justify-between">
 <p className="text-[10px] tn-text-muted italic">{t("ai.footer_note")}</p>
 <div className="flex gap-1">
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--purple-accent)]/40"></div>
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--warning)]/40"></div>
 <div className="w-1.5 h-1.5 rounded-full bg-[var(--danger)]/40"></div>
 </div>
 </div>
 </div>
 </div>
 )}
 </div>
 </div>

 </div>
 );
}
