"use client";

/*
 * ===================================================================
 * FILE: (user)/collection/page.tsx (Bộ Sưu Tập Bài Tarot)
 * BỐI CẢNH (CONTEXT):
 *   Giao diện hiển thị các lá bài Tarot mà User đã sở hữu thông qua gacha/rút bài.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lọc bài: Tất cả / Đã có / Chưa có.
 *   - Hiển thị tỷ lệ hoàn thành tiến độ thu thập thẻ (Progress Bar).
 *   - Overlay hiển thị chi tiết thẻ bài khi click vào 1 lá bài đã sở hữu (Level, Số bản sao).
 * ===================================================================
 */

import { useCollectionPage, type CollectionFilter, type CollectionSortOrder } from "@/features/collection/application/useCollectionPage";
import { Loader2, LayoutGrid, AlertCircle, ChevronRight, Sparkles, Filter, CheckCircle2, Lock } from "lucide-react";
import Image from "next/image";
import { Link } from "@/i18n/routing";
import { useTranslations } from "next-intl";
import { useCardsCatalog } from "@/shared/application/hooks/useCardsCatalog";

export default function CollectionPage() {
 const t = useTranslations("Collection");
 const tCommon = useTranslations("Common");
 const tTarot = useTranslations("Tarot");
 const { getCardImageUrl, getCardName, getCardMeaning } = useCardsCatalog();
 const {
  collection,
  isLoading,
  error,
  activeFilter,
  setActiveFilter,
  sortBy,
  setSortBy,
  selectedCardId,
  setSelectedCardId,
  selectedCardData,
  selectedUserCard,
  filteredDeck,
  totalCollected,
  totalCardCount,
  progressRatio,
 } = useCollectionPage();

 const selectedCardImageUrl = selectedCardData ? getCardImageUrl(selectedCardData.id) : undefined;
 const selectedCardName = selectedCardData
  ? (getCardName(selectedCardData.id) ?? '')
  : '';
 const selectedCardMeaning = selectedCardData
  ? (getCardMeaning(selectedCardData.id) ?? '')
  : '';
 const sortOptions: Array<{ id: CollectionSortOrder; label: string; icon: string }> = [
  { id: "id", label: "ID", icon: "📖" },
  { id: "level", label: "Level", icon: "⭐" },
  { id: "atk", label: "ATK", icon: "⚔️" },
  { id: "def", label: "DEF", icon: "🛡️" },
 ];

 if (isLoading) {
 return (
 <div className="min-h-screen tn-surface flex items-center justify-center">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--purple-accent)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--purple-accent)] relative z-10" />
 </div>
 </div>
 );
 }

 return (
 <div className="min-h-screen tn-surface tn-text-primary selection:bg-[var(--purple-accent)]/40 overflow-hidden font-sans">
 {/* ##### ZOOM OVERLAY ##### */}
  {selectedCardId !== null && selectedCardData && (
  <div className="fixed inset-0 z-[100] flex items-center justify-center p-6 md:p-12 animate-in fade-in duration-500">
 {/* Backdrop: Chỉ click vào đây mới đóng */}
 <div className="absolute inset-0 tn-overlay-strong " onClick={() => setSelectedCardId(null)}
 />
 <div className="relative z-10 max-w-4xl w-full tn-panel rounded-[3rem] p-6 sm:p-8 md:p-12 shadow-[0_30px_100px_var(--c-0-0-0-80)] animate-in zoom-in-95 slide-in-from-bottom-10 duration-500 flex flex-col md:flex-row items-center md:items-stretch gap-8 md:gap-12 text-center md:text-left overflow-hidden"
 >
 {/* Background Glow */}
 <div className="absolute -top-24 -left-24 w-64 h-64 bg-[var(--purple-accent)]/[0.08] blur-[100px] rounded-full" />
 <div className="absolute -bottom-24 -right-24 w-64 h-64 bg-[var(--warning)]/[0.05] blur-[100px] rounded-full" />

 {/* Card Visual Large */}
 <div className={`relative aspect-[14/22] w-52 sm:w-64 md:w-80 flex-shrink-0 bg-gradient-to-br from-[var(--warning)]/10 to-transparent rounded-[2.5rem] border tn-border p-5 sm:p-6 mb-2 md:mb-0 transition-transform duration-700 hover:scale-[1.02] ${selectedUserCard ? 'shadow-[0_0_50px_var(--c-251-191-36-15)]' : 'grayscale opacity-50'}`}>
 <div className="absolute inset-4 border border-[var(--warning)]/10 rounded-[1.8rem] pointer-events-none" />
 <div className="h-full flex flex-col">
 <div className="flex-1 rounded-2xl tn-overlay overflow-hidden relative shadow-inner mb-4 border tn-border-soft flex items-center justify-center">
 {selectedCardImageUrl ? (
 <Image
 src={selectedCardImageUrl}
 alt={selectedCardName || t('unknown_card')}
 fill
 unoptimized
 sizes="(max-width: 768px) 13rem, 20rem"
 className={`h-full w-full object-cover transition-all duration-500 ${selectedUserCard ? '' : 'blur-[6px]'}`}
 />
 ) : (
 <>
 <Sparkles className={`w-20 h-20 ${selectedUserCard ? 'text-[var(--warning)]/30' : 'tn-text-muted'}`} />
 <div className="absolute inset-0 flex items-center justify-center">
 <span className="text-8xl font-serif font-black tracking-tighter tn-text-primary opacity-5">
 {selectedCardData.id + 1}
 </span>
 </div>
 </>
 )}
 <div className="absolute bottom-6 left-0 right-0 z-20 px-3 text-center">
 <span className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--warning)]/60 block mb-1">
 {tTarot(`suits.${selectedCardData.suit}.full`)}
 </span>
 <h3 className="text-xl font-black tn-text-primary italic tracking-tighter leading-tight drop-shadow-lg">
 {selectedUserCard ? (selectedCardName || t('unknown_card')) : t('unknown_card')}
 </h3>
 </div>
 </div>
 </div>
 </div>

 {/* Info Section */}
 <div className="relative z-10 flex-1 flex flex-col justify-center">
 <h2 className="text-3xl md:text-4xl lg:text-5xl font-black tn-text-primary italic tracking-tight mb-4">
 {selectedUserCard ? (selectedCardName || t('unknown_card')) : t('unknown_card')}
 </h2>
 <div className="w-16 h-1 bg-[var(--warning)]/40 rounded-full mb-6 mx-auto md:mx-0" />
  <p className="tn-text-muted text-sm md:text-base font-medium leading-relaxed italic border-l-2 border-transparent md:border-[var(--warning)]/20 md:pl-4 py-2">
 {selectedUserCard ? (selectedCardMeaning || t("locked_meaning")) : t("locked_meaning")}
 </p>

 {selectedUserCard && (
 <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mt-8 md:mt-10 mx-auto md:mx-0">
 <div className="tn-panel rounded-2xl p-4 text-center transform transition duration-300 hover:scale-105 border tn-border-soft flex flex-col justify-center">
  <span className="block text-[10px] md:text-xs uppercase font-black tracking-widest tn-text-muted mb-1">{t('level_label')}</span>
  <span className="text-2xl md:text-3xl font-black text-[var(--warning)]">{selectedUserCard.level}</span>
  </div>
 <div className="tn-panel rounded-2xl p-4 text-center transform transition duration-300 hover:scale-105 border tn-border-soft flex flex-col justify-center">
  <span className="block text-[10px] md:text-xs uppercase font-black tracking-widest tn-text-muted mb-1">{t('copies_label')}</span>
  <span className="text-2xl md:text-3xl font-black tn-text-primary">{selectedUserCard.copies}</span>
  </div>
  <div className="tn-panel rounded-2xl p-4 text-center transform transition duration-300 hover:scale-105 border border-red-500/20 bg-red-500/5 flex flex-col justify-center">
    <span className="block text-[10px] md:text-xs uppercase font-black tracking-widest text-red-500/70 mb-1">ATK</span>
    <span className="text-2xl md:text-3xl font-black text-red-400 drop-shadow-[0_0_8px_rgba(248,113,113,0.5)]">{selectedUserCard.atk || 0}</span>
  </div>
  <div className="tn-panel rounded-2xl p-4 text-center transform transition duration-300 hover:scale-105 border border-blue-500/20 bg-blue-500/5 flex flex-col justify-center">
    <span className="block text-[10px] md:text-xs uppercase font-black tracking-widest text-blue-500/70 mb-1">DEF</span>
    <span className="text-2xl md:text-3xl font-black text-blue-400 drop-shadow-[0_0_8px_rgba(96,165,250,0.5)]">{selectedUserCard.def || 0}</span>
  </div>
 </div>
 )}

 <div className="mt-10 md:mt-auto pt-4 flex justify-center md:justify-start">
  <button onClick={() => setSelectedCardId(null)}
  className="px-10 py-3.5 tn-surface-strong tn-text-ink rounded-full text-[10px] md:text-xs font-black uppercase tracking-widest hover:scale-105 active:scale-95 transition-all shadow-xl"
  >
  {tCommon("close")}
  </button>
  </div>
 </div>
 </div>
 </div>
 )}

 <main className="container mx-auto px-4 sm:px-6 pt-28 pb-20 relative z-10">
 {/* Header Section - Compact & Elegant */}
 <div className="flex flex-col lg:flex-row lg:items-end justify-between gap-8 mb-16 animate-in fade-in slide-in-from-bottom-4 duration-700">
 <div className="space-y-4">
 <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-[var(--warning)]/5 border border-[var(--warning)]/10 text-[9px] uppercase tracking-[0.2em] font-black text-[var(--warning)] shadow-xl ">
 <Sparkles className="w-3 h-3" />
 {t("tag")}
 </div>
 <h1 className="text-4xl md:text-5xl font-black tracking-tighter tn-text-primary">
 {t('title')}
 </h1>
 <p className="tn-text-muted max-w-md text-sm font-medium leading-relaxed">
 {t('subtitle')}
 </p>
 </div>

 {/* Compact Collection Progress bar */}
 <div className="w-full lg:w-72 space-y-3 tn-panel-soft p-4 rounded-3xl shadow-xl">
 <div className="flex justify-between items-end">
 <span className="text-[10px] font-black uppercase tracking-widest tn-text-muted">{t('progress_label')}</span>
 <span className="text-sm font-black text-[var(--warning)] tracking-tighter">
 {totalCollected} <span className="tn-text-muted">/ {totalCardCount}</span>
 </span>
 </div>
 <div className="h-1 w-full tn-surface rounded-full overflow-hidden">
 <div
 className="h-full bg-gradient-to-r from-[var(--warning)] via-[var(--warning)] to-[var(--warning)] rounded-full transition-all duration-1000 ease-out shadow-[0_0_10px_var(--c-251-191-36-20)]"
 style={{ width: `${progressRatio}%` }}
 />
 </div>
 </div>
 </div>

  {/* Filter & Sort Controls */}
  <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 mb-10 animate-in fade-in duration-700 delay-300">
    {/* Left: Filter Tabs */}
    <div className="flex flex-wrap items-center gap-2">
      <div className="flex items-center gap-2 mr-4 tn-text-muted">
        <Filter className="w-3.5 h-3.5" />
        <span className="text-[10px] uppercase font-black tracking-widest">{t("filters_label")}</span>
      </div>
      {([
        { id: "all", label: t('filter_all'), icon: <LayoutGrid className="w-3 h-3" /> },
        { id: "owned", label: t('filter_owned'), icon: <CheckCircle2 className="w-3 h-3" /> },
        { id: "unowned", label: t('filter_unowned'), icon: <Lock className="w-3 h-3" /> }
      ] as const).map((filter) => (
        <button
          key={filter.id}
          onClick={() => setActiveFilter(filter.id as CollectionFilter)}
          className={`flex items-center gap-2 px-5 py-2 rounded-full text-xs font-black transition-all duration-300 border ${
            activeFilter === filter.id
              ? "tn-surface-strong tn-text-ink tn-border shadow-xl scale-105"
              : "tn-surface tn-text-muted tn-border-soft hover:tn-border-strong hover:tn-text-secondary"
          }`}
        >
          {filter.icon}
          {filter.label}
        </button>
      ))}
    </div>

    {/* Right: Sort Tabs */}
    <div className="flex flex-wrap items-center gap-2">
      <div className="flex items-center gap-2 mr-4 tn-text-muted">
        <Sparkles className="w-3.5 h-3.5" />
        <span className="text-[10px] uppercase font-black tracking-widest">{tCommon("sort_by") || "Sắp xếp theo"}</span>
      </div>
      {sortOptions.map((sort) => (
        <button
          key={sort.id}
          onClick={() => setSortBy(sort.id)}
          className={`flex items-center gap-2 px-4 py-1.5 rounded-xl text-[10px] font-black transition-all duration-300 border ${
            sortBy === sort.id
              ? "bg-[var(--warning)]/10 border-[var(--warning)]/30 text-[var(--warning)] shadow-lg scale-105"
              : "tn-surface tn-text-muted tn-border-soft hover:tn-border-strong hover:tn-text-secondary"
          }`}
        >
          <span className="text-xs">{sort.icon}</span>
          {sort.label}
        </button>
      ))}
    </div>
  </div>

 {error && (
 <div className="mb-10 p-4 bg-[var(--danger)]/10 border border-[var(--danger)]/20 rounded-2xl flex items-center gap-3 text-[var(--danger)] text-xs animate-in zoom-in-95">
 <AlertCircle className="w-4 h-4 flex-shrink-0" />
 <p className="font-medium">{error}</p>
 </div>
 )}

 {/* Grid UI - Compact Cards */}
 {collection.length === 0 && !error && activeFilter !== 'unowned' ? (
 <div className="text-center py-24 tn-surface-soft border tn-border-soft rounded-[3rem] animate-in fade-in duration-1000">
 <div className="w-16 h-16 tn-surface rounded-3xl flex items-center justify-center mx-auto mb-6">
 <LayoutGrid className="w-8 h-8 tn-text-muted" />
 </div>
 <h3 className="text-lg font-bold tn-text-secondary mb-2">{t('empty_title')}</h3>
 <p className="tn-text-muted mb-8 text-sm font-medium">{t('empty_desc')}</p>
 <Link href="/reading" className="group relative px-8 py-3 tn-surface-strong tn-text-ink rounded-full font-black text-xs uppercase tracking-widest transition-all hover:scale-105 active:scale-95 shadow-xl">
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
 const cardImageUrl = getCardImageUrl(deckCard.id);
 const cardName = getCardName(deckCard.id) ?? '';

 return (
 <div
 key={deckCard.id}
 onClick={() => setSelectedCardId(deckCard.id)}
 className={`group relative rounded-3xl p-3 flex flex-col items-center transition-transform duration-500 transform-gpu antialiased will-change-transform border overflow-hidden cursor-pointer ${
 isOwned
 ? "tn-panel hover:border-[var(--warning)]/40 hover:tn-surface-strong shadow-sm hover:shadow-[0_10px_30px_var(--c-245-158-11-08)]"
 : "tn-panel-overlay-soft opacity-50 grayscale"
 }`}
 >
 {/* Level/EXP Header */}
  <div className="w-full mb-2">
    {isOwned ? (
      <div className="flex flex-col gap-1.5 w-full">
        <div className="flex justify-between items-center text-[10px] font-black uppercase tracking-tighter tn-text-primary px-1">
          <span>Lv. {userCard.level}</span>
          <span className="text-[var(--warning)]">{userCard.copies % 5} / 5</span>
        </div>
        <div className="w-full h-1 tn-surface rounded-full overflow-hidden">
          <div
            className="h-full bg-[var(--warning)] shadow-[0_0_5px_var(--c-245-158-11-30)] transition-all duration-1000"
            style={{ width: `${((userCard.copies % 5) / 5) * 100}%` }}
          />
        </div>
      </div>
    ) : (
      <div className="h-4 w-full" />
    )}
  </div>

 {/* Visual Portion */}
 <div className={`w-full aspect-[14/22] rounded-2xl mb-3 flex items-center justify-center relative overflow-hidden transition-transform duration-500 transform-gpu antialiased will-change-transform ${isOwned ? 'group-hover:scale-[1.03]' : ''} ${
 isOwned ? 'bg-gradient-to-br from-[color:var(--c-215-189-226-26)] to-[color:var(--c-168-156-255-28)] border tn-border-soft' : 'tn-overlay'
 }`}>
 {cardImageUrl ? (
 <Image
 src={cardImageUrl}
 alt={cardName || t('unknown_card')}
 fill
 unoptimized
 priority={deckCard.id < 7}
 sizes="(max-width: 768px) 33vw, 220px"
 className={`h-full w-full object-cover ${isOwned ? 'backface-hidden' : 'blur-[6px]'}`}
 />
 ) : (
 <span className={`text-4xl font-serif font-black tracking-tighter opacity-20 ${isOwned ? 'text-[var(--warning)]' : 'tn-text-muted blur-[4px]'}`}>
 {deckCard.id + 1}
 </span>
 )}
 {/* Symbolic Glow for owned cards */}
 {isOwned && (
 <>
 <div className="absolute inset-0 bg-gradient-to-t from-[var(--warning)]/20 via-transparent to-transparent opacity-60"></div>
 <div className="absolute bottom-[-10%] left-[-10%] w-[120%] h-1/2 bg-[var(--warning)]/[0.03] blur-2xl rounded-full" />
 </>
 )}
 </div>

 {/* Name Label */}
 <h4 className={`text-[11px] font-black text-center leading-snug mb-3 px-1 tracking-tight min-h-[1.5rem] flex items-center justify-center ${
 isOwned ? 'tn-text-primary' : 'tn-text-muted'
 }`}>
 {isOwned ? (cardName || t('unknown_card')) : t('unknown_card')}
 </h4>

 {/* Stats - Macro-UI */}
 {/* Stats - Macro-UI */}
  {isOwned ? (
    <div className="w-full mt-auto flex gap-1 text-[9px] font-bold text-center">
      <div className="flex-1 bg-red-500/10 border border-red-500/20 text-red-400 rounded-lg py-1 shadow-inner shadow-red-500/10">
        ⚔️ {userCard.atk || 0}
      </div>
      <div className="flex-1 bg-blue-500/10 border border-blue-500/20 text-blue-400 rounded-lg py-1 shadow-inner shadow-blue-500/10">
        🛡️ {userCard.def || 0}
      </div>
    </div>
  ) : (
    <div className="w-full mt-auto py-1.5 tn-surface rounded-xl border tn-border-soft flex items-center justify-center">
      <Lock className="w-2.5 h-2.5 tn-text-muted" />
    </div>
  )}
 </div>
 );
 })}
 </div>
 )}
 </main>

 </div>
 );
}
