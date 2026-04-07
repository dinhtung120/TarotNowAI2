import { normalizeReaderStatus } from "@/features/reader/domain/readerStatus";
import { cn } from "@/lib/utils";

interface ReaderStatusBadgeProps {
 status: string;
 t: (key: string) => string;
}

export function ReaderStatusBadge({ status, t }: ReaderStatusBadgeProps) {
 const normalized = normalizeReaderStatus(status);
 if (normalized === "online") {
  return (
   <div className={cn("inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-bg-success-10 border tn-border-success-20")}>
    <div className={cn("w-2 h-2 rounded-full tn-bg-success animate-pulse")} />
    <span className={cn("tn-text-10 font-black uppercase tracking-widest tn-text-success")}>{t("profile.status.online")}</span>
   </div>
  );
 }
 if (normalized === "busy") {
  return (
   <div className={cn("inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-bg-warning-10 border tn-border-warning-20")}>
    <div className={cn("w-2 h-2 rounded-full tn-bg-warning")} />
    <span className={cn("tn-text-10 font-black uppercase tracking-widest tn-text-warning")}>{t("profile.status.busy")}</span>
   </div>
  );
 }
 return (
  <div className={cn("inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-surface-strong border tn-border")}>
   <div className={cn("w-2 h-2 rounded-full tn-bg-muted")} />
   <span className={cn("tn-text-10 font-black uppercase tracking-widest tn-text-secondary")}>{t("profile.status.offline")}</span>
  </div>
 );
}
