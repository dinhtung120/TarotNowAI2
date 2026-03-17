/**
 * Footer — Component chân trang dùng chung cho Public pages.
 *
 * === VẤN ĐỀ TRƯỚC ĐÂY ===
 * Footer chỉ có ở trang Home, viết inline ~30 dòng JSX trong page.tsx.
 * Các trang khác (Readers, Legal, Wallet) KHÔNG CÓ footer.
 *
 * === GIẢI PHÁP ===
 * Tách thành component độc lập, import vào layout.tsx.
 * Hiển thị ở TẤT CẢ public pages (Home, Readers, Legal).
 * Ẩn ở Auth pages và Admin pages (có layout riêng).
 *
 * Nội dung Footer:
 * 1. Logo + tagline
 * 2. Navigation links (Dịch vụ, Readers, Ví, Hỗ trợ)
 * 3. Legal links (ToS, Privacy)
 * 4. Social icons
 * 5. Copyright
 */

import { Link } from "@/i18n/routing";
import { Globe, Heart, MessageSquare } from "lucide-react";

export default function Footer() {
 return (
 <footer className="relative z-10 py-20 border-t border-[var(--border-subtle)] bg-[linear-gradient(180deg,var(--c-248-241-233-65)_0%,var(--c-237-231-227-88)_100%)]">
 <div className="max-w-7xl mx-auto px-6 flex flex-col items-center gap-10">
 {/* Logo + Tagline */}
 <div className="flex flex-col items-center gap-3">
 <span className="text-2xl font-black italic tracking-tighter lunar-metallic-text">
 TarotNow AI
 </span>
 <p className="text-[9px] text-[var(--text-secondary)] font-black uppercase tracking-[0.4em]">
 Lunar Bloom Guidance
 </p>
 </div>

 {/* Navigation Links */}
 <div className="flex flex-wrap justify-center gap-8 text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-[0.2em]">
 <Link href="/reading" className="hover:text-[var(--text-ink)] transition-colors">
 Dịch vụ
 </Link>
 <Link href="/readers" className="hover:text-[var(--text-ink)] transition-colors">
 Readers
 </Link>
 <Link href="/wallet" className="hover:text-[var(--text-ink)] transition-colors">
 Tín dụng
 </Link>
 <Link href="/chat" className="hover:text-[var(--text-ink)] transition-colors">
 Tin nhắn
 </Link>
 </div>

 {/* Legal Links */}
 <div className="flex flex-wrap justify-center gap-6 text-[9px] font-bold text-[var(--text-muted)] uppercase tracking-widest">
 <Link
 href="/legal/tos"
 className="hover:text-[var(--text-secondary)] transition-colors"
 >
 Điều khoản
 </Link>
 <Link
 href="/legal/privacy"
 className="hover:text-[var(--text-secondary)] transition-colors"
 >
 Bảo mật
 </Link>
 <Link
 href="/legal/ai-disclaimer"
 className="hover:text-[var(--text-secondary)] transition-colors"
 >
 AI Disclaimer
 </Link>
 </div>

 {/* Social Icons + Copyright */}
 <div className="flex flex-col items-center gap-4 pt-4">
 <div className="flex gap-3">
 {[Globe, Heart, MessageSquare].map((Icon, i) => (
 <div
 key={i}
 className="w-8 h-8 rounded-full border border-[var(--border-default)] bg-[var(--bg-glass)] hover:border-[var(--border-hover)] hover:shadow-[var(--glow-purple-sm)] transition-all flex items-center justify-center cursor-pointer text-[var(--text-secondary)] hover:text-[var(--text-ink)]"
 >
 <Icon className="w-3.5 h-3.5" />
 </div>
 ))}
 </div>
 <p className="text-[8px] text-[var(--text-muted)] font-black tracking-[0.2em] uppercase">
 © 2026 TarotNow AI • Premium Spiritual Experience • All Rights
 Reserved
 </p>
 </div>
 </div>
 </footer>
 );
}
