import { cn } from '@/lib/utils';

function PulseBlock({ className }: { className?: string }) {
 return <div className={cn('animate-pulse rounded-xl bg-slate-800/80', className)} />;
}

/** Khung chờ trong vùng main của UserLayout (sidebar/tab đã có sẵn). */
export function UserSegmentMainSkeleton() {
 return (
  <div className={cn('tn-page-x', 'py-8', 'pb-24')}>
   <div className={cn('mx-auto', 'max-w-3xl', 'space-y-6')}>
    <PulseBlock className={cn('h-10', 'w-2/3')} />
    <PulseBlock className={cn('h-4', 'w-full')} />
    <PulseBlock className={cn('h-4', 'w-5/6')} />
    <div className={cn('grid', 'grid-cols-2', 'gap-4', 'pt-6', 'sm:grid-cols-3')}>
     <PulseBlock className={cn('h-24')} />
     <PulseBlock className={cn('h-24')} />
     <PulseBlock className={cn('h-24', 'col-span-2', 'sm:col-span-1')} />
    </div>
    <PulseBlock className={cn('h-40', 'w-full')} />
   </div>
  </div>
 );
}

export function WalletSegmentSkeleton() {
 return (
  <div className={cn('tn-page-x', 'py-8', 'pb-32')}>
   <PulseBlock className={cn('mb-8', 'h-9', 'w-48')} />
   <div className={cn('mb-8', 'grid', 'gap-4', 'sm:grid-cols-2')}>
    <PulseBlock className={cn('h-32')} />
    <PulseBlock className={cn('h-32')} />
   </div>
   <PulseBlock className={cn('h-12', 'w-full', 'max-w-md', 'mb-6')} />
   <div className={cn('space-y-3')}>
    {Array.from({ length: 6 }).map((_, i) => (
     <PulseBlock key={i} className={cn('h-14', 'w-full')} />
    ))}
   </div>
  </div>
 );
}

export function ChatSegmentSkeleton() {
 return (
  <div className={cn('flex', 'h-full', 'min-h-[50vh]')}>
   <div className={cn('hidden', 'w-72', 'flex-shrink-0', 'border-r', 'border-slate-800/80', 'p-3', 'md:block')}>
    <PulseBlock className={cn('mb-4', 'h-8', 'w-full')} />
    {Array.from({ length: 8 }).map((_, i) => (
     <PulseBlock key={i} className={cn('mb-2', 'h-12', 'w-full')} />
    ))}
   </div>
   <div className={cn('flex', 'flex-1', 'flex-col', 'p-4')}>
    <PulseBlock className={cn('mb-6', 'h-10', 'w-40')} />
    <div className={cn('flex-1', 'space-y-3')}>
     {Array.from({ length: 5 }).map((_, i) => (
      <PulseBlock key={i} className={cn('h-16', i % 2 === 0 ? 'mr-12' : 'ml-12')} />
     ))}
    </div>
    <PulseBlock className={cn('mt-4', 'h-12', 'w-full')} />
   </div>
  </div>
 );
}

export function ReadingSegmentSkeleton() {
 return (
  <div className={cn('mx-auto', 'max-w-4xl', 'px-6', 'pt-10', 'pb-20')}>
   <PulseBlock className={cn('mb-10', 'h-8', 'w-56')} />
   <PulseBlock className={cn('mb-8', 'h-24', 'w-full')} />
   <div className={cn('mb-10', 'grid', 'grid-cols-2', 'gap-4', 'sm:grid-cols-4')}>
    {Array.from({ length: 4 }).map((_, i) => (
     <PulseBlock key={i} className={cn('aspect-square')} />
    ))}
   </div>
   <PulseBlock className={cn('h-32', 'w-full')} />
   <PulseBlock className={cn('mt-8', 'mx-auto', 'h-12', 'w-48')} />
  </div>
 );
}
