import { cn } from "@/lib/utils";

interface AiStreamHardCapStateProps {
 title: string;
 description: string;
}

export function AiStreamHardCapState({ title, description }: AiStreamHardCapStateProps) {
 return (
  <div className={cn("mt-4 shrink-0 text-center p-4 tn-surface-strong border tn-border-soft rounded-2xl animate-in slide-in-from-bottom-5 duration-500")}>
   <p className={cn("text-sm font-medium text-[var(--warning)]")}>{title}</p>
   <p className={cn("text-xs tn-text-muted mt-1")}>{description}</p>
  </div>
 );
}
