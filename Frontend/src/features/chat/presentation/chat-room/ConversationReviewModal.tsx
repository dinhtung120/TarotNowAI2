import { Star } from 'lucide-react';
import { useTranslations } from 'next-intl';
import Modal from '@/shared/components/ui/Modal';
import { cn } from '@/lib/utils';

interface ConversationReviewModalProps {
  comment: string;
  isOpen: boolean;
  rating: number;
  submitting: boolean;
  onClose: () => void;
  onCommentChange: (value: string) => void;
  onRatingChange: (value: number) => void;
  onSubmit: () => Promise<void>;
}

export default function ConversationReviewModal({
  comment,
  isOpen,
  rating,
  submitting,
  onClose,
  onCommentChange,
  onRatingChange,
  onSubmit,
}: ConversationReviewModalProps) {
  const t = useTranslations('Chat');

  return (
    <Modal
      isOpen={isOpen}
      size="sm"
      title={t('room.review.title')}
      onClose={onClose}
    >
      <div className={cn('space-y-4')}>
        <p className={cn('text-sm tn-text-secondary')}>{t('room.review.description')}</p>
        <div className={cn('flex items-center gap-1')} role="radiogroup" aria-label={t('room.review.rating_label')}>
          {[1, 2, 3, 4, 5].map((value) => {
            const selected = value <= rating;
            return (
              <button
                key={value}
                type="button"
                role="radio"
                aria-checked={value === rating}
                onClick={() => onRatingChange(value)}
                className={cn(
                  'rounded-md p-1 transition',
                  selected ? 'text-amber-400' : 'text-slate-500 hover:text-amber-300',
                )}
              >
                <Star className={cn('h-6 w-6', selected ? 'fill-current' : '')} />
              </button>
            );
          })}
        </div>
        <textarea
          rows={4}
          value={comment}
          onChange={(event) => onCommentChange(event.target.value)}
          placeholder={t('room.review.comment_placeholder')}
          className={cn('w-full rounded-xl border border-white/10 bg-white/5 px-3 py-2 text-sm text-white')}
        />
        <div className={cn('flex justify-end gap-2')}>
          <button
            type="button"
            disabled={submitting}
            onClick={onClose}
            className={cn('rounded-lg border border-white/10 px-3 py-2 text-xs text-slate-200')}
          >
            {t('room.review.skip')}
          </button>
          <button
            type="button"
            disabled={submitting || rating < 1 || rating > 5}
            onClick={() => void onSubmit()}
            className={cn('rounded-lg bg-emerald-500 px-3 py-2 text-xs font-semibold text-white disabled:opacity-60')}
          >
            {submitting ? t('room.review.submitting') : t('room.review.submit')}
          </button>
        </div>
      </div>
    </Modal>
  );
}
