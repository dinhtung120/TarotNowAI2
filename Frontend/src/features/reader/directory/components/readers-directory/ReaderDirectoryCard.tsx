import { Sparkles } from 'lucide-react';
import type { ReaderProfile } from '@/features/reader/shared';
import ReaderDirectoryCardProfile from '@/features/reader/directory/components/readers-directory/card/ReaderDirectoryCardProfile';
import ReaderDirectoryCardSocialLinks from '@/features/reader/directory/components/readers-directory/card/ReaderDirectoryCardSocialLinks';
import ReaderDirectoryCardSpecialties from '@/features/reader/directory/components/readers-directory/card/ReaderDirectoryCardSpecialties';
import ReaderDirectoryCardStats from '@/features/reader/directory/components/readers-directory/card/ReaderDirectoryCardStats';
import type { ReaderDirectoryCardLabels } from '@/features/reader/directory/components/readers-directory/types';
import { cn } from '@/lib/utils';

interface ReaderDirectoryCardProps {
 bio: string;
 labels: ReaderDirectoryCardLabels;
 reader: ReaderProfile;
 onSelect: (reader: ReaderProfile) => void;
}

export function ReaderDirectoryCard({
 bio,
 labels,
 reader,
 onSelect,
}: ReaderDirectoryCardProps) {
 return (
  <button
   className={cn(
    'group tn-border-soft tn-surface hover:tn-surface-strong relative flex h-full flex-col overflow-hidden rounded-[2rem] border p-6 text-left shadow-xl transition-all duration-500 hover:border-[var(--purple-accent)]/30 hover:shadow-[0_0_40px_var(--c-168-85-247-15)]',
   )}
   type="button"
   onClick={() => onSelect(reader)}
  >
   <div
    className={cn(
     'absolute top-0 right-0 p-4 opacity-[0.02] transition-all duration-700 group-hover:scale-110 group-hover:rotate-12 group-hover:opacity-[0.05]',
    )}
   >
    <Sparkles className={cn('tn-text-accent')} size={120} />
   </div>

   <div className={cn('relative z-10 flex flex-grow flex-col space-y-5')}>
    <ReaderDirectoryCardProfile labels={labels} reader={reader} />
    <p className={cn('line-clamp-2 flex-grow text-xs leading-relaxed font-medium text-[var(--text-secondary)]')}>
     {bio}
    </p>

    <div className={cn('tn-border-soft space-y-4 border-t pt-4')}>
     <ReaderDirectoryCardStats labels={labels} reader={reader} />
     <ReaderDirectoryCardSocialLinks label={labels.socialLinksLabel} reader={reader} />
     <ReaderDirectoryCardSpecialties specialties={reader.specialties} />
    </div>
   </div>
  </button>
 );
}
