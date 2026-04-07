import { Award, Flame, Users, Zap } from "lucide-react";
import { getTranslations } from "next-intl/server";
import StatItem from "@/features/home/presentation/components/stats/StatItem";
import { cn } from "@/lib/utils";

export async function StatsSection() {
  const t = await getTranslations("Index");

  return (
    <section
      className={cn("relative mx-auto w-full max-w-6xl py-20 tn-page-pad-4-6-8")}
    >
      <div className={cn("tn-grid-2-4-md tn-gap-4-8-md")}>
        <StatItem
          color="purple"
          icon={Zap}
          value="50K+"
          label={t("stats.readings")}
        />
        <StatItem
          color="amber"
          icon={Users}
          value="120+"
          label={t("stats.readers")}
        />
        <StatItem
          color="success"
          icon={Award}
          value="4.9"
          label={t("stats.rating")}
        />
        <StatItem
          color="info"
          icon={Flame}
          value="24/7"
          label={t("stats.support")}
        />
      </div>
    </section>
  );
}
