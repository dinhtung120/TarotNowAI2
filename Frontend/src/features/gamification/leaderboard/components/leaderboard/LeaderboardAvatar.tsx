import Image from "next/image";
import { cn } from "@/lib/utils";
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from "@/shared/http/assetUrl";

interface LeaderboardAvatarProps {
  avatar: string | null | undefined;
  displayName: string;
}

export default function LeaderboardAvatar({
  avatar,
  displayName,
}: LeaderboardAvatarProps) {
  const avatarSrc = resolveAvatarUrl(avatar);
  const unoptimizedAvatar = shouldUseUnoptimizedImage(avatarSrc);

  return (
    <div className={cn("relative", "h-12", "w-12", "shrink-0", "overflow-hidden", "rounded-full", "border-2", "border-slate-700", "bg-slate-800", "shadow-inner", "transition-colors")}>
      {avatarSrc ? (
        <Image
          alt={displayName}
          className={cn("object-cover")}
          fill
          sizes="48px"
          src={avatarSrc}
          unoptimized={unoptimizedAvatar}
        />
      ) : (
        <div className={cn("flex", "h-full", "w-full", "items-center", "justify-center", "text-lg", "font-bold", "text-slate-500")}>
          {displayName.slice(0, 1).toUpperCase()}
        </div>
      )}
    </div>
  );
}
