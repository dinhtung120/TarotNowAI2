import Image from 'next/image';
import { cn } from '@/lib/utils';
import { shouldUseUnoptimizedImage } from '@/shared/http/assetUrl';

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
          'relative aspect-[4/3] w-[min(75vw,20rem)] overflow-hidden rounded-2xl border border-white/10 bg-black/20',
        )}
      >
        <Image
          src={imageUrl}
          alt="media"
          fill
          sizes="(max-width: 768px) 75vw, 320px"
          loading="lazy"
          unoptimized={shouldUseUnoptimizedImage(imageUrl)}
          className={cn('object-cover')}
        />
      </div>
    </div>
  );
}
