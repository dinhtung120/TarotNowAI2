import { Facebook, Instagram, Music2 } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

interface ReaderApplySocialLinksFieldsProps {
 errors?: {
  facebookUrl?: string;
  instagramUrl?: string;
  tikTokUrl?: string;
  socialRequired?: string;
 };
 facebookUrl: string;
 instagramUrl: string;
 tikTokUrl: string;
 onChangeFacebook: (value: string) => void;
 onChangeInstagram: (value: string) => void;
 onChangeTikTok: (value: string) => void;
}

export default function ReaderApplySocialLinksFields(props: ReaderApplySocialLinksFieldsProps) {
 const t = useTranslations('ReaderApply');

 return (
  <div className={cn('space-y-3')}>
   <label className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary')}>
    {t('form.social_links_label')}
   </label>

   <div className={cn('space-y-2')}>
    <div className={cn('flex items-center gap-2')}>
     <Facebook className={cn('h-3.5 w-3.5 tn-text-secondary')} />
     <input
      value={props.facebookUrl}
      onChange={(event) => props.onChangeFacebook(event.target.value)}
      placeholder={t('form.facebook_placeholder')}
      aria-invalid={Boolean(props.errors?.facebookUrl || props.errors?.socialRequired)}
      className={cn('w-full rounded-xl border bg-white/5 px-3 py-2 text-sm tn-text-primary', props.errors?.facebookUrl ? 'tn-border-danger' : 'tn-border-soft')}
     />
    </div>
    {props.errors?.facebookUrl ? <p className={cn('tn-text-10 tn-text-danger')}>{props.errors.facebookUrl}</p> : null}
    <div className={cn('flex items-center gap-2')}>
     <Instagram className={cn('h-3.5 w-3.5 tn-text-secondary')} />
     <input
      value={props.instagramUrl}
      onChange={(event) => props.onChangeInstagram(event.target.value)}
      placeholder={t('form.instagram_placeholder')}
      aria-invalid={Boolean(props.errors?.instagramUrl)}
      className={cn('w-full rounded-xl border bg-white/5 px-3 py-2 text-sm tn-text-primary', props.errors?.instagramUrl ? 'tn-border-danger' : 'tn-border-soft')}
     />
    </div>
    {props.errors?.instagramUrl ? <p className={cn('tn-text-10 tn-text-danger')}>{props.errors.instagramUrl}</p> : null}
    <div className={cn('flex items-center gap-2')}>
     <Music2 className={cn('h-3.5 w-3.5 tn-text-secondary')} />
     <input
      value={props.tikTokUrl}
      onChange={(event) => props.onChangeTikTok(event.target.value)}
      placeholder={t('form.tiktok_placeholder')}
      aria-invalid={Boolean(props.errors?.tikTokUrl)}
      className={cn('w-full rounded-xl border bg-white/5 px-3 py-2 text-sm tn-text-primary', props.errors?.tikTokUrl ? 'tn-border-danger' : 'tn-border-soft')}
     />
    </div>
    {props.errors?.tikTokUrl ? <p className={cn('tn-text-10 tn-text-danger')}>{props.errors.tikTokUrl}</p> : null}
   </div>

   <p className={cn('tn-text-10 tn-text-muted')}>{t('form.social_links_hint')}</p>
   {props.errors?.socialRequired ? <p className={cn('tn-text-10 tn-text-danger')}>{props.errors.socialRequired}</p> : null}
  </div>
 );
}
