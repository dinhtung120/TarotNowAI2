'use client';

import { forwardRef, type Ref } from 'react';
import InputFieldMeta from '@/shared/components/ui/input/InputFieldMeta';
import { getTextareaDomProps } from '@/shared/components/ui/input/getTextareaDomProps';
import { baseInputStyles } from '@/shared/components/ui/input/inputStyles';
import type { CombinedProps, InputProps } from '@/shared/components/ui/input/input.types';
import { cn } from '@/lib/utils';

const Input = forwardRef<HTMLInputElement | HTMLTextAreaElement, CombinedProps>(({ label, error, hint, leftIcon, fullWidth = true, className, ...rest }, ref) => {
  const isTextarea = 'isTextarea' in rest && rest.isTextarea;
  const inputRef = ref as Ref<HTMLInputElement>;
  const textareaRef = ref as Ref<HTMLTextAreaElement>;

  return (
    <div className={cn('space-y-1.5', fullWidth ? 'w-full' : '')}>
      <InputFieldMeta label={label} error={error} hint={hint} />
      <div className={cn('relative')}>
        {leftIcon ? <div className={cn('pointer-events-none absolute inset-y-0 left-0 flex items-center pl-4 tn-text-secondary')}>{leftIcon}</div> : null}
        {isTextarea ? (
          <textarea
            ref={textareaRef}
            className={cn(baseInputStyles, 'tn-minh-80 resize-none', leftIcon ? 'pl-11' : '', error ? 'tn-border-danger-50 tn-field-danger' : '', className)}
            {...getTextareaDomProps(rest as CombinedProps)}
          />
        ) : (
          <input
            ref={inputRef}
            className={cn(baseInputStyles, leftIcon ? 'pl-11' : '', error ? 'tn-border-danger-50 tn-field-danger' : '', className)}
            {...(rest as InputProps)}
          />
        )}
      </div>
    </div>
  );
});

Input.displayName = 'Input';

export default Input;
