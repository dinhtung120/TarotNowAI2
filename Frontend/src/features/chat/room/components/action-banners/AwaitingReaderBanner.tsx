import { cn } from "@/lib/utils";

export default function AwaitingReaderBanner() {
  return (
    <div
      className={cn(
        "border-b border-white/10 bg-white/5 px-4 py-2 text-xs text-[var(--text-secondary)]",
      )}
    >
      Đang chờ Reader chấp nhận hoặc từ chối câu hỏi.
    </div>
  );
}
