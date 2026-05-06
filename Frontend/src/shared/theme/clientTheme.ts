'use client';

import {
 DEFAULT_THEME,
 getThemeStylesheetHref,
 isValidTheme,
 THEME_COOKIE_KEY,
 THEME_COOKIE_MAX_AGE_SECONDS,
 THEME_STORAGE_KEY,
 THEME_STYLESHEET_ID,
 type ThemeId,
} from '@/shared/models/theme';

function readStoredTheme(): ThemeId | null {
 try {
  const rawTheme = window.localStorage.getItem(THEME_STORAGE_KEY);
  return isValidTheme(rawTheme) ? rawTheme : null;
 } catch {
  return null;
 }
}

function persistTheme(themeId: ThemeId): void {
 try {
  window.localStorage.setItem(THEME_STORAGE_KEY, themeId);
 } catch {
 }

 const isSecureContext = window.location.protocol === 'https:';
 document.cookie = `${THEME_COOKIE_KEY}=${themeId}; Path=/; Max-Age=${THEME_COOKIE_MAX_AGE_SECONDS}; SameSite=Lax${isSecureContext ? '; Secure' : ''}`;
}

function ensureThemeStylesheetLink(): HTMLLinkElement {
 const existing = document.getElementById(THEME_STYLESHEET_ID);
 if (existing instanceof HTMLLinkElement) {
  return existing;
 }

 const link = document.createElement('link');
 link.id = THEME_STYLESHEET_ID;
 link.rel = 'stylesheet';
 document.head.appendChild(link);
 return link;
}

function syncThemeStylesheet(themeId: ThemeId): void {
 const link = ensureThemeStylesheetLink();
 const nextHref = getThemeStylesheetHref(themeId);
 if (link.getAttribute('href') !== nextHref) {
  link.setAttribute('href', nextHref);
 }
}

export function applyTheme(themeId: ThemeId): ThemeId {
 document.documentElement.setAttribute('data-theme', themeId);
 syncThemeStylesheet(themeId);
 persistTheme(themeId);
 return themeId;
}

export function resolveClientTheme(fallback: ThemeId = DEFAULT_THEME): ThemeId {
 const storedTheme = readStoredTheme();
 if (storedTheme) {
  return storedTheme;
 }

 const domTheme = document.documentElement.getAttribute('data-theme');
 if (isValidTheme(domTheme)) {
  return domTheme;
 }

 return fallback;
}
