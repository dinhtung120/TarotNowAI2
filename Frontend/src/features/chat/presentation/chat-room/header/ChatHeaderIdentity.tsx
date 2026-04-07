import { cn } from "@/lib/utils";

interface ChatHeaderIdentityProps {
  loading: boolean;
  otherName: string;
  readerStatus: { text: string; color: string };
  title: string;
  hasConversation: boolean;
}

export default function ChatHeaderIdentity({
  loading,
  otherName,
  readerStatus,
  title,
  hasConversation,
}: ChatHeaderIdentityProps) {
  return (
    <div className={cn("min-w-0")}>
      <p className={cn("truncate font-semibold text-white")}>
        {otherName || title}
      </p>
      {!hasConversation && loading ? (
        <p className={cn("text-[11px] text-[var(--text-secondary)]")}>...</p>
      ) : (
        <p className={cn("text-[11px]", readerStatus.color)}>
          {readerStatus.text}
        </p>
      )}
    </div>
  );
}
