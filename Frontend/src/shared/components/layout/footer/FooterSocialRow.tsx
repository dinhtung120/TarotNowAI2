import type { LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";

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
  <div className={cn("flex", "flex-col", "items-center", "gap-3", "pt-2")}>
   <div className={cn("flex", "gap-3")}>
   {items.map(({ icon: Icon, label, href }) => (
     <a key={label} href={href} target="_blank" rel="noopener noreferrer" aria-label={label} title={label} className={cn("flex", "h-11", "w-11", "cursor-pointer", "items-center", "justify-center", "rounded-full", "border", "border-slate-700", "bg-slate-900/60", "text-slate-400", "transition-all")}>
      <Icon className={cn("h-4", "w-4")} />
     </a>
    ))}
   </div>
   <p className={cn("text-center", "tn-text-8", "font-black", "uppercase", "leading-tight", "tn-tracking-02", "text-slate-400")}>{copyright}</p>
  </div>
 );
}
