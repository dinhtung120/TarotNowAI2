import { ArrowLeft, User } from "lucide-react";
import { Button } from "@/shared/ui";
import { cn } from "@/lib/utils";

interface ReaderProfileNotFoundStateProps {
 backLabel: string;
 description: string;
 onBack: () => void;
}

export function ReaderProfileNotFoundState({
 backLabel,
 description,
 onBack,
}: ReaderProfileNotFoundStateProps) {
 return (
  <div className={cn("tn-h-60vh flex flex-col items-center justify-center space-y-6")}>
   <div className={cn("w-24 h-24 tn-panel-soft rounded-full flex items-center justify-center mb-2 shadow-inner")}>
    <User className={cn("w-10 h-10 tn-text-tertiary opacity-50")} />
   </div>
   <p className={cn("tn-text-secondary text-sm font-medium")}>{description}</p>
   <Button variant="ghost" onClick={onBack} className={cn("tn-text-secondary tn-hover-text-primary mt-4")}>
    <ArrowLeft className={cn("w-4 h-4 mr-2")} />
    {backLabel}
   </Button>
  </div>
 );
}
