import { cn } from "@/lib/utils";

export function AiStreamStreamingDots() {
 return (
  <span className={cn("inline-flex space-x-1 mt-2")}>
   <span className={cn("w-1.5 h-1.5 tn-dot-accent rounded-full animate-bounce tn-anim-delay-neg-300")} />
   <span className={cn("w-1.5 h-1.5 tn-dot-accent rounded-full animate-bounce tn-anim-delay-neg-150")} />
   <span className={cn("w-1.5 h-1.5 tn-dot-accent rounded-full animate-bounce")} />
  </span>
 );
}
