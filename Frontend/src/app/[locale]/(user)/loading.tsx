
import { Loader2 } from "lucide-react";
import { cn } from '@/lib/utils';

export default function UserRouteLoading() {
  return (
    <div className={cn("flex min-h-[60vh] w-full items-center justify-center")}>
      <div className={cn("flex flex-col items-center gap-4")}>
        <Loader2 className={cn("h-10 w-10 animate-spin text-[var(--purple-accent)]")} />
        <p className={cn("text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]")}>
          Loading
        </p>
      </div>
    </div>
  );
}
