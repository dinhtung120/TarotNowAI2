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
  <div className={cn("tn-footer-social-wrap")}>
   <div className={cn("tn-footer-social-row")}>
   {items.map(({ icon: Icon, label, href }) => (
     <a key={label} href={href} target="_blank" rel="noopener noreferrer" aria-label={label} title={label} className={cn("tn-footer-social-button")}>
      <Icon className={cn("tn-footer-social-icon")} />
     </a>
    ))}
   </div>
   <p className={cn("tn-footer-copyright")}>{copyright}</p>
  </div>
 );
}
