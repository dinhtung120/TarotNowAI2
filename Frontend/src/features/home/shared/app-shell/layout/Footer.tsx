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
  <footer className={cn("tn-footer-shell")}>
   <div className={cn("tn-footer-container")}>
    <FooterBrandSection tagline={t("tagline")} />
    <FooterLinksRow items={navLinks} className={cn("tn-footer-nav-row")} linkClassName={cn("tn-footer-nav-link")} />
    <FooterLinksRow items={legalLinks} className={cn("tn-footer-legal-row")} linkClassName={cn("tn-footer-legal-link")} />
    <FooterSocialRow items={socialItems} copyright={t("copyright", { year: "2026" })} />
   </div>
  </footer>
 );
}
