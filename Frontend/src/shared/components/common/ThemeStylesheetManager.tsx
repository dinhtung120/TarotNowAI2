'use client';

import { useEffect } from 'react';
import { DEFAULT_THEME, resolveTheme, type ThemeId } from '@/shared/domain/theme';
import { applyTheme, resolveClientTheme } from '@/shared/infrastructure/theme/clientTheme';

interface ThemeStylesheetManagerProps {
 initialTheme: ThemeId;
}

export default function ThemeStylesheetManager({ initialTheme }: ThemeStylesheetManagerProps) {
 useEffect(() => {
  const fallbackTheme = resolveTheme(initialTheme, DEFAULT_THEME);
  const resolvedTheme = resolveClientTheme(fallbackTheme);
  applyTheme(resolvedTheme);
 }, [initialTheme]);

 return null;
}
