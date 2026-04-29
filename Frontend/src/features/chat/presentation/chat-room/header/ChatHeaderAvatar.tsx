import Image from "next/image";
import { cn } from "@/lib/utils";
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from '@/shared/infrastructure/http/assetUrl';

interface ChatHeaderAvatarProps {
  otherAvatar: string | null;
  otherName: string;
}

export default function ChatHeaderAvatar({
  otherAvatar,
  otherName,
}: ChatHeaderAvatarProps) {
  const avatarSrc = resolveAvatarUrl(otherAvatar);
  const unoptimizedAvatar = shouldUseUnoptimizedImage(avatarSrc);

  if (avatarSrc) {
    return (
      <Image
        alt={otherName || "user"}
        className={cn(
          "h-9 w-9 rounded-full border border-white/10 object-cover",
        )}
        height={36}
        src={avatarSrc}
        sizes="36px"
        loading="lazy"
        unoptimized={unoptimizedAvatar}
        width={36}
      />
    );
  }

  return (
    <div
      className={cn(
        "flex h-9 w-9 items-center justify-center rounded-full border border-[var(--purple-accent)]/30 bg-[var(--purple-accent)]/20 text-sm font-black text-[var(--purple-accent)]",
      )}
    >
      {(otherName || "?").charAt(0).toUpperCase()}
    </div>
  );
}
