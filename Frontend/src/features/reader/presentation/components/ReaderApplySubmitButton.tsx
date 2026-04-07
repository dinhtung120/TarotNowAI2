import { Loader2, Send } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReaderApplySubmitButtonProps {
  disabled: boolean;
  submitting: boolean;
}

export default function ReaderApplySubmitButton({ disabled, submitting }: ReaderApplySubmitButtonProps) {
  const t = useTranslations('ReaderApply');

  return (
    <button
      id="reader-submit-btn"
      type="submit"
      disabled={disabled}
      className={cn('group relative w-full overflow-hidden rounded-2xl tn-bg-accent p-5 text-center text-sm font-black uppercase tracking-widest text-white shadow-xl transition-all duration-300 tn-disabled-not-allowed tn-disabled-bg-disabled')}
    >
      {submitting ? (
        <span className={cn('flex items-center justify-center gap-3')}>
          <Loader2 className={cn('h-4 w-4 animate-spin')} />
          {t('form.submitting')}
        </span>
      ) : (
        <span className={cn('flex items-center justify-center gap-3')}>
          <Send className={cn('h-4 w-4')} />
          {t('form.submit')}
        </span>
      )}
    </button>
  );
}
