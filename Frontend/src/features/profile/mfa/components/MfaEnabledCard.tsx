import { ArrowRight, ShieldCheck } from "lucide-react";
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { Button, GlassCard } from "@/shared/ui";
import { cn } from "@/lib/utils";

interface MfaEnabledCardProps {
 ctaLabel: string;
 description: string;
 title: string;
}

export function MfaEnabledCard({ ctaLabel, description, title }: MfaEnabledCardProps) {
 return (
  <GlassCard className={cn("!p-8 text-center space-y-4")}>
   <div className={cn("tn-mfa-enabled-icon-shell mx-auto w-24 h-24 rounded-full flex items-center justify-center mb-6")}>
    <ShieldCheck className={cn("w-12 h-12 tn-text-success")} />
   </div>
   <h2 className={cn("text-2xl font-black tn-text-primary uppercase italic tracking-tight")}>{title}</h2>
   <p className={cn("tn-text-secondary text-sm font-medium")}>{description}</p>
   <div className={cn("pt-6")}>
    <Link href="/wallet/withdraw">
     <Button variant="primary" className={cn("tn-mfa-enabled-cta tn-text-primary border-none")}>
      {ctaLabel} <ArrowRight className={cn("w-4 h-4 ml-2")} />
     </Button>
    </Link>
   </div>
  </GlassCard>
 );
}
