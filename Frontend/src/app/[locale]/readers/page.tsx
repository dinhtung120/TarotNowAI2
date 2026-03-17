'use client';

import React, { useEffect, useState } from 'react';
import { listReaders, type ReaderProfile } from '@/actions/readerActions';
import {
  Users, Search, Star, Wifi, WifiOff, MessageCircle,
  Loader2, Sparkles, ChevronLeft, ChevronRight, Gem, Filter
} from 'lucide-react';
import { useRouter } from '@/i18n/routing';

/**
 * Trang danh sách Reader công khai (Directory Listing).
 *
 * Tính năng:
 * → Hiển thị grid card Reader với glassmorphism design.
 * → Bộ lọc: chuyên môn (specialty), trạng thái online, tìm kiếm tên.
 * → Phân trang UI.
 * → Status indicator (online/offline/accepting_questions).
 * → Click vào card → xem profile chi tiết.
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

  // Danh sách chuyên môn — hardcoded vì ít thay đổi
  const specialties = [
    { value: '', label: 'Tất cả' },
    { value: 'love', label: '💕 Tình yêu' },
    { value: 'career', label: '💼 Sự nghiệp' },
    { value: 'general', label: '🔮 Tổng quát' },
    { value: 'health', label: '🌿 Sức khỏe' },
    { value: 'finance', label: '💰 Tài chính' },
  ];

  const statusOptions = [
    { value: '', label: 'Tất cả' },
    { value: 'accepting_questions', label: '🟢 Đang nhận câu hỏi' },
    { value: 'online', label: '🟡 Trực tuyến' },
    { value: 'offline', label: '⚫ Ngoại tuyến' },
  ];

  const pageSize = 12;

  // Fetch danh sách Reader mỗi khi filter/page thay đổi
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
   * Helper: Hiển thị status indicator theo trạng thái online.
   * → accepting_questions: xanh lá + nhấp nháy.
   * → online: vàng.
   * → offline: xám.
   */
  const getStatusIndicator = (status: string) => {
    switch (status) {
      case 'accepting_questions':
        return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-emerald-400 animate-pulse" /><span className="text-emerald-400">Sẵn sàng</span></div>;
      case 'online':
        return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-amber-400" /><span className="text-amber-400">Trực tuyến</span></div>;
      default:
        return <div className="flex items-center gap-1.5"><div className="w-2 h-2 rounded-full bg-zinc-600" /><span className="text-zinc-600">Ngoại tuyến</span></div>;
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-6 py-16 space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <div className="space-y-4">
          <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md text-left">
            <Sparkles className="w-3 h-3 text-purple-400" />
            Reader Directory
          </div>
          <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic text-left">
            Nhà Xem Bài
          </h1>
          <p className="text-zinc-500 font-medium max-w-lg text-sm leading-relaxed text-left">
            Tìm Reader phù hợp với bạn. Chỉ những Reader đang sẵn sàng mới có thể nhận câu hỏi.
          </p>
        </div>
        <div className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">
          {totalCount} Reader
        </div>
      </div>

      {/* Filters */}
      <div className="flex flex-col md:flex-row gap-4">
        {/* Search */}
        <div className="relative flex-1">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-600" />
          <input
            id="reader-search"
            type="text"
            placeholder="Tìm kiếm theo tên..."
            value={searchTerm}
            onChange={(e) => { setSearchTerm(e.target.value); setPage(1); }}
            className="w-full bg-white/[0.02] border border-white/10 rounded-xl pl-11 pr-4 py-3 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-purple-500/30 transition-all"
          />
        </div>

        {/* Specialty Filter */}
        <select
          id="reader-specialty-filter"
          value={selectedSpecialty}
          onChange={(e) => { setSelectedSpecialty(e.target.value); setPage(1); }}
          className="bg-white/[0.02] border border-white/10 rounded-xl px-4 py-3 text-sm text-white focus:outline-none focus:border-purple-500/30 transition-all appearance-none cursor-pointer"
        >
          {specialties.map(s => <option key={s.value} value={s.value} className="bg-zinc-900">{s.label}</option>)}
        </select>

        {/* Status Filter */}
        <select
          id="reader-status-filter"
          value={selectedStatus}
          onChange={(e) => { setSelectedStatus(e.target.value); setPage(1); }}
          className="bg-white/[0.02] border border-white/10 rounded-xl px-4 py-3 text-sm text-white focus:outline-none focus:border-purple-500/30 transition-all appearance-none cursor-pointer"
        >
          {statusOptions.map(s => <option key={s.value} value={s.value} className="bg-zinc-900">{s.label}</option>)}
        </select>
      </div>

      {/* Loading */}
      {loading && (
        <div className="h-[40vh] flex flex-col items-center justify-center space-y-4">
          <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
          <span className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">Đang tải danh sách...</span>
        </div>
      )}

      {/* Empty State */}
      {!loading && readers.length === 0 && (
        <div className="h-[40vh] flex flex-col items-center justify-center space-y-4">
          <Users className="w-16 h-16 text-zinc-800" />
          <p className="text-zinc-600 text-sm font-medium">Chưa có Reader nào phù hợp với bộ lọc.</p>
        </div>
      )}

      {/* Reader Grid */}
      {!loading && readers.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {readers.map((reader) => (
            <button
              key={reader.id}
              onClick={() => router.push(`/readers/${reader.userId}` as any)}
              className="group relative bg-white/[0.02] hover:bg-white/[0.04] backdrop-blur-3xl rounded-[2rem] border border-white/5 hover:border-purple-500/20 p-8 text-left transition-all duration-500 shadow-2xl overflow-hidden"
            >
              {/* Background Decoration */}
              <div className="absolute top-0 right-0 p-6 opacity-[0.03] group-hover:opacity-[0.06] transition-opacity duration-500">
                <Star size={100} className="text-purple-400" />
              </div>

              <div className="relative z-10 space-y-5">
                {/* Avatar + Name */}
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-purple-500/20 to-purple-600/10 border border-purple-500/20 flex items-center justify-center text-lg font-black text-purple-400">
                    {reader.displayName?.charAt(0)?.toUpperCase() || '?'}
                  </div>
                  <div>
                    <div className="text-sm font-black text-white">{reader.displayName || 'Reader'}</div>
                    <div className="text-[10px] font-medium">
                      {getStatusIndicator(reader.status)}
                    </div>
                  </div>
                </div>

                {/* Bio */}
                <p className="text-xs text-zinc-500 leading-relaxed line-clamp-2">
                  {reader.bioVi || reader.bioEn || 'Chưa có mô tả.'}
                </p>

                {/* Stats */}
                <div className="flex items-center gap-4">
                  <div className="flex items-center gap-1.5">
                    <Star className="w-3 h-3 text-amber-400" />
                    <span className="text-[10px] font-bold text-zinc-400">
                      {reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}
                    </span>
                    <span className="text-[10px] text-zinc-600">
                      ({reader.totalReviews})
                    </span>
                  </div>
                  <div className="flex items-center gap-1.5">
                    <Gem className="w-3 h-3 text-purple-400" />
                    <span className="text-[10px] font-bold text-zinc-400">{reader.diamondPerQuestion} 💎/câu hỏi</span>
                  </div>
                </div>

                {/* Specialties */}
                {reader.specialties.length > 0 && (
                  <div className="flex flex-wrap gap-1.5">
                    {reader.specialties.slice(0, 3).map((spec) => (
                      <span key={spec} className="px-2 py-0.5 rounded-full bg-purple-500/10 text-purple-400 text-[9px] font-bold uppercase tracking-wider border border-purple-500/10">
                        {spec}
                      </span>
                    ))}
                    {reader.specialties.length > 3 && (
                      <span className="px-2 py-0.5 text-zinc-600 text-[9px]">+{reader.specialties.length - 3}</span>
                    )}
                  </div>
                )}
              </div>
            </button>
          ))}
        </div>
      )}

      {/* Pagination */}
      {!loading && totalPages > 1 && (
        <div className="flex items-center justify-center gap-4">
          <button
            onClick={() => setPage(p => Math.max(1, p - 1))}
            disabled={page === 1}
            className="p-3 rounded-xl bg-white/[0.02] border border-white/10 hover:border-purple-500/30 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
          >
            <ChevronLeft className="w-4 h-4 text-zinc-400" />
          </button>
          <span className="text-[10px] font-black uppercase tracking-widest text-zinc-500">
            Trang {page} / {totalPages}
          </span>
          <button
            onClick={() => setPage(p => Math.min(totalPages, p + 1))}
            disabled={page === totalPages}
            className="p-3 rounded-xl bg-white/[0.02] border border-white/10 hover:border-purple-500/30 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
          >
            <ChevronRight className="w-4 h-4 text-zinc-400" />
          </button>
        </div>
      )}
    </div>
  );
}
