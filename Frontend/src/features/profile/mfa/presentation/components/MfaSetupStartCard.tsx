import { AlertTriangle, KeyRound, Loader2, ShieldAlert } from "lucide-react";
import { Button, GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";

interface MfaSetupStartCardProps {
 actionLabel: string;
 description: string;
 errorMessage: string;
 loading: boolean;
 onStartSetup: () => void;
 title: string;
}

export function MfaSetupStartCard({
 actionLabel,
 description,
 errorMessage,
 loading,
 onStartSetup,
 title,
}: MfaSetupStartCardProps) {
 return (
  <GlassCard className={cn("!p-8 text-center space-y-6")}>
   <div className={cn("mx-auto w-24 h-24 bg-[var(--warning)]/10 border border-[var(--warning)]/30 rounded-full flex items-center justify-center shadow-[0_0_30px_var(--c-245-158-11-20)] animate-pulse mb-6")}>
    <ShieldAlert className={cn("w-12 h-12 text-[var(--warning)]")} />
   </div>
   <div className={cn("space-y-3")}>
    <h2 className={cn("text-2xl font-black tn-text-primary uppercase italic tracking-tight")}>{title}</h2>
    <p className={cn("text-[var(--warning)] text-sm font-medium")}>{description}</p>
   </div>
   {errorMessage ? (
    <div className={cn("text-[var(--danger)] text-xs font-bold uppercase tracking-widest flex justify-center items-center gap-2 bg-[var(--danger-bg)] border border-[var(--danger)]/30 p-4 rounded-xl")}>
     <AlertTriangle className={cn("w-4 h-4")} /> {errorMessage}
    </div>
   ) : null}
   <div className={cn("pt-4")}>
    <Button
     variant="primary"
     onClick={onStartSetup}
     disabled={loading}
     className={cn("w-full h-14 !bg-[var(--warning)]/20 hover:!bg-[var(--warning)]/30 text-[var(--warning)] border-[var(--warning)]/30")}
    >
     {loading ? <Loader2 className={cn("w-5 h-5 animate-spin mr-2")} /> : <KeyRound className={cn("w-5 h-5 mr-2")} />}
     {actionLabel}
    </Button>
   </div>
  </GlassCard>
 );
}
