import { Facebook, Instagram, Music2 } from "lucide-react";
import { getTranslations } from "next-intl/server";
import { FooterBrandSection } from "./footer/FooterBrandSection";
import { FooterLinksRow } from "./footer/FooterLinksRow";
import { FooterSocialRow } from "./footer/FooterSocialRow";

export default async function Footer() {
 const t = await getTranslations("Footer");
 const navLinks = [
  { href: "/reading", label: t("links.reading") },
  { href: "/readers", label: t("links.readers") },
  { href: "/wallet", label: t("links.wallet") },
  { href: "/chat", label: t("links.chat") },
 ];
 const legalLinks = [
  { href: "/legal/tos", label: t("legal.tos") },
  { href: "/legal/privacy", label: t("legal.privacy") },
  { href: "/legal/ai-disclaimer", label: t("legal.ai") },
 ];
 const socialItems = [
  { icon: Facebook, label: t("social.facebook"), href: "https://www.facebook.com/" },
  { icon: Instagram, label: t("social.instagram"), href: "https://www.instagram.com/" },
  { icon: Music2, label: t("social.tiktok"), href: "https://www.tiktok.com/" },
 ];

 return (
  <footer className="relative z-10 py-14 border-t border-[var(--border-subtle)] bg-[linear-gradient(180deg,var(--c-248-241-233-65)_0%,var(--c-237-231-227-88)_100%)]">
   <div className="max-w-7xl mx-auto px-6 flex flex-col items-center gap-6">
    <FooterBrandSection tagline={t("tagline")} />
    <FooterLinksRow items={navLinks} className="flex flex-wrap justify-center gap-6 text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-[0.2em]" linkClassName="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-ink)] transition-colors" />
    <FooterLinksRow items={legalLinks} className="flex flex-wrap justify-center gap-4 text-[9px] font-bold text-[var(--text-muted)] uppercase tracking-widest" linkClassName="inline-flex items-center min-h-11 px-1 hover:text-[var(--text-secondary)] transition-colors" />
    <FooterSocialRow items={socialItems} copyright={t("copyright", { year: "2026" })} />
   </div>
  </footer>
 );
}
