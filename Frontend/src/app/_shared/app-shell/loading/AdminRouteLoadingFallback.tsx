import { cn } from '@/lib/utils';

export default function AdminRouteLoadingFallback() {
  return (
    <div className={cn('flex min-h-[35vh] items-center justify-center text-sm text-slate-400')}>
      Loading…
    </div>
  );
}
