'use client';

import { useEffect } from 'react';
import { DEFAULT_THEME, resolveTheme, type ThemeId } from '@/shared/domain/theme';
import { applyTheme, resolveClientTheme } from '@/shared/infrastructure/theme/clientTheme';

interface UseThemeStylesheetManagerArgs {
  initialTheme: ThemeId;
}

export function useThemeStylesheetManager({ initialTheme }: UseThemeStylesheetManagerArgs) {
  useEffect(() => {
    const fallbackTheme = resolveTheme(initialTheme, DEFAULT_THEME);
    const resolvedTheme = resolveClientTheme(fallbackTheme);
    applyTheme(resolvedTheme);
  }, [initialTheme]);
}
