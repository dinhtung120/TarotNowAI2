import { Check } from "lucide-react";
import { THEME_OPTIONS, type ThemeId } from "@/shared/models/theme";
import { cn } from "@/lib/utils";

interface ThemeSwitcherDropdownProps {
 selectedTheme: ThemeId;
 activeLabel: string;
 onApplyTheme: (themeId: string) => void;
}

export function ThemeSwitcherDropdown({ selectedTheme, activeLabel, onApplyTheme }: ThemeSwitcherDropdownProps) {
 return (
  <div className={cn("tn-theme-switcher-dropdown", "tn-panel")}>
   <div className={cn("px-2", "pb-1", "text-xs", "font-black", "uppercase", "tn-tracking-018", "tn-text-secondary")}>{activeLabel}</div>
   <div className={cn("max-h-40", "space-y-1", "overflow-y-auto", "pr-1")} role="listbox" aria-label="Theme options">
    {THEME_OPTIONS.map((option) => {
     const isActive = option.id === selectedTheme;
     return (
      <button key={option.id} type="button" onClick={() => onApplyTheme(option.id)} className={cn("h-11", "w-full", "rounded-xl", "border", "px-3", "text-left", "text-xs", "font-semibold", "transition-all", isActive ? "border-violet-400/40 bg-violet-100/70 text-slate-900" : "tn-surface tn-border-soft tn-text-secondary")}>
       <span className={cn("inline-flex", "w-full", "items-center", "justify-between", "gap-3")}>
        <span>{option.label}</span>
        {isActive ? <Check className={cn("h-4", "w-4", "text-violet-500")} /> : null}
       </span>
      </button>
     );
    })}
   </div>
  </div>
 );
}
