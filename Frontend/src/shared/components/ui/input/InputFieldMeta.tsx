import { cn } from '@/lib/utils';

interface InputFieldMetaProps {
  error?: string;
  hint?: string;
  label?: string;
  isValueEmpty?: boolean;
  renderLabel?: boolean;
  renderMeta?: boolean;
}

/**
 * Component hiển thị thông tin bổ trợ cho ô nhập liệu (Nhãn, Lỗi, Gợi ý)
 */
export default function InputFieldMeta({ 
  error, 
  hint, 
  label, 
  isValueEmpty = false,
  renderLabel = true,
  renderMeta = true
}: InputFieldMetaProps) {
  // Logic: Nếu ô nhập đang trống thì không hiện lỗi theo yêu cầu của người dùng
  const finalError = isValueEmpty ? undefined : error;

  return (
    <>
      {renderLabel && label ? (
        <label className={cn('ml-1 block tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}>
          {label}
        </label>
      ) : null}
      
      {renderMeta ? (
        <div className={cn('min-h-[1rem] pt-0.5')}>
          {finalError ? (
            <p className={cn('ml-1 tn-text-11 font-medium tn-text-danger')}>{finalError}</p>
          ) : hint ? (
            <p className={cn('ml-1 tn-text-11 font-medium tn-text-muted')}>{hint}</p>
          ) : null}
        </div>
      ) : null}
    </>
  );
}
