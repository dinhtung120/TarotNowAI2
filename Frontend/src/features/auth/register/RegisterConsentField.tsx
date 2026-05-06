import { useState } from 'react';
import { useTranslations } from 'next-intl';
import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import type { RegisterFormValues } from '@/features/auth/shared/schemas';
import { cn } from '@/lib/utils';
import Modal from '@/shared/ui/Modal';
import { TermsOfServiceContent, PrivacyPolicyContent } from '@/features/legal/pages/components/LegalContent';

interface RegisterConsentFieldProps {
  errors: FieldErrors<RegisterFormValues>;
  register: UseFormRegister<RegisterFormValues>;
}

/**
 * Component xử lý phần đồng ý điều khoản dịch vụ và chính sách bảo mật.
 * Sử dụng Modal để hiển thị nội dung pháp lý ngay tại chỗ thay vì chuyển trang.
 */
export default function RegisterConsentField({ errors, register }: RegisterConsentFieldProps) {
  const t = useTranslations('Auth');
  const [showTos, setShowTos] = useState(false);
  const [showPrivacy, setShowPrivacy] = useState(false);

  return (
    <div className={cn('pt-1')}>
      <label className={cn('tn-consent-label group flex cursor-pointer items-center gap-1 rounded-xl border border-transparent p-1 transition-all')}>
        <div className={cn('relative flex h-8 w-8 shrink-0 items-center justify-center')}>
          <input 
            type="checkbox" 
            {...register('hasConsented')} 
            className={cn('tn-consent-checkbox-input absolute inset-0 h-8 w-8 cursor-pointer opacity-0')} 
          />
          <span className={cn('tn-consent-checkbox-box pointer-events-none h-4 w-4 rounded-md border tn-border-accent-50 bg-white/5 transition-all')} />
          <svg 
            className={cn('tn-consent-checkbox-check pointer-events-none absolute h-2.5 w-2.5 text-white opacity-0 transition-opacity')} 
            viewBox="0 0 14 10" 
            fill="none" 
            xmlns="http://www.w3.org/2000/svg"
          >
            <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
          </svg>
        </div>
        <div className={cn('flex flex-wrap items-center gap-x-1.5 gap-y-0.5 pt-0')}>
          <span className={cn('tn-consent-text tn-text-11 font-medium leading-snug tn-text-secondary transition-colors')}>
            {t('register.consent_prefix')}
          </span>
          <button 
            type="button"
            onClick={(e) => {
              e.preventDefault();
              setShowTos(true);
            }} 
            className={cn('tn-text-11 inline-flex h-6 items-center border-b tn-border-accent-30 tn-text-accent tn-hover-underline')}
          >
            {t('register.consent_terms')}
          </button>
          <span className={cn('tn-text-11 font-medium tn-text-secondary')}>{t('register.consent_and')}</span>
          <button 
            type="button"
            onClick={(e) => {
              e.preventDefault();
              setShowPrivacy(true);
            }} 
            className={cn('tn-text-11 inline-flex h-6 items-center border-b tn-border-accent-30 tn-text-accent tn-hover-underline')}
          >
            {t('register.consent_privacy')}
          </button>
        </div>
      </label>
      
      {errors.hasConsented ? (
        <p className={cn('ml-4 mt-1 tn-text-11 font-medium tn-text-danger')}>
          {errors.hasConsented.message}
        </p>
      ) : null}

      {/* Popup Điều khoản dịch vụ */}
      <Modal 
        isOpen={showTos} 
        onClose={() => setShowTos(false)} 
        title="Điều khoản dịch vụ"
        size="lg"
      >
        <TermsOfServiceContent />
      </Modal>

      {/* Popup Chính sách bảo mật */}
      <Modal 
        isOpen={showPrivacy} 
        onClose={() => setShowPrivacy(false)} 
        title="Chính sách bảo mật"
        size="lg"
      >
        <PrivacyPolicyContent />
      </Modal>
    </div>
  );
}
