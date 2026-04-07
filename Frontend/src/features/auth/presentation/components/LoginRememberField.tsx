import { cn } from "@/lib/utils";
import type { UseFormRegisterReturn } from "react-hook-form";

interface LoginRememberFieldProps {
 label: string;
 register: UseFormRegisterReturn<"rememberMe">;
}

export function LoginRememberField({ label, register }: LoginRememberFieldProps) {
 return (
  <div className={cn("flex items-center ml-1 py-1")}>
   <label className={cn("flex items-center gap-3 min-h-11 cursor-pointer")}>
    <input type="checkbox" {...register} className={cn("h-5 w-5 rounded border tn-border-accent-50 tn-overlay-soft")} />
    <span className={cn("text-sm font-medium tn-text-secondary select-none")}>{label}</span>
   </label>
  </div>
 );
}
