"use client";

import { ThemeSwitcherButton } from "@/shared/app-shell/theme/theme-switcher/ThemeSwitcherButton";
import { ThemeSwitcherDropdown } from "@/shared/app-shell/theme/theme-switcher/ThemeSwitcherDropdown";
import { useThemeSwitcherState } from "@/shared/app-shell/theme/theme-switcher/useThemeSwitcherState";
import { cn } from "@/lib/utils";

export default function ThemeSwitcher() {
 const {
  containerRef,
  isOpen,
  selectedTheme,
  activeLabel,
  setIsOpen,
  handleApplyTheme,
 } = useThemeSwitcherState();

 return (
  <div ref={containerRef} className={cn("relative")}>
   <ThemeSwitcherButton isOpen={isOpen} onClick={() => setIsOpen((prev) => !prev)} />
   {isOpen ? (
    <ThemeSwitcherDropdown
     selectedTheme={selectedTheme}
     activeLabel={activeLabel}
     onApplyTheme={handleApplyTheme}
    />
   ) : null}
  </div>
 );
}
