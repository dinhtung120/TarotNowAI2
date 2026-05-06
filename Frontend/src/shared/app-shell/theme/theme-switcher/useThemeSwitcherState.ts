"use client";

import { useEffect, useMemo, useRef, useState } from "react";
import { DEFAULT_THEME, isValidTheme, THEME_OPTIONS, type ThemeId } from "@/shared/models/theme";
import { applyTheme, resolveClientTheme } from "@/shared/theme/clientTheme";

export function useThemeSwitcherState() {
 const containerRef = useRef<HTMLDivElement>(null);
 const [isOpen, setIsOpen] = useState(false);
 const [selectedTheme, setSelectedTheme] = useState<ThemeId>(() => {
  if (typeof document === "undefined" || typeof window === "undefined") return DEFAULT_THEME;
  return resolveClientTheme(DEFAULT_THEME);
 });

 useEffect(() => {
  applyTheme(selectedTheme);
 }, [selectedTheme]);

 useEffect(() => {
  if (!isOpen) return;
  const onKeyDown = (event: KeyboardEvent) => event.key === "Escape" && setIsOpen(false);
  const onMouseDown = (event: MouseEvent) => containerRef.current && !containerRef.current.contains(event.target as Node) && setIsOpen(false);
  document.addEventListener("keydown", onKeyDown);
  document.addEventListener("mousedown", onMouseDown);
  return () => {
   document.removeEventListener("keydown", onKeyDown);
   document.removeEventListener("mousedown", onMouseDown);
  };
 }, [isOpen]);

 const activeLabel = useMemo(() => THEME_OPTIONS.find((option) => option.id === selectedTheme)?.label ?? "Theme", [selectedTheme]);
 const handleApplyTheme = (themeId: string) => {
  if (!isValidTheme(themeId)) return;
  setSelectedTheme(themeId);
  setIsOpen(false);
 };

 return {
  containerRef,
  isOpen,
  selectedTheme,
  activeLabel,
  setIsOpen,
  handleApplyTheme,
 };
}
