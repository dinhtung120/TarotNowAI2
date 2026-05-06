'use client';

import type { ThemeId } from '@/shared/models/theme';
import { useThemeStylesheetManager } from '@/shared/app-shell/common/hooks/useThemeStylesheetManager';

interface ThemeStylesheetManagerProps {
 initialTheme: ThemeId;
}

export default function ThemeStylesheetManager({ initialTheme }: ThemeStylesheetManagerProps) {
 useThemeStylesheetManager({ initialTheme });

 return null;
}
