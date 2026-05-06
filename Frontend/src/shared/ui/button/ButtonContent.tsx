import type { ReactNode } from 'react';
import { Loader2 } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ButtonContentProps {
  children: ReactNode;
  isLoading: boolean;
  leftIcon?: ReactNode;
  rightIcon?: ReactNode;
}

export default function ButtonContent({ children, isLoading, leftIcon, rightIcon }: ButtonContentProps) {
  return (
    <>
      {isLoading ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : leftIcon}
      {children}
      {!isLoading ? rightIcon : null}
    </>
  );
}
