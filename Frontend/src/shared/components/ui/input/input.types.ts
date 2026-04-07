import type { InputHTMLAttributes, ReactNode, TextareaHTMLAttributes } from 'react';

export interface InputProps extends Omit<InputHTMLAttributes<HTMLInputElement>, 'size'> {
  error?: string;
  fullWidth?: boolean;
  hint?: string;
  label?: string;
  leftIcon?: ReactNode;
}

export interface TextareaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  error?: string;
  fullWidth?: boolean;
  hint?: string;
  isTextarea: true;
  label?: string;
  leftIcon?: ReactNode;
}

export type CombinedProps = (InputProps & { isTextarea?: false }) | TextareaProps;
