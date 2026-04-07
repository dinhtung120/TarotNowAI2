import { ChevronDown, Palette } from "lucide-react";

interface ThemeSwitcherButtonProps {
 isOpen: boolean;
 onClick: () => void;
}

export function ThemeSwitcherButton({ isOpen, onClick }: ThemeSwitcherButtonProps) {
 return (
  <button type="button" onClick={onClick} className="tn-panel inline-flex items-center gap-2 rounded-xl px-3 py-2 text-xs font-black uppercase tracking-[0.18em] tn-text-primary hover:tn-surface-strong transition-all min-h-11" aria-haspopup="listbox" aria-expanded={isOpen}>
   <Palette className="h-3.5 w-3.5" />
   <span className="hidden sm:inline">Theme</span>
   <ChevronDown className={`h-3.5 w-3.5 transition-transform ${isOpen ? "rotate-180" : ""}`} />
  </button>
 );
}
