import { cn } from "@/lib/utils";

export function AiStreamStreamingDots() {
 return (
  <span className={cn("inline-flex space-x-1 mt-2")}>
   <span className={cn("w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.3s]")} />
   <span className={cn("w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce [animation-delay:-0.15s]")} />
   <span className={cn("w-1.5 h-1.5 bg-[var(--purple-accent)] rounded-full animate-bounce")} />
  </span>
 );
}
