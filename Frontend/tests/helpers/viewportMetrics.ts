export type PageMetrics = {
 title: string;
 viewportWidth: number;
 documentWidth: number;
 hasHorizontalOverflow: boolean;
 overflowingNodes: string[];
 offscreenInteractive: string[];
 smallTapTargets: string[];
};

export const metricsScript = (): PageMetrics => {
 const doc = document.documentElement;
 const body = document.body;
 const viewportWidth = window.innerWidth;
 const documentWidth = Math.max(doc.scrollWidth, body.scrollWidth);

 const isVisible = (el: HTMLElement) => {
  const style = window.getComputedStyle(el);
  const rect = el.getBoundingClientRect();
  return (
   rect.width > 0 &&
   rect.height > 0 &&
   style.visibility !== 'hidden' &&
   style.display !== 'none' &&
   style.opacity !== '0'
  );
 };

 const toSelector = (el: Element) => {
  const id = el.getAttribute('id');
  if (id) return `#${id}`;
  const cls = (el.getAttribute('class') || '').trim().split(/\s+/).filter(Boolean);
  if (cls.length > 0) return `${el.tagName.toLowerCase()}.${cls.slice(0, 2).join('.')}`;
  return el.tagName.toLowerCase();
 };

 const overflowingNodes = Array.from(document.querySelectorAll<HTMLElement>('body *'))
  .filter((el) => {
   if (!isVisible(el)) return false;
   const rect = el.getBoundingClientRect();
   return rect.right > viewportWidth + 2 && rect.left < viewportWidth;
  })
  .slice(0, 20)
  .map((el) => `${toSelector(el)} (${Math.round(el.getBoundingClientRect().width)}px)`);

 const interactiveNodes = Array.from(
  document.querySelectorAll<HTMLElement>(
   'a, button, input, select, textarea, [role="button"], [role="link"]'
  )
 ).filter(isVisible);

 const offscreenInteractive = interactiveNodes
  .filter((el) => {
   const rect = el.getBoundingClientRect();
   return rect.left < -1 || rect.right > viewportWidth + 1;
  })
  .slice(0, 20)
  .map((el) => toSelector(el));

 const smallTapTargets = interactiveNodes
  .filter((el) => {
   const rect = el.getBoundingClientRect();
   return rect.width < 44 || rect.height < 44;
  })
  .slice(0, 20)
  .map((el) => {
   const rect = el.getBoundingClientRect();
   return `${toSelector(el)} (${Math.round(rect.width)}x${Math.round(rect.height)})`;
  });

 return {
  title: document.title,
  viewportWidth,
  documentWidth,
  hasHorizontalOverflow: documentWidth > viewportWidth + 1,
  overflowingNodes,
  offscreenInteractive,
  smallTapTargets,
 };
};
