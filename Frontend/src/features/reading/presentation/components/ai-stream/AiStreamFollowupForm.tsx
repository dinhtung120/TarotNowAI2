import { useEffect } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import type { FormEvent } from "react";
import { RefreshCw, Send } from "lucide-react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { cn } from "@/lib/utils";
import { AiStreamFollowupBadge } from "./AiStreamFollowupBadge";

interface AiStreamFollowupFormProps {
 isStreaming: boolean;
 isSendingFollowup: boolean;
 followupText: string;
 freeSlotsRemaining: number;
 placeholder: string;
 hint: string;
 freeBadgeText: string;
 paidBadgeText: string;
 onFollowupTextChange: (value: string) => void;
 onSubmit: (event: FormEvent<HTMLFormElement>) => void;
}

const aiStreamFollowupFormSchema = z.object({
 followupText: z.string().trim().min(1).max(1000),
});

type AiStreamFollowupFormValues = z.infer<typeof aiStreamFollowupFormSchema>;

export function AiStreamFollowupForm({ isStreaming, isSendingFollowup, followupText, freeSlotsRemaining, placeholder, hint, freeBadgeText, paidBadgeText, onFollowupTextChange, onSubmit }: AiStreamFollowupFormProps) {
 const { handleSubmit, setValue, watch } = useForm<AiStreamFollowupFormValues>({
  resolver: zodResolver(aiStreamFollowupFormSchema),
  defaultValues: {
   followupText,
  },
 });

 const watchedFollowupText = watch("followupText") ?? "";

 useEffect(() => {
  setValue("followupText", followupText, { shouldDirty: false, shouldValidate: false });
 }, [followupText, setValue]);

 useEffect(() => {
  onFollowupTextChange(watchedFollowupText);
 }, [onFollowupTextChange, watchedFollowupText]);

 const submitWithValidation = handleSubmit(() => {
  onSubmit({
   preventDefault: () => undefined,
   stopPropagation: () => undefined,
  } as unknown as FormEvent<HTMLFormElement>);
 });

 return (
  <form onSubmit={submitWithValidation} className={cn("mt-4 shrink-0 relative animate-in slide-in-from-bottom-5 duration-500")}>
   <div className={cn("absolute -top-6 left-2 flex items-center gap-2")}>
    <AiStreamFollowupBadge
     freeSlotsRemaining={freeSlotsRemaining}
     freeBadgeText={freeBadgeText}
     paidBadgeText={paidBadgeText}
    />
   </div>
   <div className={cn("relative")}>
    <input
     type="text"
     value={watchedFollowupText}
     onChange={(event) =>
      setValue("followupText", event.target.value, { shouldDirty: true, shouldValidate: true })
     }
     disabled={isStreaming || isSendingFollowup}
     placeholder={placeholder}
     className={cn("w-full tn-field tn-field-accent border-[var(--purple-accent)]/30 tn-text-primary rounded-2xl px-6 py-4 pr-16 disabled:opacity-50 font-serif")}
    />
    <button
     type="submit"
     disabled={!watchedFollowupText.trim() || isStreaming || isSendingFollowup}
     className={cn("absolute right-2 top-2 bottom-2 aspect-square bg-[var(--purple-accent)] hover:bg-[var(--purple-accent)] disabled:tn-surface-strong disabled:tn-text-muted tn-text-primary rounded-xl flex items-center justify-center transition-colors")}
    >
     {isSendingFollowup ? <RefreshCw className={cn("w-5 h-5 animate-spin")} /> : <Send className={cn("w-5 h-5")} />}
    </button>
   </div>
   <p className={cn("text-center text-[10px] tn-text-muted mt-2 font-mono uppercase tracking-wider")}>{hint}</p>
  </form>
 );
}
