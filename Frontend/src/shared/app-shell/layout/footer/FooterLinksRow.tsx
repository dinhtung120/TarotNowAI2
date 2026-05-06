import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import { cn } from "@/lib/utils";

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
  <div className={cn(className)}>
   {items.map((item) => (
    <Link key={item.href} href={item.href} className={cn(linkClassName)}>
     {item.label}
    </Link>
   ))}
  </div>
 );
}
