import { useCallback, useEffect, useRef, useState } from 'react';

export function useMessageDropdownState() {
 const [isOpen, setIsOpen] = useState(false);
 const dropdownRef = useRef<HTMLDivElement>(null);
 const buttonRef = useRef<HTMLButtonElement>(null);

 useEffect(() => {
  const handleClickOutside = (event: MouseEvent) => {
   if (dropdownRef.current?.contains(event.target as Node)) return;
   if (buttonRef.current?.contains(event.target as Node)) return;
   setIsOpen(false);
  };

  document.addEventListener('mousedown', handleClickOutside);
  return () => document.removeEventListener('mousedown', handleClickOutside);
 }, []);

 const toggleOpen = useCallback(() => {
  setIsOpen((value) => !value);
 }, []);

 const close = useCallback(() => {
  setIsOpen(false);
 }, []);

 return {
  buttonRef,
  close,
  dropdownRef,
  isOpen,
  toggleOpen,
 };
}
