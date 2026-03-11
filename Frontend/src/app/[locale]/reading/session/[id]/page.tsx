"use client";

import { useState, useEffect } from "react";
import { useParams } from "next/navigation";
import { useRouter } from "@/i18n/routing";
import { revealReadingSession } from "@/actions/readingActions";
import { TAROT_DECK } from "@/lib/tarotData";
import { Sparkles, ArrowLeft, RefreshCw } from "lucide-react";
import AiInterpretationStream from "@/components/AiInterpretationStream";

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

    // Calculate when all cards have finished flipping
    const allCardsFlipped = cards.length > 0 && flippedIndex >= cards.length - 1;

    return (
        <div className="min-h-screen bg-black text-white p-6 pt-24 overflow-hidden relative">
            <div className="max-w-6xl mx-auto relative z-10">

                {/* Header */}
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

                {/* Question Display */}
                {question && (
                    <div className="bg-zinc-900/40 p-6 rounded-2xl border border-purple-500/20 mb-12 text-center max-w-2xl mx-auto backdrop-blur-sm">
                        <p className="text-sm text-purple-400 uppercase tracking-widest mb-2 font-semibold">Tâm Niệm</p>
                        <p className="text-xl font-serif text-white italic">&quot;{question}&quot;</p>
                    </div>
                )}

                {/* Main Action Area */}
                {cards.length === 0 ? (
                    <div className="flex flex-col items-center justify-center py-10 w-full min-h-[60vh]">
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

                                            {/* Minimalist Card Back Center */}
                                            <div className="absolute inset-0 flex items-center justify-center z-10">
                                                <div className="w-16 h-24 border border-purple-400/30 rounded flex items-center justify-center bg-black/20 backdrop-blur-sm">
                                                    <Sparkles className="w-6 h-6 text-purple-300/60" />
                                                </div>
                                            </div>
                                            {/* Outer Border */}
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
                                        {pickedCards.length}/${cardsToDraw} lá bài
                                    </p>
                                </div>

                                {/* Card Arc Layout 78 lá (chồng rải vừa phải & đè lên nhau thêm chút) */}
                                <div className={`relative w-full h-[300px] md:h-[400px] flex justify-center -mt-24 sm:-mt-28 mb-20 transition-opacity duration-1000
                                                ${pickedCards.length === cardsToDraw ? 'opacity-30 blur-sm pointer-events-none' : ''}`}>
                                    {Array.from({ length: 78 }).map((_, idx) => {
                                        const isPicked = pickedCards.includes(idx);
                                        // Vòng cung thắt hẹp lại 1 tí (-50 đến +50) để bài đè lên nhau sát hơn
                                        const angle = -50 + (idx * (100 / 77));

                                        return (
                                            <div
                                                key={idx}
                                                className="absolute bottom-0 left-1/2 w-[55px] sm:w-[65px] md:w-[85px] aspect-[2/3] -translate-x-1/2 origin-bottom group"
                                                style={{
                                                    // Rút ngắn bán kính vòng cong thêm 1 chút
                                                    transformOrigin: "center clamp(230px, 42vw, 600px)",
                                                    transform: `rotate(${angle}deg)`,
                                                    // Khi hover thì đưa lá bài lên trên cùng để không bị đè
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
                                                    {/* Card back design center piece */}
                                                    <div className="absolute inset-1 border border-purple-400/30 rounded-sm opacity-60 pointer-events-none"></div>
                                                </div>
                                            </div>
                                        );
                                    })}
                                </div>

                                {/* Centered Overlay when cards are fully picked */}
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
                ) : (
                    /* Cards Display Area */
                    <div className="space-y-16">
                        <div className="flex flex-wrap justify-center gap-8 perspective-1000">
                            {cards.map((cardId, index) => {
                                const isFlipped = flippedIndex >= index;
                                const cardData = TAROT_DECK.find(c => c.id === cardId) || TAROT_DECK[0];

                                return (
                                    <div key={index} className="flex flex-col items-center gap-6">
                                        {/* 3D Flip Card Container */}
                                        <div className="relative w-56 h-80 preserve-3d transition-transform duration-1000 ease-out cursor-pointer group"
                                            style={{ transform: isFlipped ? 'rotateY(180deg)' : 'rotateY(0deg)' }}>

                                            {/* Card Back */}
                                            <div className="absolute inset-0 backface-hidden w-full h-full bg-gradient-to-br from-zinc-900 to-black rounded-2xl border-2 border-zinc-700 flex flex-col items-center justify-center shadow-2xl">
                                                <div className="w-48 h-72 border border-zinc-600/50 rounded-xl m-2 flex items-center justify-center bg-[url('https://www.transparenttextures.com/patterns/stardust.png')]">
                                                    <Sparkles className="w-8 h-8 text-zinc-600" />
                                                </div>
                                            </div>

                                            {/* Card Front */}
                                            <div className="absolute inset-0 backface-hidden w-full h-full bg-gradient-to-b from-amber-100 to-amber-50 rounded-2xl border-2 border-amber-300/50 shadow-[0_0_30px_rgba(251,191,36,0.2)] flex flex-col items-center p-4"
                                                style={{ transform: 'rotateY(180deg)' }}>
                                                <div className="w-full text-center border-b border-amber-300 pb-2 mb-4">
                                                    <span className="text-xs font-bold text-amber-600 uppercase tracking-widest">{cardData.suit}</span>
                                                </div>
                                                {/* Placeholder for Card Art */}
                                                <div className="flex-1 w-full bg-zinc-800 rounded-lg mb-4 flex items-center justify-center shadow-inner overflow-hidden relative">
                                                    <div className="absolute inset-0 bg-gradient-to-tr from-purple-900/50 to-transparent"></div>
                                                    <span className="text-6xl font-serif text-amber-500/80 drop-shadow-lg">{index + 1}</span>
                                                </div>
                                                <h3 className="text-lg font-bold text-zinc-900 font-serif leading-tight text-center">{cardData.name}</h3>
                                            </div>
                                        </div>

                                        {/* Interpretation Text (Fades in after flip) */}
                                        <div className={`w-56 text-center transition-opacity duration-1000 delay-500 ${isFlipped ? 'opacity-100' : 'opacity-0'}`}>
                                            <p className="text-sm font-medium text-purple-300 mb-1">Ý Nghĩa Cốt Lõi</p>
                                            <p className="text-xs text-zinc-400 leading-relaxed">{cardData.meaning}</p>
                                        </div>
                                    </div>
                                );
                            })}
                        </div>

                        {/* AI Streaming Section (Chỉ hiện khi tất cả lá bài đã lật xong) */}
                        {allCardsFlipped && (
                            <AiInterpretationStream
                                sessionId={sessionId}
                                cards={cards}
                                onComplete={() => console.log("AI Stream Finished")}
                            />
                        )}
                    </div>
                )}
            </div>

            {/* Decorative Background Elements */}
            <div className="fixed top-1/4 left-10 w-96 h-96 bg-purple-900/20 rounded-full blur-[100px] pointer-events-none"></div>
            <div className="fixed bottom-1/4 right-10 w-96 h-96 bg-amber-900/10 rounded-full blur-[100px] pointer-events-none"></div>
        </div>
    );
}
