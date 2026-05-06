import { ChevronDown, Palette } from "lucide-react";
import { cn } from "@/lib/utils";

interface ThemeSwitcherButtonProps {
 isOpen: boolean;
 onClick: () => void;
}

export function ThemeSwitcherButton({ isOpen, onClick }: ThemeSwitcherButtonProps) {
 return (
  <button
   type="button"
   onClick={onClick}
   className={cn("tn-panel", "inline-flex", "min-h-11", "items-center", "gap-2", "rounded-xl", "px-3", "py-2", "text-xs", "font-black", "uppercase", "tn-tracking-018", "tn-text-primary", "transition-all")}
   aria-haspopup="listbox"
   aria-expanded={isOpen}
  >
   <Palette className={cn("h-4", "w-4")} />
   <span className={cn("inline")}>Theme</span>
   <ChevronDown className={cn("h-4", "w-4", "transition-transform", isOpen ? "rotate-180" : null)} />
  </button>
 );
}
