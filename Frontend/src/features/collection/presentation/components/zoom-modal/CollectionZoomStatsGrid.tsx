import type { UserCollectionDto } from "@/features/collection/application/actions";
import CollectionZoomStatCard from "@/features/collection/presentation/components/zoom-modal/CollectionZoomStatCard";
import { cn } from "@/lib/utils";

interface CollectionZoomStatsGridProps {
  labels: { copiesLabel: string; levelLabel: string };
  userCard: UserCollectionDto;
}

export default function CollectionZoomStatsGrid({
  labels,
  userCard,
}: CollectionZoomStatsGridProps) {
  const stats = [
    {
      label: labels.levelLabel,
      value: userCard.level,
      valueClassName: "text-[var(--warning)]",
      wrapperClassName: "tn-panel tn-border-soft",
    },
    {
      label: labels.copiesLabel,
      value: userCard.copies,
      valueClassName: "tn-text-primary",
      wrapperClassName: "tn-panel tn-border-soft",
    },
    {
      label: "ATK",
      value: userCard.atk || 0,
      valueClassName:
        "text-red-400 drop-shadow-[0_0_8px_rgba(248,113,113,0.5)]",
      wrapperClassName: "tn-panel border-red-500/20 bg-red-500/5",
    },
    {
      label: "DEF",
      value: userCard.def || 0,
      valueClassName:
        "text-blue-400 drop-shadow-[0_0_8px_rgba(96,165,250,0.5)]",
      wrapperClassName: "tn-panel border-blue-500/20 bg-blue-500/5",
    },
  ];

  return (
    <div
      className={cn(
        "mx-auto mt-8 grid grid-cols-2 gap-4 md:mx-0 md:mt-10 md:grid-cols-4",
      )}
    >
      {stats.map((stat) => (
        <CollectionZoomStatCard key={stat.label} {...stat} />
      ))}
    </div>
  );
}
