import Image from "next/image";

interface LeaderboardAvatarProps {
  avatar: string | null | undefined;
  displayName: string;
}

export default function LeaderboardAvatar({
  avatar,
  displayName,
}: LeaderboardAvatarProps) {
  return (
    <div className="relative h-12 w-12 shrink-0 overflow-hidden rounded-full border-2 border-slate-700 bg-slate-800 shadow-inner transition-colors group-hover:border-slate-500">
      {avatar ? (
        <Image
          alt={displayName}
          className="object-cover"
          fill
          sizes="48px"
          src={avatar}
        />
      ) : (
        <div className="flex h-full w-full items-center justify-center text-lg font-bold text-slate-500">
          {displayName.slice(0, 1).toUpperCase()}
        </div>
      )}
    </div>
  );
}
