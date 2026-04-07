import { Award } from "lucide-react";

interface UnlockedInfoSectionProps {
 unlockedAtLabel: string;
 unlockedAt: string;
}

export function UnlockedInfoSection({ unlockedAtLabel, unlockedAt }: UnlockedInfoSectionProps) {
 return (
  <section className="bg-amber-500/5 rounded-2xl p-4 border border-amber-500/20 flex items-center gap-3">
   <Award className="w-8 h-8 text-amber-500/50" />
   <div className="text-xs">
    <p className="text-amber-200/50 font-bold uppercase tracking-wider">{unlockedAtLabel}</p>
    <p className="text-amber-200 font-black text-sm">{new Date(unlockedAt).toLocaleDateString()}</p>
   </div>
  </section>
 );
}
