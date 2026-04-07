import { Link } from "@/i18n/routing";
import { normalizeReaderStatus } from "@/features/reader/domain/readerStatus";
import FeaturedReaderAvatar from "@/features/home/presentation/components/featured-readers/FeaturedReaderAvatar";
import FeaturedReaderMeta from "@/features/home/presentation/components/featured-readers/FeaturedReaderMeta";
import type { FeaturedReaderCardProps } from "@/features/home/presentation/components/featured-readers/types";
import { cn } from "@/lib/utils";

export default function FeaturedReaderCard({
  profileCta,
  reader,
}: FeaturedReaderCardProps) {
  const status = normalizeReaderStatus(reader.status);
  const statusClassName =
    status === "online"
      ? "bg-[var(--success)] animate-pulse"
      : status === "busy"
        ? "bg-[var(--warning)]"
        : "bg-[var(--text-muted)]";

  return (
    <Link
      href={`/readers/${reader.userId}`}
      className={cn(
        "group preserve-3d relative h-96 overflow-hidden rounded-[2.5rem] border border-[var(--border-default)] bg-[var(--bg-surface)] shadow-[var(--shadow-card)] transition-all duration-700 hover:-translate-y-4 hover:border-[var(--border-focus)]",
      )}
    >
      <FeaturedReaderAvatar
        avatarUrl={reader.avatarUrl}
        displayName={reader.displayName}
      />
      <div
        className={cn(
          "absolute inset-0 z-10 bg-gradient-to-t from-[var(--bg-void)] via-[var(--bg-void)]/40 to-transparent",
        )}
      />
      <div
        className={cn(
          "absolute inset-0 z-10 bg-[var(--purple-glow)] opacity-0 transition-opacity duration-700 group-hover:opacity-100",
        )}
      />
      <FeaturedReaderMeta
        reader={reader}
        profileCta={profileCta}
        statusClassName={statusClassName}
      />
    </Link>
  );
}
