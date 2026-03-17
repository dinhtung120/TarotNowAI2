'use client';

import React, { useState, useEffect } from 'react';
import { submitReaderApplication, getMyReaderRequest, type MyReaderRequest } from '@/actions/readerActions';
import {
  FileText, Send, Clock, CheckCircle2, XCircle, Loader2, Sparkles, Star, ScrollText
} from 'lucide-react';

/**
 * Trang đăng ký trở thành Reader (Nhà xem bài Tarot).
 *
 * Thiết kế:
 * → Kiểm tra trạng thái đơn hiện tại trước khi hiển thị form.
 * → Nếu đã có đơn pending → hiển thị status card thay vì form.
 * → Nếu chưa có đơn hoặc đã bị reject → hiển thị form đăng ký.
 * → Premium astral design theo phong cách chung của hệ thống.
 */
export default function ReaderApplyPage() {
  // State quản lý form và trạng thái đơn
  const [introText, setIntroText] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState<'success' | 'error'>('success');
  const [existingRequest, setExistingRequest] = useState<MyReaderRequest | null>(null);
  const [loading, setLoading] = useState(true);

  // Fetch trạng thái đơn hiện tại khi component mount
  useEffect(() => {
    const fetchStatus = async () => {
      const result = await getMyReaderRequest();
      setExistingRequest(result);
      setLoading(false);
    };
    fetchStatus();
  }, []);

  /**
   * Xử lý submit — gửi đơn xin Reader lên backend.
   * Sau khi thành công, refresh trạng thái đơn.
   */
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (introText.length < 20) {
      setMessage('Lời giới thiệu phải có ít nhất 20 ký tự.');
      setMessageType('error');
      return;
    }
    setSubmitting(true);
    setMessage('');

    const result = await submitReaderApplication(introText);
    setMessage(result.message);
    setMessageType(result.success ? 'success' : 'error');
    setSubmitting(false);

    if (result.success) {
      // Refresh trạng thái đơn sau khi gửi thành công
      const updatedRequest = await getMyReaderRequest();
      setExistingRequest(updatedRequest);
    }
  };

  // Loading state
  if (loading) {
    return (
      <div className="min-h-[60vh] flex flex-col items-center justify-center space-y-4">
        <Loader2 className="w-10 h-10 text-purple-500 animate-spin" />
        <span className="text-[10px] font-black uppercase tracking-[0.3em] text-zinc-600">
          Kiểm tra trạng thái đăng ký...
        </span>
      </div>
    );
  }

  // Nếu đã có đơn pending → hiển thị status card
  if (existingRequest?.hasRequest && existingRequest.status === 'pending') {
    return (
      <div className="max-w-2xl mx-auto px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000">
        <div className="relative overflow-hidden bg-gradient-to-br from-amber-500/10 to-transparent backdrop-blur-3xl rounded-[3rem] border border-amber-500/20 p-12 shadow-2xl">
          <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none">
            <Clock size={180} className="text-amber-400" />
          </div>
          <div className="relative z-10 space-y-6 text-center">
            <div className="w-16 h-16 mx-auto rounded-2xl bg-amber-500/20 flex items-center justify-center border border-amber-500/30">
              <Clock className="w-8 h-8 text-amber-400" />
            </div>
            <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter">
              Đơn Đang Chờ Duyệt
            </h1>
            <p className="text-zinc-400 text-sm leading-relaxed max-w-md mx-auto">
              Đơn đăng ký Reader của bạn đã được gửi thành công và đang chờ admin xem xét.
              Bạn sẽ nhận được thông báo khi có kết quả.
            </p>
            <div className="p-6 rounded-2xl bg-black/40 border border-white/5 text-left space-y-2">
              <div className="text-[10px] font-black uppercase tracking-widest text-amber-400">Lời giới thiệu đã gửi</div>
              <p className="text-xs text-zinc-400 leading-relaxed">{existingRequest.introText}</p>
            </div>
            <div className="text-[10px] font-black uppercase tracking-[0.2em] text-zinc-600">
              Gửi lúc: {new Date(existingRequest.createdAt || '').toLocaleString('vi-VN')}
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Nếu đã được approved
  if (existingRequest?.hasRequest && existingRequest.status === 'approved') {
    return (
      <div className="max-w-2xl mx-auto px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000">
        <div className="relative overflow-hidden bg-gradient-to-br from-emerald-500/10 to-transparent backdrop-blur-3xl rounded-[3rem] border border-emerald-500/20 p-12 shadow-2xl">
          <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none">
            <CheckCircle2 size={180} className="text-emerald-400" />
          </div>
          <div className="relative z-10 space-y-6 text-center">
            <div className="w-16 h-16 mx-auto rounded-2xl bg-emerald-500/20 flex items-center justify-center border border-emerald-500/30">
              <CheckCircle2 className="w-8 h-8 text-emerald-400" />
            </div>
            <h1 className="text-3xl font-black text-white uppercase italic tracking-tighter">
              Bạn Đã Là Reader!
            </h1>
            <p className="text-zinc-400 text-sm leading-relaxed max-w-md mx-auto">
              Chúc mừng! Đơn đăng ký Reader của bạn đã được phê duyệt. Hãy cập nhật hồ sơ và bắt đầu nhận câu hỏi.
            </p>
          </div>
        </div>
      </div>
    );
  }

  // Nếu bị rejected → cho phép submit lại
  const wasRejected = existingRequest?.hasRequest && existingRequest.status === 'rejected';

  // Form đăng ký
  return (
    <div className="max-w-2xl mx-auto px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      <div className="space-y-10">
        {/* Header */}
        <div className="text-center space-y-4">
          <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-purple-500/5 border border-purple-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-purple-400 shadow-xl backdrop-blur-md">
            <Sparkles className="w-3 h-3 text-purple-400" />
            Reader Application
          </div>
          <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
            Trở Thành Reader
          </h1>
          <p className="text-zinc-500 font-medium max-w-lg mx-auto text-sm leading-relaxed">
            Chia sẻ khả năng đọc bài Tarot của bạn với cộng đồng. Hãy cho chúng tôi biết tại sao bạn muốn trở thành Reader.
          </p>
        </div>

        {/* Rejected Notice */}
        {wasRejected && (
          <div className="p-6 rounded-2xl bg-red-500/5 border border-red-500/20 space-y-2">
            <div className="flex items-center gap-2">
              <XCircle className="w-4 h-4 text-red-400" />
              <span className="text-[10px] font-black uppercase tracking-widest text-red-400">Đơn trước đã bị từ chối</span>
            </div>
            {existingRequest?.adminNote && (
              <p className="text-xs text-zinc-400 leading-relaxed">Lý do: {existingRequest.adminNote}</p>
            )}
            <p className="text-xs text-zinc-500">Bạn có thể gửi đơn mới bên dưới.</p>
          </div>
        )}

        {/* Form */}
        <form onSubmit={handleSubmit} className="space-y-8">
          {/* Info Cards */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {[
              { icon: ScrollText, title: 'Giới thiệu bản thân', desc: 'Chia sẻ kinh nghiệm Tarot', color: 'purple' },
              { icon: Clock, title: 'Chờ phê duyệt', desc: 'Admin sẽ xem xét đơn', color: 'amber' },
              { icon: Star, title: 'Bắt đầu nhận câu hỏi', desc: 'Sau khi được duyệt', color: 'emerald' }
            ].map((step, i) => (
              <div key={i} className="p-5 rounded-2xl bg-white/[0.02] border border-white/5 text-center space-y-2">
                <step.icon className={`w-6 h-6 mx-auto text-${step.color}-400`} />
                <div className="text-[10px] font-black uppercase tracking-widest text-zinc-300">{step.title}</div>
                <p className="text-[10px] text-zinc-600">{step.desc}</p>
              </div>
            ))}
          </div>

          {/* Textarea */}
          <div className="space-y-3">
            <label className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-zinc-400">
              <FileText className="w-3 h-3" />
              Lời giới thiệu *
            </label>
            <textarea
              id="reader-intro-text"
              value={introText}
              onChange={(e) => setIntroText(e.target.value)}
              placeholder="Hãy chia sẻ kinh nghiệm đọc bài Tarot của bạn, tại sao bạn muốn trở thành Reader, và bạn có thể mang lại giá trị gì cho cộng đồng..."
              rows={6}
              className="w-full bg-white/[0.02] border border-white/10 rounded-2xl p-6 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-purple-500/40 focus:ring-1 focus:ring-purple-500/20 transition-all resize-none"
            />
            <div className="flex justify-between">
              <span className={`text-[10px] ${introText.length >= 20 ? 'text-zinc-600' : 'text-red-400'}`}>
                Tối thiểu 20 ký tự
              </span>
              <span className="text-[10px] text-zinc-600">{introText.length}/2000</span>
            </div>
          </div>

          {/* Message */}
          {message && (
            <div className={`p-4 rounded-xl text-sm ${
              messageType === 'success' 
                ? 'bg-emerald-500/10 border border-emerald-500/20 text-emerald-400' 
                : 'bg-red-500/10 border border-red-500/20 text-red-400'
            }`}>
              {message}
            </div>
          )}

          {/* Submit Button */}
          <button
            id="reader-submit-btn"
            type="submit"
            disabled={submitting || introText.length < 20}
            className="w-full group relative overflow-hidden bg-gradient-to-r from-purple-600 to-purple-500 hover:from-purple-500 hover:to-purple-400 disabled:from-zinc-800 disabled:to-zinc-700 rounded-2xl p-5 text-center font-black uppercase tracking-widest text-sm text-white transition-all duration-300 shadow-xl disabled:cursor-not-allowed"
          >
            {submitting ? (
              <span className="flex items-center justify-center gap-3">
                <Loader2 className="w-4 h-4 animate-spin" />
                Đang gửi đơn...
              </span>
            ) : (
              <span className="flex items-center justify-center gap-3">
                <Send className="w-4 h-4" />
                Gửi Đơn Đăng Ký
              </span>
            )}
          </button>
        </form>
      </div>
    </div>
  );
}
