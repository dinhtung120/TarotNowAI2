'use client';

import React from 'react';
import {
 Users, Search, Star,
 Loader2, Sparkles, Gem, Filter, Activity, X, MessageCircle
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import { useLocale, useTranslations } from 'next-intl';
import { SectionHeader, GlassCard, Pagination, Button } from '@/shared/components/ui';
import { useReadersDirectoryPage } from '@/features/reader/application/useReadersDirectoryPage';
import { normalizeReaderStatus } from '@/features/reader/domain/readerStatus';
import { createConversation } from '@/features/chat/public';
import type { ReaderProfile } from '@/features/reader/application/actions';
import toast from 'react-hot-toast';

/*
 * ===================================================================
 * FILE: readers/page.tsx (Danh bạ Reader)
 * BỐI CẢNH (CONTEXT):
 *   Trang danh sách công khai các Reader, hiển thị dưới dạng thẻ (Directory Listing).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Cho phép tìm kiếm, lọc theo chuyên môn (Specialty) và trạng thái (Online/Accepting).
 *   - Phân trang (Pagination) dữ liệu.
 *   - Nhấn vào Reader sẽ chuyển hướng đến trang Chi Tiết Reader (`/readers/[id]`).
 * ===================================================================
 */
export default function ReaderDirectoryPage() {
 const t = useTranslations("Readers");
 const locale = useLocale();
 const router = useRouter();
 const {
 readers,
 totalCount,
 page,
 setPage,
 loading,
 searchInput,
 setSearchInput,
 selectedSpecialty,
 setSelectedSpecialty,
 selectedStatus,
 setSelectedStatus,
 totalPages,
 } = useReadersDirectoryPage();
 const [selectedReader, setSelectedReader] = React.useState<ReaderProfile | null>(null);
 const [startingChat, setStartingChat] = React.useState(false);

 // Danh sách chuyên môn
 const specialties = [
 { value: '', label: t('directory.specialties.all') },
 { value: 'love', label: t('directory.specialties.love') },
 { value: 'career', label: t('directory.specialties.career') },
 { value: 'general', label: t('directory.specialties.general') },
 { value: 'health', label: t('directory.specialties.health') },
 { value: 'finance', label: t('directory.specialties.finance') },
 ];

 const statusOptions = [
 { value: '', label: t('directory.statuses.all') },
 { value: 'accepting_questions', label: t('directory.statuses.accepting') },
 { value: 'online', label: t('directory.statuses.online') },
 { value: 'away', label: t('directory.statuses.away') },
 { value: 'offline', label: t('directory.statuses.offline') },
 ];

 /**
 * Helper: Hiển thị status indicator
 */
 const getStatusIndicator = (status: string) => {
 switch (normalizeReaderStatus(status)) {
 case 'accepting_questions':
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--success)] shadow-[0_0_8px_var(--c-16-185-129-50)] animate-pulse" /><span className="text-[var(--success)] font-black uppercase tracking-wider text-[9px]">{t("directory.status_indicator.ready")}</span></div>;
 case 'online':
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_var(--c-245-158-11-50)]" /><span className="text-[var(--warning)] font-black uppercase tracking-wider text-[9px]">{t("directory.status_indicator.busy")}</span></div>;
 case 'away':
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_var(--c-245-158-11-50)]" /><span className="text-[var(--warning)] font-black uppercase tracking-wider text-[9px]">{t("directory.status_indicator.away")}</span></div>;
 default:
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--text-muted)]" /><span className="tn-text-muted font-bold uppercase tracking-wider text-[9px]">{t("directory.status_indicator.offline")}</span></div>;
 }
 };

 const selectedBio =
  selectedReader
   ? (
     locale === "vi"
      ? (selectedReader.bioVi || selectedReader.bioEn || selectedReader.bioZh)
      : locale === "en"
       ? (selectedReader.bioEn || selectedReader.bioVi || selectedReader.bioZh)
       : (selectedReader.bioZh || selectedReader.bioEn || selectedReader.bioVi)
    ) || t("directory.bio_fallback")
   : '';

 const handleStartConversation = async () => {
  if (!selectedReader) return;
  setStartingChat(true);
  try {
   const result = await createConversation(selectedReader.userId);
   if (!result.success || !result.data?.id) {
    toast.error(t("profile.toast_create_conversation_fail"));
    return;
   }

   setSelectedReader(null);
   router.push(`/chat/${result.data.id}`);
  } finally {
   setStartingChat(false);
  }
 };

 return (
 <div className="max-w-7xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {selectedReader ? (
  <div className="fixed inset-0 md:left-64 z-[100] flex items-center justify-center p-6 md:p-12 animate-in fade-in duration-500">
   <div className="absolute inset-0 tn-overlay-strong" onClick={() => setSelectedReader(null)} />
   <div className="relative z-10 max-w-3xl w-full tn-panel rounded-[2.5rem] p-6 sm:p-8 md:p-10 shadow-[0_30px_100px_var(--c-0-0-0-80)] animate-in zoom-in-95 slide-in-from-bottom-10 duration-500 overflow-hidden">
    <div className="absolute top-4 right-4">
     <button
      type="button"
      onClick={() => setSelectedReader(null)}
      className="w-9 h-9 rounded-xl tn-surface border tn-border-soft flex items-center justify-center hover:tn-surface-strong"
     >
      <X className="w-4 h-4" />
     </button>
    </div>

    <div className="space-y-6">
     <div className="flex items-center gap-4">
      <div className="w-16 h-16 rounded-full tn-surface-strong border-2 tn-border flex items-center justify-center text-xl font-black tn-text-primary overflow-hidden">
       {selectedReader.displayName?.charAt(0)?.toUpperCase() || '?'}
      </div>
      <div className="min-w-0">
       <h3 className="text-2xl font-black italic tracking-tight line-clamp-1">{selectedReader.displayName || t("directory.reader_fallback")}</h3>
       <div className="mt-1">{getStatusIndicator(selectedReader.status)}</div>
      </div>
     </div>

     <p className="text-sm text-[var(--text-secondary)] leading-relaxed">
      {selectedBio}
     </p>

     <div className="flex flex-wrap items-center gap-3">
      <div className="flex items-center gap-1.5 tn-surface px-3 py-1.5 rounded-lg border tn-border-soft">
       <Star className="w-3.5 h-3.5 text-[var(--warning)]" fill="currentColor" />
       <span className="text-xs font-black tn-text-primary">
        {selectedReader.avgRating > 0 ? selectedReader.avgRating.toFixed(1) : '--'}
       </span>
       <span className="text-[10px] text-[var(--text-tertiary)] font-bold">
        ({selectedReader.totalReviews})
       </span>
      </div>
      <div className="flex items-center gap-1.5 bg-[var(--purple-accent)]/10 px-3 py-1.5 rounded-lg border border-[var(--purple-accent)]/20">
       <Gem className="w-3.5 h-3.5 text-[var(--purple-accent)]" />
       <span className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)]">{selectedReader.diamondPerQuestion} {t("directory.per_question_suffix")}</span>
      </div>
     </div>

     {selectedReader.specialties.length > 0 ? (
      <div className="flex flex-wrap gap-2">
       {selectedReader.specialties.map((spec) => (
        <span key={spec} className="px-3 py-1 rounded-lg tn-surface text-[var(--text-secondary)] text-[10px] font-black uppercase tracking-wider border tn-border">
         {spec}
        </span>
       ))}
      </div>
     ) : null}

     <div className="pt-4 border-t tn-border-soft flex justify-end">
      <Button
       onClick={handleStartConversation}
       disabled={startingChat}
       className="px-6 py-3"
      >
       {startingChat ? <Loader2 className="w-4 h-4 animate-spin mr-2" /> : <MessageCircle className="w-4 h-4 mr-2" />}
       {t("profile.cta_send_question")}
      </Button>
     </div>
    </div>
   </div>
  </div>
 ) : null}

 {/* Header & Stats */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 mb-2">
 <SectionHeader
 tag={t("directory.tag")}
 tagIcon={<Users className="w-3 h-3 text-[var(--purple-accent)]" />}
 title={t("directory.title")}
 subtitle={t("directory.subtitle")}
 className="mb-0 text-left items-start md:mb-0"
 />
 <div className="tn-panel rounded-2xl px-5 py-3 flex items-center gap-3 self-start md:self-auto">
 <div className="w-10 h-10 rounded-full bg-[var(--purple-accent)]/20 flex items-center justify-center">
 <Users className="w-5 h-5 text-[var(--purple-accent)]" />
 </div>
 <div>
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("directory.total_label")}</div>
 <div className="text-2xl font-black tn-text-primary leading-none">{totalCount}</div>
 </div>
 </div>
 </div>

 {/* Filters */}
 <GlassCard className="!p-4 md:sticky md:top-24 md:z-30 shadow-2xl">
 <div className="flex flex-col md:flex-row gap-4">
 {/* Search */}
 <div className="relative flex-1">
 <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--text-tertiary)]" />
 <input
 id="reader-search"
 type="text"
 placeholder={t("directory.search_placeholder")}
 value={searchInput}
 onChange={(e) => { setSearchInput(e.target.value); setPage(1); }}
 className="w-full tn-field rounded-xl pl-11 pr-4 py-3 text-sm tn-text-primary placeholder:text-[var(--text-tertiary)] tn-field-accent transition-all font-medium"
 />
 </div>

 {/* Specialty Filter */}
 <div className="relative md:w-48 shrink-0">
 <select
 value={selectedSpecialty}
 onChange={(e) => { setSelectedSpecialty(e.target.value); setPage(1); }}
 className="w-full tn-field rounded-xl pl-4 pr-10 py-3 text-sm tn-text-primary tn-field-accent transition-all appearance-none cursor-pointer font-medium"
 >
 {specialties.map(s => <option key={s.value} value={s.value} className="tn-surface-strong tn-text-primary">{s.label}</option>)}
 </select>
 <Filter className="absolute right-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--text-tertiary)] pointer-events-none" />
 </div>

 {/* Status Filter */}
 <div className="relative md:w-48 shrink-0">
 <select
 value={selectedStatus}
 onChange={(e) => { setSelectedStatus(e.target.value); setPage(1); }}
 className="w-full tn-field rounded-xl pl-4 pr-10 py-3 text-sm tn-text-primary tn-field-accent transition-all appearance-none cursor-pointer font-medium"
 >
 {statusOptions.map(s => <option key={s.value} value={s.value} className="tn-surface-strong tn-text-primary">{s.label}</option>)}
 </select>
 <Activity className="absolute right-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--text-tertiary)] pointer-events-none" />
 </div>
 </div>
 </GlassCard>

 {/* Loading */}
 {loading && (
 <div className="h-[40vh] flex flex-col items-center justify-center space-y-6">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--purple-accent)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--purple-accent)] relative z-10" />
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t("directory.loading")}</div>
 </div>
 )}

 {/* Empty State */}
 {!loading && readers.length === 0 && (
 <GlassCard className="h-[40vh] flex flex-col items-center justify-center space-y-4 border-dashed">
 <Users className="w-16 h-16 text-[var(--text-tertiary)] opacity-50" />
 <p className="text-[var(--text-secondary)] text-sm font-medium">{t("directory.empty")}</p>
 </GlassCard>
 )}

 {/* Reader Grid */}
 {!loading && readers.length > 0 && (
 <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
 {readers.map((reader) => (
 <button
 key={reader.id}
 type="button"
 onClick={() => setSelectedReader(reader)}
 className="group relative tn-surface hover:tn-surface-strong rounded-[2rem] border tn-border-soft hover:border-[var(--purple-accent)]/30 p-6 text-left transition-all duration-500 shadow-xl hover:shadow-[0_0_40px_var(--c-168-85-247-15)] overflow-hidden flex flex-col h-full"
 >
 {/* Background Decoration */}
 <div className="absolute top-0 right-0 p-4 opacity-[0.02] group-hover:opacity-[0.05] group-hover:rotate-12 group-hover:scale-110 transition-all duration-700">
 <Sparkles size={120} className="text-[var(--purple-accent)]" />
 </div>

 <div className="relative z-10 space-y-5 flex-grow flex flex-col">
 {/* Avatar + Name + Status */}
 <div className="flex items-center gap-4">
 <div className="relative w-14 h-14 shrink-0">
 <div className="absolute inset-0 bg-gradient-to-br from-[var(--purple-accent)]/40 to-[var(--warning)]/20 rounded-full blur-md opacity-50 group-hover:opacity-100 transition-opacity" />
 <div className="w-full h-full rounded-full tn-surface-strong border-2 tn-border flex items-center justify-center text-xl font-black tn-text-primary relative z-10 overflow-hidden">
 {reader.displayName?.charAt(0)?.toUpperCase() || '?'}
 </div>
 </div>
 <div>
 <h3 className="text-base font-black tn-text-primary italic tracking-tight line-clamp-1">{reader.displayName || t("directory.reader_fallback")}</h3>
 <div className="mt-1">
 {getStatusIndicator(reader.status)}
 </div>
 </div>
 </div>

 {/* Bio */}
 <p className="text-xs text-[var(--text-secondary)] font-medium leading-relaxed line-clamp-2 flex-grow">
 {(
  locale === "vi"
   ? (reader.bioVi || reader.bioEn || reader.bioZh)
   : locale === "en"
    ? (reader.bioEn || reader.bioVi || reader.bioZh)
    : (reader.bioZh || reader.bioEn || reader.bioVi)
 ) || t("directory.bio_fallback")}
 </p>

 <div className="space-y-4 pt-4 border-t tn-border-soft">
 {/* Stats */}
 <div className="flex items-center justify-between">
 <div className="flex items-center gap-1.5 tn-surface px-2.5 py-1.5 rounded-lg border tn-border-soft">
 <Star className="w-3.5 h-3.5 text-[var(--warning)]" fill="currentColor" />
 <span className="text-xs font-black tn-text-primary">
 {reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}
 </span>
 <span className="text-[10px] text-[var(--text-tertiary)] font-bold">
 ({reader.totalReviews})
 </span>
 </div>
 <div className="flex items-center gap-1.5 bg-[var(--purple-accent)]/10 px-2.5 py-1.5 rounded-lg border border-[var(--purple-accent)]/20">
 <Gem className="w-3.5 h-3.5 text-[var(--purple-accent)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)]">{reader.diamondPerQuestion} {t("directory.per_question_suffix")}</span>
 </div>
 </div>

 {/* Specialties */}
 {reader.specialties.length > 0 && (
 <div className="flex flex-wrap gap-1.5">
 {reader.specialties.slice(0, 3).map((spec) => (
 <span key={spec} className="px-2 py-1 rounded-md tn-surface text-[var(--text-secondary)] text-[9px] font-black uppercase tracking-wider border tn-border group-hover:border-[var(--text-secondary)]/30 transition-colors">
 {spec}
 </span>
 ))}
 {reader.specialties.length > 3 && (
 <span className="px-2 py-1 flex items-center justify-center rounded-md tn-surface text-[var(--text-tertiary)] text-[9px] font-black border tn-border-soft">
 +{reader.specialties.length - 3}
 </span>
 )}
 </div>
 )}
 </div>
 </div>
 </button>
 ))}
 </div>
 )}

 {/* Pagination */}
 {!loading && (
 <Pagination
 currentPage={page}
 totalPages={totalPages}
 onPageChange={setPage}
 className="mt-8"
 />
 )}
 </div>
 );
}
