import ThemeSwitcher from "@/shared/components/common/ThemeSwitcher";
import LanguageSwitcher from "@/shared/components/common/LanguageSwitcher";
import { cn } from "@/lib/utils";

interface FooterBrandSectionProps {
 tagline: string;
}

export function FooterBrandSection({ tagline }: FooterBrandSectionProps) {
 return (
  <div className={cn("relative", "w-full", "max-w-6xl")}>
   <div className={cn("relative", "tn-show-sm", "min-h-14")}>
    <div className={cn("absolute", "left-1/2", "top-1/2", "flex", "-translate-x-1/2", "-translate-y-1/2", "flex-col", "items-center", "gap-1", "text-center")}>
     <span className={cn("lunar-metallic-text", "text-2xl", "font-black", "italic", "tracking-tighter")}>TarotNow AI</span>
     <p className={cn("tn-footer-tagline", "tn-text-secondary")}>{tagline}</p>
    </div>
    <div className={cn("absolute", "right-0", "top-1/2", "flex", "-translate-y-1/2", "items-center", "gap-2")}>
     <ThemeSwitcher />
     <LanguageSwitcher />
    </div>
   </div>
   <div className={cn("tn-hide-sm", "flex-col", "items-center", "gap-3", "text-center")}>
    <span className={cn("lunar-metallic-text", "text-2xl", "font-black", "italic", "tracking-tighter")}>TarotNow AI</span>
    <p className={cn("tn-footer-tagline", "tn-text-secondary")}>{tagline}</p>
    <div className={cn("flex", "items-center", "gap-2")}>
     <ThemeSwitcher />
     <LanguageSwitcher />
    </div>
   </div>
  </div>
 );
}
