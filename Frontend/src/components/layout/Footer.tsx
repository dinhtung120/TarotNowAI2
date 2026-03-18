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
import { getTranslations } from "next-intl/server";
import { Facebook, Instagram, Music2 } from "lucide-react";
import ThemeSwitcher from "@/components/common/ThemeSwitcher";
import LanguageSwitcher from "@/components/common/LanguageSwitcher";

export default async function Footer() {
 const t = await getTranslations("Footer");
 const socialItems = [
  { icon: Facebook, label: t("social.facebook"), href: "https://www.facebook.com/" },
  { icon: Instagram, label: t("social.instagram"), href: "https://www.instagram.com/" },
  { icon: Music2, label: t("social.tiktok"), href: "https://www.tiktok.com/" },
 ];

 return (
 <footer className="relative z-10 py-14 border-t border-[var(--border-subtle)] bg-[linear-gradient(180deg,var(--c-248-241-233-65)_0%,var(--c-237-231-227-88)_100%)]">
 <div className="max-w-7xl mx-auto px-6 flex flex-col items-center gap-6">
 {/* Brand + Switchers */}
 <div className="relative w-full max-w-6xl">
 <div className="relative min-h-14 hidden sm:block">
 <div className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 flex flex-col items-center gap-1 text-center">
 <span className="text-2xl font-black italic tracking-tighter lunar-metallic-text">TarotNow AI</span>
 <p className="text-[9px] text-[var(--text-secondary)] font-black uppercase tracking-[0.4em]">{t("tagline")}</p>
 </div>
 <div className="absolute right-0 top-1/2 -translate-y-1/2 flex items-center gap-2">
 <ThemeSwitcher />
 <LanguageSwitcher />
 </div>
 </div>
 <div className="sm:hidden flex flex-col items-center gap-3 text-center">
 <span className="text-2xl font-black italic tracking-tighter lunar-metallic-text">TarotNow AI</span>
 <p className="text-[9px] text-[var(--text-secondary)] font-black uppercase tracking-[0.4em]">{t("tagline")}</p>
 <div className="flex items-center gap-2">
 <ThemeSwitcher />
 <LanguageSwitcher />
 </div>
 </div>
 </div>

 {/* Navigation Links */}
<div className="flex flex-wrap justify-center gap-6 text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-[0.2em]">
 <Link href="/reading" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-ink)] transition-colors">{t("links.reading")}</Link>
 <Link href="/readers" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-ink)] transition-colors">{t("links.readers")}</Link>
 <Link href="/wallet" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-ink)] transition-colors">{t("links.wallet")}</Link>
 <Link href="/chat" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-ink)] transition-colors">{t("links.chat")}</Link>
</div>

 {/* Legal Links */}
<div className="flex flex-wrap justify-center gap-4 text-[9px] font-bold text-[var(--text-muted)] uppercase tracking-widest">
 <Link href="/legal/tos" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-secondary)] transition-colors">{t("legal.tos")}</Link>
 <Link href="/legal/privacy" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-secondary)] transition-colors">{t("legal.privacy")}</Link>
 <Link href="/legal/ai-disclaimer" className="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-secondary)] transition-colors">{t("legal.ai")}</Link>
</div>

 {/* Social Icons + Copyright */}
 <div className="flex flex-col items-center gap-3 pt-2">
 <div className="flex gap-3">
 {socialItems.map(({ icon: Icon, label, href }) => (
 <a
 key={label}
 href={href}
 target="_blank"
 rel="noopener noreferrer"
 aria-label={label}
 title={label}
 className="w-11 h-11 rounded-full border border-[var(--border-default)] bg-[var(--bg-glass)] hover:border-[var(--border-hover)] hover:shadow-[var(--glow-purple-sm)] transition-all flex items-center justify-center cursor-pointer text-[var(--text-secondary)] hover:text-[var(--text-ink)]"
 >
 <Icon className="w-3.5 h-3.5" />
 </a>
 ))}
 </div>
 <p className="text-[8px] text-[var(--text-muted)] font-black tracking-[0.2em] uppercase leading-tight text-center">
 {t("copyright", { year: "2026" })}
 </p>
 </div>
 </div>
 </footer>
 );
}
