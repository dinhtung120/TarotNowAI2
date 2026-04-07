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
   <div className={cn("inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 shadow-[0_0_15px_var(--c-16-185-129-10)]")}>
    <div className={cn("w-2 h-2 rounded-full bg-[var(--success)] animate-pulse shadow-[0_0_8px_currentColor]")} />
    <span className={cn("text-[10px] font-black uppercase tracking-widest text-[var(--success)]")}>{t("profile.status.online")}</span>
   </div>
  );
 }
 if (normalized === "busy") {
  return (
   <div className={cn("inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-[0_0_15px_var(--c-245-158-11-10)]")}>
    <div className={cn("w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_currentColor]")} />
    <span className={cn("text-[10px] font-black uppercase tracking-widest text-[var(--warning)]")}>{t("profile.status.busy")}</span>
   </div>
  );
 }
 return (
  <div className={cn("inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full tn-surface-strong border tn-border")}>
   <div className={cn("w-2 h-2 rounded-full bg-[var(--text-muted)]")} />
   <span className={cn("text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]")}>{t("profile.status.offline")}</span>
  </div>
 );
}
