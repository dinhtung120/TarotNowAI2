import { Check } from "lucide-react";
import { THEME_OPTIONS, type ThemeId } from "@/shared/domain/theme";

interface ThemeSwitcherDropdownProps {
 selectedTheme: ThemeId;
 activeLabel: string;
 onApplyTheme: (themeId: string) => void;
}

export function ThemeSwitcherDropdown({ selectedTheme, activeLabel, onApplyTheme }: ThemeSwitcherDropdownProps) {
 return (
  <div className="absolute left-1/2 sm:left-auto right-auto sm:right-0 -translate-x-1/2 sm:translate-x-0 bottom-[calc(100%+8px)] sm:bottom-auto sm:top-[calc(100%+8px)] w-[calc(100vw-32px)] max-w-[288px] sm:max-w-none sm:w-[18rem] tn-panel rounded-2xl p-2 shadow-[var(--shadow-elevated)] z-[100]">
   <div className="px-2 pb-1 text-[10px] font-black uppercase tracking-[0.18em] tn-text-secondary">{activeLabel}</div>
   <div className="space-y-1 max-h-[10.75rem] overflow-y-auto pr-1" role="listbox" aria-label="Theme options">
    {THEME_OPTIONS.map((option) => {
     const isActive = option.id === selectedTheme;
     return (
      <button key={option.id} type="button" onClick={() => onApplyTheme(option.id)} className={`h-11 w-full rounded-xl px-3 text-left text-xs font-semibold transition-all ${isActive ? "bg-[var(--purple-100)] border border-[var(--border-focus)] tn-text-ink" : "tn-surface border tn-border-soft tn-text-secondary hover:tn-surface-strong hover:tn-text-primary"}`}>
       <span className="inline-flex w-full items-center justify-between gap-3">
        <span>{option.label}</span>
        {isActive ? <Check className="h-3.5 w-3.5 text-[var(--purple-accent)]" /> : null}
       </span>
      </button>
     );
    })}
   </div>
  </div>
 );
}
