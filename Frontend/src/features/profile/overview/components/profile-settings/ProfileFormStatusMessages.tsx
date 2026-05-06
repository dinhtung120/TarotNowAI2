import { ShieldCheck } from "lucide-react";
import { cn } from "@/lib/utils";

interface ProfileFormStatusMessagesProps {
  errorMsg: string;
  successMsg: string;
}

export default function ProfileFormStatusMessages({
  errorMsg,
  successMsg,
}: ProfileFormStatusMessagesProps) {
  return (
    <div className={cn("mb-8 space-y-3")}>
      {successMsg ? (
        <div
          className={cn(
            "animate-in slide-in-from-top-2 flex items-center gap-3 rounded-xl border border-[var(--success)]/30 bg-[var(--success-bg)] p-4 text-xs font-bold tracking-widest text-[var(--success)] uppercase duration-500",
          )}
        >
          <ShieldCheck className={cn("h-4 w-4")} />
          {successMsg}
        </div>
      ) : null}
      {errorMsg ? (
        <div
          className={cn(
            "animate-in slide-in-from-top-2 flex items-center gap-3 rounded-xl border border-[var(--danger)]/30 bg-[var(--danger-bg)] p-4 text-xs font-bold tracking-widest text-[var(--danger)] uppercase duration-500",
          )}
        >
          <ShieldCheck className={cn("h-4 w-4")} />
          {errorMsg}
        </div>
      ) : null}
    </div>
  );
}
