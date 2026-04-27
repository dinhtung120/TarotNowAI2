const SCROLL_LOCK_CLASS = 'tn-scroll-lock';

let activeScrollLockCount = 0;

function applyScrollLockClass(): void {
 document.body.classList.add(SCROLL_LOCK_CLASS);
 document.documentElement.classList.add(SCROLL_LOCK_CLASS);
}

function removeScrollLockClass(): void {
 document.body.classList.remove(SCROLL_LOCK_CLASS);
 document.documentElement.classList.remove(SCROLL_LOCK_CLASS);
}

export function acquirePageScrollLock(): () => void {
 if (typeof document === 'undefined') {
  return () => undefined;
 }

 activeScrollLockCount += 1;
 if (activeScrollLockCount === 1) {
  applyScrollLockClass();
 }

 let released = false;
 return () => {
  if (released) {
   return;
  }

  released = true;
  activeScrollLockCount = Math.max(0, activeScrollLockCount - 1);
  if (activeScrollLockCount === 0) {
   removeScrollLockClass();
  }
 };
}
