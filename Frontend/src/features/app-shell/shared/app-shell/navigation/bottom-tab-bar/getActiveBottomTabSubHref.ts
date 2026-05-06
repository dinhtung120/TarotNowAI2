import type { BottomTabSubItem } from './config';

export function getActiveBottomTabSubHref(
 activeSubItems: BottomTabSubItem[],
 pathname: string,
 matchesPath: (candidatePath: string, prefix: string) => boolean,
) {
 return activeSubItems.reduce<string | null>((currentActiveHref, sub) => {
  const bestMatchLength = Math.max(
   ...sub.matchPrefixes.map((prefix) => matchesPath(pathname, prefix) ? prefix.length : -1),
  );
  if (bestMatchLength < 0) {
   return currentActiveHref;
  }

  if (!currentActiveHref) {
   return sub.href;
  }

  const current = activeSubItems.find((item) => item.href === currentActiveHref);
  if (!current) {
   return sub.href;
  }

  const currentBestMatchLength = Math.max(
   ...current.matchPrefixes.map((prefix) => matchesPath(pathname, prefix) ? prefix.length : -1),
  );
  if (bestMatchLength > currentBestMatchLength) {
   return sub.href;
  }

  return bestMatchLength === currentBestMatchLength && sub.href.length > current.href.length
   ? sub.href
   : currentActiveHref;
 }, null);
}
