import { ArrowLeft } from "lucide-react";
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from "@/lib/utils";

interface ForgotPasswordBackLinkProps {
  label: string;
}

export default function ForgotPasswordBackLink({
  label,
}: ForgotPasswordBackLinkProps) {
  return (
    <div className={cn("mb-6", "flex", "justify-center")}>
      <Link
        className={cn(
          "group",
          "inline-flex",
          "min-h-11",
          "items-center",
          "px-2",
          "text-xs",
          "font-bold",
          "uppercase",
          "tracking-widest",
          "transition-colors",
          "tn-text-secondary",
        )}
        href="/login"
      >
        <ArrowLeft className={cn("mr-1.5", "h-4", "w-4", "transition-transform")} />
        {label}
      </Link>
    </div>
  );
}
