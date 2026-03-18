"use client";

import { usePathname, useRouter } from "@/i18n/routing";
import { Check, ChevronDown } from "lucide-react";
import { useLocale } from "next-intl";
import { useEffect, useMemo, useRef, useState } from "react";

const LOCALE_OPTIONS = [
  { id: "vi", label: "Tiếng Việt", flag: "🇻🇳" },
  { id: "en", label: "English", flag: "🇺🇸" },
  { id: "zh", label: "中文", flag: "🇨🇳" },
] as const;

const LOCALE_ID_SET: ReadonlySet<string> = new Set(LOCALE_OPTIONS.map((option) => option.id));

function isValidLocale(locale: string | null): locale is (typeof LOCALE_OPTIONS)[number]["id"] {
  return Boolean(locale && LOCALE_ID_SET.has(locale));
}

export default function LanguageSwitcher() {
  const router = useRouter();
  const pathname = usePathname();
  const locale = useLocale();
  const containerRef = useRef<HTMLDivElement>(null);
  const [isOpen, setIsOpen] = useState(false);

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

  const activeLocale = useMemo(() => {
    if (!isValidLocale(locale)) return LOCALE_OPTIONS[0];
    return LOCALE_OPTIONS.find((option) => option.id === locale) ?? LOCALE_OPTIONS[0];
  }, [locale]);

  const applyLocale = (localeId: string) => {
    if (!isValidLocale(localeId)) return;

    if (localeId !== locale) {
      router.replace(pathname, { locale: localeId });
    }

    setIsOpen(false);
  };

  return (
    <div ref={containerRef} className="relative">
      <button
        type="button"
        onClick={() => setIsOpen((prev) => !prev)}
        className="tn-panel inline-flex items-center gap-2 rounded-xl px-3 py-2 text-xs font-black uppercase tracking-[0.18em] tn-text-primary hover:tn-surface-strong transition-all min-h-11"
        aria-label={`Current language: ${activeLocale.label}`}
        aria-haspopup="listbox"
        aria-expanded={isOpen}
      >
        <span className="text-base leading-none" aria-hidden="true">{activeLocale.flag}</span>
        <ChevronDown className={`h-3.5 w-3.5 transition-transform ${isOpen ? "rotate-180" : ""}`} />
      </button>

      {isOpen && (
        <div className="absolute right-0 mt-2 w-[12rem] tn-panel rounded-2xl p-2 shadow-[var(--shadow-elevated)]">
          <div className="px-2 pb-1 text-[10px] font-black uppercase tracking-[0.18em] tn-text-secondary">Language</div>

          <div className="space-y-1" role="listbox" aria-label="Language options">
            {LOCALE_OPTIONS.map((option) => {
              const isActive = option.id === locale;

              return (
                <button
                  key={option.id}
                  type="button"
                  onClick={() => applyLocale(option.id)}
                  className={`w-full rounded-xl px-3 py-2.5 text-left text-xs font-semibold transition-all min-h-11 ${
                    isActive
                      ? "bg-[var(--purple-100)] border border-[var(--border-focus)] tn-text-ink"
                      : "tn-surface border tn-border-soft tn-text-secondary hover:tn-surface-strong hover:tn-text-primary"
                  }`}
                >
                  <span className="inline-flex w-full items-center justify-between gap-3">
                    <span className="inline-flex items-center gap-2">
                      <span className="text-base leading-none" aria-hidden="true">{option.flag}</span>
                      <span>{option.label}</span>
                    </span>
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
