'use client';

import { forwardRef, useState, type Ref } from 'react';
import InputFieldMeta from '@/shared/components/ui/input/InputFieldMeta';
import { getTextareaDomProps } from '@/shared/components/ui/input/getTextareaDomProps';
import { baseInputStyles } from '@/shared/components/ui/input/inputStyles';
import type { CombinedProps, InputProps } from '@/shared/components/ui/input/input.types';
import { cn } from '@/lib/utils';

const Input = forwardRef<HTMLInputElement | HTMLTextAreaElement, CombinedProps>(({ label, error, hint, leftIcon, rightElement, fullWidth = true, className, onChange, ...rest }, ref) => {
  const isTextarea = 'isTextarea' in rest && rest.isTextarea;
  const inputRef = ref as Ref<HTMLInputElement>;
  const textareaRef = ref as Ref<HTMLTextAreaElement>;

  // Theo dõi giá trị để ẩn/hiện lỗi khi ô trống theo yêu cầu
  const [isValueEmpty, setIsValueEmpty] = useState(true);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setIsValueEmpty(e.target.value === '');
    if (onChange) onChange(e);
  };

  // Logic: Chỉ hiển thị viền đỏ nếu có lỗi và ô KHÔNG trống
  const showUIError = !!error && !isValueEmpty;

  return (
    <div className={cn('flex flex-col', fullWidth ? 'w-full' : '')}>
      {/* 1. Nhãn (Label) luôn nằm ở trên cùng */}
      <InputFieldMeta label={label} renderMeta={false} />
      
      <div className={cn('relative mt-1')}>
        {leftIcon ? <div className={cn('pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4 tn-text-secondary')}>{leftIcon}</div> : null}
        {rightElement ? <div className={cn('absolute inset-y-0 right-0 flex items-center pr-4')}>{rightElement}</div> : null}
        
        {isTextarea ? (
          <textarea
            ref={textareaRef}
            className={cn(baseInputStyles, 'tn-minh-80 resize-none', leftIcon ? 'pl-11' : '', rightElement ? 'pr-11' : '', showUIError ? 'tn-border-danger-50 tn-field-danger' : '', className)}
            {...getTextareaDomProps(rest as CombinedProps)}
            onChange={handleChange}
          />
        ) : (
          <input
            ref={inputRef}
            className={cn(baseInputStyles, leftIcon ? 'pl-11' : '', rightElement ? 'pr-11' : '', showUIError ? 'tn-border-danger-50 tn-field-danger' : '', className)}
            {...(rest as InputProps)}
            onChange={handleChange}
          />
        )}
      </div>

      {/* 2. Thông báo Lỗi hoặc Gợi ý nằm ở dưới, có vùng trống cố định để chống xô lệch layout */}
      <InputFieldMeta 
        error={error} 
        hint={hint} 
        isValueEmpty={isValueEmpty} 
        renderLabel={false} 
      />
    </div>
  );
});

Input.displayName = 'Input';

export default Input;
