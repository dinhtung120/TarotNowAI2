import type { LucideIcon } from "lucide-react";

interface FooterSocialItem {
 icon: LucideIcon;
 label: string;
 href: string;
}

interface FooterSocialRowProps {
 items: FooterSocialItem[];
 copyright: string;
}

export function FooterSocialRow({ items, copyright }: FooterSocialRowProps) {
 return (
  <div className="flex flex-col items-center gap-3 pt-2">
   <div className="flex gap-3">
    {items.map(({ icon: Icon, label, href }) => (
     <a key={label} href={href} target="_blank" rel="noopener noreferrer" aria-label={label} title={label} className="w-11 h-11 rounded-full border border-[var(--border-default)] bg-[var(--bg-glass)] hover:border-[var(--border-hover)] hover:shadow-[var(--glow-purple-sm)] transition-all flex items-center justify-center cursor-pointer text-[var(--text-secondary)] hover:text-[var(--text-ink)]">
      <Icon className="w-3.5 h-3.5" />
     </a>
    ))}
   </div>
   <p className="text-[8px] text-[var(--text-muted)] font-black tracking-[0.2em] uppercase leading-tight text-center">{copyright}</p>
  </div>
 );
}
