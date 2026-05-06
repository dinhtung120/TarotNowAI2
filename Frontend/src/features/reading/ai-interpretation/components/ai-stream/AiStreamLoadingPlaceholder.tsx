import { Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface AiStreamLoadingPlaceholderProps {
 message: string;
}

export function AiStreamLoadingPlaceholder({ message }: AiStreamLoadingPlaceholderProps) {
 return (
  <div className={cn("h-full flex items-center justify-center tn-text-accent-50")}>
   <div className={cn("flex flex-col items-center")}>
    <Sparkles className={cn("w-10 h-10 animate-pulse mb-4")} />
    <p className={cn("font-serif italic animate-pulse")}>{message}</p>
   </div>
  </div>
 );
}
