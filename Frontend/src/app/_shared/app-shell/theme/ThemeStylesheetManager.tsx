'use client';

import type { ThemeId } from '@/app/_shared/models/theme';
import { useThemeStylesheetManager } from '@/app/_shared/app-shell/common/hooks/useThemeStylesheetManager';

interface ThemeStylesheetManagerProps {
 initialTheme: ThemeId;
}

export default function ThemeStylesheetManager({ initialTheme }: ThemeStylesheetManagerProps) {
 useThemeStylesheetManager({ initialTheme });

 return null;
}
