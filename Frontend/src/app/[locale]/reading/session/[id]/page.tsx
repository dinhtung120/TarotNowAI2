"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { revealReadingSession } from "@/actions/readingActions";
import { TAROT_CARD_COUNT, TAROT_DECK } from "@/lib/tarotData";
import { Sparkles, ArrowLeft, RefreshCw, Dices } from "lucide-react";
import AiInterpretationStream from "@/components/AiInterpretationStream";
import AstralBackground from "@/components/layout/AstralBackground";
import { useTranslations } from "next-intl";

const SHUFFLE_CARD_COUNT = 9;

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

export default function ReadingSessionPage() {
 const params = useParams();
 const router = useRouter();
 const sessionId = params.id as string;
 const t = useTranslations("ReadingSession");
 const tAi = useTranslations("AiInterpretation");
 const tTarot = useTranslations("Tarot");
 const sessionShort = sessionId.split("-")[0];

 const [cards, setCards] = useState<number[]>([]);

 const [isRevealing, setIsRevealing] = useState(false);
 const [error, setError] = useState("");
 const [flippedIndex, setFlippedIndex] = useState<number>(-1); // Index của lá bài đang được lật
 const [pickedCards, setPickedCards] = useState<number[]>([]);
 const [isShuffling, setIsShuffling] = useState(true);
 const [shufflePaths] = useState<Record<string, string | number>[]>(generateShufflePaths);
 const flipTimersRef = useRef<number[]>([]);

 useEffect(() => {
 const timer = window.setTimeout(() => setIsShuffling(false), 2200);
 return () => clearTimeout(timer);
 }, []);

 useEffect(() => {
 return () => {
 flipTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
 flipTimersRef.current = [];
 };
 }, []);

 // Restore data từ sessionStorage
 const isBrowser = typeof window !== "undefined";
 const question = isBrowser ? sessionStorage.getItem(`question_${sessionId}`) : "";
 const cardsToDraw = isBrowser ? parseInt(sessionStorage.getItem(`cardsToDraw_${sessionId}`) || "1", 10) : 1;
 const deckIndexes = useMemo(
 () => Array.from({ length: TAROT_CARD_COUNT }, (_, index) => index),
 [],
 );
 const pickedCardSet = useMemo(() => new Set(pickedCards), [pickedCards]);
 const tarotById = useMemo(() => {
 const map = new Map<number, (typeof TAROT_DECK)[number]>();
 TAROT_DECK.forEach((card) => {
 map.set(card.id, card);
 });
 return map;
 }, []);

 const handleReveal = async () => {
 setIsRevealing(true);
 setError("");
 flipTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
 flipTimersRef.current = [];
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
 if (isRevealing || pickedCards.length >= cardsToDraw) return;

 const remainingCount = cardsToDraw - pickedCards.length;
 const availableIdxs = deckIndexes.filter((idx) => !pickedCardSet.has(idx));

 // Xáo trộn mảng availableIdxs bằng thuật toán Fisher-Yates đơn giản
 const shuffled = [...availableIdxs];
 for (let i = shuffled.length - 1; i > 0; i--) {
 const j = Math.floor(Math.random() * (i + 1));
 [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
 }

 const newPicks = shuffled.slice(0, remainingCount);

 // Hiệu ứng bốc từng lá một để tăng tính trải nghiệm "magic"
 newPicks.forEach((idx, i) => {
 window.setTimeout(() => {
 setPickedCards(prev => {
 if (prev.includes(idx)) return prev; // Avoid duplicates just in case
 return [...prev, idx];
 });
 }, i * 200);
 });
 };

 // Calculate when all cards have finished flipping
 const allCardsFlipped = cards.length > 0 && flippedIndex >= cards.length - 1;

 return (
 <div className="min-h-screen tn-text-primary p-4 md:p-6 pt-24 overflow-x-hidden relative font-sans">
 <AstralBackground variant="subtle" particleCount={8} />
 <div className="max-w-[1600px] mx-auto relative z-10 h-full">
 {/* Header - Fixed at top of the layout */}
 <div className="flex items-center justify-between mb-8 pb-4 border-b tn-border">
 <button
 onClick={() => router.push("/reading")}
 className="flex items-center tn-text-secondary hover:tn-text-primary transition"
 >
 <ArrowLeft className="w-5 h-5 mr-2" />
 {t("header.back_to_setup")}
 </button>
 <div className="text-right">
 <h1 className="text-2xl font-bold bg-gradient-to-r from-[var(--purple-accent)] to-[var(--warning)] text-transparent bg-clip-text">
 {t("header.title")}
 </h1>
 <p className="text-xs tn-text-muted font-mono mt-1">{t("header.session", { id: sessionShort })}</p>
 </div>
 </div>

 <div className="grid grid-cols-1 lg:grid-cols-[55fr_45fr] gap-8 items-start">
 {/* LEFT COLUMN: Cards & Question (55% width) */}
 <div className={`${cards.length > 0 ? 'lg:col-span-1' : 'lg:col-span-2'} space-y-8`}>
 {/* Question Display */}
 {question && (
 <div className="tn-overlay p-6 rounded-2xl border border-[var(--purple-accent)]/20 text-center ">
 <p className="text-sm text-[var(--purple-accent)] uppercase tracking-widest mb-2 font-semibold">{t("question.label")}</p>
 <p className="text-xl font-serif tn-text-primary italic">&quot;{question}&quot;</p>
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
 {shufflePaths.map((path, i) => (
 <div
 key={i}
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
 <h2 className="text-2xl text-[var(--purple-accent)] font-medium mb-2 drop-shadow-[0_0_10px_var(--c-168-85-247-50)]">
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
 className="mt-4 relative z-50 flex items-center gap-2 px-6 py-2.5 rounded-full tn-surface-strong border border-[var(--purple-accent)]/50 text-xs font-bold text-[var(--purple-accent)] hover:bg-[var(--purple-accent)]/60 hover:border-[var(--purple-accent)] hover:tn-text-primary transition-all shadow-[0_0_20px_var(--c-168-85-247-20)] group active:scale-95"
 >
 <Dices className="w-4 h-4 group-hover:rotate-12 transition-transform" />
 {t("pick.random")}
 </button>
 )}
 </div>

 <div className={`relative w-full h-[300px] md:h-[400px] flex justify-center -mt-24 sm:-mt-28 mb-20 transition-opacity duration-300
 ${pickedCards.length === cardsToDraw ? 'opacity-35 pointer-events-none' : ''}`}>
 {deckIndexes.map((idx) => {
 const isPicked = pickedCardSet.has(idx);
 const angle = -52 + (idx * (104 / Math.max(1, deckIndexes.length - 1)));

 return (
 <div
 key={idx}
 className="absolute bottom-0 left-1/2 w-[55px] sm:w-[65px] md:w-[75px] aspect-[2/3] -translate-x-1/2 origin-bottom group tarot-card-fan"
 style={{
 transformOrigin: "center clamp(240px, 41vw, 560px)",
 transform: `rotate(${angle}deg)`,
 zIndex: isPicked ? 100 : idx,
 }}
 >
 <div
 onClick={() => {
 if (isRevealing) return;
 if (isPicked) {
 setPickedCards(prev => prev.filter(p => p !== idx));
 } else if (pickedCards.length < cardsToDraw) {
 setPickedCards(prev => [...prev, idx]);
 }
 }}
 className={`w-full h-full relative cursor-pointer transition-all duration-150 ease-out transform rounded-md border border-[var(--purple-accent)]/35 bg-gradient-to-br from-[var(--purple-accent)]/95 to-[color:var(--c-61-49-80-55)] shadow-sm tarot-deck-card
 ${isPicked
 ? 'ring-2 ring-[var(--warning)] shadow-[0_0_12px_var(--c-251-191-36-80)] opacity-100 -translate-y-8 sm:-translate-y-12 scale-110 md:scale-120 z-50'
 : pickedCards.length < cardsToDraw
 ? 'hover:-translate-y-5 md:hover:-translate-y-8 hover:shadow-[0_0_10px_var(--c-168-85-247-70)] hover:border-[var(--purple-accent)] hover:scale-105 opacity-90 hover:z-[99]'
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
 onClick={() => setPickedCards(prev => prev.slice(0, -1))}
 className="mt-6 text-sm font-medium tn-text-secondary hover:tn-text-primary transition-colors"
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

 {/* Revealed Cards Area (3-column layout) */}
 {cards.length > 0 && (
 <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3 md:gap-4 perspective-1000 items-start">
 {cards.map((cardId, index) => {
 const isFlipped = flippedIndex >= index;
 const cardMeta = tarotById.get(cardId) || TAROT_DECK[0];

 return (
 <div key={index} className="flex flex-col items-center gap-3 w-full max-w-[180px] mx-auto">
 {/* 3D Flip Card Container */}
 <div className="relative w-full aspect-[2/3.2] preserve-3d transition-transform duration-700 ease-out cursor-pointer group tarot-card-flip"
 style={{ transform: isFlipped ? 'rotateY(180deg)' : 'rotateY(0deg)' }}>

 {/* Card Back */}
 <div className="absolute inset-0 backface-hidden w-full h-full tn-grad-lunar rounded-xl border-2 tn-border flex flex-col items-center justify-center shadow-xl">
 <div className="w-[85%] h-[90%] border tn-border-soft rounded-lg flex items-center justify-center tn-starfield">
 <Sparkles className="w-8 h-8 tn-text-muted" />
 </div>
 </div>

 {/* Card Front */}
 <div className="absolute inset-0 backface-hidden w-full h-full bg-gradient-to-b from-[var(--warning)] to-[var(--warning)] rounded-xl border-2 border-[var(--warning)]/50 shadow-[0_0_20px_var(--c-251-191-36-15)] flex flex-col items-center p-4"
 style={{ transform: 'rotateY(180deg)' }}>
 <div className="w-full text-center border-b border-[var(--warning)] pb-1.5 mb-2.5">
 <span className="text-[10px] font-bold text-[var(--warning)] uppercase tracking-widest">
 {tTarot(`suits.${cardMeta.suit}.full`)}
 </span>
 </div>
 <div className="flex-1 w-full tn-surface-strong rounded-md mb-2.5 flex items-center justify-center shadow-inner overflow-hidden relative">
 <div className="absolute inset-0 bg-gradient-to-tr from-[var(--purple-accent)]/40 to-transparent"></div>
 <span className="text-4xl font-serif text-[var(--warning)]/70 drop-shadow-md">{index + 1}</span>
 </div>
 <h3 className="text-sm font-bold tn-text-ink font-serif leading-tight text-center">
 {tTarot(`cards.c${cardId}.name`)}
 </h3>
 </div>
 </div>

 {/* Meaning Short text */}
 <div className={`text-center transition-opacity duration-1000 delay-500 ${isFlipped ? 'opacity-100' : 'opacity-0'}`}>
 <p className="text-[10px] font-semibold text-[var(--purple-accent)] mb-1 uppercase tracking-widest">{t("cards.meaning_label")}</p>
 <p className="text-xs tn-text-secondary leading-relaxed px-2 line-clamp-3">
 {tTarot(`cards.c${cardId}.meaning`)}
 </p>
 </div>
 </div>
 );
 })}
 </div>
 )}
 </div>

 {/* RIGHT COLUMN: AI Interpretation (45% width) */}
 {cards.length > 0 && (
 <div className="lg:col-span-1 h-full sticky top-24">
 <div className="tn-surface-strong border tn-border shadow-2xl rounded-3xl overflow-hidden flex flex-col max-h-[calc(100vh-160px)]">
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
 <div className="p-6 md:p-8 overflow-y-auto custom-scrollbar flex-1 bg-gradient-to-b from-transparent to-[var(--purple-accent)]/5">
 {allCardsFlipped ? (
 <AiInterpretationStream
 sessionId={sessionId}
 cards={cards}
 />
 ) : (
 <div className="h-64 flex flex-col items-center justify-center text-center space-y-4 opacity-40">
 <RefreshCw className="w-10 h-10 tn-text-muted animate-spin" />
 <p className="tn-text-muted font-serif italic max-w-xs px-4">{t("ai.waiting_flip")}</p>
 </div>
 )}
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
