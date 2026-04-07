import { RefreshCcw } from "lucide-react";

interface VerifyEmailResendButtonProps {
 isResending: boolean;
 resendTimer: number;
 resendLabel: string;
 resendWithTimerLabel: (seconds: number) => string;
 onResend: () => void;
}

export function VerifyEmailResendButton({ isResending, resendTimer, resendLabel, resendWithTimerLabel, onResend }: VerifyEmailResendButtonProps) {
 return (
  <button onClick={onResend} disabled={resendTimer > 0 || isResending} className="text-[10px] font-black uppercase tracking-widest tn-text-muted hover:tn-text-primary transition-all flex items-center justify-center gap-2 mx-auto disabled:opacity-50 disabled:cursor-not-allowed group min-h-11 px-3 rounded-xl hover:tn-surface-soft">
   <RefreshCcw className={`w-3.5 h-3.5 ${isResending ? "animate-spin" : "group-hover:rotate-180 transition-transform duration-500"}`} />
   {resendTimer > 0 ? resendWithTimerLabel(resendTimer) : resendLabel}
  </button>
 );
}
