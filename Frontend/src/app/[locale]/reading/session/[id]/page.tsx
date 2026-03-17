"use client";

import { useState, useEffect } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { revealReadingSession } from "@/actions/readingActions";
import { TAROT_DECK } from "@/lib/tarotData";
import { Sparkles, ArrowLeft, RefreshCw, Dices } from "lucide-react";
import AiInterpretationStream from "@/components/AiInterpretationStream";
import AstralBackground from "@/components/layout/AstralBackground";

// Generate positions for an elegant fanning and splitting shuffle effect
const generateShufflePaths = () => {
    return Array.from({ length: 15 }).map((_, i) => {
        // We'll divide cards into left hand (negative) and right hand (positive)
        const isLeft = i % 2 === 0;
        const directionMultiplier = isLeft ? -1 : 1;
        const fanAngle = (i / 15) * 40 - 20; // Fan from -20deg to 20deg
        const offset = 120 * directionMultiplier;

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

    const [cards, setCards] = useState<number[]>([]);

    const [isRevealing, setIsRevealing] = useState(false);
    const [error, setError] = useState("");
    const [flippedIndex, setFlippedIndex] = useState<number>(-1); // Index của lá bài đang được lật
    const [pickedCards, setPickedCards] = useState<number[]>([]);
    const [isShuffling, setIsShuffling] = useState(true);
    const [shufflePaths] = useState<Record<string, string | number>[]>(generateShufflePaths);

    useEffect(() => {
        const timer = setTimeout(() => setIsShuffling(false), 4000);
        return () => clearTimeout(timer);
    }, []);

    // Restore data từ sessionStorage
    const isBrowser = typeof window !== "undefined";
    const question = isBrowser ? sessionStorage.getItem(`question_${sessionId}`) : "";
    const cardsToDraw = isBrowser ? parseInt(sessionStorage.getItem(`cardsToDraw_${sessionId}`) || "1", 10) : 1;

    const handleReveal = async () => {
        setIsRevealing(true);
        setError("");

        const response = await revealReadingSession({ sessionId });

        if (response.success && response.data) {
            setCards(response.data.cards);


            // Auto-flip từng lá bài một với delay
            response.data.cards.forEach((_, idx) => {
                setTimeout(() => {
                    setFlippedIndex(idx);
                }, (idx + 1) * 800);
            });

        } else {
            setError(response.error || "Failed to reveal cards.");
        }
        setIsRevealing(false);
    };

    /**
     * Tự động chọn ngẫu nhiên các lá bài còn thiếu.
     * Logic: Lấy bộ index 0-77, loại trừ các lá đã chọn, 
     * xáo trộn và lấy đủ số lượng còn thiếu.
     */
    const handleRandomSelect = () => {
        if (isRevealing || pickedCards.length >= cardsToDraw) return;

        const remainingCount = cardsToDraw - pickedCards.length;
        const availableIdxs = Array.from({ length: 78 })
            .map((_, i) => i)
            .filter(idx => !pickedCards.includes(idx));

        // Xáo trộn mảng availableIdxs bằng thuật toán Fisher-Yates đơn giản
        const shuffled = [...availableIdxs];
        for (let i = shuffled.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
        }

        const newPicks = shuffled.slice(0, remainingCount);
        console.log(`[TarotNow] Picking ${newPicks.length} cards randomly:`, newPicks);

        // Hiệu ứng bốc từng lá một để tăng tính trải nghiệm "magic"
        newPicks.forEach((idx, i) => {
            setTimeout(() => {
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
        <div className="min-h-screen text-white p-4 md:p-6 pt-24 overflow-x-hidden relative font-sans">
            <AstralBackground />
            <div className="max-w-[1600px] mx-auto relative z-10 h-full">
                
                {/* Header - Fixed at top of the layout */}
                <div className="flex items-center justify-between mb-8 pb-4 border-b border-zinc-800">
                    <button
                        onClick={() => router.push("/reading")}
                        className="flex items-center text-zinc-400 hover:text-white transition"
                    >
                        <ArrowLeft className="w-5 h-5 mr-2" />
                        Về trang rút bài
                    </button>
                    <div className="text-right">
                        <h1 className="text-2xl font-bold bg-gradient-to-r from-purple-400 to-amber-400 text-transparent bg-clip-text">
                            The Mystical Realm
                        </h1>
                        <p className="text-xs text-zinc-500 font-mono mt-1">Session: {sessionId.split('-')[0]}...</p>
                    </div>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-[55fr_45fr] gap-8 items-start">
                    
                    {/* LEFT COLUMN: Cards & Question (55% width) */}
                    <div className={`${cards.length > 0 ? 'lg:col-span-1' : 'lg:col-span-2'} space-y-8`}>
                        {/* Question Display */}
                        {question && (
                            <div className="bg-zinc-900/40 p-6 rounded-2xl border border-purple-500/20 text-center backdrop-blur-sm">
                                <p className="text-sm text-purple-400 uppercase tracking-widest mb-2 font-semibold">Tâm Niệm</p>
                                <p className="text-xl font-serif text-white italic">&quot;{question}&quot;</p>
                            </div>
                        )}

                        {/* Drawing Phase Area */}
                        {cards.length === 0 && (
                            <div className="flex flex-col items-center justify-center py-10 w-full min-h-[50vh]">
                                {error && (
                                    <p className="text-red-400 mb-6 bg-red-900/30 px-6 py-3 rounded-xl border border-red-500/30">
                                        {error}
                                    </p>
                                )}

                                {isShuffling ? (
                                    <div className="flex flex-col items-center justify-center animate-in fade-in zoom-in duration-1000 mt-12">
                                        <style dangerouslySetInnerHTML={{
                                            __html: `
                                            @keyframes elegant-shuffle {
                                                0% { transform: translate(0, 0) rotate(0deg) scale(1); opacity: 1; z-index: var(--z); }
                                                30% { transform: translate(var(--tx), var(--ty)) rotate(var(--r)) scale(1.05); opacity: 0.9; z-index: var(--z); }
                                                50% { transform: translate(var(--tx2), var(--ty2)) rotate(var(--r2)) scale(1.1); opacity: 0.95; z-index: calc(var(--z) + 10); }
                                                80% { transform: translate(calc(var(--tx) * -0.5), var(--ty)) rotate(calc(var(--r) * -1)) scale(1.05); opacity: 0.9; z-index: var(--z); }
                                                100% { transform: translate(0, 0) rotate(0deg) scale(1); opacity: 1; z-index: var(--z); }
                                            }
                                            .tarot-shuffling-card {
                                                animation: elegant-shuffle var(--anim-duration) ease-in-out infinite;
                                                animation-delay: var(--anim-delay);
                                                transform-origin: bottom center;
                                            }
                                        `}} />
                                        <div className="relative w-36 h-56 perspective-1000 mb-20 mt-10">
                                            {shufflePaths.map((path, i) => (
                                                <div
                                                    key={i}
                                                    className="absolute inset-0 bg-gradient-to-br from-indigo-900 via-purple-900 to-zinc-900 rounded-xl border-2 border-purple-500/30 shadow-[0_10px_30px_rgba(168,85,247,0.2)] tarot-shuffling-card"
                                                    style={{
                                                        '--tx': path.tx, '--ty': path.ty, '--r': path.r,
                                                        '--tx2': path.tx2, '--ty2': path.ty2, '--r2': path.r2,
                                                        '--anim-duration': path.duration,
                                                        '--anim-delay': path.delay,
                                                        '--z': path.z,
                                                    } as React.CSSProperties}
                                                >
                                                    <div className="absolute inset-0 w-full h-full opacity-50 flex items-center justify-center bg-[url('https://www.transparenttextures.com/patterns/stardust.png')] z-0 rounded-xl mix-blend-overlay"></div>
                                                    <div className="absolute inset-0 flex items-center justify-center z-10">
                                                        <div className="w-16 h-24 border border-purple-400/30 rounded flex items-center justify-center bg-black/20 backdrop-blur-sm">
                                                            <Sparkles className="w-6 h-6 text-purple-300/60" />
                                                        </div>
                                                    </div>
                                                    <div className="absolute inset-2 border border-purple-400/20 rounded-lg pointer-events-none"></div>
                                                </div>
                                            ))}
                                        </div>
                                        <h2 className="text-2xl font-serif font-medium bg-gradient-to-r from-purple-300 via-pink-300 to-amber-300 text-transparent bg-clip-text animate-pulse drop-shadow-sm">
                                            Thiết Lập Kết Nối...
                                        </h2>
                                        <p className="text-sm text-zinc-400 mt-3 flex items-center">
                                            <RefreshCw className="w-4 h-4 mr-2 animate-spin text-purple-400" />
                                            Các lá bài đang được thanh tẩy
                                        </p>
                                    </div>
                                ) : (
                                    <div className="w-full flex flex-col items-center animate-in zoom-in-95 duration-700">
                                        <div className="mb-4 text-center">
                                            <h2 className="text-2xl text-purple-300 font-medium mb-2 drop-shadow-[0_0_10px_rgba(168,85,247,0.5)]">
                                                {pickedCards.length < cardsToDraw
                                                    ? `Hãy tĩnh tâm và chọn ${cardsToDraw - pickedCards.length} lá bài`
                                                    : "Các lá bài đã an bài"}
                                            </h2>
                                            <p className="text-sm text-zinc-400">
                                                {pickedCards.length}/{cardsToDraw} lá bài
                                            </p>
                                            
                                            {pickedCards.length < cardsToDraw && (
                                                <button
                                                    onClick={handleRandomSelect}
                                                    className="mt-4 relative z-50 flex items-center gap-2 px-6 py-2.5 rounded-full bg-zinc-800/90 border border-purple-500/50 text-xs font-bold text-purple-200 hover:bg-purple-900/60 hover:border-purple-400 hover:text-white transition-all shadow-[0_0_20px_rgba(168,85,247,0.2)] backdrop-blur-md group active:scale-95"
                                                >
                                                    <Dices className="w-4 h-4 group-hover:rotate-12 transition-transform" />
                                                    Chọn ngẫu nhiên
                                                </button>
                                            )}
                                        </div>

                                        <div className={`relative w-full h-[300px] md:h-[400px] flex justify-center -mt-24 sm:-mt-28 mb-20 transition-opacity duration-1000
                                                        ${pickedCards.length === cardsToDraw ? 'opacity-30 blur-sm pointer-events-none' : ''}`}>
                                            {Array.from({ length: 78 }).map((_, idx) => {
                                                const isPicked = pickedCards.includes(idx);
                                                const angle = -50 + (idx * (100 / 77));

                                                return (
                                                    <div
                                                        key={idx}
                                                        className="absolute bottom-0 left-1/2 w-[55px] sm:w-[65px] md:w-[75px] aspect-[2/3] -translate-x-1/2 origin-bottom group"
                                                        style={{
                                                            transformOrigin: "center clamp(230px, 42vw, 600px)",
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
                                                            className={`w-full h-full relative cursor-pointer transition-all duration-[300ms] ease-out transform rounded-md border border-purple-500/40 bg-gradient-to-br from-indigo-900 to-zinc-900 shadow-sm
                                                                ${isPicked
                                                                    ? 'ring-2 ring-amber-400 shadow-[0_0_20px_rgba(251,191,36,0.8)] opacity-100 -translate-y-8 sm:-translate-y-12 scale-110 md:scale-125 z-50'
                                                                    : pickedCards.length < cardsToDraw
                                                                        ? 'hover:-translate-y-6 md:hover:-translate-y-10 hover:shadow-[0_0_20px_rgba(168,85,247,0.7)] hover:border-purple-300 hover:scale-110 opacity-90 hover:z-[99]'
                                                                        : 'opacity-50 cursor-default'
                                                                }`}
                                                        >
                                                            <div className="absolute inset-0 w-full h-full opacity-40 flex items-center justify-center bg-[url('https://www.transparenttextures.com/patterns/stardust.png')] mix-blend-overlay">
                                                                <Sparkles className="w-3 h-3 text-purple-300/50" />
                                                            </div>
                                                            <div className="absolute inset-1 border border-purple-400/30 rounded-sm opacity-60 pointer-events-none"></div>
                                                        </div>
                                                    </div>
                                                );
                                            })}
                                        </div>

                                        {pickedCards.length === cardsToDraw && (
                                            <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm animate-in fade-in zoom-in duration-500">
                                                <div className="bg-zinc-900/80 p-8 rounded-3xl border border-purple-500/30 shadow-[0_0_50px_rgba(168,85,247,0.2)] text-center flex flex-col items-center max-w-md mx-4">
                                                    <div className="w-16 h-16 bg-purple-500/20 rounded-full flex items-center justify-center mb-6">
                                                        <Sparkles className="w-8 h-8 text-amber-300 animate-pulse" />
                                                    </div>
                                                    <h3 className="text-2xl font-serif font-bold text-white mb-2">Các Lá Bài Đã Được Chọn</h3>
                                                    <p className="text-zinc-400 mb-8 text-sm">Vũ trụ đã lắng nghe. Hãy hít một hơi thật sâu trước khi đón nhận thông điệp.</p>

                                                    <button
                                                        onClick={handleReveal}
                                                        disabled={isRevealing}
                                                        className="group relative inline-flex items-center justify-center w-full px-8 py-4 font-bold text-white transition-all duration-300 font-pj rounded-2xl overflow-hidden bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-500 hover:to-pink-500 hover:scale-[1.02] shadow-[0_0_40px_rgba(168,85,247,0.4)]"
                                                    >
                                                        {isRevealing ? (
                                                            <RefreshCw className="w-5 h-5 animate-spin mr-3 text-white" />
                                                        ) : (
                                                            <Sparkles className="w-5 h-5 mr-3 text-amber-200 group-hover:animate-spin" />
                                                        )}
                                                        {isRevealing ? "Đang truyền năng lượng..." : "Truyền Năng Lượng & Lật Bài"}
                                                    </button>

                                                    {!isRevealing && (
                                                        <button
                                                            onClick={() => setPickedCards(prev => prev.slice(0, -1))}
                                                            className="mt-6 text-sm font-medium text-zinc-400 hover:text-white transition-colors"
                                                        >
                                                            Đổi lá bài khác
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
                                    const cardData = TAROT_DECK.find(c => c.id === cardId) || TAROT_DECK[0];

                                    return (
                                        <div key={index} className="flex flex-col items-center gap-3 w-full max-w-[180px] mx-auto">
                                            {/* 3D Flip Card Container */}
                                            <div className="relative w-full aspect-[2/3.2] preserve-3d transition-transform duration-1000 ease-out cursor-pointer group"
                                                style={{ transform: isFlipped ? 'rotateY(180deg)' : 'rotateY(0deg)' }}>

                                                {/* Card Back */}
                                                <div className="absolute inset-0 backface-hidden w-full h-full bg-gradient-to-br from-zinc-900 to-black rounded-xl border-2 border-zinc-700 flex flex-col items-center justify-center shadow-xl">
                                                    <div className="w-[85%] h-[90%] border border-zinc-600/50 rounded-lg flex items-center justify-center bg-[url('https://www.transparenttextures.com/patterns/stardust.png')]">
                                                        <Sparkles className="w-8 h-8 text-zinc-600" />
                                                    </div>
                                                </div>

                                                {/* Card Front */}
                                                <div className="absolute inset-0 backface-hidden w-full h-full bg-gradient-to-b from-amber-100 to-amber-50 rounded-xl border-2 border-amber-300/50 shadow-[0_0_20px_rgba(251,191,36,0.15)] flex flex-col items-center p-4"
                                                    style={{ transform: 'rotateY(180deg)' }}>
                                                    <div className="w-full text-center border-b border-amber-300 pb-1.5 mb-2.5">
                                                        <span className="text-[10px] font-bold text-amber-600 uppercase tracking-widest">{cardData.suit}</span>
                                                    </div>
                                                    <div className="flex-1 w-full bg-zinc-800 rounded-md mb-2.5 flex items-center justify-center shadow-inner overflow-hidden relative">
                                                        <div className="absolute inset-0 bg-gradient-to-tr from-purple-900/40 to-transparent"></div>
                                                        <span className="text-4xl font-serif text-amber-500/70 drop-shadow-md">{index + 1}</span>
                                                    </div>
                                                    <h3 className="text-sm font-bold text-zinc-900 font-serif leading-tight text-center">{cardData.name}</h3>
                                                </div>
                                            </div>

                                            {/* Meaning Short text */}
                                            <div className={`text-center transition-opacity duration-1000 delay-500 ${isFlipped ? 'opacity-100' : 'opacity-0'}`}>
                                                <p className="text-[10px] font-semibold text-purple-400 mb-1 uppercase tracking-widest">Ý nghĩa</p>
                                                <p className="text-xs text-zinc-400 leading-relaxed px-2 line-clamp-3">{cardData.meaning}</p>
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
                            <div className="bg-[#111111]/80 border border-zinc-800 shadow-2xl rounded-3xl overflow-hidden backdrop-blur-xl flex flex-col max-h-[calc(100vh-160px)]">
                                <div className="p-5 border-b border-zinc-800 flex items-center justify-between bg-zinc-900/50">
                                    <div className="flex items-center gap-3">
                                        <div className="w-10 h-10 rounded-full bg-purple-500/10 flex items-center justify-center border border-purple-500/20">
                                            <Sparkles className="w-5 h-5 text-purple-400 animate-pulse" />
                                        </div>
                                        <div>
                                            <h2 className="text-lg font-bold text-white">Lời Diễn Giải Từ Vũ Trụ</h2>
                                            <p className="text-[10px] text-zinc-500 font-mono tracking-tighter uppercase">AI Divine Stream • {sessionId.substring(0, 8)}</p>
                                        </div>
                                    </div>
                                    <div className="px-3 py-1 bg-purple-500/10 border border-purple-500/20 rounded-full">
                                        <span className="text-[10px] text-purple-400 font-bold uppercase tracking-widest">Live</span>
                                    </div>
                                </div>
                                
                                <div className="p-6 md:p-8 overflow-y-auto custom-scrollbar flex-1 bg-gradient-to-b from-transparent to-purple-900/5">
                                    {allCardsFlipped ? (
                                        <AiInterpretationStream
                                            sessionId={sessionId}
                                            cards={cards}
                                            onComplete={() => console.log("AI Stream Finished")}
                                        />
                                    ) : (
                                        <div className="h-64 flex flex-col items-center justify-center text-center space-y-4 opacity-40">
                                            <RefreshCw className="w-10 h-10 text-zinc-600 animate-spin" />
                                            <p className="text-zinc-500 font-serif italic max-w-xs px-4">Đang chuẩn bị kết nối tâm linh... Vui lòng lật hết các lá bài để nhận thông điệp.</p>
                                        </div>
                                    )}
                                </div>

                                {/* Bottom decorative area */}
                                <div className="px-6 py-4 bg-zinc-900/30 border-t border-zinc-800 flex items-center justify-between">
                                    <p className="text-[10px] text-zinc-500 italic">Lắng nghe rung động từ những lá bài của bạn.</p>
                                    <div className="flex gap-1">
                                        <div className="w-1.5 h-1.5 rounded-full bg-purple-500/40"></div>
                                        <div className="w-1.5 h-1.5 rounded-full bg-amber-500/40"></div>
                                        <div className="w-1.5 h-1.5 rounded-full bg-pink-500/40"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>

            {/* Background elements are handled by AstralBackground */}
            
            {/* Custom Scrollbar Styles */}
            <style jsx global>{`
                .custom-scrollbar::-webkit-scrollbar {
                    width: 4px;
                }
                .custom-scrollbar::-webkit-scrollbar-track {
                    background: transparent;
                }
                .custom-scrollbar::-webkit-scrollbar-thumb {
                    background: #27272a;
                    border-radius: 10px;
                }
                .custom-scrollbar::-webkit-scrollbar-thumb:hover {
                    background: #3f3f46;
                }
            `}</style>
        </div>
    );
}
