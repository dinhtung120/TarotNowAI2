import Image from "next/image";
import { cn } from "@/lib/utils";

interface ReaderDirectoryCardAvatarProps {
  avatarUrl?: string | null;
  displayName: string;
}

export default function ReaderDirectoryCardAvatar({
  avatarUrl,
  displayName,
}: ReaderDirectoryCardAvatarProps) {
  return (
    <div className={cn("relative h-14 w-14 shrink-0")}>
      <div
        className={cn(
          "absolute inset-0 rounded-full bg-gradient-to-br from-[var(--purple-accent)]/40 to-[var(--warning)]/20 opacity-50 blur-md transition-opacity group-hover:opacity-100",
        )}
      />
      <div
        className={cn(
          "tn-border tn-text-primary relative z-10 flex h-full w-full items-center justify-center overflow-hidden rounded-full border-2 bg-white/5 text-xl font-black",
        )}
      >
        {avatarUrl ? (
          <Image
            alt={displayName}
            className={cn("h-full w-full object-cover")}
            fill
            sizes="56px"
            src={avatarUrl}
            unoptimized
          />
        ) : (
          displayName?.charAt(0)?.toUpperCase() || "?"
        )}
      </div>
    </div>
  );
}
