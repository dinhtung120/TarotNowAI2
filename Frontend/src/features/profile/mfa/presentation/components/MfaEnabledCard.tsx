import { ArrowRight, ShieldCheck } from "lucide-react";
import { Link } from "@/i18n/routing";
import { Button, GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";

interface MfaEnabledCardProps {
 ctaLabel: string;
 description: string;
 title: string;
}

export function MfaEnabledCard({ ctaLabel, description, title }: MfaEnabledCardProps) {
 return (
  <GlassCard className={cn("!p-8 text-center space-y-4")}>
   <div className={cn("mx-auto w-24 h-24 bg-[var(--success-bg)] border border-[var(--success)]/30 rounded-full flex items-center justify-center shadow-[0_0_30px_var(--c-16-185-129-20)] mb-6")}>
    <ShieldCheck className={cn("w-12 h-12 text-[var(--success)]")} />
   </div>
   <h2 className={cn("text-2xl font-black tn-text-primary uppercase italic tracking-tight")}>{title}</h2>
   <p className={cn("text-[var(--text-secondary)] text-sm font-medium")}>{description}</p>
   <div className={cn("pt-6")}>
    <Link href="/wallet/withdraw">
     <Button variant="primary" className={cn("!bg-[var(--success)] hover:!bg-[var(--success)]/80 tn-text-primary border-none shadow-[0_0_20px_var(--c-16-185-129-30)] hover:shadow-[0_0_30px_var(--c-16-185-129-50)]")}>
      {ctaLabel} <ArrowRight className={cn("w-4 h-4 ml-2")} />
     </Button>
    </Link>
   </div>
  </GlassCard>
 );
}
