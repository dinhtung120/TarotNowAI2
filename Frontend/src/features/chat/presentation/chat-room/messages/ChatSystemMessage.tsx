import { cn } from '@/lib/utils';

interface ChatSystemMessageProps {
  content: string;
}

export default function ChatSystemMessage({
  content,
}: ChatSystemMessageProps) {
  return (
    <div className={cn('flex justify-center py-1')}>
      <div
        className={cn(
          'rounded-full bg-white/5 px-3 py-1 text-[10px] text-[var(--text-secondary)]',
        )}
      >
        {content}
      </div>
    </div>
  );
}
