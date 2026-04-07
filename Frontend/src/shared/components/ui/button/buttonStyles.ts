import { cn } from '@/lib/utils';

export type ButtonVariant = 'primary' | 'brand' | 'secondary' | 'ghost' | 'danger';
export type ButtonSize = 'sm' | 'md' | 'lg';

export const buttonVariantStyles: Record<ButtonVariant, string> = {
  primary: cn(
    'bg-[var(--bg-elevated)] text-[var(--text-ink)] border border-[var(--border-default)]',
    'hover:bg-[var(--bg-surface-hover)] hover:border-[var(--border-hover)] hover:shadow-[var(--glow-purple-md)]',
    'active:scale-[0.97] shadow-[var(--shadow-card)]',
  ),
  brand: cn(
    'bg-gradient-to-r from-[var(--purple-gradient-from)] via-[var(--purple-gradient-via)] to-[var(--purple-gradient-to)]',
    'text-[var(--text-ink)] border border-[var(--border-hover)] hover:brightness-105',
    'active:scale-[0.97] shadow-[var(--glow-purple-sm)] hover:shadow-[var(--glow-purple-lg)]',
  ),
  secondary: cn(
    'bg-[var(--bg-glass)] text-[var(--text-primary)] border border-[var(--border-default)]',
    'hover:bg-[var(--bg-glass-hover)] hover:border-[var(--border-hover)] hover:shadow-[var(--shadow-card)]',
    'active:scale-[0.97]',
  ),
  ghost: cn('bg-transparent text-[var(--text-secondary)]', 'hover:bg-[var(--purple-50)] hover:text-[var(--text-ink)]'),
  danger: cn(
    'bg-[var(--danger)]/10 text-[var(--danger)] border border-[var(--danger)]/20',
    'hover:bg-[var(--danger)]/20 hover:border-[var(--danger)]/30 active:scale-[0.97]',
  ),
};

export const buttonSizeStyles: Record<ButtonSize, string> = {
  sm: 'px-3 py-1.5 min-h-11 text-[10px] font-bold tracking-widest rounded-xl gap-1.5',
  md: 'px-5 py-2.5 min-h-11 text-[11px] font-black tracking-widest rounded-2xl gap-2',
  lg: 'px-8 py-4 min-h-12 text-xs font-black tracking-[0.2em] rounded-2xl gap-3',
};

export const buttonBaseStyles = cn(
  'inline-flex items-center justify-center uppercase cursor-pointer',
  'transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed disabled:scale-100',
);
