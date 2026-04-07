import { cn } from "@/lib/utils";

interface SessionQuestionCardProps {
  label: string;
  question: string;
}

export default function SessionQuestionCard({
  label,
  question,
}: SessionQuestionCardProps) {
  return (
    <div
      className={cn(
        "rounded-2xl border border-[var(--purple-accent)]/20 p-6 text-center tn-overlay",
      )}
    >
      <p
        className={cn(
          "mb-2 text-sm font-semibold uppercase tracking-widest text-[var(--purple-accent)]",
        )}
      >
        {label}
      </p>
      <p className={cn("tn-text-lg-xl-sm italic font-serif tn-text-primary")}>
        &quot;{question}&quot;
      </p>
    </div>
  );
}
