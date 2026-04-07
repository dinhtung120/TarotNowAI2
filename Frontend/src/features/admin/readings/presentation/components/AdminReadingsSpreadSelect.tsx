import { cn } from "@/lib/utils";

interface AdminReadingsSpreadSelectProps {
 value: string;
 onChange: (value: string) => void;
 labels: {
  all: string;
  daily: string;
  spread3: string;
  spread5: string;
  spread10: string;
 };
}

export function AdminReadingsSpreadSelect({ value, onChange, labels }: AdminReadingsSpreadSelectProps) {
 return (
  <select value={value} onChange={(event) => onChange(event.target.value)} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all appearance-none cursor-pointer shadow-inner")}>
   <option value="" className={cn("tn-surface")}>{labels.all}</option>
   <option value="daily_1" className={cn("tn-surface")}>{labels.daily}</option>
   <option value="spread_3" className={cn("tn-surface")}>{labels.spread3}</option>
   <option value="spread_5" className={cn("tn-surface")}>{labels.spread5}</option>
   <option value="spread_10" className={cn("tn-surface")}>{labels.spread10}</option>
  </select>
 );
}
