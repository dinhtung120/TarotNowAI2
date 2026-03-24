/*
 * ===================================================================
 * COMPONENT: ThemeSwitcher
 * BỐI CẢNH (CONTEXT):
 *   Nút chuyển đổi Giao diện/Chủ đề (Theme) của người dùng.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Quản lý danh sách 16 Themes hệ thống cao cấp (Prismatic Royal, Astral...).
 *   - Lấy Theme hiện tại từ `localStorage` và `data-theme` của Document.
 *   - Cập nhật trực tiếp class `data-theme` trên HTML Element khi đổi Theme.
 *   - Xử lý đóng Dropdown khi click ra ngoài hoặc bấm nút Escape.
 * ===================================================================
 */
'use client';

import { Check, ChevronDown, Palette } from 'lucide-react';
import { useEffect, useMemo, useRef, useState } from 'react';
import { DEFAULT_THEME, isValidTheme, THEME_OPTIONS, type ThemeId } from '@/shared/domain/theme';
import { applyTheme, resolveClientTheme } from '@/shared/infrastructure/theme/clientTheme';

export default function ThemeSwitcher() {
  const containerRef = useRef<HTMLDivElement>(null);
  const [isOpen, setIsOpen] = useState(false);
  const [selectedTheme, setSelectedTheme] = useState<ThemeId>(() => {
    if (typeof document === 'undefined' || typeof window === 'undefined') {
      return DEFAULT_THEME;
    }

    return resolveClientTheme(DEFAULT_THEME);
  });

  useEffect(() => {
    applyTheme(selectedTheme);
  }, [selectedTheme]);

  useEffect(() => {
    if (!isOpen) return;

    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') {
        setIsOpen(false);
      }
    };

    document.addEventListener('keydown', onKeyDown);
    return () => document.removeEventListener('keydown', onKeyDown);
  }, [isOpen]);

  useEffect(() => {
    if (!isOpen) return;

    const onMouseDown = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener('mousedown', onMouseDown);
    return () => document.removeEventListener('mousedown', onMouseDown);
  }, [isOpen]);

  const activeLabel = useMemo(
    () => THEME_OPTIONS.find((option) => option.id === selectedTheme)?.label ?? 'Theme',
    [selectedTheme],
  );

  const handleApplyTheme = (themeId: string) => {
    if (!isValidTheme(themeId)) return;

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
        <ChevronDown className={`h-3.5 w-3.5 transition-transform ${isOpen ? 'rotate-180' : ''}`} />
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
                  onClick={() => handleApplyTheme(option.id)}
                  className={`h-11 w-full rounded-xl px-3 text-left text-xs font-semibold transition-all ${
                    isActive
                      ? 'bg-[var(--purple-100)] border border-[var(--border-focus)] tn-text-ink'
                      : 'tn-surface border tn-border-soft tn-text-secondary hover:tn-surface-strong hover:tn-text-primary'
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
