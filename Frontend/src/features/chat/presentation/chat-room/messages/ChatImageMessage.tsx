import Image from 'next/image';
import { cn } from '@/lib/utils';

interface ChatImageMessageProps {
  imageUrl: string;
  isMe: boolean;
}

export default function ChatImageMessage({
  imageUrl,
  isMe,
}: ChatImageMessageProps) {
  return (
    <div className={cn('flex', isMe ? 'justify-end' : 'justify-start')}>
      <div
        className={cn(
          'max-w-[75%] overflow-hidden rounded-2xl border border-white/10',
        )}
      >
        <Image
          src={imageUrl}
          alt="media"
          width={320}
          height={240}
          unoptimized
          className={cn('h-auto w-full')}
        />
      </div>
    </div>
  );
}
