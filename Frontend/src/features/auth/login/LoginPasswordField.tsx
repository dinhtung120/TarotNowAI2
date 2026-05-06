"use client";

import { useState } from 'react';
import { Eye, EyeOff, Lock } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Input } from '@/shared/ui';

interface LoginPasswordFieldProps {
 label: string;
 placeholder: string;
 error?: string;
 registerField: Record<string, unknown>;
}

export function LoginPasswordField(props: LoginPasswordFieldProps) {
 const [showPassword, setShowPassword] = useState(false);

 return (
  <Input
   label={props.label}
   type={showPassword ? 'text' : 'password'}
   leftIcon={<Lock className={cn('h-5', 'w-5')} />}
   placeholder={props.placeholder}
   error={props.error}
   rightElement={
    <button
     type="button"
     onClick={() => setShowPassword(!showPassword)}
     className={cn('tn-text-secondary hover:tn-text-primary transition-colors focus:outline-none')}
     aria-label={showPassword ? 'Hide password' : 'Show password'}
    >
     {showPassword ? <EyeOff className={cn('h-5', 'w-5')} /> : <Eye className={cn('h-5', 'w-5')} />}
    </button>
   }
   {...props.registerField}
  />
 );
}
