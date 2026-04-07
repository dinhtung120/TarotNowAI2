import { Link } from "@/i18n/routing";

interface FooterLinkItem {
 href: string;
 label: string;
}

interface FooterLinksRowProps {
 items: FooterLinkItem[];
 className: string;
 linkClassName: string;
}

export function FooterLinksRow({ items, className, linkClassName }: FooterLinksRowProps) {
 return (
  <div className={className}>
   {items.map((item) => (
    <Link key={item.href} href={item.href} className={linkClassName}>
     {item.label}
    </Link>
   ))}
  </div>
 );
}
