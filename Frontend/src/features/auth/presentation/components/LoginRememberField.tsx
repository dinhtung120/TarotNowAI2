import { cn } from "@/lib/utils";
import type { UseFormRegisterReturn } from "react-hook-form";

interface LoginRememberFieldProps {
 label: string;
 register: UseFormRegisterReturn<"rememberMe">;
}

export function LoginRememberField({ label, register }: LoginRememberFieldProps) {
  return (
    <div className={cn("flex items-center")}>
      <label className={cn("tn-consent-label group flex cursor-pointer items-center gap-1 rounded-xl border border-transparent p-1 transition-all")}>
        <div className={cn("relative flex h-8 w-8 shrink-0 items-center justify-center")}>
          <input 
            type="checkbox" 
            {...register} 
            className={cn("tn-consent-checkbox-input absolute inset-0 h-8 w-8 cursor-pointer opacity-0")} 
          />
          <span className={cn("tn-consent-checkbox-box pointer-events-none h-4 w-4 rounded-md border tn-border-accent-50 bg-white/5 transition-all")} />
          <svg 
            className={cn("tn-consent-checkbox-check pointer-events-none absolute h-2.5 w-2.5 text-white opacity-0 transition-opacity")} 
            viewBox="0 0 14 10" 
            fill="none" 
            xmlns="http://www.w3.org/2000/svg"
          >
            <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
          </svg>
        </div>
        <span className={cn("tn-text-11 font-medium tn-text-secondary select-none transition-colors")}>
          {label}
        </span>
      </label>
    </div>
  );
}
