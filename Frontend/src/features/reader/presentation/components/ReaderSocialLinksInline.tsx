import { Facebook, Instagram, Music2 } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderSocialLinksInlineProps {
 facebookUrl?: string | null;
 instagramUrl?: string | null;
 tikTokUrl?: string | null;
 size?: 'sm' | 'md';
 className?: string;
}

export default function ReaderSocialLinksInline({
 facebookUrl,
 instagramUrl,
 tikTokUrl,
 size = 'sm',
 className,
}: ReaderSocialLinksInlineProps) {
 const links = [
  { key: 'facebook', href: facebookUrl, icon: Facebook },
  { key: 'instagram', href: instagramUrl, icon: Instagram },
  { key: 'tiktok', href: tikTokUrl, icon: Music2 },
 ].filter((item) => Boolean(item.href));

 if (links.length === 0) {
  return null;
 }

 const iconSizeClass = size === 'md' ? 'h-4 w-4' : 'h-3.5 w-3.5';
 const wrapperPaddingClass = size === 'md' ? 'p-2' : 'p-1.5';

 return (
  <div className={cn('flex items-center gap-2', className)}>
   {links.map((item) => (
    <a
     key={item.key}
     href={item.href!}
     target="_blank"
     rel="noreferrer noopener"
     className={cn('tn-border-soft tn-surface rounded-md border', wrapperPaddingClass, 'tn-hover-border-accent-50')}
     aria-label={item.key}
    >
     <item.icon className={cn(iconSizeClass, 'tn-text-secondary')} />
    </a>
   ))}
  </div>
 );
}
