import { ArrowLeft } from "lucide-react";
import { Link } from "@/i18n/routing";

interface ForgotPasswordBackLinkProps {
  label: string;
}

export default function ForgotPasswordBackLink({
  label,
}: ForgotPasswordBackLinkProps) {
  return (
    <div className="mb-6 flex justify-center">
      <Link
        className="group tn-text-secondary hover:tn-text-primary inline-flex min-h-11 items-center px-2 text-xs font-bold tracking-widest uppercase transition-colors"
        href="/login"
      >
        <ArrowLeft className="mr-1.5 h-4 w-4 transform transition-transform group-hover:-translate-x-1" />
        {label}
      </Link>
    </div>
  );
}
