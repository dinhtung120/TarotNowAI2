'use client';

import { MessageSquareText } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

export default function ChatInboxPage() {
  return (
    <div className={cn("h-full flex flex-col items-center justify-center p-6 text-center animate-in fade-in duration-500")}>
      <GlassCard className={cn("max-w-md !p-12 flex flex-col items-center gap-6 border-white/5 bg-white/5 shadow-2xl")}>
        <div className={cn("w-20 h-20 rounded-full tn-bg-accent-20 flex items-center justify-center border border-white/10")}>
          <MessageSquareText className={cn("w-10 h-10 tn-text-accent opacity-80")} />
        </div>

        <div className={cn("space-y-2")}>
          <h2 className={cn("text-xl font-bold text-white tracking-tight")}>Chào mừng đến với TarotNow Chat</h2>
          <p className={cn("tn-text-secondary text-sm leading-relaxed")}>
            Chọn một cuộc trò chuyện từ danh sách bên trái để bắt đầu thảo luận với Reader của bạn.
            Nếu bạn chưa có cuộc trò chuyện nào, hãy truy cập thư mục Reader để bắt đầu một phiên tư vấn mới.
          </p>
        </div>

        <div className={cn("pt-4 flex items-center gap-4 tn-text-10 tn-text-secondary uppercase tn-tracking-02")}>
          <span className={cn("w-8 h-px bg-white/10")}></span>
          <span>Bảo mật • Riêng tư</span>
          <span className={cn("w-8 h-px bg-white/10")}></span>
        </div>
      </GlassCard>
    </div>
  );
}
