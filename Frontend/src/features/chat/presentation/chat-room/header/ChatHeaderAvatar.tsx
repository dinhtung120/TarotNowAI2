import Image from "next/image";
import { cn } from "@/lib/utils";

interface ChatHeaderAvatarProps {
  otherAvatar: string | null;
  otherName: string;
}

export default function ChatHeaderAvatar({
  otherAvatar,
  otherName,
}: ChatHeaderAvatarProps) {
  if (otherAvatar) {
    return (
      <Image
        alt={otherName || "user"}
        className={cn(
          "h-9 w-9 rounded-full border border-white/10 object-cover",
        )}
        height={36}
        src={otherAvatar}
        unoptimized
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
