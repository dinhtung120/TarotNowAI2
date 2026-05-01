import Image from 'next/image';
import { cn } from '@/lib/utils';
import { shouldUseUnoptimizedImage } from '@/shared/infrastructure/http/assetUrl';

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
          'max-w-[75%] min-h-36 min-w-36 overflow-hidden rounded-2xl border border-white/10 bg-black/20',
        )}
      >
        <Image
          src={imageUrl}
          alt="media"
          width={320}
          height={240}
          sizes="(max-width: 768px) 75vw, 320px"
          loading="lazy"
          unoptimized={shouldUseUnoptimizedImage(imageUrl)}
          className={cn('h-auto min-h-36 w-full object-cover')}
        />
      </div>
    </div>
  );
}
