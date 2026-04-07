export function isRouteActive(pathname: string, href: string) {
 if (href === '/admin') return pathname === href;
 return pathname.startsWith(href);
}
