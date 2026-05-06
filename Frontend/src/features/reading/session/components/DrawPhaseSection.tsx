import DrawPhaseContent from '@/features/reading/session/components/DrawPhaseContent';
import ShuffleIntro from '@/features/reading/session/components/ShuffleIntro';
import type { DrawPhaseSectionProps } from '@/features/reading/session/components/DrawPhaseSection.types';
import { cn } from '@/lib/utils';

export default function DrawPhaseSection({ error, isShuffling, shufflePaths, shuffleSubtitle, shuffleTitle, ...contentProps }: DrawPhaseSectionProps) {
  return (
    <div className={cn('flex tn-min-h-50vh w-full flex-col items-center justify-center py-10')}>
      {error ? <p className={cn('mb-6 rounded-xl border tn-border-danger-50 tn-bg-danger-soft px-6 py-3 tn-text-danger')}>{error}</p> : null}
      {isShuffling ? <ShuffleIntro shufflePaths={shufflePaths} title={shuffleTitle} subtitle={shuffleSubtitle} /> : <DrawPhaseContent {...contentProps} />}
    </div>
  );
}
