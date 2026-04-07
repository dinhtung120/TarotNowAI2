'use client';

import dynamic from 'next/dynamic';
import { useParams } from 'next/navigation';
import ReadingSessionContent from '@/features/reading/session/presentation/session-page/ReadingSessionContent';
import { useReadingSessionPageState } from '@/features/reading/session/presentation/useReadingSessionPageState';
import { cn } from '@/lib/utils';

const AiInterpretationStream = dynamic(
  () => import('@/features/reading/presentation/components/AiInterpretationStream'),
  { loading: () => null },
);

const AstralBackground = dynamic(
  () => import('@/shared/components/layout/AstralBackground'),
  { ssr: false, loading: () => null },
);

export default function ReadingSessionPage() {
  const params = useParams();
  const sessionId = String(params.id ?? '');
  const { contentProps } = useReadingSessionPageState(sessionId);

  return (
    <div className={cn('relative min-h-dvh overflow-x-hidden tn-session-page-pad-md font-sans tn-text-primary')}>
      <AstralBackground variant="subtle" particleCount={8} />
      <ReadingSessionContent {...contentProps} AiInterpretationStream={AiInterpretationStream} />
    </div>
  );
}
