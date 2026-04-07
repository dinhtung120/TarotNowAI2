import { FileText } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReaderApplyIntroFieldProps {
  introText: string;
  setIntroText: (value: string) => void;
}

export default function ReaderApplyIntroField({ introText, setIntroText }: ReaderApplyIntroFieldProps) {
  const t = useTranslations('ReaderApply');

  return (
    <div className={cn('space-y-3')}>
      <label htmlFor="reader-intro-text" className={cn('flex items-center gap-2 tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
        <FileText className={cn('h-3 w-3')} />
        {t('form.intro_label')}
      </label>
      <textarea
        id="reader-intro-text"
        value={introText}
        onChange={(event) => setIntroText(event.target.value)}
        placeholder={t('form.intro_placeholder')}
        rows={6}
        className={cn('w-full resize-none rounded-2xl border border-white/10 bg-white/5 p-6 text-sm tn-text-primary tn-placeholder transition-all')}
      />
      <div className={cn('flex justify-between')}>
        <span className={cn('tn-text-10', introText.length >= 20 ? 'tn-text-muted' : 'tn-text-danger')}>{t('form.min_chars')}</span>
        <span className={cn('tn-text-10 tn-text-muted')}>{introText.length}/2000</span>
      </div>
    </div>
  );
}
