'use client';

import { forwardRef, type ButtonHTMLAttributes, type ReactNode } from 'react';
import ButtonContent from '@/shared/components/ui/button/ButtonContent';
import { buttonBaseStyles, buttonSizeStyles, buttonVariantStyles, type ButtonSize, type ButtonVariant } from '@/shared/components/ui/button/buttonStyles';
import { cn } from '@/lib/utils';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  fullWidth?: boolean;
  isLoading?: boolean;
  leftIcon?: ReactNode;
  rightIcon?: ReactNode;
  size?: ButtonSize;
  variant?: ButtonVariant;
}

const Button = forwardRef<HTMLButtonElement, ButtonProps>(({ variant = 'primary', size = 'md', isLoading = false, leftIcon, rightIcon, fullWidth = false, disabled, children, className = '', type = 'button', ...props }, ref) => (
  <button
    ref={ref}
    type={type}
    disabled={disabled || isLoading}
    className={cn(buttonBaseStyles, buttonVariantStyles[variant], buttonSizeStyles[size], fullWidth ? 'w-full' : '', className)}
    {...props}
  >
    <ButtonContent isLoading={isLoading} leftIcon={leftIcon} rightIcon={rightIcon}>
      {children}
    </ButtonContent>
  </button>
));

Button.displayName = 'Button';

export default Button;
