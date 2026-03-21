/*
 * ===================================================================
 * FILE: page.tsx (Admin Reader Requests)
 * BỐI CẢNH (CONTEXT):
 *   Trang Admin để quản lý Kiểm duyệt các tài khoản xin làm Reader.
 *   
 * BẢO MẬT & LUỒNG DỮ LIỆU:
 *   Sử dụng Client Component ('use client') tích hợp với Server Actions (`listReaderRequests`, `processReaderRequest`)
 *   để ẩn giấu logic xác thực backend trong khi vẫn giữ lại ưu điểm tương tác nhạy của React.
 * ===================================================================
 */
'use client';

import React from 'react';
import {
 Users, CheckCircle2, XCircle, Clock, Loader2,
 FileText, Filter, Eye, ShieldCheck
} from 'lucide-react';
import { SectionHeader, GlassCard, Button, Input, FilterTabs, StepPagination } from '@/components/ui';
import { useAdminReaderRequests } from '@/features/admin/reader-requests/application/useAdminReaderRequests';

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
 const {
  t,
  locale,
  requests,
  totalCount,
  page,
  setPage,
  statusFilter,
  setStatusFilter,
  loading,
  processing,
  adminNote,
  setAdminNote,
  selectedRequest,
  setSelectedRequest,
  totalPages,
  handleProcess,
 } = useAdminReaderRequests();

 /** Helper: Badge status */
 const getStatusBadge = (status: string) => {
 switch (status) {
 case 'pending':
 return <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-[var(--warning)]/10 text-[var(--warning)] text-[9px] font-black uppercase tracking-widest border border-[var(--warning)]/20 shadow-inner"><Clock className="w-3 h-3"/> {t("reader_requests.status.pending")}</span>;
 case 'approved':
 return <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-[var(--success)]/10 text-[var(--success)] text-[9px] font-black uppercase tracking-widest border border-[var(--success)]/20 shadow-inner"><CheckCircle2 className="w-3 h-3"/> {t("reader_requests.status.approved")}</span>;
 case 'rejected':
 return <span className="inline-flex items-center gap-1.5 px-3 py-1 rounded-full bg-[var(--danger)]/10 text-[var(--danger)] text-[9px] font-black uppercase tracking-widest border border-[var(--danger)]/20 shadow-inner"><XCircle className="w-3 h-3"/> {t("reader_requests.status.rejected")}</span>;
 default:
 return null;
 }
 };

 return (
 <div className="max-w-5xl mx-auto px-4 sm:px-6 py-16 space-y-8 animate-in fade-in duration-700">
 {/* Header */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("reader_requests.header.tag")}
 tagIcon={<ShieldCheck className="w-3 h-3 text-[var(--purple-accent)]" />}
 title={t("reader_requests.header.title")}
 subtitle={t("reader_requests.header.subtitle")}
 className="mb-0 text-left items-start"
 />
 <div className="flex items-center gap-4 tn-panel rounded-[2rem] p-2 pr-4 shadow-inner min-w-max">
 <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-inner">
 <Users className="w-4 h-4 text-[var(--purple-accent)]" />
 </div>
 <div className="space-y-0.5">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("reader_requests.summary.total_label")}</div>
 <div className="text-sm font-black tn-text-primary italic tracking-tighter drop-shadow-sm">{totalCount}</div>
 </div>
 </div>
 </div>

 {/* Status Filter Tabs */}
 <FilterTabs
 value={statusFilter}
 options={[
 {
 value: 'pending',
 label: t("reader_requests.filters.pending"),
 icon: <Clock className="w-4 h-4" />,
 activeClassName: 'bg-[var(--warning)]/10 border border-[var(--warning)]/30 text-[var(--warning)] shadow-md'
 },
 {
 value: 'approved',
 label: t("reader_requests.filters.approved"),
 icon: <CheckCircle2 className="w-4 h-4" />,
 activeClassName: 'bg-[var(--success)]/10 border border-[var(--success)]/30 text-[var(--success)] shadow-md'
 },
 {
 value: 'rejected',
 label: t("reader_requests.filters.rejected"),
 icon: <XCircle className="w-4 h-4" />,
 activeClassName: 'bg-[var(--danger)]/10 border border-[var(--danger)]/30 text-[var(--danger)] shadow-md'
 },
 {
 value: '',
 label: t("reader_requests.filters.all"),
 icon: <Filter className="w-4 h-4" />,
 activeClassName: 'bg-[var(--purple-accent)]/10 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] shadow-md'
 },
 ]}
 onChange={(value) => { setStatusFilter(value); setPage(1); }}
 />

 {/* Loading */}
 {loading && (
 <div className="py-20 flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-8 h-8 text-[var(--purple-accent)] animate-spin" />
 </div>
 )}

 {/* Empty */}
 {!loading && requests.length === 0 && (
 <GlassCard className="py-20 flex flex-col items-center justify-center space-y-4 text-center">
 <div className="w-16 h-16 rounded-full tn-surface flex items-center justify-center mb-2 shadow-inner">
 <FileText className="w-8 h-8 text-[var(--text-tertiary)]" />
 </div>
 <p className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("reader_requests.states.empty")}</p>
 </GlassCard>
 )}

 {/* Requests List */}
 {!loading && requests.length > 0 && (
 <div className="space-y-6">
 {requests.map((req) => (
 <GlassCard
 key={req.id}
 className="space-y-6 group hover:border-[var(--purple-accent)]/30 transition-all !p-8"
 >
 {/* Row: Status + User + Date */}
 <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 border-b tn-border-soft pb-4">
 <div className="flex items-center gap-4">
 {getStatusBadge(req.status)}
 <span className="text-xs font-bold text-[var(--text-secondary)] flex items-center gap-2">
 <Users className="w-4 h-4" />
 {t("reader_requests.row.id_prefix", { id: req.userId.substring(0, 8) })}
 </span>
 </div>
 <div className="flex items-center gap-2 text-[10px] font-black text-[var(--text-tertiary)] uppercase tracking-tighter tn-surface px-3 py-1.5 rounded-lg shadow-inner">
 <Clock className="w-3.5 h-3.5" />
 {new Date(req.createdAt).toLocaleString(locale)}
 </div>
 </div>

 {/* Intro Text */}
 <div className="p-5 rounded-2xl tn-panel-soft shadow-inner">
 <div className="flex items-center justify-between mb-3">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2">
 <FileText className="w-3.5 h-3.5" />
 {t("reader_requests.row.intro_title")}
 </div>
 {req.introText.length > 150 && selectedRequest?.id !== req.id && (
 <button
 onClick={() => setSelectedRequest(req)}
 className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] hover:tn-text-primary flex items-center gap-1.5 transition-colors"
 >
 <Eye className="w-3.5 h-3.5" /> {t("reader_requests.row.expand")}
 </button>
 )}
 </div>
 <p className="text-xs text-[var(--text-secondary)] leading-relaxed italic border-l-2 border-[var(--purple-accent)]/30 pl-4 py-1">
 {selectedRequest?.id === req.id
 ? req.introText
 : req.introText.length > 150
 ? req.introText.substring(0, 150) + '...'
 : req.introText
 }
 </p>
 </div>

 {/* Admin Note (if reviewed) */}
 {req.adminNote && (
 <div className="p-4 rounded-2xl bg-[var(--purple-accent)]/5 border border-[var(--purple-accent)]/20 shadow-inner">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] mb-2 flex items-center gap-2">
 <ShieldCheck className="w-3.5 h-3.5" />
 {t("reader_requests.row.admin_note_title")}
 </div>
 <p className="text-xs font-medium text-[var(--text-secondary)]">{req.adminNote}</p>
 </div>
 )}

 {/* Actions — chỉ hiện cho đơn pending */}
 {req.status === 'pending' && (
 <div className="space-y-4 pt-2">
 {/* Admin Note Input */}
 <Input
 placeholder={t("reader_requests.input.admin_note_placeholder")}
 value={selectedRequest?.id === req.id ? adminNote : ''}
 onFocus={() => setSelectedRequest(req)}
 onChange={(e) => { setSelectedRequest(req); setAdminNote(e.target.value); }}
 className="w-full text-xs font-black tracking-widest tn-text-primary shadow-inner tn-surface"
 />

 {/* Approve/Reject Buttons */}
 <div className="flex gap-4">
 <Button
 variant="secondary"
 id={`approve-${req.id}`}
 onClick={() => handleProcess(req.id, 'approve')}
 disabled={processing === req.id}
 className="flex-1 py-4 bg-[var(--success)] tn-text-primary hover:bg-[var(--success)] hover:brightness-110 shadow-md"
 >
 {processing === req.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <CheckCircle2 className="w-4 h-4" />}
 {t("reader_requests.actions.approve")}
 </Button>
 <Button
 variant="danger"
 id={`reject-${req.id}`}
 onClick={() => handleProcess(req.id, 'reject')}
 disabled={processing === req.id}
 className="flex-1 py-4 shadow-md bg-[var(--danger)]/20 border border-[var(--danger)]/30 text-[var(--danger)] hover:bg-[var(--danger)] hover:tn-text-primary"
 >
 {processing === req.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <XCircle className="w-4 h-4" />}
 {t("reader_requests.actions.reject")}
 </Button>
 </div>
 </div>
 )}
 </GlassCard>
 ))}
 </div>
 )}

 {/* Pagination */}
{!loading && totalPages > 1 && (
 <StepPagination
 className="flex items-center justify-center gap-4 pt-6"
 currentLabel={t("reader_requests.pagination.summary", { page, total: totalPages })}
 canPrev={page > 1}
 canNext={page < totalPages}
 onPrev={() => setPage((currentPage) => Math.max(1, currentPage - 1))}
 onNext={() => setPage((currentPage) => Math.min(totalPages, currentPage + 1))}
 />
)}
 </div>
 );
}
