import type { RefObject } from "react";
import { useCallback, useEffect, useRef, useState } from "react";

interface UseNavbarMenuStateResult {
 avatarMenuOpen: boolean;
 mobileMenuOpen: boolean;
 avatarMenuRef: RefObject<HTMLDivElement | null>;
 closeAvatarMenu: () => void;
 closeMenus: () => void;
 toggleAvatarMenu: () => void;
 toggleMobileMenu: () => void;
}

export function useNavbarMenuState(pathname: string): UseNavbarMenuStateResult {
 const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
 const [avatarMenuOpen, setAvatarMenuOpen] = useState(false);
 const avatarMenuRef = useRef<HTMLDivElement>(null);

 useEffect(() => {
  const handleClickOutside = (event: MouseEvent) => {
   if (avatarMenuRef.current && !avatarMenuRef.current.contains(event.target as Node)) {
    setAvatarMenuOpen(false);
   }
  };
  document.addEventListener("mousedown", handleClickOutside);
  return () => document.removeEventListener("mousedown", handleClickOutside);
 }, []);

 useEffect(() => {
  const resetMenuTimer = window.setTimeout(() => {
   setMobileMenuOpen(false);
   setAvatarMenuOpen(false);
  }, 0);
  return () => window.clearTimeout(resetMenuTimer);
 }, [pathname]);

 const closeAvatarMenu = useCallback(() => {
  setAvatarMenuOpen(false);
 }, []);

 const closeMenus = useCallback(() => {
  setAvatarMenuOpen(false);
  setMobileMenuOpen(false);
 }, []);

 const toggleAvatarMenu = useCallback(() => {
  setAvatarMenuOpen((current) => !current);
 }, []);

 const toggleMobileMenu = useCallback(() => {
  setMobileMenuOpen((current) => !current);
 }, []);

 return {
  avatarMenuOpen,
  mobileMenuOpen,
  avatarMenuRef,
  closeAvatarMenu,
  closeMenus,
  toggleAvatarMenu,
  toggleMobileMenu,
 };
}
