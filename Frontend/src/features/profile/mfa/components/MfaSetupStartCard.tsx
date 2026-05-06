import { AlertTriangle, KeyRound, Loader2, ShieldAlert } from "lucide-react";
import { Button, GlassCard } from "@/shared/ui";
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
   <div className={cn("tn-mfa-setup-icon-shell mx-auto w-24 h-24 rounded-full flex items-center justify-center animate-pulse mb-6")}>
    <ShieldAlert className={cn("w-12 h-12 tn-text-warning")} />
   </div>
   <div className={cn("space-y-3")}>
    <h2 className={cn("text-2xl font-black tn-text-primary uppercase italic tracking-tight")}>{title}</h2>
    <p className={cn("tn-text-warning text-sm font-medium")}>{description}</p>
   </div>
   {errorMessage ? (
    <div className={cn("tn-text-danger text-xs font-bold uppercase tracking-widest flex justify-center items-center gap-2 tn-bg-danger-soft border tn-border-danger p-4 rounded-xl")}>
     <AlertTriangle className={cn("w-4 h-4")} /> {errorMessage}
    </div>
   ) : null}
   <div className={cn("pt-4")}>
    <Button
     variant="primary"
     onClick={onStartSetup}
     disabled={loading}
     className={cn("tn-mfa-setup-action-btn w-full h-14")}
    >
     {loading ? <Loader2 className={cn("w-5 h-5 animate-spin mr-2")} /> : <KeyRound className={cn("w-5 h-5 mr-2")} />}
     {actionLabel}
    </Button>
   </div>
  </GlassCard>
 );
}
