export const DEFAULT_THEME = 'prismatic-royal' as const;

export const THEME_STORAGE_KEY = 'tn:selected-theme';
export const THEME_COOKIE_KEY = 'tn_theme';
export const THEME_STYLESHEET_ID = 'tn-theme-stylesheet';
export const THEME_COOKIE_MAX_AGE_SECONDS = 60 * 60 * 24 * 365;

export const THEME_OPTIONS = [
 { id: 'prismatic-royal', label: 'Prismatic Royal' },
 { id: 'prismatic-aurora', label: 'Prismatic Aurora' },
 { id: 'prismatic-opaline', label: 'Prismatic Opaline' },
 { id: 'astral-premium', label: 'Astral Premium' },
 { id: 'paper-grimoire', label: 'Paper Grimoire' },
 { id: 'obsidian-gold', label: 'Obsidian Gold' },
 { id: 'moonstone-silver', label: 'Moonstone Silver' },
 { id: 'starlit-abyss', label: 'Starlit Abyss' },
 { id: 'mystic-dark', label: 'Mystic Dark' },
 { id: 'lunar-bloom', label: 'Lunar Bloom' },
 { id: 'neon-oracle', label: 'Neon Oracle' },
 { id: 'zen-garden', label: 'Zen Garden' },
 { id: 'arctic-frost', label: 'Arctic Frost' },
 { id: 'jade-lotus', label: 'Jade Lotus' },
 { id: 'matrix-arcane', label: 'Matrix Arcane' },
 { id: 'candy-holo', label: 'Candy Holo' },
] as const;

export type ThemeId = (typeof THEME_OPTIONS)[number]['id'];

const THEME_ID_SET: ReadonlySet<string> = new Set(THEME_OPTIONS.map((theme) => theme.id));

export function isValidTheme(theme: string | null | undefined): theme is ThemeId {
 return typeof theme === 'string' && THEME_ID_SET.has(theme);
}

export function resolveTheme(theme: string | null | undefined, fallback: ThemeId = DEFAULT_THEME): ThemeId {
 return isValidTheme(theme) ? theme : fallback;
}

export function getThemeStylesheetHref(themeId: ThemeId): string {
 return `/themes/${themeId}.css`;
}
