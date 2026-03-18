"use client";

import { Check, ChevronDown, Palette } from "lucide-react";
import { useEffect, useMemo, useRef, useState } from "react";

const STORAGE_KEY = "tn:selected-theme";

const THEME_OPTIONS = [
  { id: "prismatic-royal", label: "Prismatic Royal" },
  { id: "prismatic-aurora", label: "Prismatic Aurora" },
  { id: "prismatic-opaline", label: "Prismatic Opaline" },
  { id: "astral-premium", label: "Astral Premium" },
  { id: "paper-grimoire", label: "Paper Grimoire" },
  { id: "obsidian-gold", label: "Obsidian Gold" },
  { id: "moonstone-silver", label: "Moonstone Silver" },
  { id: "starlit-abyss", label: "Starlit Abyss" },
  { id: "mystic-dark", label: "Mystic Dark" },
  { id: "lunar-bloom", label: "Lunar Bloom" },
  { id: "neon-oracle", label: "Neon Oracle" },
  { id: "zen-garden", label: "Zen Garden" },
  { id: "arctic-frost", label: "Arctic Frost" },
  { id: "jade-lotus", label: "Jade Lotus" },
  { id: "matrix-arcane", label: "Matrix Arcane" },
  { id: "candy-holo", label: "Candy Holo" },
] as const;

const THEME_ID_SET: ReadonlySet<string> = new Set(THEME_OPTIONS.map((option) => option.id));

function isValidTheme(theme: string | null): theme is (typeof THEME_OPTIONS)[number]["id"] {
  return Boolean(theme && THEME_ID_SET.has(theme));
}

export default function ThemeSwitcher() {
  const containerRef = useRef<HTMLDivElement>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [selectedTheme, setSelectedTheme] = useState<string>(() => {
    if (typeof document === "undefined" || typeof window === "undefined") {
      return "prismatic-royal";
    }

    const domTheme = document.documentElement.getAttribute("data-theme");
    const storedTheme = window.localStorage.getItem(STORAGE_KEY);

    if (isValidTheme(storedTheme)) return storedTheme;
    if (isValidTheme(domTheme)) return domTheme;
    return "prismatic-royal";
  });

  useEffect(() => {
    const root = document.documentElement;

    if (root.getAttribute("data-theme") !== selectedTheme) {
      root.setAttribute("data-theme", selectedTheme);
    }

    window.localStorage.setItem(STORAGE_KEY, selectedTheme);
  }, [selectedTheme]);

  useEffect(() => {
    if (!isOpen) return;

    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        setIsOpen(false);
      }
    };

    document.addEventListener("keydown", onKeyDown);
    return () => document.removeEventListener("keydown", onKeyDown);
  }, [isOpen]);

  useEffect(() => {
    if (!isOpen) return;

    const onMouseDown = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener("mousedown", onMouseDown);
    return () => document.removeEventListener("mousedown", onMouseDown);
  }, [isOpen]);

  const activeLabel = useMemo(
    () => THEME_OPTIONS.find((option) => option.id === selectedTheme)?.label ?? "Theme",
    [selectedTheme],
  );

  const applyTheme = (themeId: string) => {
    if (!isValidTheme(themeId)) return;

    document.documentElement.setAttribute("data-theme", themeId);
    window.localStorage.setItem(STORAGE_KEY, themeId);
    setSelectedTheme(themeId);
    setIsOpen(false);
  };

  return (
    <div ref={containerRef} className="relative">
      <button
        type="button"
        onClick={() => setIsOpen((prev) => !prev)}
        className="tn-panel inline-flex items-center gap-2 rounded-xl px-3 py-2 text-xs font-black uppercase tracking-[0.18em] tn-text-primary hover:tn-surface-strong transition-all min-h-11"
        aria-haspopup="listbox"
        aria-expanded={isOpen}
      >
        <Palette className="h-3.5 w-3.5" />
        <span className="hidden sm:inline">Theme</span>
        <ChevronDown className={`h-3.5 w-3.5 transition-transform ${isOpen ? "rotate-180" : ""}`} />
      </button>

      {isOpen && (
        <div className="absolute right-0 mt-2 w-[18rem] tn-panel rounded-2xl p-2 shadow-[var(--shadow-elevated)]">
          <div className="px-2 pb-1 text-[10px] font-black uppercase tracking-[0.18em] tn-text-secondary">
            {activeLabel}
          </div>

          <div className="space-y-1 max-h-[10.75rem] overflow-y-auto pr-1" role="listbox" aria-label="Theme options">
            {THEME_OPTIONS.map((option) => {
              const isActive = option.id === selectedTheme;

              return (
                <button
                  key={option.id}
                  type="button"
                  onClick={() => applyTheme(option.id)}
                  className={`h-11 w-full rounded-xl px-3 text-left text-xs font-semibold transition-all ${
                    isActive
                      ? "bg-[var(--purple-100)] border border-[var(--border-focus)] tn-text-ink"
                      : "tn-surface border tn-border-soft tn-text-secondary hover:tn-surface-strong hover:tn-text-primary"
                  }`}
                >
                  <span className="inline-flex w-full items-center justify-between gap-3">
                    <span>{option.label}</span>
                    {isActive ? <Check className="h-3.5 w-3.5 text-[var(--purple-accent)]" /> : null}
                  </span>
                </button>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
}
