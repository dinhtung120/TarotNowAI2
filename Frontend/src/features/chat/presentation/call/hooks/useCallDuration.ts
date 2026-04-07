import { useEffect, useState } from 'react';

interface UseCallDurationArgs {
  isOpen: boolean;
  sessionId?: string;
}

export function useCallDuration({ isOpen, sessionId }: UseCallDurationArgs): string {
  const [duration, setDuration] = useState(0);

  useEffect(() => {
    if (!isOpen) return undefined;

    let elapsed = 0;
    const timer = window.setInterval(() => {
      elapsed += 1;
      setDuration(elapsed);
    }, 1000);

    return () => window.clearInterval(timer);
  }, [isOpen, sessionId]);

  const elapsedSeconds = isOpen ? duration : 0;
  const minutes = Math.floor(elapsedSeconds / 60).toString().padStart(2, '0');
  const seconds = (elapsedSeconds % 60).toString().padStart(2, '0');
  return `${minutes}:${seconds}`;
}
