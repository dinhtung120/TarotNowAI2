import { Award, Flame, Users, Zap } from "lucide-react";
import { getTranslations } from "next-intl/server";
import StatItem from "@/features/home/presentation/components/stats/StatItem";
import { cn } from "@/lib/utils";

export async function StatsSection() {
  const t = await getTranslations("Index");

  return (
    <section
      className={cn("relative mx-auto w-full max-w-6xl px-4 py-20 sm:px-6")}
    >
      <div className={cn("grid grid-cols-2 gap-4 md:grid-cols-4 md:gap-8")}>
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
