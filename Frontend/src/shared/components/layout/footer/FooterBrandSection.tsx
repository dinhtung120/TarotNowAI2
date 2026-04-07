import ThemeSwitcher from "@/shared/components/common/ThemeSwitcher";
import LanguageSwitcher from "@/shared/components/common/LanguageSwitcher";

interface FooterBrandSectionProps {
 tagline: string;
}

export function FooterBrandSection({ tagline }: FooterBrandSectionProps) {
 return (
  <div className="relative w-full max-w-6xl">
   <div className="relative min-h-14 hidden sm:block">
    <div className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 flex flex-col items-center gap-1 text-center">
     <span className="text-2xl font-black italic tracking-tighter lunar-metallic-text">TarotNow AI</span>
     <p className="text-[9px] text-[var(--text-secondary)] font-black uppercase tracking-[0.4em]">{tagline}</p>
    </div>
    <div className="absolute right-0 top-1/2 -translate-y-1/2 flex items-center gap-2">
     <ThemeSwitcher />
     <LanguageSwitcher />
    </div>
   </div>
   <div className="sm:hidden flex flex-col items-center gap-3 text-center">
    <span className="text-2xl font-black italic tracking-tighter lunar-metallic-text">TarotNow AI</span>
    <p className="text-[9px] text-[var(--text-secondary)] font-black uppercase tracking-[0.4em]">{tagline}</p>
    <div className="flex items-center gap-2">
     <ThemeSwitcher />
     <LanguageSwitcher />
    </div>
   </div>
  </div>
 );
}
