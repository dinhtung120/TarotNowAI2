import Image from "next/image";
import { cn } from "@/lib/utils";
import { resolveAvatarUrl } from "@/shared/infrastructure/http/assetUrl";

interface FeaturedReaderAvatarProps {
  avatarUrl?: string | null;
  displayName: string;
}

export default function FeaturedReaderAvatar({
  avatarUrl,
  displayName,
}: FeaturedReaderAvatarProps) {
  const avatarSrc = resolveAvatarUrl(avatarUrl);

  if (avatarSrc) {
    return (
      <Image
        src={avatarSrc}
        alt={displayName}
        fill
        sizes="(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 25vw"
        unoptimized
        className={cn(
          "absolute inset-0 z-0 h-full w-full object-cover transition-transform duration-700 group-hover:scale-110",
        )}
      />
    );
  }

  return (
    <div
      className={cn(
        "absolute inset-0 z-0 flex h-full w-full items-center justify-center bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--bg-surface)]",
      )}
    >
      <span
        className={cn(
          "text-6xl font-black text-[var(--text-muted)]/30 uppercase italic select-none",
        )}
      >
        {displayName.charAt(0) || "?"}
      </span>
    </div>
  );
}
