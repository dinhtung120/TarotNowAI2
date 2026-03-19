'use client';

import React, { useEffect, useState } from 'react';
import { listReaders, type ReaderProfile } from '@/actions/readerActions';
import {
 Users, Search, Star,
 Loader2, Sparkles, Gem, Filter, Activity
} from 'lucide-react';
import { Link } from '@/i18n/routing';
import { useLocale, useTranslations } from 'next-intl';
import { SectionHeader, GlassCard, Pagination } from '@/components/ui';

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
 // State quản lý danh sách và bộ lọc
 const [readers, setReaders] = useState<ReaderProfile[]>([]);
 const [totalCount, setTotalCount] = useState(0);
 const [page, setPage] = useState(1);
 const [loading, setLoading] = useState(true);
 const [searchInput, setSearchInput] = useState('');
 const [searchTerm, setSearchTerm] = useState('');
 const [selectedSpecialty, setSelectedSpecialty] = useState('');
 const [selectedStatus, setSelectedStatus] = useState('');

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
 { value: 'offline', label: t('directory.statuses.offline') },
 ];

 const pageSize = 12;

 useEffect(() => {
 const debounceTimer = window.setTimeout(() => {
 setSearchTerm(searchInput.trim());
 }, 300);

 return () => window.clearTimeout(debounceTimer);
 }, [searchInput]);

 // Fetch danh sách
 useEffect(() => {
 const fetchReaders = async () => {
 setLoading(true);
 const result = await listReaders(page, pageSize, selectedSpecialty, selectedStatus, searchTerm);
 if (result) {
 setReaders(result.readers);
 setTotalCount(result.totalCount);
 }
 setLoading(false);
 };
 fetchReaders();
 }, [page, selectedSpecialty, selectedStatus, searchTerm]);

 const totalPages = Math.ceil(totalCount / pageSize);

 /**
 * Helper: Hiển thị status indicator
 */
 const getStatusIndicator = (status: string) => {
 switch (status) {
 case 'accepting_questions':
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--success)] shadow-[0_0_8px_var(--c-16-185-129-50)] animate-pulse" /><span className="text-[var(--success)] font-black uppercase tracking-wider text-[9px]">{t("directory.status_indicator.ready")}</span></div>;
 case 'online':
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_var(--c-245-158-11-50)]" /><span className="text-[var(--warning)] font-black uppercase tracking-wider text-[9px]">{t("directory.status_indicator.busy")}</span></div>;
 default:
 return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--text-muted)]" /><span className="tn-text-muted font-bold uppercase tracking-wider text-[9px]">{t("directory.status_indicator.offline")}</span></div>;
 }
 };

 return (
 <div className="max-w-7xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
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
 <GlassCard className="!p-4 sticky top-24 z-30 shadow-2xl ">
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
 <Link
 key={reader.id}
 href={`/readers/${reader.userId}`}
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
 </Link>
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
