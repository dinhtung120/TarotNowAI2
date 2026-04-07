import { cn } from "@/lib/utils";
import type { UseFormRegisterReturn } from "react-hook-form";

interface LoginRememberFieldProps {
 label: string;
 register: UseFormRegisterReturn<"rememberMe">;
}

export function LoginRememberField({ label, register }: LoginRememberFieldProps) {
 return (
  <div className={cn("flex items-center ml-1 py-1")}>
   <label className={cn("flex items-center gap-3 min-h-11 cursor-pointer group")}>
    <div className={cn("relative flex items-center justify-center w-11 h-11")}>
     <input type="checkbox" {...register} className={cn("peer absolute inset-0 w-11 h-11 opacity-0 cursor-pointer")} />
     <span className={cn("pointer-events-none w-5 h-5 border border-[var(--purple-accent)]/50 rounded tn-overlay-soft peer-checked:bg-[var(--purple-accent)] transition-all")} />
     <svg className={cn("absolute w-2.5 h-2.5 tn-text-ink pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity")} viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" /></svg>
    </div>
    <span className={cn("text-sm font-medium tn-text-secondary group-hover:tn-text-secondary transition-colors select-none")}>{label}</span>
   </label>
  </div>
 );
}
