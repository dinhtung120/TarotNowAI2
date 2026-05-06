import { Facebook, Instagram, Music2 } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/shared';
import { cn } from '@/lib/utils';

interface ReaderDirectoryCardSocialLinksProps {
 label: string;
 reader: ReaderProfile;
}

export default function ReaderDirectoryCardSocialLinks({
 label,
 reader,
}: ReaderDirectoryCardSocialLinksProps) {
 const links = [
  { key: 'fb', href: reader.facebookUrl, icon: Facebook },
  { key: 'ig', href: reader.instagramUrl, icon: Instagram },
  { key: 'tk', href: reader.tikTokUrl, icon: Music2 },
 ].filter((item) => Boolean(item.href));

 if (links.length === 0) {
  return null;
 }

 return (
  <div className={cn('flex items-center gap-2')}>
   <span className={cn('tn-text-10 font-black uppercase tn-text-tertiary')}>
    {label}
   </span>
   <div className={cn('flex items-center gap-2')}>
    {links.map((item) => (
     <a
      key={item.key}
      href={item.href!}
      target="_blank"
      rel="noreferrer noopener"
      className={cn('tn-border-soft tn-surface rounded-md border p-1.5 tn-hover-border-accent-50')}
      aria-label={item.key}
     >
      <item.icon className={cn('h-3.5 w-3.5 tn-text-secondary')} />
     </a>
    ))}
   </div>
  </div>
 );
}
