'use client';

import React, { useEffect, useState } from 'react';
import { listReaderRequests, processReaderRequest, type AdminReaderRequest } from '@/actions/adminActions';
import {
  Users, CheckCircle2, XCircle, Clock, Loader2, Sparkles,
  FileText, ChevronLeft, ChevronRight, Filter, Eye
} from 'lucide-react';

/**
 * Trang Admin quản lý đơn xin Reader (Approval Queue).
 *
 * Tính năng:
 * → Danh sách đơn xin Reader có phân trang.
 * → Filter theo status (pending/approved/rejected).
 * → Xem chi tiết lời giới thiệu.
 * → Approve/Reject với admin note.
 * → Premium astral design đồng bộ với admin dashboard.
 */
export default function AdminReaderRequestsPage() {
  // State quản lý danh sách và filter
  const [requests, setRequests] = useState<AdminReaderRequest[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState('pending');
  const [loading, setLoading] = useState(true);
  const [processing, setProcessing] = useState<string | null>(null);
  const [adminNote, setAdminNote] = useState('');
  const [selectedRequest, setSelectedRequest] = useState<AdminReaderRequest | null>(null);

  const pageSize = 10;

  // Fetch danh sách đơn
  const fetchRequests = async () => {
    setLoading(true);
    const result = await listReaderRequests(page, pageSize, statusFilter);
    if (result) {
      setRequests(result.requests);
      setTotalCount(result.totalCount);
    }
    setLoading(false);
  };

  useEffect(() => { fetchRequests(); }, [page, statusFilter]);

  const totalPages = Math.ceil(totalCount / pageSize);

  /**
   * Xử lý approve/reject đơn Reader.
   * Sau khi thành công, refresh danh sách.
   */
  const handleProcess = async (requestId: string, action: 'approve' | 'reject') => {
    setProcessing(requestId);
    const success = await processReaderRequest(requestId, action, adminNote);
    if (success) {
      setAdminNote('');
      setSelectedRequest(null);
      await fetchRequests();
    }
    setProcessing(null);
  };

  /** Helper: Badge status */
  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'pending':
        return <span className="px-2 py-0.5 rounded-full bg-amber-500/10 text-amber-400 text-[9px] font-bold uppercase border border-amber-500/20">Chờ duyệt</span>;
      case 'approved':
        return <span className="px-2 py-0.5 rounded-full bg-emerald-500/10 text-emerald-400 text-[9px] font-bold uppercase border border-emerald-500/20">Đã duyệt</span>;
      case 'rejected':
        return <span className="px-2 py-0.5 rounded-full bg-red-500/10 text-red-400 text-[9px] font-bold uppercase border border-red-500/20">Đã từ chối</span>;
      default:
        return null;
    }
  };

  return (
    <div className="max-w-5xl mx-auto px-6 py-16 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <div className="space-y-4">
          <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md text-left">
            <Sparkles className="w-3 h-3" />
            Admin — Reader Approval
          </div>
          <h1 className="text-4xl font-black tracking-tighter text-white uppercase italic text-left">
            Duyệt Đơn Reader
          </h1>
          <p className="text-zinc-500 font-medium text-sm text-left">
            Xem xét và phê duyệt các đơn đăng ký trở thành Reader.
          </p>
        </div>
        <div className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">
          {totalCount} đơn
        </div>
      </div>

      {/* Status Filter Tabs */}
      <div className="flex gap-2">
        {[
          { value: 'pending', label: 'Chờ duyệt', icon: Clock, color: 'amber' },
          { value: 'approved', label: 'Đã duyệt', icon: CheckCircle2, color: 'emerald' },
          { value: 'rejected', label: 'Đã từ chối', icon: XCircle, color: 'red' },
          { value: '', label: 'Tất cả', icon: Filter, color: 'zinc' },
        ].map((tab) => (
          <button
            key={tab.value}
            onClick={() => { setStatusFilter(tab.value); setPage(1); }}
            className={`flex items-center gap-2 px-4 py-2 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all border ${
              statusFilter === tab.value
                ? `bg-${tab.color}-500/10 border-${tab.color}-500/30 text-${tab.color}-400`
                : 'bg-white/[0.02] border-white/5 text-zinc-600 hover:border-white/10'
            }`}
          >
            <tab.icon className="w-3 h-3" />
            {tab.label}
          </button>
        ))}
      </div>

      {/* Loading */}
      {loading && (
        <div className="h-[30vh] flex flex-col items-center justify-center space-y-4">
          <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
        </div>
      )}

      {/* Empty */}
      {!loading && requests.length === 0 && (
        <div className="h-[30vh] flex flex-col items-center justify-center space-y-4">
          <FileText className="w-16 h-16 text-zinc-800" />
          <p className="text-zinc-600 text-sm">Không có đơn nào.</p>
        </div>
      )}

      {/* Requests List */}
      {!loading && requests.length > 0 && (
        <div className="space-y-4">
          {requests.map((req) => (
            <div
              key={req.id}
              className="relative bg-white/[0.02] backdrop-blur-3xl rounded-2xl border border-white/5 hover:border-white/10 p-6 transition-all space-y-4"
            >
              {/* Row: Status + User + Date */}
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-4">
                  {getStatusBadge(req.status)}
                  <span className="text-xs font-bold text-zinc-400">User: {req.userId.substring(0, 8)}...</span>
                </div>
                <span className="text-[10px] text-zinc-700">
                  {new Date(req.createdAt).toLocaleString('vi-VN')}
                </span>
              </div>

              {/* Intro Text */}
              <div className="p-4 rounded-xl bg-black/30 border border-white/5">
                <div className="text-[9px] font-black uppercase tracking-widest text-zinc-600 mb-2">Lời giới thiệu</div>
                <p className="text-xs text-zinc-400 leading-relaxed">
                  {selectedRequest?.id === req.id
                    ? req.introText
                    : req.introText.length > 150
                      ? req.introText.substring(0, 150) + '...'
                      : req.introText
                  }
                </p>
                {req.introText.length > 150 && selectedRequest?.id !== req.id && (
                  <button
                    onClick={() => setSelectedRequest(req)}
                    className="mt-2 text-[10px] text-purple-400 hover:text-purple-300 flex items-center gap-1"
                  >
                    <Eye className="w-3 h-3" /> Xem đầy đủ
                  </button>
                )}
              </div>

              {/* Admin Note (if reviewed) */}
              {req.adminNote && (
                <div className="p-3 rounded-lg bg-purple-500/5 border border-purple-500/10">
                  <div className="text-[9px] font-black uppercase tracking-widest text-purple-400 mb-1">Ghi chú Admin</div>
                  <p className="text-xs text-zinc-400">{req.adminNote}</p>
                </div>
              )}

              {/* Actions — chỉ hiện cho đơn pending */}
              {req.status === 'pending' && (
                <div className="space-y-3 pt-2 border-t border-white/5">
                  {/* Admin Note Input */}
                  <input
                    type="text"
                    placeholder="Ghi chú admin (tùy chọn)..."
                    value={selectedRequest?.id === req.id ? adminNote : ''}
                    onFocus={() => setSelectedRequest(req)}
                    onChange={(e) => { setSelectedRequest(req); setAdminNote(e.target.value); }}
                    className="w-full bg-white/[0.02] border border-white/10 rounded-lg px-4 py-2 text-xs text-white placeholder:text-zinc-700 focus:outline-none focus:border-purple-500/30"
                  />

                  {/* Approve/Reject Buttons */}
                  <div className="flex gap-3">
                    <button
                      id={`approve-${req.id}`}
                      onClick={() => handleProcess(req.id, 'approve')}
                      disabled={processing === req.id}
                      className="flex-1 flex items-center justify-center gap-2 px-4 py-2.5 rounded-xl bg-emerald-600/20 hover:bg-emerald-600/30 border border-emerald-500/20 text-emerald-400 text-[10px] font-black uppercase tracking-widest transition-all disabled:opacity-50"
                    >
                      {processing === req.id ? <Loader2 className="w-3 h-3 animate-spin" /> : <CheckCircle2 className="w-3 h-3" />}
                      Phê duyệt
                    </button>
                    <button
                      id={`reject-${req.id}`}
                      onClick={() => handleProcess(req.id, 'reject')}
                      disabled={processing === req.id}
                      className="flex-1 flex items-center justify-center gap-2 px-4 py-2.5 rounded-xl bg-red-600/20 hover:bg-red-600/30 border border-red-500/20 text-red-400 text-[10px] font-black uppercase tracking-widest transition-all disabled:opacity-50"
                    >
                      {processing === req.id ? <Loader2 className="w-3 h-3 animate-spin" /> : <XCircle className="w-3 h-3" />}
                      Từ chối
                    </button>
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {/* Pagination */}
      {!loading && totalPages > 1 && (
        <div className="flex items-center justify-center gap-4">
          <button
            onClick={() => setPage(p => Math.max(1, p - 1))}
            disabled={page === 1}
            className="p-3 rounded-xl bg-white/[0.02] border border-white/10 hover:border-purple-500/30 disabled:opacity-30 transition-all"
          >
            <ChevronLeft className="w-4 h-4 text-zinc-400" />
          </button>
          <span className="text-[10px] font-black uppercase tracking-widest text-zinc-500">
            Trang {page} / {totalPages}
          </span>
          <button
            onClick={() => setPage(p => Math.min(totalPages, p + 1))}
            disabled={page === totalPages}
            className="p-3 rounded-xl bg-white/[0.02] border border-white/10 hover:border-purple-500/30 disabled:opacity-30 transition-all"
          >
            <ChevronRight className="w-4 h-4 text-zinc-400" />
          </button>
        </div>
      )}
    </div>
  );
}
