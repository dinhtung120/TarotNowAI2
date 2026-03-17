'use client';

import React, { useEffect, useState } from 'react';
import { listReaders, type ReaderProfile } from '@/actions/readerActions';
import {
  Users, Search, Star, Wifi, WifiOff, MessageCircle,
  Loader2, Sparkles, ChevronLeft, ChevronRight, Gem, Filter, Activity
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';
import UserLayout from '@/components/layout/UserLayout';
import { SectionHeader, GlassCard, Button, Input } from '@/components/ui';

/**
 * Trang danh sách Reader công khai (Directory Listing).
 */
export default function ReaderDirectoryPage() {
  const router = useRouter();
  // State quản lý danh sách và bộ lọc
  const [readers, setReaders] = useState<ReaderProfile[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedSpecialty, setSelectedSpecialty] = useState('');
  const [selectedStatus, setSelectedStatus] = useState('');

  // Danh sách chuyên môn
  const specialties = [
    { value: '', label: 'Tất cả chuyên môn' },
    { value: 'love', label: '💕 Tình yêu' },
    { value: 'career', label: '💼 Sự nghiệp' },
    { value: 'general', label: '🔮 Tổng quát' },
    { value: 'health', label: '🌿 Sức khỏe' },
    { value: 'finance', label: '💰 Tài chính' },
  ];

  const statusOptions = [
    { value: '', label: 'Tất cả trạng thái' },
    { value: 'accepting_questions', label: '🟢 Đang nhận khách' },
    { value: 'online', label: '🟡 Trực tuyến (Bận)' },
    { value: 'offline', label: '⚫ Ngoại tuyến' },
  ];

  const pageSize = 12;

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
        return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--success)] shadow-[0_0_8px_rgba(16,185,129,0.5)] animate-pulse" /><span className="text-[var(--success)] font-black uppercase tracking-wider text-[9px]">Sẵn sàng</span></div>;
      case 'online':
        return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_rgba(245,158,11,0.5)]" /><span className="text-[var(--warning)] font-black uppercase tracking-wider text-[9px]">Đang bận</span></div>;
      default:
        return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-zinc-600" /><span className="text-zinc-500 font-bold uppercase tracking-wider text-[9px]">Ngoại tuyến</span></div>;
    }
  };

  return (
    <UserLayout>
      <div className="max-w-7xl mx-auto px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
        
        {/* Header & Stats */}
        <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 mb-2">
          <SectionHeader
            tag="Directory"
            tagIcon={<Users className="w-3 h-3 text-[var(--purple-accent)]" />}
            title="Nhà Nữ Tế"
            subtitle="Tìm Reader phù hợp với bạn. Chỉ những người đang sẵn sàng mới có thể nhận câu hỏi."
            className="mb-0 text-left items-start md:mb-0"
          />
          <div className="bg-white/[0.03] border border-white/10 rounded-2xl px-5 py-3 flex items-center gap-3 backdrop-blur-md self-start md:self-auto">
             <div className="w-10 h-10 rounded-full bg-[var(--purple-accent)]/20 flex items-center justify-center">
                <Users className="w-5 h-5 text-[var(--purple-accent)]" />
             </div>
             <div>
               <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Tổng số Reader</div>
               <div className="text-2xl font-black text-white leading-none">{totalCount}</div>
             </div>
          </div>
        </div>

        {/* Filters */}
        <GlassCard className="!p-4 sticky top-24 z-30 shadow-2xl backdrop-blur-3xl">
          <div className="flex flex-col md:flex-row gap-4">
            {/* Search */}
            <div className="relative flex-1">
              <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--text-tertiary)]" />
              <input
                id="reader-search"
                type="text"
                placeholder="Tìm kiếm theo tên..."
                value={searchTerm}
                onChange={(e) => { setSearchTerm(e.target.value); setPage(1); }}
                className="w-full bg-white/[0.02] border border-white/10 rounded-xl pl-11 pr-4 py-3 text-sm text-white placeholder:text-[var(--text-tertiary)] focus:outline-none focus:border-[var(--purple-accent)]/50 focus:ring-1 focus:ring-[var(--purple-accent)]/20 transition-all font-medium"
              />
            </div>

            {/* Specialty Filter */}
            <div className="relative md:w-48 shrink-0">
               <select
                 value={selectedSpecialty}
                 onChange={(e) => { setSelectedSpecialty(e.target.value); setPage(1); }}
                 className="w-full bg-white/[0.02] border border-white/10 rounded-xl pl-4 pr-10 py-3 text-sm text-white focus:outline-none focus:border-[var(--purple-accent)]/50 focus:ring-1 focus:ring-[var(--purple-accent)]/20 transition-all appearance-none cursor-pointer font-medium"
               >
                 {specialties.map(s => <option key={s.value} value={s.value} className="bg-zinc-900 text-white">{s.label}</option>)}
               </select>
               <Filter className="absolute right-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[var(--text-tertiary)] pointer-events-none" />
            </div>

            {/* Status Filter */}
            <div className="relative md:w-48 shrink-0">
               <select
                 value={selectedStatus}
                 onChange={(e) => { setSelectedStatus(e.target.value); setPage(1); }}
                 className="w-full bg-white/[0.02] border border-white/10 rounded-xl pl-4 pr-10 py-3 text-sm text-white focus:outline-none focus:border-[var(--purple-accent)]/50 focus:ring-1 focus:ring-[var(--purple-accent)]/20 transition-all appearance-none cursor-pointer font-medium"
               >
                 {statusOptions.map(s => <option key={s.value} value={s.value} className="bg-zinc-900 text-white">{s.label}</option>)}
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
             <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">Đang tải danh sách...</div>
          </div>
        )}

        {/* Empty State */}
        {!loading && readers.length === 0 && (
          <GlassCard className="h-[40vh] flex flex-col items-center justify-center space-y-4 border-dashed">
            <Users className="w-16 h-16 text-[var(--text-tertiary)] opacity-50" />
            <p className="text-[var(--text-secondary)] text-sm font-medium">Chưa có Reader nào phù hợp với bộ lọc.</p>
          </GlassCard>
        )}

        {/* Reader Grid */}
        {!loading && readers.length > 0 && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {readers.map((reader) => (
              <button
                key={reader.id}
                onClick={() => router.push(`/readers/${reader.userId}` as any)}
                className="group relative bg-white/[0.02] hover:bg-white/[0.05] backdrop-blur-3xl rounded-[2rem] border border-white/5 hover:border-[var(--purple-accent)]/30 p-6 text-left transition-all duration-500 shadow-xl hover:shadow-[0_0_40px_rgba(168,85,247,0.15)] overflow-hidden flex flex-col h-full"
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
                       <div className="w-full h-full rounded-full bg-zinc-900 border-2 border-white/10 flex items-center justify-center text-xl font-black text-white relative z-10 overflow-hidden">
                         {reader.displayName?.charAt(0)?.toUpperCase() || '?'}
                       </div>
                    </div>
                    <div>
                      <h3 className="text-base font-black text-white italic tracking-tight line-clamp-1">{reader.displayName || 'Reader'}</h3>
                      <div className="mt-1">
                        {getStatusIndicator(reader.status)}
                      </div>
                    </div>
                  </div>

                  {/* Bio */}
                  <p className="text-xs text-[var(--text-secondary)] font-medium leading-relaxed line-clamp-2 flex-grow">
                    {reader.bioVi || reader.bioEn || 'Chưa có mô tả chi tiết.'}
                  </p>

                  <div className="space-y-4 pt-4 border-t border-white/5">
                     {/* Stats */}
                     <div className="flex items-center justify-between">
                       <div className="flex items-center gap-1.5 bg-white/[0.03] px-2.5 py-1.5 rounded-lg border border-white/5">
                         <Star className="w-3.5 h-3.5 text-[var(--warning)]" fill="currentColor" />
                         <span className="text-xs font-black text-white">
                           {reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}
                         </span>
                         <span className="text-[10px] text-[var(--text-tertiary)] font-bold">
                           ({reader.totalReviews})
                         </span>
                       </div>
                       <div className="flex items-center gap-1.5 bg-[var(--purple-accent)]/10 px-2.5 py-1.5 rounded-lg border border-[var(--purple-accent)]/20">
                         <Gem className="w-3.5 h-3.5 text-[var(--purple-accent)]" />
                         <span className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)]">{reader.diamondPerQuestion} / Q</span>
                       </div>
                     </div>

                     {/* Specialties */}
                     {reader.specialties.length > 0 && (
                       <div className="flex flex-wrap gap-1.5">
                         {reader.specialties.slice(0, 3).map((spec) => (
                           <span key={spec} className="px-2 py-1 rounded-md bg-white/[0.03] text-[var(--text-secondary)] text-[9px] font-black uppercase tracking-wider border border-white/10 group-hover:border-[var(--text-secondary)]/30 transition-colors">
                             {spec}
                           </span>
                         ))}
                         {reader.specialties.length > 3 && (
                           <span className="px-2 py-1 flex items-center justify-center rounded-md bg-white/[0.02] text-[var(--text-tertiary)] text-[9px] font-black border border-white/5">
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
        {!loading && totalPages > 1 && (
          <div className="flex items-center justify-center gap-6 pt-8">
            <button
              onClick={() => setPage(p => Math.max(1, p - 1))}
              disabled={page === 1}
              className="p-3.5 rounded-xl bg-white/[0.03] border border-white/10 hover:bg-white/[0.08] hover:border-[var(--purple-accent)]/50 disabled:opacity-30 disabled:hover:border-white/10 disabled:hover:bg-white/[0.03] disabled:cursor-not-allowed transition-all"
            >
              <ChevronLeft className="w-5 h-5 text-white" />
            </button>
            <div className="bg-white/[0.02] border border-white/5 py-2 px-6 rounded-xl">
               <span className="text-[11px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]">
                 Trang <span className="text-white">{page}</span> / {totalPages}
               </span>
            </div>
            <button
              onClick={() => setPage(p => Math.min(totalPages, p + 1))}
              disabled={page === totalPages}
              className="p-3.5 rounded-xl bg-white/[0.03] border border-white/10 hover:bg-white/[0.08] hover:border-[var(--purple-accent)]/50 disabled:opacity-30 disabled:hover:border-white/10 disabled:hover:bg-white/[0.03] disabled:cursor-not-allowed transition-all"
            >
              <ChevronRight className="w-5 h-5 text-white" />
            </button>
          </div>
        )}
      </div>
    </UserLayout>
  );
}
