import { Facebook, Instagram, Music2 } from "lucide-react";
import { getTranslations } from "next-intl/server";
import { cn } from "@/lib/utils";
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
  <footer className={cn("relative", "z-10", "border-t", "border-slate-700/40", "bg-gradient-to-b", "from-stone-200/80", "to-stone-100/90", "py-14")}>
   <div className={cn("mx-auto", "flex", "max-w-7xl", "flex-col", "items-center", "gap-6", "px-6")}>
    <FooterBrandSection tagline={t("tagline")} />
    <FooterLinksRow items={navLinks} className={cn("flex", "flex-wrap", "justify-center", "gap-6", "text-xs", "font-black", "uppercase", "tn-tracking-02", "text-slate-600")} linkClassName={cn("inline-flex", "min-h-11", "items-center", "px-1", "transition-colors")} />
    <FooterLinksRow items={legalLinks} className={cn("flex", "flex-wrap", "justify-center", "gap-4", "text-xs", "font-bold", "uppercase", "tracking-widest", "text-slate-500")} linkClassName={cn("inline-flex", "min-h-11", "items-center", "px-1", "transition-colors")} />
    <FooterSocialRow items={socialItems} copyright={t("copyright", { year: "2026" })} />
   </div>
  </footer>
 );
}
