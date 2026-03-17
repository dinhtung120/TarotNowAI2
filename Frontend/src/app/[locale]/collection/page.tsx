"use client";

/**
 * Trang Bộ Sưu Tập (Collection Page)
 * Tái thiết kế: Premium & Compact.
 * - Nền Nebula & Particles đồng bộ.
 * - Hệ thống bộ lọc: Tất cả, Đã có, Chưa có.
 * - Card UI: Borderless, Glassmorphism, Amber Glow cho thẻ đã sở hữu.
 * - Tối ưu Typography & Progress bar gọn gàng.
 */

import { useEffect, useState, useMemo } from "react";
import { getUserCollection, UserCollectionDto } from "@/actions/collectionActions";
import { TAROT_DECK } from "@/lib/tarotData";
import { Loader2, LayoutGrid, AlertCircle, ChevronRight, Sparkles, Filter, CheckCircle2, Lock } from "lucide-react";
import { Link } from "@/i18n/routing";
import { useTranslations } from "next-intl";

type FilterType = "all" | "owned" | "unowned";

export default function CollectionPage() {
    const t = useTranslations("Collection");
    const [collection, setCollection] = useState<UserCollectionDto[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");
    const [activeFilter, setActiveFilter] = useState<FilterType>("all");
    const [selectedCardId, setSelectedCardId] = useState<number | null>(null);

    useEffect(() => {
        const fetchCollection = async () => {
            const result = await getUserCollection();
            if (result.success && result.data) {
                setCollection(result.data);
            } else {
                setError(result.error || "Failed to load collection");
            }
            setIsLoading(false);
        };
        fetchCollection();
    }, []);

    // Tìm thẻ bài đang được phóng to
    const selectedCardData = useMemo(() => {
        if (selectedCardId === null) return null;
        return TAROT_DECK.find(c => c.id === selectedCardId);
    }, [selectedCardId]);

    const selectedUserCard = useMemo(() => {
        return collection.find(c => c.cardId === selectedCardId);
    }, [collection, selectedCardId]);

    // Logic lọc thẻ bài thông minh
    const filteredDeck = useMemo(() => {
        return TAROT_DECK.filter(deckCard => {
            const isOwned = collection.some(c => c.cardId === deckCard.id);
            if (activeFilter === "owned") return isOwned;
            if (activeFilter === "unowned") return !isOwned;
            return true; // "all"
        });
    }, [collection, activeFilter]);

    if (isLoading) {
        return (
            <div className="min-h-screen bg-[#020108] flex items-center justify-center">
                <div className="relative group">
                    <div className="absolute inset-x-0 top-0 h-40 w-40 bg-purple-600/20 blur-[60px] rounded-full animate-pulse" />
                    <Loader2 className="w-12 h-12 animate-spin text-purple-400 relative z-10" />
                </div>
            </div>
        );
    }

    const totalCollected = collection.length;
    const progressRatio = (totalCollected / 78) * 100;

    return (
        <div className="min-h-screen bg-[#020108] text-zinc-100 selection:bg-purple-500/40 overflow-hidden font-sans">
            {/* ##### ZOOM OVERLAY ##### */}
            {selectedCardId !== null && selectedCardData && (
                <div className="fixed inset-0 z-[100] flex items-center justify-center p-6 md:p-12 animate-in fade-in duration-500">
                    {/* Backdrop: Chỉ click vào đây mới đóng */}
                    <div 
                        className="absolute inset-0 bg-black/80 backdrop-blur-xl" 
                        onClick={() => setSelectedCardId(null)}
                    />
                    
                    <div 
                        className="relative z-10 max-w-lg w-full bg-white/[0.03] border border-white/10 rounded-[3rem] p-8 md:p-12 shadow-[0_30px_100px_rgba(0,0,0,0.8)] animate-in zoom-in-95 slide-in-from-bottom-10 duration-500 flex flex-col items-center text-center overflow-hidden"
                    >
                        {/* Background Glow */}
                        <div className="absolute -top-24 -left-24 w-64 h-64 bg-purple-600/[0.08] blur-[100px] rounded-full" />
                        <div className="absolute -bottom-24 -right-24 w-64 h-64 bg-amber-500/[0.05] blur-[100px] rounded-full" />

                        {/* Card Visual Large */}
                        <div className={`relative aspect-[2/3.2] w-64 md:w-72 bg-gradient-to-br from-amber-50/10 to-transparent backdrop-blur-3xl rounded-[2.5rem] border border-white/10 p-6 mb-8 transition-transform duration-700 hover:scale-[1.02] ${selectedUserCard ? 'shadow-[0_0_50px_rgba(251,191,36,0.15)]' : 'grayscale opacity-50'}`}>
                            <div className="absolute inset-4 border border-amber-500/10 rounded-[1.8rem] pointer-events-none" />
                            <div className="h-full flex flex-col">
                                <div className="flex-1 rounded-2xl bg-zinc-950 overflow-hidden relative shadow-inner mb-4 border border-white/5 flex items-center justify-center">
                                    <Sparkles className={`w-20 h-20 ${selectedUserCard ? 'text-amber-500/30' : 'text-zinc-800'}`} />
                                    <div className="absolute inset-0 flex items-center justify-center">
                                        <span className="text-8xl font-serif font-black tracking-tighter text-white opacity-5">
                                            {selectedCardData.id + 1}
                                        </span>
                                    </div>
                                    <div className="absolute bottom-6 left-0 right-0 z-20 px-3 text-center">
                                        <span className="text-[10px] font-black uppercase tracking-[0.2em] text-amber-500/60 block mb-1">
                                            {selectedCardData.suit}
                                        </span>
                                        <h3 className="text-xl font-black text-white italic tracking-tighter leading-tight drop-shadow-lg">
                                            {selectedUserCard ? selectedCardData.name : t('unknown_card')}
                                        </h3>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* Info Section */}
                        <div className="relative z-10 space-y-4 max-w-sm">
                            <h2 className="text-2xl font-black text-white italic tracking-tight">
                                {selectedUserCard ? selectedCardData.name : t('unknown_card')}
                            </h2>
                            <p className="text-zinc-400 text-sm font-medium leading-relaxed italic">
                                {selectedUserCard ? selectedCardData.meaning : "Vũ trụ chưa khai mở thông điệp của lá bài này cho bạn."}
                            </p>

                            {selectedUserCard && (
                                <div className="grid grid-cols-2 gap-4 mt-8">
                                    <div className="bg-white/5 border border-white/10 rounded-2xl p-3 text-center">
                                        <span className="block text-[10px] uppercase font-black tracking-widest text-zinc-500 mb-1">{t('level_label')}</span>
                                        <span className="text-lg font-black text-amber-500">{selectedUserCard.level}</span>
                                    </div>
                                    <div className="bg-white/5 border border-white/10 rounded-2xl p-3 text-center">
                                        <span className="block text-[10px] uppercase font-black tracking-widest text-zinc-500 mb-1">{t('copies_label')}</span>
                                        <span className="text-lg font-black text-zinc-100">{selectedUserCard.copies}</span>
                                    </div>
                                </div>
                            )}

                            <button 
                                onClick={() => setSelectedCardId(null)}
                                className="mt-8 px-8 py-3 bg-white text-black rounded-full text-[10px] font-black uppercase tracking-widest hover:scale-105 active:scale-95 transition-all shadow-xl"
                            >
                                Đóng Giao Thức
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {/* ===== PREMIUM BACKGROUND SYSTEM ===== */}
            <div className="fixed inset-0 z-0 pointer-events-none">
                <div className="absolute inset-0 bg-[url('https://grainy-gradients.vercel.app/noise.svg')] opacity-[0.03] mix-blend-overlay"></div>
                <div className="absolute top-[-10%] left-[-10%] w-[70vw] h-[70vw] bg-purple-900/[0.07] blur-[120px] rounded-full animate-pulse" />
                <div className="absolute bottom-[-10%] right-[-10%] w-[60vw] h-[60vw] bg-indigo-950/[0.05] blur-[130px] rounded-full animate-pulse delay-1000" />
                
                {/* Spiritual Particles */}
                <div className="absolute inset-0">
                    {Array.from({ length: 20 }).map((_, i) => (
                        <div
                            key={i}
                            className="absolute w-px h-px bg-zinc-400 rounded-full animate-float opacity-[0.1]"
                            style={{
                                top: `${Math.random() * 100}%`,
                                left: `${Math.random() * 100}%`,
                                animationDuration: `${20 + Math.random() * 30}s`,
                                animationDelay: `${-Math.random() * 30}s`,
                            }}
                        />
                    ))}
                </div>
            </div>

            <main className="container mx-auto px-6 pt-28 pb-20 relative z-10">
                {/* Header Section - Compact & Elegant */}
                <div className="flex flex-col lg:flex-row lg:items-end justify-between gap-8 mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700">
                    <div className="space-y-4">
                        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-amber-500/5 border border-amber-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-amber-500 shadow-xl backdrop-blur-md">
                            <Sparkles className="w-3 h-3" />
                            Arcane Sanctuary
                        </div>
                        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white">
                            {t('title')}
                        </h1>
                        <p className="text-zinc-500 max-w-md text-sm font-medium leading-relaxed">
                            {t('subtitle')}
                        </p>
                    </div>

                    {/* Compact Collection Progress bar */}
                    <div className="w-full lg:w-72 space-y-3 bg-white/[0.02] border border-white/5 p-4 rounded-3xl backdrop-blur-sm shadow-xl">
                        <div className="flex justify-between items-end">
                            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-500">{t('progress_label')}</span>
                            <span className="text-sm font-black text-amber-400 tracking-tighter">{totalCollected} <span className="text-zinc-600">/ 78</span></span>
                        </div>
                        <div className="h-1 w-full bg-white/5 rounded-full overflow-hidden">
                            <div
                                className="h-full bg-gradient-to-r from-amber-600 via-amber-400 to-yellow-300 rounded-full transition-all duration-1000 ease-out shadow-[0_0_10px_rgba(251,191,36,0.2)]"
                                style={{ width: `${progressRatio}%` }}
                            />
                        </div>
                    </div>
                </div>

                {/* Filter Controls - Modern Tabs */}
                <div className="flex flex-wrap items-center gap-2 mb-10 animate-in fade-in duration-700 delay-300">
                    <div className="flex items-center gap-2 mr-4 text-zinc-500">
                        <Filter className="w-3.5 h-3.5" />
                        <span className="text-[10px] uppercase font-black tracking-widest">Filters</span>
                    </div>
                    {([
                        { id: "all", label: t('filter_all'), icon: <LayoutGrid className="w-3 h-3" /> },
                        { id: "owned", label: t('filter_owned'), icon: <CheckCircle2 className="w-3 h-3" /> },
                        { id: "unowned", label: t('filter_unowned'), icon: <Lock className="w-3 h-3" /> }
                    ] as const).map((filter) => (
                        <button
                            key={filter.id}
                            onClick={() => setActiveFilter(filter.id)}
                            className={`flex items-center gap-2 px-5 py-2 rounded-full text-xs font-black transition-all duration-300 border ${
                                activeFilter === filter.id
                                    ? "bg-white text-black border-white shadow-xl scale-105"
                                    : "bg-white/[0.03] text-zinc-500 border-white/[0.05] hover:border-white/20 hover:text-zinc-300"
                            }`}
                        >
                            {filter.icon}
                            {filter.label}
                        </button>
                    ))}
                </div>

                {error && (
                    <div className="mb-10 p-4 bg-red-500/10 border border-red-500/20 rounded-2xl flex items-center gap-3 text-red-400 text-xs animate-in zoom-in-95">
                        <AlertCircle className="w-4 h-4 flex-shrink-0" />
                        <p className="font-medium">{error}</p>
                    </div>
                )}

                {/* Grid UI - Compact Cards */}
                {collection.length === 0 && !error && activeFilter !== 'unowned' ? (
                    <div className="text-center py-24 bg-white/[0.01] border border-white/5 rounded-[3rem] animate-in fade-in duration-1000">
                        <div className="w-16 h-16 bg-white/5 rounded-3xl flex items-center justify-center mx-auto mb-6">
                            <LayoutGrid className="w-8 h-8 text-zinc-600" />
                        </div>
                        <h3 className="text-lg font-bold text-zinc-300 mb-2">{t('empty_title')}</h3>
                        <p className="text-zinc-500 mb-8 text-sm font-medium">{t('empty_desc')}</p>
                        <Link href="/reading" className="group relative px-8 py-3 bg-white text-black rounded-full font-black text-xs uppercase tracking-widest transition-all hover:scale-105 active:scale-95 shadow-xl">
                            <span className="flex items-center gap-2">
                                {t('cta_draw')} <ChevronRight className="w-4 h-4" />
                            </span>
                        </Link>
                    </div>
                ) : (
                    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-7 gap-4 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-500">
                        {filteredDeck.map((deckCard) => {
                            const userCard = collection.find(c => c.cardId === deckCard.id);
                            const isOwned = !!userCard;

                            return (
                                <div
                                    key={deckCard.id}
                                    onClick={() => setSelectedCardId(deckCard.id)}
                                    className={`group relative rounded-3xl p-3 flex flex-col items-center transition-all duration-500 border backdrop-blur-sm overflow-hidden cursor-pointer ${
                                        isOwned
                                            ? "bg-white/[0.03] border-white/10 hover:border-amber-500/40 hover:bg-white/[0.06] shadow-sm hover:shadow-[0_10px_30px_rgba(245,158,11,0.08)]"
                                            : "bg-black/40 border-white/[0.02] opacity-40 grayscale hover:grayscale-0 hover:opacity-100"
                                    }`}
                                >
                                    {/* Suit Tag - Compact */}
                                    <div className="w-full text-center mt-1 mb-2">
                                        <div className={`text-[8px] font-black uppercase tracking-[0.2em] px-2 py-0.5 rounded-full inline-block ${
                                            isOwned ? 'text-amber-500 bg-amber-500/5 border border-amber-500/10' : 'text-zinc-600 bg-zinc-900 overflow-hidden'
                                        }`}>
                                            {deckCard.suit.split(' ')[0]}
                                        </div>
                                    </div>

                                    {/* Visual Portion */}
                                    <div className={`w-full aspect-[2.5/4] rounded-2xl mb-3 flex items-center justify-center relative overflow-hidden transition-transform duration-500 group-hover:scale-[1.03] ${
                                        isOwned 
                                            ? 'bg-gradient-to-br from-zinc-800 to-zinc-900 border border-white/5' 
                                            : 'bg-zinc-900/40'
                                    }`}>
                                        <span className={`text-4xl font-serif font-black tracking-tighter opacity-20 ${isOwned ? 'text-amber-100' : 'text-zinc-800'}`}>
                                            {deckCard.id + 1}
                                        </span>
                                        
                                        {/* Symbolic Glow for owned cards */}
                                        {isOwned && (
                                            <>
                                                <div className="absolute inset-0 bg-gradient-to-t from-amber-900/20 via-transparent to-transparent opacity-60"></div>
                                                <div className="absolute bottom-[-10%] left-[-10%] w-[120%] h-1/2 bg-amber-500/[0.03] blur-2xl rounded-full" />
                                            </>
                                        )}
                                    </div>

                                    {/* Name Label */}
                                    <h4 className={`text-[11px] font-black text-center leading-snug mb-3 px-1 tracking-tight min-h-[1.5rem] flex items-center justify-center ${
                                        isOwned ? 'text-zinc-100' : 'text-zinc-600'
                                    }`}>
                                        {isOwned ? deckCard.name : t('unknown_card')}
                                    </h4>

                                    {/* Stats - Macro-UI */}
                                    {isOwned ? (
                                        <div className="w-full space-y-2 mt-auto">
                                            <div className="flex justify-between items-center text-[9px] font-black uppercase tracking-tighter text-zinc-500">
                                                <span>{t('level_label')} <span className="text-amber-500">{userCard.level}</span></span>
                                                <span className="opacity-60">{userCard.copies} {t('copies_label')}</span>
                                            </div>
                                            <div className="w-full h-0.5 bg-white/5 rounded-full overflow-hidden">
                                                <div 
                                                    className="h-full bg-amber-500 shadow-[0_0_5px_rgba(245,158,11,0.3)] transition-all duration-1000" 
                                                    style={{ width: `${Math.min((userCard.copies / 5) * 100, 100)}%` }} 
                                                />
                                            </div>
                                        </div>
                                    ) : (
                                        <div className="w-full mt-auto py-1.5 bg-white/[0.02] rounded-xl border border-white/[0.03] flex items-center justify-center">
                                            <Lock className="w-2.5 h-2.5 text-zinc-700" />
                                        </div>
                                    )}
                                </div>
                            );
                        })}
                    </div>
                )}
            </main>

            {/* Float Animation Styling */}
            <style dangerouslySetInnerHTML={{
                __html: `
                @keyframes float {
                    0% { transform: translateY(0) translateX(0); opacity: 0; }
                    10% { opacity: 0.15; }
                    90% { opacity: 0.15; }
                    100% { transform: translateY(-100vh) translateX(40px); opacity: 0; }
                }
                .animate-float {
                    animation-name: float;
                    animation-timing-function: linear;
                    animation-iteration-count: infinite;
                }
            `}} />
        </div>
    );
}
