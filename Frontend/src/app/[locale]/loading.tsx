
import { Loader2 } from "lucide-react";
import { cn } from '@/lib/utils';

export default function LocaleLoading() {
  return (
    <div className={cn("flex tn-min-h-70vh items-center justify-center")}>
      <div className={cn("flex flex-col items-center gap-4")}>
        <Loader2 className={cn("h-10 w-10 animate-spin tn-text-accent")} />
        <p className={cn("tn-text-10 font-black uppercase tn-tracking-03 tn-text-secondary")}>
          Loading
        </p>
      </div>
    </div>
  );
}
