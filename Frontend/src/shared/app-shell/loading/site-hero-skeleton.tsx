import { cn } from '@/lib/utils';

function Pulse({ className }: { className?: string }) {
 return <div className={cn('animate-pulse rounded-2xl bg-white/5', className)} />;
}

export function SiteHeroSkeleton() {
 return (
  <div
   className={cn(
    'min-h-dvh',
    'flex flex-col items-center justify-center px-4 pt-20 pb-28 sm:px-6 md:pb-24'
   )}
  >
   <div className={cn('flex w-full max-w-6xl flex-col items-center gap-8 text-center')}>
    <Pulse className={cn('h-6 w-40')} />
    <Pulse className={cn('h-14 w-full max-w-2xl')} />
    <Pulse className={cn('h-14 w-full max-w-xl')} />
    <Pulse className={cn('h-5 w-full max-w-lg')} />
    <div className={cn('mt-6 flex flex-wrap justify-center gap-4')}>
     <Pulse className={cn('h-12 w-40')} />
     <Pulse className={cn('h-12 w-44')} />
    </div>
   </div>
   <Pulse className={cn('mt-16 h-8 w-8 rounded-full')} />
  </div>
 );
}
