import { RefreshCcw } from "lucide-react";
import { cn } from "@/lib/utils";

interface VerifyEmailResendButtonProps {
 isResending: boolean;
 resendTimer: number;
 resendLabel: string;
 resendWithTimerLabel: (seconds: number) => string;
 onResend: () => void;
}

export function VerifyEmailResendButton({ isResending, resendTimer, resendLabel, resendWithTimerLabel, onResend }: VerifyEmailResendButtonProps) {
 return (
  <button
   type="button"
   onClick={onResend}
   disabled={resendTimer > 0 || isResending}
   className={cn(
    "mx-auto",
    "flex",
    "min-h-11",
    "items-center",
    "justify-center",
    "gap-2",
    "rounded-xl",
    "px-3",
    "text-xs",
    "font-black",
    "uppercase",
    "tracking-widest",
    "transition-all",
    "tn-text-muted",
    "disabled:cursor-not-allowed",
    "disabled:opacity-50",
   )}
  >
   <RefreshCcw className={cn("h-4", "w-4", isResending ? "animate-spin" : "transition-transform", "duration-500")} />
   {resendTimer > 0 ? resendWithTimerLabel(resendTimer) : resendLabel}
  </button>
 );
}
